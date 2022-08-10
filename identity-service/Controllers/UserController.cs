using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace identity_service.Controllers
{
    [Route("api/identity/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly IList<User> users = new List<User>() { new Controllers.User { Id="1",UserName="admin"}, new Controllers.User { Id = "2", UserName = "hongyan" } };
        private readonly IConfiguration configuration;
        private readonly MyDbContext dbContext;

        public UserController(IConfiguration configuration,MyDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(string userName)
        {
            var user=new User();
            user.Id = Guid.NewGuid().ToString();
            user.UserName= userName;
            //using (var connection=new MySqlConnection(this.configuration["Db"]))
            //{
            //    connection.Open();
            //    string sql = $"insert into Users values('{user.Id}','{user.UserName}')";
            //    MySqlCommand cmd = new MySqlCommand(sql,connection);
            //    cmd.ExecuteNonQuery();

            //    connection.Close();
            //}

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return Ok();
        }

        [Route("getbyid")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            //User user = null;
            //using (var connection = new MySqlConnection(this.configuration["Db"]))
            //{
            //    connection.Open();
            //    string sql = $"select * from Users where id='{id}'";
            //    MySqlCommand cmd = new MySqlCommand(sql,connection);
            //    MySqlDataAdapter da = new MySqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    DataSet ds = new DataSet();
            //    da.Fill(ds);

            //    DataTable tb = ds.Tables[0];
            //    if(tb.Rows.Count > 0)
            //    {
            //        user = new User();
            //        user.Id = tb.Rows[0]["Id"].ToString();
            //        user.UserName = tb.Rows[0]["UserName"].ToString();
            //    }

            //    connection.Close();
            //}

            User user = this.dbContext.Users.FirstOrDefault(o => o.Id == id);

            if (user == null)
            {
                return NotFound(new { Message="根据Id找不到用户"});
            }

            return Ok(user);
        }
    }

    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
