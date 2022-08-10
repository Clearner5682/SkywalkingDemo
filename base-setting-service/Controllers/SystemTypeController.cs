using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace base_setting_service.Controllers
{
    [Route("api/base-setting/systemtype")]
    [ApiController]
    public class SystemTypeController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public SystemTypeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(string id, string systemTypeName)
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(this.configuration["Redis"]);
            var db = redis.GetDatabase();

            HashEntry[] hashEntries = new HashEntry[2];
            hashEntries[0] = new HashEntry("Id", id);
            hashEntries[1] = new HashEntry("SystemTypeName", systemTypeName);

            db.HashSet($"SystemType:{id}", hashEntries);

            return Ok();
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<IActionResult> GetById(string id)
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(this.configuration["Redis"]);
            var db = redis.GetDatabase();

            var hashEntity = db.HashGetAll($"SystemType:{id}");

            SystemType systemType = null;
            if (hashEntity != null && hashEntity.Length > 0)
            {
                systemType = new SystemType();
                systemType.Id = hashEntity.FirstOrDefault(o => o.Name.ToString() == "Id").Value.ToString();
                systemType.SystemTypeName = hashEntity.FirstOrDefault(o => o.Name.ToString() == "SystemTypeName").Value.ToString();
            }

            if (systemType == null)
            {
                return NotFound(new { Message = "根据此Id找不到SystemType" });
            }

            //return Ok(new SystemType { Id=id,SystemTypeName="Test"});

            return Ok(systemType);
        }
    }

    public class SystemType
    {
        public string Id { get; set; }
        public string SystemTypeName { get; set; }
    }
}
