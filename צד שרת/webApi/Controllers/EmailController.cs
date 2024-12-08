using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dto;
using Bll;
using System.Web.Http.Cors;

namespace webApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailController : ApiController
    {
        // GET: api/Email
        public List<EmailDto> Get()
        {
            return null;
        }

        // GET: api/Email/5
        public List<EmailDto> Get(string mail)
        {
            return null;
        }



        //מקבל אובייקט משתמש
        //מחזיר מיילים של משתמש מסוים 
        [HttpPost]
        [Route("api/Email/GetEmails")]
        public List<EmailDto> GetEmails([FromBody] UserDto user)
        {
            //אם הגיע מייל ריק
            if (user.mail == "" || user.mail == null)
                return null;
            //החזר את כל המיילים השייכים למשתמש זה
            if (ClassDB.GetUserByMail(user.mail) != null)
            {
                return ClassDB.GetEmailsByUser(ClassDB.GetUserByMail(user.mail).userId);
            }
            return null;
        }


        // POST: api/Email
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Email/5
        public void Put(int id, [FromBody] string value)
        {


        }

        // DELETE: api/Email/5
        public void Delete(int id)
        {
        }
    }
}
