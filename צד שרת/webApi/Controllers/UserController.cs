using Bll;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace webApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        //מקבל פרטי משתמש
        //מחברת את המשתמש למערכת
        [HttpPost]
        [Route("api/User/Login")]
        public Validation Login([FromBody] UserDto user)
        {
            //התחברות של משתמש חדש
            return ClassDB.Login(user);
        }


        [HttpPost]
        [Route("api/User/CheckConection")]
        public string CheckConection()
        {
            return "Success";
        }

        // POST: api/User
        public void Post([FromBody]UserDto user)
        {
            ClassDB.AddUser(user);
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
