using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace process_control_service.Controllers
{
    [Route("api/process-control/test")]
    [ApiController]
    [Authorize]
    public class ProcessControlController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public ProcessControlController(IConfiguration configuration,IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll(string userId,string systemTypeId)
        {
            User user = null;
            SystemType systemType = null;

            try
            {
                var userClient = this.httpClientFactory.CreateClient();
                userClient.BaseAddress = new Uri(this.configuration["RemoteServices:Default"]);
                var systemTypeClient = this.httpClientFactory.CreateClient();
                systemTypeClient.BaseAddress = new Uri(this.configuration["RemoteServices:Default"]);

                var userResponse = await userClient.GetAsync("/api/identity/user/getbyid?id=" + userId);
                var systemTypeResponse = await systemTypeClient.GetAsync("/api/base-setting/systemtype/getbyid?id=" + systemTypeId);

                if (userResponse.IsSuccessStatusCode)
                {
                    var userResponseStr = await userResponse.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(userResponseStr);
                }
                else
                {
                    throw new Exception("请求用户信息失败");
                }

                if (systemTypeResponse.IsSuccessStatusCode)
                {
                    var systemTypeResponseStr = await systemTypeResponse.Content.ReadAsStringAsync();
                    systemType = JsonConvert.DeserializeObject<SystemType>(systemTypeResponseStr);
                }
                else
                {
                    throw new Exception("请求SystemType失败");
                }
            }
            catch (Exception ex) 
            {
                throw;
            }

            return Ok(new { UserId=user.Id,UserName=user.UserName,SystemTypeId=systemType.Id,SystemTypeName=systemType.SystemTypeName});
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    public class SystemType
    {
        public string Id { get; set; }
        public string SystemTypeName { get; set; }
    }
}
