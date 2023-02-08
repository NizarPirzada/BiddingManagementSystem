
using Contracts;
using Microsoft.AspNetCore.Mvc;
using MissionControl.DataTransferObject.Application;
using Newtonsoft.Json;
using NLog;
using PanacealogicsSales.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PanacealogicsSales.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public static Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public AccountController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        [HttpPost("Index")]
        public Task<string> Index()
        {
            var data = "";
            return Task.Run(() =>
            {
                return JsonConvert.SerializeObject(data);
            });
        }

        [HttpPost("socialuser")]
        public async Task<string> socialuser([FromBody] SocialUser social)
        {
            var response = new Response();
            try
            {
                var data = (from socialuserlist in _repository.SocialUser.FindByCondition(x => x.social_id == social.social_id)
                            join userlist in _repository.User.FindAll()
                            on socialuserlist.user_id equals userlist.user_id
                            select new respUser
                            {
                                user_id = userlist.user_id,
                                username = userlist.username,
                                role_id = userlist.role_id,
                                social_id = socialuserlist.social_id

                            }).FirstOrDefault();

                if (data != null)
                {
                    return JsonConvert.SerializeObject(data);

                }
                else
                {
                    var respValue = insertdata(social);

                    return JsonConvert.DeserializeObject(respValue).ToString();

                }
            }
            catch (Exception ex)
            {

                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();

        }

        [HttpGet("Userlist")]
        public async Task<string> Userlist(int? userId)
        {
            var response = new Response();
            try
            {
                if(userId==0)
                {
                    var data = _repository.User.FindAll().ToList();
                    if (data != null)
                    {
                        return JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        return data.ToString();
                    }
                }
                else
                {
                    var data = _repository.User.FindByCondition(x => x.role_id == 2).ToList();
                    if (data != null)
                    {
                        return JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        return data.ToString();
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
        [HttpPost("insertdata")]
        public string insertdata([FromBody] SocialUser social)
        {
            var response = new Response();
            try
            {
                var Userobj = new User
                {
                    role_id = 2,
                    username = social.name,
                    date = DateTime.UtcNow,
                    is_active = true,
                    social_id = social.social_id,
                    password = DateTime.Now.Second.ToString()

                };
                var respuser = _repository.User.Create(Userobj);
                var respvalue = _repository.User.FindByCondition(x => x.social_id == social.social_id).FirstOrDefault();
                social.user_id = respvalue.user_id;
                var respsocaluser = _repository.SocialUser.Create(social);


                return JsonConvert.SerializeObject(Userobj);
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
