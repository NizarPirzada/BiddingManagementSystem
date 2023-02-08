using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MissionControl.DataTransferObject.Application;
using Newtonsoft.Json;
using NLog;
using PanacealogicsSales.Entities.Models;

namespace PanacealogicsSales.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        protected readonly IHubContext<MessageHub> _messageHub;
        private IRepositoryWrapper _repository;
        public static Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public MessageController([NotNull] IHubContext<MessageHub> messageHub, IRepositoryWrapper repository)
        {
            _repository = repository;
            _messageHub = messageHub;

        }

        [Route("api/message/senddm")]
        [HttpPost]
        public async Task<IActionResult> SendDM([FromBody] RequestMessage message)
        {
            await _messageHub.Clients.Client(message.reciverUserId).SendAsync("getDM", $"{"hello"} said : {message.Message}");
            return Ok();
        }
        [HttpPost("ProcessMessage")]
        public async Task<string> ProcessedClientMessage(int threadId)
        {
            var response = new Response();
            try
            {
                var respProcessMsg = _repository.ThreadMessage.FindByCondition(x => x.thread_id == threadId && x.is_processed == false).ToList();
                if (respProcessMsg != null)
                {
                    return JsonConvert.SerializeObject(respProcessMsg);
                }
            }
            catch (Exception ex)
            {

                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();
        }
        [HttpPost("UpdateIsProcessed")]
        public async Task<Response> UpdateIsProcessed(int threadId)
        {
            var response = new Response();
            try
            {
                var isprocessedMsgExist = _repository.ThreadMessage.FindByCondition(x => x.thread_id == threadId).ToList();
                if (isprocessedMsgExist != null)
                {

                    foreach (var item in isprocessedMsgExist)
                    {
                        var threadMessage = new ThreadMessage
                        {
                            thread_message_id = item.thread_message_id,
                            created_date = item.created_date,
                            is_client_message = item.is_client_message,
                            is_processed = true,
                            message = item.message,
                            is_read = item.is_read,
                            thread_id = item.thread_id,
                            project_id = item.project_id


                        };
                        var respUpdate = _repository.ThreadMessage.Update(threadMessage);
                    }

                    response.StatusCode = StatusType.Success;
                }

            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;
        }
        [HttpPost("SearchThread")]
        public async Task<string> SearchThreadMessage(string searchClient, int user_2)
        {
            var dbResp = new List<ThreadDTO>();
            var response = new Response();
            try
            {
            var data = _repository.Thread.FindByCondition(x => x.user_1 == searchClient && x.user_2 == user_2).ToList();
            if (data != null)
            {

                foreach (var item in data)
                {
                    var listObj = (from threadlist in _repository.Thread.FindByCondition(x => x.user_1 == searchClient && x.user_2 == user_2)
                                   join threadmessage in _repository.ThreadMessage.FindByCondition(x => x.thread_id == item.thread_id).OrderByDescending(x => x.created_date).Take(1)
                                   on threadlist.thread_id equals threadmessage.thread_id
                                   select new ThreadDTO()
                                   {
                                       user_1 = threadlist.user_1,
                                       message = threadmessage.message,
                                       has_new_message = threadlist.has_new_messages
                                   }).ToList();

                    dbResp.AddRange(listObj);
                }
                if (dbResp != null)
                {
                        return JsonConvert.SerializeObject(dbResp);
                }

            }
            }
            catch (Exception ex )
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();
           
        }

        [HttpPost("ClientMessage")]
        public async Task<Response> ClientMessageSave([FromBody] ThreadDTO respthread)
        {

            var response = new Response();
            try
            {
                var ifClientExist = _repository.Thread.FindByCondition(x => x.project_id == respthread.project_id).ToList();
                if (ifClientExist.Count > 0)
                {
                    var respThread = _repository.Thread.FindByCondition(x => x.project_id == respthread.project_id).FirstOrDefault();
                    var thread = new Thread
                    {
                        thread_id = respThread.thread_id,
                        project_id = respThread.project_id,
                        created_date = respThread.created_date,
                        has_new_messages = true,
                        user_1 = respThread.user_1,
                        user_2 = respThread.user_2,
                        last_updated = respThread.last_updated

                    };
                    _repository.Thread.Update(thread);
                    var theadMessage = new ThreadMessage
                    {
                        is_client_message = true,
                        created_date = DateTime.UtcNow,
                        is_processed = false,
                        message = respthread.message,
                        is_read = false,
                        thread_id = respThread.thread_id,
                        project_id = (Guid)respthread.project_id,

                    };
                    var respThreadMessage = _repository.ThreadMessage.Create(theadMessage);
                    SendMessage(respThread.user_2, respThread.user_1);
                    response.StatusCode = StatusType.Success;
                }
                else
                {
                    var clientName = "";
                    var respdb = _repository.Project.FindByCondition(x => x.project_id == respthread.project_id).FirstOrDefault();
                    if (respthread.user_1.Contains(" ") && respthread.user_1.Contains("_"))
                    {
                        var iniObj = respthread.user_1.Replace(" ", "-");
                        var repUnderscore = iniObj.Replace("_", "-");
                        int index = repUnderscore.IndexOf("-");
                        clientName = repUnderscore.Remove(index, 1).Insert(index, "_");
                        
                    }
                    else if (respthread.user_1.Contains("_"))
                    {
                        var iniObj = respthread.user_1.Replace("_", "-");
                        int index = iniObj.IndexOf("-");
                        clientName = iniObj.Remove(index, 1).Insert(index, "_");
                        
                    }
                    else if (respthread.user_1.Contains(" "))
                    {
                        var obsj = respthread.user_1.Replace(" ", "-");
                        int index = obsj.IndexOf("-");
                        clientName = obsj.Remove(index, 1).Insert(index, "_");
                    }
                    else if (respthread.user_1.Contains(""))
                    {
                        clientName = respthread.user_1.Insert(1, "_");
                    }

                    var thread = new Thread
                    {
                        project_id = (Guid)respthread.project_id,
                        created_date = DateTime.UtcNow,
                        has_new_messages = true,
                        user_1 = clientName,
                        user_2 = (int)respdb.user_id,
                        last_updated = DateTime.UtcNow

                    };

                    var respThread = _repository.Thread.Create(thread);
                    if(respThread == 200)
                    {
                    var respThreadId = _repository.Thread.FindByCondition(x => x.user_1 == clientName && x.user_2 == respdb.user_id).FirstOrDefault();
                    var theadMessage = new ThreadMessage
                    {
                        is_client_message = true,
                        created_date = DateTime.UtcNow,
                        is_processed = false,
                        message = respthread.message,
                        is_read = false,
                        thread_id = respThreadId.thread_id,
                        project_id = respThreadId.project_id

                    };
                    var respThreadMessage = _repository.ThreadMessage.Create(theadMessage);
                    SendMessage((int)respdb.user_id, respthread.user_1);
                    }
                    response.StatusCode = StatusType.Success;
                }


            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;

        }
        [HttpPost("ChangethreadUser")]
        public async Task<Response> UpdateChatUser(int threadId,int changeUserId)
        {
            var response = new Response();
            try
            {
                var data = _repository.Thread.FindByCondition(x => x.thread_id == threadId).FirstOrDefault();
                if(data !=null)
                {
                    var thread = new Thread
                    {
                        thread_id = data.thread_id,
                        project_id = data.project_id,
                        created_date = data.created_date,
                        has_new_messages = data.has_new_messages,
                        user_1 = data.user_1,
                        user_2 = changeUserId,
                        last_updated = DateTime.UtcNow

                    };
                    var respUpdate = _repository.Thread.Update(thread);
                    response.StatusCode = StatusType.Success;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

            [HttpPost("ChangeChatUser")]
        public async Task<Response> ChangeChatUser(Guid projectId)
        {
            var response = new Response();
            try
            {
                var data = _repository.Thread.FindByCondition(x => x.project_id == projectId).FirstOrDefault();
                if(data !=null)
                {
                    var thread = new Thread
                    {
                        thread_id = data.thread_id,
                        project_id = data.project_id,
                        created_date = data.created_date,
                        has_new_messages = data.has_new_messages,
                        user_1 = data.user_1,
                        user_2 = 1,
                        last_updated = DateTime.UtcNow

                    };
                    var respUpdate = _repository.Thread.Update(thread);
                    var threadHistory = new ThreadHistory
                    {
                        thread_id = data.thread_id,
                        previous_user = data.user_2,
                        created_date = DateTime.UtcNow

                    };
                    var respThreadHistory = _repository.ThreadHistory.Create(threadHistory);
                    response.StatusCode = StatusType.Success;

                }
                else
                {
                    response.StatusCode = StatusType.NotFound;
                }
               

            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;
        }
        [HttpPost("UpdateUserRead")]
        public async Task<Response> UpdateIsRead(string user1, int user2)
        {
            var response = new Response();
            try
            {
                var respThread = _repository.Thread.FindByCondition(x => x.user_1 == user1 && x.user_2 == user2).FirstOrDefault();
                if(respThread != null)
                {
                    var threadObj = new Thread
                    {
                        thread_id = respThread.thread_id,
                        project_id = respThread.project_id,
                        created_date = respThread.created_date,
                        has_new_messages = false,
                        user_1 = respThread.user_1,
                        user_2 = respThread.user_2,
                        last_updated = respThread.last_updated

                    };
                    var respUpdate = _repository.Thread.Update(threadObj);
                    if(respUpdate ==200)
                    {
                        response.StatusCode = StatusType.Success;
                    }
                   
                }
               
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;
        }
        [HttpPost("IsUserRead")]
        public async Task<Response> IsUserRead(int threadId)
        {
            var response = new Response();
            try
            {
                var data = _repository.ThreadMessage.FindByCondition(x => x.thread_id == threadId).FirstOrDefault();
                var threadMessage = new ThreadMessage
                {
                    thread_message_id = data.thread_message_id,
                    created_date = data.created_date,
                    is_client_message = data.is_client_message,
                    is_processed = data.is_processed,
                    message = data.message,
                    is_read = true,
                    thread_id = threadId,
                    project_id = data.project_id

                };
                var respUpdate = _repository.ThreadMessage.Update(threadMessage);
                response.StatusCode = StatusType.Success;

            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;
        }
        [HttpGet("Threadlist")]
        public Task<string> Userlist(int userId)
        {
            var dbResp = new List<ThreadDTO>();
            var response = new Response();
            var data = _repository.Thread.FindByCondition(x => x.user_2 == userId).ToList();
            if (data != null)
            {

                foreach (var item in data)
                {
                    var listObj = (from threadlist in _repository.Thread.FindByCondition(x => x.user_2 == userId)
                                   join threadmessage in _repository.ThreadMessage.FindByCondition(x => x.thread_id == item.thread_id).OrderByDescending(x => x.created_date).Take(1)
                                   on threadlist.thread_id equals threadmessage.thread_id
                                   select new ThreadDTO()
                                   {
                                       user_1 = threadlist.user_1,
                                       message = threadmessage.message,
                                       has_new_message = threadlist.has_new_messages
                                   }).ToList();

                    dbResp.AddRange(listObj);
                }



                if (dbResp != null)
                {

                    return Task.Run(() =>
                    {
                        return JsonConvert.SerializeObject(dbResp);
                    });

                }

            }

            return Task.Run(() =>
            {
                return response.ToString();
            });

        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(int user2, string user1)
        {
            var getThread = _repository.Thread.FindByCondition(x => x.user_2 == user2 && x.user_1 == user1).FirstOrDefault();
            var dbresp = (from projectlist in _repository.Project.FindByCondition(x => x.project_id == getThread.project_id)
                          join threadmessagelist in _repository.ThreadMessage.FindByCondition(x => x.thread_id == getThread.thread_id)
                          on projectlist.project_id equals threadmessagelist.project_id
                          select new ThreadMessageDTO()
                          {
                              thread_message_id = threadmessagelist.thread_message_id,
                              thread_id = threadmessagelist.thread_id,
                              message = threadmessagelist.message,
                              is_client_message = threadmessagelist.is_client_message,
                              created_date = threadmessagelist.created_date,
                              is_processed = threadmessagelist.is_processed,
                              is_read = threadmessagelist.is_read,
                              project_id = threadmessagelist.project_id,
                              project_name = projectlist.name,
                              receiver_id = user1,
                              user_2=user2
                              
                          }).ToList();
            if(dbresp !=null)
            {
            List<string> myconlist = MessageHub.connectionlist;
            foreach (var item in myconlist)
            {
                await _messageHub.Clients.Client(item).SendAsync("getDM", $"{JsonConvert.SerializeObject(dbresp)}");
            }
            }

            return Ok();
        }
        [HttpPost("UserMessage")]
        public async Task<string> UserMessageSave([FromBody] ThreadDTO respthread)
        {

            var response = new Response();
            try
            {
                var respThread = _repository.Thread.FindByCondition(x => x.user_1 == respthread.user_1 && x.user_2 == respthread.user_2).FirstOrDefault();
                if(respThread !=null)
                {
                var theadMessage = new ThreadMessage
                {
                    is_client_message = false,
                    created_date = DateTime.UtcNow,
                    is_processed = false,
                    message = respthread.message,
                    is_read = false,
                    thread_id = respThread.thread_id,
                    project_id = respThread.project_id

                };
                var respThreadMessage = _repository.ThreadMessage.Create(theadMessage);
                    if(respThreadMessage ==200)
                    {
                        var respMessagedata = _repository.ThreadMessage.FindByCondition(x => x.thread_id == respThread.thread_id).ToList();
                        return JsonConvert.SerializeObject(respMessagedata);
                    }
                }
              
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();

        }

        [HttpPost("GetMessage")]
        public async Task<string> MessageList(int senderId, string receiverId)
        {
            var response = new Response();
            try
            {

                var getThread = _repository.Thread.FindByCondition(x => x.user_2 == senderId && x.user_1 == receiverId).FirstOrDefault();
            var dbMessageresp = (from projectlist in _repository.Project.FindByCondition(x => x.project_id == getThread.project_id)
                              join threadmessagelist in _repository.ThreadMessage.FindByCondition(x => x.thread_id == getThread.thread_id)
                              on projectlist.project_id equals threadmessagelist.project_id
                              select new ThreadMessageDTO()
                              {
                                  thread_message_id = threadmessagelist.thread_message_id,
                                  thread_id = threadmessagelist.thread_id,
                                  message = threadmessagelist.message,
                                  is_client_message = threadmessagelist.is_client_message,
                                  created_date = threadmessagelist.created_date,
                                  is_processed = threadmessagelist.is_processed,
                                  is_read = threadmessagelist.is_read,
                                  project_id = threadmessagelist.project_id,
                                  project_name = projectlist.name,
                                  receiver_id = receiverId
                              }).ToList();
                if (dbMessageresp != null)
                {
                    IsUserRead(getThread.thread_id);
                    return JsonConvert.SerializeObject(dbMessageresp);
                }
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();

        }
    }
}