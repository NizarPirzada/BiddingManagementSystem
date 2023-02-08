using Contracts;
using Entities;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MissionControl.DataTransferObject.Application;
using Newtonsoft.Json;
using NLog;
using PanacealogicsSales.Entities.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Project = PanacealogicsSales.Entities.Models.Project;

namespace PanacealogicsSales.Web.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private ILogger<ProjectController> _logger;
        protected readonly IHubContext<MessageHub> _messageHub;
        public static Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        Guid guid = Guid.NewGuid();
        public ProjectController(IRepositoryWrapper repository, [NotNull] IHubContext<MessageHub> messageHub, ILogger<ProjectController> logger)
        {

            _repository = repository;
            _messageHub = messageHub;
            _logger = logger;
        }
        [HttpPost("Save2")]
        public int Save2([FromBody] Project2 project2)
        {

            var data = _repository.Project2.Create2(project2);
            return data;
        }
        [HttpPost("Save")]
        public async Task<Response> Save([FromBody] ProjectObjDTO projectObjDTO)
        {

            var response = new Response();
            try
            {
                var isExtId = _repository.Project.FindByCondition(x => x.external_project_id == projectObjDTO.external_project_id).ToList();
                if (isExtId.Count > 0)
                {
                    response.StatusCode = StatusType.Conflict;
                }
                else
                {
                   var userShiftId= UserShift();

                    var projectObj = new Project
                    {
                        project_id=guid,
                        name = projectObjDTO.name,
                        skills= projectObjDTO.skills,
                        desc = projectObjDTO.desc,
                        date = DateTime.UtcNow,
                        is_deleted =false,
                        user_id = userShiftId,
                        project_type = projectObjDTO.project_type,
                        status = "Pending",
                        project_time = projectObjDTO.project_time,
                        budget = projectObjDTO.budget,
                        client_country = projectObjDTO.client_country,
                        client_reputation = projectObjDTO.client_reputation,
                        project_value = projectObjDTO.project_value,
                        external_project_id = projectObjDTO.external_project_id,

                    };
                    var proposalObj = new Proposal
                    {
                        generated_proposal = projectObjDTO.generated_proposal,
                        project_id = projectObj.project_id,
                        user_id= userShiftId

                    };

                    var respDb = _repository.Project.FindByCondition(x => x.project_id == projectObj.project_id).FirstOrDefault();
                    if(respDb ==null)
                    {
                       var respProject = _repository.Project.Create(projectObj);
                           SendDM(); //send message with Signal r //
                       var respProposal=  _repository.Proposal.Create(proposalObj);
                        if(respProject == 200 && respProposal == 200)
                        {
                            response.StatusCode = StatusType.Success;
                        }
                        else
                        {
                            response.StatusCode = StatusType.BadRequest;
                        }
                      
                    }
                    else
                    {
                        response.StatusCode = StatusType.Conflict;
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
        [HttpPost("ProjectStats")]
        public async Task<string> ProjectStats([FromBody] ProjectCountDTO respProjectCount)
        {

            var response = new Response();
            try
            {
                if (respProjectCount.userId == null)
                {
                    var respTotalProject = _repository.Project.FindAll().Count();
                    var respApprovedProject = _repository.Project.FindByCondition(x => x.status == "Approved").Count();
                    var respMissedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == true).Count();
                    var respProposedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == false).Count();
                    var projectCount = new ProjectCountDTO
                    {
                        TotalProject = respTotalProject,
                        MyProject = respApprovedProject,
                        MissedProject = respMissedProject,
                        ProposedProject = respProposedProject

                    };

                    return JsonConvert.SerializeObject(projectCount);
                }
                else if (respProjectCount.userId != null && respProjectCount.fromDate == null && respProjectCount.toDate == null)
                {
                    var respTotalProject = _repository.Project.FindAll().Where(x => x.user_id == respProjectCount.userId).Count();
                    var respMyProject = _repository.Project.FindByCondition(x => x.user_id == respProjectCount.userId).Count();
                    var respMissedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == true  & x.assign_user_id == respProjectCount.userId).Count();
                    var respProposedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == false && x.assign_user_id == respProjectCount.userId).Count();
                    var projectCount = new ProjectCountDTO
                    {
                        TotalProject = respTotalProject,
                        MyProject = respMyProject,
                        MissedProject = respMissedProject,
                        ProposedProject = respProposedProject
                    };

                    return JsonConvert.SerializeObject(projectCount);
                }

                else
                {
                    var respTotalProject = _repository.Project.FindAll().Where(x => x.user_id == respProjectCount.userId && x.date > respProjectCount.fromDate && x.date < respProjectCount.toDate).Count();
                    var respMyProject = _repository.Project.FindByCondition(x => x.user_id == respProjectCount.userId && x.date > respProjectCount.fromDate && x.date < respProjectCount.toDate).Count();
                    var respMissedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == true & x.date > respProjectCount.fromDate && x.date < respProjectCount.toDate & x.assign_user_id== respProjectCount.userId).Count();
                   
                    var respProposedProject = _repository.UserProject.FindByCondition(x => x.is_default_submission == false & x.date > respProjectCount.fromDate && x.date < respProjectCount.toDate & x.assign_user_id == respProjectCount.userId).Count();
                    var projectCount = new ProjectCountDTO
                    {
                        TotalProject = respTotalProject,
                        MyProject = respMyProject,
                        MissedProject = respMissedProject,
                        ProposedProject = respProposedProject
                    };

                    return JsonConvert.SerializeObject(projectCount);
                }
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();

        }
        [HttpPost("SendDM")]
        public async Task<IActionResult> SendDM()
        {
            var resp = _repository.Project.FindByCondition(x => x.status == "Pending").OrderByDescending(x => x.date);

            List<string> myconlist = MessageHub.connectionlist;
            foreach (var item in myconlist)
            {
                await _messageHub.Clients.Client(item).SendAsync("getDM", $"{JsonConvert.SerializeObject(resp)} ");

            }
            return Ok();
        }

        [HttpPost("Update")]
        public async Task<Response> Update(Guid projectId, int userid, string generatedProposal, string proposal, string mainProposal)
        {
            var response = new Response();
            try
            {
                if (projectId != null && userid != null)
                {
                    var resp = _repository.Project.FindByCondition(x => x.project_id == projectId).FirstOrDefault();
                    var respProposal = _repository.Proposal.FindByCondition(x => x.project_id == projectId).FirstOrDefault();

                    var projectObj = new Project
                    {
                        project_id = resp.project_id,
                        name = resp.name,
                        desc = resp.desc,
                        date = resp.date,
                        is_deleted = resp.is_deleted,
                        status = "Submitted",
                        budget = resp.budget,
                        project_type = resp.project_type,
                        user_id = userid,
                        skills = resp.skills,
                        external_project_id = resp.external_project_id,
                        project_time = "02:00:00"
                    };

                    var proposalObj = new Proposal
                    {
                        proposal_id = respProposal.proposal_id,
                        project_id = projectId,
                        main_proposal = mainProposal,
                        date = respProposal.date,
                        proposal_date = DateTime.UtcNow,
                        last_updated = DateTime.UtcNow,
                        generated_proposal = generatedProposal,
                        user_id = userid

                    };
                    var userprojectObj = new UserProject
                    {
                        user_id = userid,
                        project_id = projectId,
                        date = DateTime.UtcNow,
                        is_processed = false,
                        is_default_submission = false,
                        assign_user_id = userid
                    };
                    var isExistPro = _repository.UserProject.FindByCondition(x => x.project_id == projectId).ToList();
                    if (isExistPro.Count > 0)
                    {
                        response.StatusCode = StatusType.Conflict;
                    }
                    else
                    {
                        Userproject(userprojectObj);
                        _repository.Proposal.Update(proposalObj);
                        _repository.Project.Update(projectObj);
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
        [HttpPost("UserShift")]
        public int UserShift()
        {
           
            try
            {
                var value = _repository.UserShift.FindAll().ToList();
                foreach(var item in value)
                {
                    var startShift = item.shift_start;
                    var endShift = item.shift_end;
                    var currentTime = DateTime.Now.TimeOfDay;
                    if (currentTime > startShift && currentTime < endShift)
                    {
                        return item.user_id;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return 0;
        }
            [HttpPost("UpdateWithDefault")]
        public async Task<Response> UpdateWithDefault(Guid projectId)
        {
            var response = new Response();
            try
            {
                var resp = _repository.Project.FindByCondition(x => x.project_id == projectId).FirstOrDefault();
                var respProposal = _repository.Proposal.FindByCondition(x => x.project_id == projectId).FirstOrDefault();
                var projectObj = new Project
                {
                    project_id = resp.project_id,
                    name = resp.name,
                    desc = resp.desc,
                    date = resp.date,
                    is_deleted = resp.is_deleted,
                    status = "Submitted",
                    budget = resp.budget,
                    project_type = resp.project_type,
                    user_id = resp.user_id,
                    skills = resp.skills,
                    external_project_id = resp.external_project_id,
                    project_time = "02:00:00"
                };
                var proposalObj = new Proposal
                {
                    proposal_id = respProposal.proposal_id,
                    project_id = projectId,
                    main_proposal = "Default Proposal",
                    proposal_date = DateTime.UtcNow,
                    last_updated = DateTime.UtcNow,
                    generated_proposal = "Default Proposal",
                    user_id = 6,
                    date=resp.date

                };
                var userprojectObj = new UserProject
                {
                    user_id = 6,
                    project_id = resp.project_id,
                    date = DateTime.UtcNow,
                    is_processed = false,
                    is_default_submission = true,
                    assign_user_id = resp.user_id
                };
                var isExistPro = _repository.UserProject.FindByCondition(x => x.project_id == projectId).ToList();
                if (isExistPro.Count > 0)
                {
                    response.StatusCode = StatusType.Conflict;
                }
                else
                {
                    Userproject(userprojectObj);
                    _repository.Project.Update(projectObj);
                    _repository.Proposal.Update(proposalObj);
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
        [HttpPost("UpdateProjectStatus")]
        public async Task<Response> UpdateProcessed(Guid projectId, string status)
        {
            var response = new Response();
            try
            {
                var isprocessedMsgExist = _repository.Project.FindByCondition(x => x.project_id == projectId & status == status).FirstOrDefault();
                if (isprocessedMsgExist != null)
                {
                    var projectObj = new Project
                    {
                        project_id = isprocessedMsgExist.project_id,
                        name = isprocessedMsgExist.name,
                        desc = isprocessedMsgExist.desc,
                        date = isprocessedMsgExist.date,
                        is_deleted = isprocessedMsgExist.is_deleted,
                        status = "Approved",
                        budget = isprocessedMsgExist.budget,
                        project_type = isprocessedMsgExist.project_type,
                        user_id = isprocessedMsgExist.user_id,
                        skills = isprocessedMsgExist.skills,
                        external_project_id = isprocessedMsgExist.external_project_id,
                        project_time = "02:00:00"
                    };
                    var respUpdate = _repository.Project.Update(projectObj);
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

        [HttpPost("UpdateProposal")]
        public async Task<Response> UpdateProposal([FromBody] UserProject userProject)
        {
            var response = new Response();
            try
            {

                var resp = _repository.UserProject.FindByCondition(x => x.is_processed == userProject.is_processed).ToList();
                foreach (var dbObj in resp)
                {
                    var projectID = dbObj.project_id;
                    ProUpdateProposal(projectID);
                }
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response;
        }
        [HttpPost("ProUpdateProposal")]
        public async void ProUpdateProposal(Guid projectID)
        {
            var response = new Response();
            try
            {
              var dbResp = (from projectlist in _repository.Project.FindByCondition(x => x.project_id == projectID).OrderByDescending(x=>x.date)
                              join myprojectlist in _repository.UserProject.FindByCondition(x => x.project_id == projectID)
                              on projectlist.project_id equals myprojectlist.project_id
                              join proposallist in _repository.Proposal.FindByCondition(x => x.project_id == projectID)
                              on projectlist.project_id equals proposallist.project_id
                              select new ProjectDTO()
                              {
                                  name = projectlist.name,
                                  date = projectlist.date,
                                  external_project_id = projectlist.external_project_id,
                                  user_project_id = myprojectlist.user_project_id,
                                  user_id = projectlist.user_id,
                                  project_id = projectlist.project_id,
                                  is_processed = myprojectlist.is_processed
                              }).FirstOrDefault();

                var project2Obj = new Project2()
                {
                    name = dbResp.name,
                    date = DateTime.UtcNow,
                    external_project_id = dbResp.external_project_id,
                    proposal = dbResp.generated_proposal
                };
                var dbProject = _repository.Project2.Create2(project2Obj);
                var userProject = new UserProject()
                {
                    user_project_id = (int)dbResp.user_project_id,
                    user_id = (int)dbResp.user_id,
                    project_id = projectID,
                    date = dbResp.date,
                    is_processed = true

                };
                var dbUserProject = _repository.UserProject.Update(userProject);

            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;

            }

        }
        [HttpPost("deleteProject")]
        public async Task<string> deleteProject(Guid projectid)
        {
            var response = new Response();
            try
            {
                 Project data = _repository.Project.FindByCondition(x=>x.project_id==projectid).FirstOrDefault();
                UserProject userProjdata = _repository.UserProject.FindByCondition(x => x.project_id == projectid).FirstOrDefault();
                Proposal userProposaldata = _repository.Proposal.FindByCondition(x => x.project_id == projectid).FirstOrDefault();
                if(data !=null)
                {
                    _repository.Project.Delete(data);
                }
                if (userProposaldata != null)
                {
                    _repository.Proposal.Delete(userProposaldata);
                }
                if (userProjdata != null)
                {
                    _repository.UserProject.Delete(userProjdata);
                }
                response.StatusCode = StatusType.Success;
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();
        }
        [HttpPost("Userproject")]
        public Task<string> Userproject([FromBody] UserProject userproject)
        {
            var data = _repository.UserProject.Create(userproject);
            return Task.Run(() =>
            {
                return JsonConvert.SerializeObject(data);
            });
        }
        [HttpPost("dashboardProjectlist")]
        public async Task<string> projectList(int? userId)
        {
            var response = new Response();
            try
            {
                var myporjectlist = (
                from projectlist in _repository.Project.FindAll().OrderByDescending(x=>x.date)
                join myuserlist in _repository.User.FindAll() on projectlist.user_id equals myuserlist.user_id
                select new ProjectDTO()
                {
                    project_id = projectlist.project_id,
                    name = projectlist.name,
                   
                    desc = projectlist.desc,
                   
                    username = myuserlist.username,
                    date = projectlist.date,
                    status = projectlist.status,
                    skills = projectlist.skills,
                  
                }).DistinctBy(p => new { p.project_id }).ToList();
                if (myporjectlist != null)
                {
                    return JsonConvert.SerializeObject(myporjectlist);
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
            return response.ToString();
        }

        [HttpPost("projectlist")]
        public async Task<string> projectlist(DateTime? fromDate, DateTime? toDate, int? userId)
        {
            var response = new Response();
            try
            {
              if (fromDate != null && toDate != null && userId != null)
              {
               var porjectlist = (
               from projectlist in _repository.Project.FindByCondition(x => x.date > fromDate && x.date < toDate && x.user_id == userId).OrderByDescending(x => x.date)
               join myuserlist in _repository.User.FindAll() on projectlist.user_id equals myuserlist.user_id
               select new ProjectDTO()
               {
                   project_id = projectlist.project_id,
                   name = projectlist.name,
                   desc = projectlist.desc,
                   username = myuserlist.username,
                   date = projectlist.date,
                   status = projectlist.status,
                   skills = projectlist.skills,

               }).DistinctBy(p => new { p.project_id }).ToList();
                   if (porjectlist != null)
                    {
                        return JsonConvert.SerializeObject(porjectlist);
                    }
                    else
                    {
                        response.StatusCode = StatusType.NotFound;
                    }
               }
                  else
                {
                    var data = _repository.Project.FindByCondition(x => x.status == "Pending").OrderByDescending(x => x.date);

                    if (data != null)
                    {
                        return JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        response.StatusCode = StatusType.NotFound;
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
        [HttpPost("changethreaduser")]
        public async Task<string> threadlist()
        {
            var response = new Response();
            try
            {
                var myporjectlist = (
             from threadlist in _repository.Thread.FindAll()
             join myuserlist in _repository.User.FindAll() 
             on threadlist.user_2 equals myuserlist.user_id
             join projectlist in _repository.Project.FindAll()
             on threadlist.project_id equals projectlist.project_id
             select new ThreadDTO()
             {
                 thread_id = threadlist.thread_id,
                 project_id = threadlist.project_id,
                 created_time = threadlist.created_date,
                 user_1 = threadlist.user_1,
                 user_2 = myuserlist.user_id,
                 username=myuserlist.username,
                 project_name=projectlist.name
                
             }).DistinctBy(p => new { p.project_id }).ToList();
                if (myporjectlist != null)
                {
                    return JsonConvert.SerializeObject(myporjectlist);
                }
                else
                {
                    response.StatusCode = StatusType.NotFound;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response.ToString();
        }
            [HttpPost("myprojectlist")]
        public async Task<string> myprojectlist(int userId)
        {
            var response = new Response();
            try
            {
                if (userId > 0)
                {
                    var myporjectlist = (
                    from projectlist in _repository.Project.FindByCondition(x => x.user_id == userId & x.status == "Submitted")
                    join myprojectlist in _repository.UserProject.FindByCondition(x => x.user_id == userId)
                    on projectlist.project_id equals myprojectlist.project_id orderby myprojectlist.date
                    join myuserlist in _repository.User.FindByCondition(x => x.user_id == userId)
                    on myprojectlist.user_id  equals myuserlist.user_id orderby myprojectlist.date
                    join proposallist in _repository.Proposal.FindByCondition(x => x.user_id == userId)
                    on myprojectlist.project_id equals proposallist.project_id orderby proposallist.proposal_date
                    select new ProjectDTO()
                    {
                        name = projectlist.name,
                        date = projectlist.date,
                        desc = projectlist.desc,
                        project_id = projectlist.project_id,
                        username = myuserlist.username,
                        proposal_date = proposallist.proposal_date,
                        client_country = projectlist.client_country,
                        project_value = projectlist.project_value,
                        client_reputation = projectlist.client_reputation,
                        main_proposal = proposallist.main_proposal,
                    }).DistinctBy(p => new { p.project_id }).ToList();
                    if (myporjectlist != null)
                    {
                        return JsonConvert.SerializeObject(myporjectlist);
                    }
                    else
                    {
                        response.StatusCode = StatusType.NotFound;
                    }
                }
                else
                {
                    var myporjectlist = (from projectlist in _repository.Project.FindByCondition(x => x.status == "Submitted").OrderByDescending(x=>x.date)
                                         join myprojectlist in _repository.UserProject.FindAll()
                                       on projectlist.project_id equals myprojectlist.project_id 
                                         join myuserlist in _repository.User.FindAll()
                                         on myprojectlist.user_id  equals myuserlist.user_id 
                                         join proposallist in _repository.Proposal.FindAll()
                                         on myprojectlist.project_id equals proposallist.project_id 
                                         select new ProjectDTO()
                                         {
                                             name = projectlist.name,
                                             date = projectlist.date,
                                             desc = projectlist.desc,
                                             project_id = projectlist.project_id,
                                             username = myuserlist.username,
                                             proposal_date = proposallist.proposal_date,
                                             client_country = projectlist.client_country,
                                             project_value = projectlist.project_value,
                                             client_reputation = projectlist.client_reputation,
                                             main_proposal = proposallist.main_proposal,

                                         }).DistinctBy(p => new { p.project_id }).ToList();
                    if (myporjectlist != null)
                    {

                        return JsonConvert.SerializeObject(myporjectlist);
                    }
                    else
                    {
                        response.StatusCode = StatusType.NotFound;

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

    }
}