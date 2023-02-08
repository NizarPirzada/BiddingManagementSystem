using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MissionControl.DataTransferObject.Application;
using Newtonsoft.Json;
using NLog;
using PanacealogicsSales.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        protected readonly IHubContext<MessageHub> _messageHub;
        public static Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public HomeController(IRepositoryWrapper repository, [NotNull] IHubContext<MessageHub> messageHub)
        {
            _repository = repository;
            _messageHub = messageHub;
        }

        [HttpPost("Authenticate")]
        public Task<string> Authenticate([FromBody] User user)
        {

            var data = _repository.User.FindByCondition(x => x.username.Equals(user.username) && x.password.Equals(user.password)).FirstOrDefault();

            if (data != null)
            {

                var value = new { userid = data.user_id, roleid = data.role_id };
                var json = JsonConvert.SerializeObject(value);
                return Task.FromResult(json);

            }
            else
            {
                return Task.FromResult("0");
            }

        }

        [HttpPost("Save")]
        public async void Save([FromBody] Product product)
        {
            var response = new Response();
            try
            {
                var data = _repository.Product.Create(product);
                await SendDM(product);
                response.StatusCode = StatusType.Success;
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
        }

        [HttpPost("SendDM")]
        public async Task<string> SendDM([FromBody] Product product)
        {
            var response = new Response();
            try
            {
                List<string> myconlist = MessageHub.connectionlist;
                foreach (var item in myconlist)
                {
                    await _messageHub.Clients.Client(item).SendAsync("getDM", $"{product.name},{product.desc},{product.amount},{product.date}");

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
        [HttpGet("Skillslist")]
        public async Task<string> Skillslist()
        {
            var response = new Response();
            try
            {
                var data = _repository.Skill.FindAll().Select(x => x.name).ToList();
                if (data != null)
                {
                    return JsonConvert.SerializeObject(data);
                }
                else
                {
                    return data.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error("logging Error", ex);
                response.StatusCode = StatusType.ServerError;
            }
            return response.ToString();

        }

        [HttpPost("Getlist")]
        public Task<string> Getlist([FromBody] Product account)
        {

            var data = _repository.Product.FindByCondition(x => x.amount.Equals(account.amount));
            if (data != null)
            {

                return Task.Run(() =>
               {
                   return JsonConvert.SerializeObject(data);
               });

            }
            else
            {
                return Task.Run(() =>
                {
                    return data.ToString();
                });
            }

        }
    }
}
