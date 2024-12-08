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

    public class SubjectController : ApiController
    {
        // GET: api/Subject
        public List<SubjectDto> Get()
        {
            return null;
        }


        //שליפת כל הנושאים של משתמש מסוים לפי המייל שלו
        [HttpPost]
        [Route("api/Subject/GetSubjects")]
        public List<SubjectDto> GetSubjects([FromBody] UserDto user)
        {
            //בודק אם יש בעיה במה שנשלח לפונקציה
            if (user.mail == "" || user.mail == null || ClassDB.GetUserByMail(user.mail) == null)
                return null;
            //שליפת כל המיילים של משתמש מסוים
            return ClassDB.GetAllSubjectsByUserId(ClassDB.GetUserByMail(user.mail).userId);
        }


        //שליפת כל הנושאים של המשתמש לפי הקוד שלו בדאטה
        [HttpPost]
        [Route("api/Subject/GetSubjectsByUserId")]
        public List<SubjectDto> GetSubjectsByUserId([FromBody] UserDto user)
        {
            //אם אותו משתמש לא נמצא בדאטה
            if (ClassDB.GetAllUsers().FirstOrDefault(x => x.userId == user.userId) == null)
                return null;
            return ClassDB.GetAllSubjectsByUserId(user.userId);
        }


        // מקבלת קוד אימייל בדאטה ונושא חדש
        // מעדכנת למייל את הנושא החדש 
        [HttpPost]
        [Route("api/Subject/ChangeEmailSubject")]
        public string ChangeEmailSubject([FromBody] ChangeSubject details)
        {
            //בודק אם יש בעיה משתנים שנשלחו 
            if (details.newSubject == "" || details.newSubject == null || details.emailId == 0)
                return "Error";
            //עידכון נושא למייל מסוים
            ClassDB.UpdateEmailSubject(details.emailId, details.newSubject);
            return "Success";
        }


        //מקבל אוביקט מסוג שינוי צבע
        //מעדכן את הנושא בצבע החדש
        [HttpPost]
        [Route("api/Subject/ChangeSubjectColor")]
        public string ChangeSubjectColor([FromBody] ChangeColor details)
        {
            //בודק אם יש בעיה משתנים שנשלחו 
            if (details.subjectId == null || details.newColor == "")
                return "Error";
            //עידכון צבע לנושא מסוים
            ClassDB.UpdateSubjectColor((int)details.subjectId, details.newColor);
            return "Success";
        }

        //מקבל קוד נושא
        //מוחק את הנושא מהדאטה
        [HttpPost]
        [Route("api/Subject/DeleteSubject")]
        public string DeleteSubject([FromBody] ChangeColor details)
        {
            //בודק אם יש בעיה משתנים שנשלחו 
            if (details.subjectId == null)
                return "Error";
            //מחיקת הנושא
            ClassDB.DeleteSubject((int)details.subjectId);
            return "Success";
        }


        //מקבל פרטי נושא
        //מוסיף את הנושא למשתמש
        [HttpPost]
        [Route("api/Subject/AddSubject")]
        public string AddSubject([FromBody] Subject sub)
        {
            UserDto user = ClassDB.GetUserByMail(sub.userMail);
            if (user != null)
            {
                SubjectDto subjectDto = new SubjectDto();
                subjectDto.subjectName = sub.subjectName;
                subjectDto.parentID = ClassDB.GetSubjectIdBySubAndUser(sub.parentName, user.userId);
                subjectDto.userId = user.userId;
                subjectDto.color = sub.color;
                ClassDB.AddSubject(subjectDto);
                return "Success";
            }
            return "Error";
        }



        // GET: api/Subject/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Subject
        public void Post(SubjectDto s)
        {
            ClassDB.AddSubject(s);
        }

        // PUT: api/Subject/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Subject/5
        public void Delete(int id)
        {
            ClassDB.DeleteSubject(id);
        }



    }


    public class ChangeSubject
    {
        public int emailId { get; set; }
        public string newSubject { get; set; }
        public ChangeSubject(int e, string s)
        {
            emailId = e;
            newSubject = s;
        }
    }
    public class ChangeColor
    {
        public string newColor { get; set; }
        public int? subjectId { get; set; }

        public ChangeColor(string c, int s)
        {
            newColor = c;
            subjectId = s;
        }
    }

    public class Subject
    {
        public string subjectName { get; set; }
        public string parentName { get; set; }
        public string userMail { get; set; }
        public string color { get; set; }

    }
}