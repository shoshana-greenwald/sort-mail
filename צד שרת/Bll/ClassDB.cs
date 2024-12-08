using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
using Bll.BuildStructs;


namespace Bll
{
    public static partial class ClassDB
    {

        //static TextClassificationProjectEntities db = new TextClassificationProjectEntities();

        public static Validation Login(UserDto user)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                Validation v = new Validation();
                foreach (User_tbl u in db.User_tbl)
                {
                    if (u.userPassword == user.userPassword && u.mail == user.mail)
                    {
                        v.User = UserDto.DalToDto(u);
                        v.Status = 200;
                        return v;
                    }
                    if (u.mail == user.mail)
                        v.ExistMail = true;

                }
                return v;
            }
        }

        //select
        //===========================================================================


        #region words
        //מחזירה את כל המילים שבמאגר  
        public static List<WordDto> GetAllWords()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<WordDto> lst = new List<WordDto>();
                foreach (var item in db.Word_tbl.ToList())
                {
                    lst.Add(WordDto.DalToDto(item));
                }
                return lst;
            }
        }



        //מקבלת קוד מילה
        //מחזירה את המילה
        public static WordDto GetWordByID(int id)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.Word_tbl.ToList())
                {
                    if (item.wordId == id)
                    {
                        return (WordDto.DalToDto(item));
                    }
                }
                return null;
            }
        }

        //מקבלת מחרוזת של מילה
        //מחזירה את הקוד שלה
        public static int GetIDByWord(string word)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                Word_tbl w = db.Word_tbl.ToList().FirstOrDefault(item => item.wordName == word);
                if (w != null)
                    return w.wordId;
                return -1;
            }
        }

        #endregion

        #region wordForSubjet


        //מחזירה את כל המילים הקשורות לנושאים
        public static List<WordForSubjectDto> GetAllWordForSubject()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<WordForSubjectDto> lst = new List<WordForSubjectDto>();
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    lst.Add(WordForSubjectDto.DalToDto(item));
                }
                return lst;
            }
        }
        //מחזירה את כל המילים הקשורות לנושא מסוים
        public static List<WordForSubjectDto> GetCommonWordsForSubject(int subId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<WordForSubjectDto> lst = new List<WordForSubjectDto>();
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.subjectId == subId && item.isCommon)
                        lst.Add(WordForSubjectDto.DalToDto(item));
                }
                return lst;
            }
        }
        //מחזירה את המשקל הכי נמוך שקיים במערכת
        public static double GetMinWeightForWord()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                double minWeight = Double.MaxValue;
                if (db.WordForSubject_tbl.ToList().Count == 0) return 1;
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.wordWeight < minWeight)
                        minWeight = (double)item.wordWeight;
                }
                return minWeight;
            }
        }

        //מקבלת קוד מילה וקוד נושא
        //מחזיר את המילה שקשורה לנושא
        public static WordForSubjectDto GetWordForSubjectByWordAndSub(int wordID, int subID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                foreach (WordForSubject_tbl item in db.WordForSubject_tbl.ToList().Where(item => item.wordId == wordID && item.subjectId == subID))
                {
                    return (WordForSubjectDto.DalToDto(item));
                }

                return null;
            }
        }
        //מקבלת קוד מילה וקוד נושא
        //מחזיר את משקל המילה שקשורה לנושא
        public static double? GetWeightWordForSubjectByWordAndSub(int subID, int wordID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.wordId == wordID && item.subjectId == subID)
                    {
                        return item.wordWeight;
                    }
                }
                return null;
            }
        }

        //מקבלת מילון נושאים של משתמש
        //מחזירה מילון של כל הקשירויות עם כל הנושאים
        public static Dictionary<int, List<WordForSubjectDto>> GetWordsForSubjectByListOfSubs(Dictionary<int, SubjectDto> subjects)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                Dictionary<int, List<WordForSubjectDto>> dict = new Dictionary<int, List<WordForSubjectDto>>();

                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (subjects.ContainsKey(item.subjectId))
                    {
                        if (!dict.ContainsKey(item.wordId))
                            dict.Add(item.wordId, new List<WordForSubjectDto>());
                        dict[item.wordId].Add(WordForSubjectDto.DalToDto(item));
                    }
                }
                return dict;
            }
        }

        //מקבלת קוד מילה
        //מחזירה רשימה של כל הקשירויות שלה עם כל הנושאים
        public static List<WordForSubjectDto> GetWordsForSubjectByWord(int wordID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<WordForSubjectDto> lst = new List<WordForSubjectDto>();
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.wordId == wordID)
                    {
                        lst.Add(WordForSubjectDto.DalToDto(item));
                    }
                }
                return lst;
            }
        }
        //מקבלת נושא
        //מחזירה את סך כל המילים שקשורות לנושא
        public static int GetTotalWordsForSub(int subID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                int count = 0;
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.subjectId == subID)
                        count++;
                }
                return count;
            }
        }
        //מקבלת קוד מילה וקוד נושא
        //מחזירה משתנה באוביקט כמה המילה היתה בנושא המסוים וכמה היא לא הופיעה
        public static Statistic GetStatisForWordBySub(int subId, int wordId)
        {//מחזיר כמה המילה נמצאת בנושא וכמה לא
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                WordForSubjectDto wfs = GetWordForSubjectByWordAndSub(wordId, subId);
                int yes;
                if (wfs != null)
                    yes = (int)wfs.wordWeight;
                else yes = 0;
                int no = 0;
                foreach (var item in db.WordForSubject_tbl.ToList())
                {
                    if (item.subjectId == subId && item.wordId != wordId)
                        no += (int)item.wordWeight;
                }
                return new Statistic(subId, yes, no);
            }
        }
        //מקבלת מילה
        //מחזירה מילון בו עבור כל נושא יש אוביקט המכיל כמה המילה הופיעה בו וכמה לא
        public static Dictionary<int, Statistic> GetStatisForWord(int wordId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                Dictionary<int, Statistic> dict = new Dictionary<int, Statistic>();
                foreach (var item in db.Subject_tbl.ToList())
                {
                    dict.Add(item.subjectId, GetStatisForWordBySub(item.subjectId, wordId));
                }
                return dict;
            }
        }

        #endregion

        #region subjects


        //מחזירה את כל הנושאים של משתמש מסוים
        public static List<SubjectDto> GetAllSubjectsByUserId(int userId)
        {
            using (TextClassificationProjectEntities db1 = new TextClassificationProjectEntities())
            {

                List<SubjectDto> lst = new List<SubjectDto>();
                foreach (var item in db1.Subject_tbl.ToList())
                {
                    if (item.userId == userId)
                        lst.Add(SubjectDto.DalToDto(item));
                }

                return lst;
            }
        }

        //מקבלת קוד משתמש
        //מחזירה מילון של כל הנושאים שלו
        public static Dictionary<int, SubjectDto> GetDictOfAllSubjectsByUserId(int userId)
        {
            using (TextClassificationProjectEntities db1 = new TextClassificationProjectEntities())
            {

                Dictionary<int, SubjectDto> lst = new Dictionary<int, SubjectDto>();
                foreach (var item in db1.Subject_tbl.ToList())
                {
                    if (item.userId == userId)
                        lst.Add(item.subjectId, SubjectDto.DalToDto(item));
                }

                return lst;
            }
        }
        //פונקציה המקבלת קוד משתמש 
        //ומחזירה מילון בו עבור כל נושא של המשתמש יש את מספר ילדיו
        public static Dictionary<int, int> GetDictOfNumOfChildren(int userId)
        {
            //שליפת כל הנושאים של המשתמש
            List<SubjectDto> subjects = GetAllSubjectsByUserId(userId);
            Dictionary<int, int> NumOfChildren = new Dictionary<int, int>();
            //איתחול מילון עבור כל נושא
            foreach (SubjectDto subject in subjects)
                NumOfChildren.Add(subject.subjectId, 0);
            //עבור כל נושא מעלים אחד בנושא של אבא שלו 
            foreach (SubjectDto subject in subjects)
                if (subject.parentID != null)
                    NumOfChildren[(int)subject.parentID]++;
            return NumOfChildren;
        }

        //הפונקציה מקבלת קוד נושא ומילון בו עבור כל נושא יש את מספר ילדיו
        //הפונקציה מחזירה את מספר צאצאיו של אותו נושא שהתקבל
        public static int GetNumOfChildrenByPID(int sub, Dictionary<int, int> numOfChildren)
        {
            SubjectDto subject = GetSubjectByID(sub);
            if (subject == null) return 0;
            List<SubjectDto> children = GetSubjectsByParentId(sub);
            if (children.Count == 0) return 0;
            int sum = 0;
            foreach (var item in children)
            {
                sum += (numOfChildren[item.subjectId] + GetNumOfChildrenByPID(item.subjectId, numOfChildren));
            }
            return numOfChildren[sub] + sum;
        }


        //מקבלת קוד נושא
        //מחזירה את הנושא
        public static SubjectDto GetSubjectByID(int? sub)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                if (sub == null) return null;
                foreach (var item in db.Subject_tbl.ToList())
                {
                    if (item.subjectId == sub)
                    {
                        return (SubjectDto.DalToDto(item));
                    }
                }
                return null;
            }
        }

        //מקבלת שם נושא
        //מחזירה את קוד הנושא
        public static int? GetSubjectIdBySubAndUser(string sub, int userId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.Subject_tbl.ToList())
                {
                    if (item.subjectName == sub && item.userId == userId)
                    {
                        return (SubjectDto.DalToDto(item)).subjectId;
                    }
                }
                return null;
            }
        }
        //מקבלת קוד נושא
        //מחזירה רשימה של קודים של הבנים שלו
        public static List<int> GetSubjectsIDByParentId(int pID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<int> lst = new List<int>();
                foreach (var item in db.Subject_tbl.ToList())
                {
                    if (item.parentID == pID)
                    {
                        lst.Add(item.subjectId);
                    }
                }

                return lst;
            }
        }
        //מקבלת קוד נושא
        //מחזירה רשימה של הבנים שלו
        public static List<SubjectDto> GetSubjectsByParentId(int pID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<SubjectDto> lst = new List<SubjectDto>();
                foreach (var item in db.Subject_tbl.ToList())
                {
                    if (item.parentID == pID)
                    {
                        lst.Add(SubjectDto.DalToDto(item));
                    }
                }

                return lst;
            }
        }
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        //מחזירה מילון בו לכל נושא יש קוד וקוד אביו
        //המילון ממוין בסדר עולה לפי רמת הנושא בעץ הכללי
        public static Dictionary<int, int?> GetSubjectDict(int userId)
        {//מחזיר ממוין לפי רמות קוד נושא וקוד אבא
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                Dictionary<int, int?> dict = new Dictionary<int, int?>();

                foreach (var item in db.Subject_tbl.ToList())
                {
                    if (item.userId == userId)
                        dict.Add(item.subjectId, GetSubjectLevel(item.subjectId));
                }

                Dictionary<int, int?> dict2 = dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int?> sub in dict2)
                {
                    dict[sub.Key] = GetSubjectByID(sub.Key).parentID;
                }
                return dict;

            }
        }
        //מקבלת קוד נושא
        //מחזירה את רמתו של הנושא בעץ הכללי
        public static int GetSubjectLevel(int id)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                int Level = 0;
                int? pId = GetSubjectByID(id).parentID;
                //מציאת רמת הנושא
                while (pId != null)
                {
                    Level++;
                    pId = GetSubjectByID((int)pId).parentID;
                }
                return Level;
            }
        }
        #endregion

        #region contactForSub


        //מקבל קוד איש קשר ורשימת נושאים של משתמש
        //מחזיר את כל הנושאים של האיש קשר שהתקבל קשור אליהם
        public static List<ContactForSubjectDto> GetContactsForSubsByContact(int conID, Dictionary<int, SubjectDto> subjects)
        {
            List<ContactForSubjectDto> lst = new List<ContactForSubjectDto>();
            if (conID == -1) return lst;
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                foreach (var item in db.ContactForSubject_tbl.ToList())
                {
                    if (item.contactManID == conID && subjects.ContainsKey(item.subjectId))
                    {
                        lst.Add(ContactForSubjectDto.DalToDto(item));
                    }
                }
                return lst;
            }
        }
        //מקבלת קוד איש קשר ונושא
        //מחזיר את האיש קשר לנושא
        public static ContactForSubjectDto GetContactForSubByIdAndSub(int conID, int sub)
        {

            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.ContactForSubject_tbl.ToList())
                {
                    if (item.contactManID == conID && item.subjectId == sub)
                    {
                        return ContactForSubjectDto.DalToDto(item);
                    }
                }
                return null;
            }
        }
        #endregion

        #region contactMan


        //מקבל כתובת איש קשר
        //מחזיר את הקוד שלו
        public static int GetcontactIdByEmail(string address)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.ContactMan_tbl.ToList())
                {
                    if (item.email.Equals(address))
                    {
                        return (ContactManDto.DalToDto(item)).contactManId;
                    }
                }
                return -1;
            }
        }

        #endregion

        #region emails


        // מחזירה את כל המיילים שבמאגר  
        public static List<EmailDto> GetAllEmails()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<EmailDto> lst = new List<EmailDto>();
                foreach (var item in db.Email_tbl.ToList())
                {
                    lst.Add(EmailDto.DalToDto(item));
                }
                return lst;
            }

        }
        // מחזירה את כל המיילים שבמאגר של משתמש מסוים  
        public static List<EmailDto> GetEmailsByUser(int userID)
        {
            using (TextClassificationProjectEntities db1 = new TextClassificationProjectEntities())
            {

                List<EmailDto> lst = new List<EmailDto>();
                foreach (var item in db1.Email_tbl.ToList())
                {
                    if (item.toUserId == userID)
                        lst.Add(EmailDto.DalToDto(item));
                }
                return lst;
            }
        }
        //מחזירה אוביקט מייל לפי קוד משתמש וקוד המייל באאוטלוק
        public static EmailDto GetEmailByUserAndOutlookId(int userId, string emailId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<EmailDto> mails = GetAllEmails();
                foreach (EmailDto mail in mails)
                {
                    if (mail.IdInOutLook == emailId && mail.toUserId == userId)
                        return mail;
                }
                return null;
            }

        }
        //מחזירה אוביקט מייל לפי קוד
        public static EmailDto GetEmailById(int id)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<EmailDto> mails = GetAllEmails();
                foreach (EmailDto mail in mails)
                {
                    if (mail.mailID == id)
                        return mail;
                }
                return null;
            }
        }

        //מחזירה את מספר המיילים הקיימים במאגר
        public static int GetEmailAmount()
        {
            return GetAllEmails().Count;
        }
        #endregion

        #region UpdateKeySentence

        public static List<UpdateKeySentenceDto> GetWordsLstToUpdateKeySentenceByMailId(int mailId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<UpdateKeySentenceDto> lst = new List<UpdateKeySentenceDto>();
                foreach (var line in db.UpdateKeySentence_tbl.ToList())
                {
                    if (line.mailID == mailId)
                        lst.Add(UpdateKeySentenceDto.DalToDto(line));
                }
                return lst;

            }
        }

        public static List<UpdateKeySentenceForSemilarDto> GetSemilarWordsLstToUpdateKeySentenceByMailId(int mailId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<UpdateKeySentenceForSemilarDto> lst = new List<UpdateKeySentenceForSemilarDto>();
                foreach (var line in db.UpdateKeySentenceForSemilar_tbl.ToList())
                {
                    if (line.mailID == mailId)
                        lst.Add(UpdateKeySentenceForSemilarDto.DalToDto(line));
                }
                return lst;

            }
        }

        #endregion

        #region updateMailSubject

        public static List<UpdateSubjectDto> GetListForUpdateSubject(int mailId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<UpdateSubjectDto> lst = new List<UpdateSubjectDto>();
                foreach (UpdateSubject_tbl line in db.UpdateSubject_tbl)
                {
                    if (line.mailID == mailId)
                        lst.Add(UpdateSubjectDto.DalToDto(line));
                }
                return lst;
            }
        }
        #endregion

        #region user

        //פונקציה המחזירה את כל המשתמשים הרשומים במערכת
        public static List<UserDto> GetAllUsers()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                List<UserDto> lst = new List<UserDto>();
                foreach (User_tbl line in db.User_tbl)
                {
                    lst.Add(UserDto.DalToDto(line));
                }
                return lst;
            }
        }
        public static UserDto GetUserByMail(string mail)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
                return UserDto.DalToDto(db.User_tbl.FirstOrDefault(line => line.mail == mail));

        }
        #endregion

        #region algorithmMistake

        //מקבלת מילה ונושא חדש וישן
        //מחזירה את הרשומה המתאימה
        public static AlgorithmMistakeDto GetMistakeByWordAndTwoSubjets(string word, int? oldSub, int newSub)
        {
            if (oldSub == null) return null;
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.AlgorithmMistake_tbl.ToList())
                {
                    if (item.word == word && item.newSubject == newSub && item.oldSubject == oldSub)
                    {
                        return (AlgorithmMistakeDto.DalToDto(item));
                    }
                }
                return null;
            }
        }

        #endregion
      
        
        
        //מקבלת מילה
        //מחזירה את מילת הבסיס שלה
        public static string GetBaseWordByWord(string word)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                foreach (var item in db.BaseWord_tbl.ToList())
                {
                    if (item.word == word)
                    {
                        return item.@base;
                    }
                }
                return null;
            }
        }

        #region baseWord



        #endregion




        //insert
        //===========================================================================

        //הוספת מילה
        public static void AddWord(WordDto w)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                int wordId = GetIDByWord(w.wordName);
                //אם המילה אינה קיימת
                if (wordId == -1)
                {
                    db.Word_tbl.Add(w.DtoToDal());
                    db.SaveChanges();
                }
            }
        }

        // הוספת מילת בסיס
        public static void AddBaseWord(BaseWordDto baseWord)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                string bw = GetBaseWordByWord(baseWord.word);
                //אם המילה אינה קיימת
                if (bw == null)
                {
                    db.BaseWord_tbl.Add(baseWord.DtoToDal());
                    db.SaveChanges();
                }
            }
        }


        //הוספת נושא למשתמש
        public static void AddSubject(SubjectDto s)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                //בדיקה האם הנושא קיים כבר
                int? subId = GetSubjectIdBySubAndUser(s.subjectName, s.userId);
                if (subId == null)
                {
                    //הוספת הנושא
                    db.Subject_tbl.Add(s.DtoToDal());
                    db.SaveChanges();
                    //מילון של מילים נרדפות ואחוז הדימיון שלהם לנושא
                    Dictionary<string, double> semilarWords = HebrewNlp.NlpForSimilarWords(Synonyms.GetSimilarWords(s.subjectName));
                    //מציאת משקל מינימלי של מילה לנושא במאגר
                    double minWeight = GetMinWeightForWord();
                    if (minWeight < 2) minWeight = 1;
                    subId = GetSubjectIdBySubAndUser(s.subjectName, s.userId);
                    //מעבר על כל המילים הנרדפות
                    foreach (KeyValuePair<string, double> item in semilarWords)
                    {
                        int wordId = GetIDByWord(item.Key);
                        //אם המילה אינה קיימת
                        if (wordId == -1)
                        {
                            AddWord(new WordDto() { wordName = item.Key }); //הוספת המילה למאגר
                            db.SaveChanges();
                            wordId = GetIDByWord(item.Key);
                        }
                        //בניית אוביקט מילה לנושא
                        WordForSubjectDto wfs = new WordForSubjectDto();
                        wfs.subjectId = (int)subId;
                        wfs.wordId = wordId;
                        wfs.isCommon = false;
                        //הכפלה פי שתיים של הציון המינימלי כדי שההכפלה בדימיון לא תוריד את המינימום
                        wfs.wordWeight = minWeight * 2 * item.Value / 100;
                        //הכנסה למאגר
                        AddWordForSubject(wfs);
                    }
                    db.SaveChanges();
                }
            }
        }

        //הוספת מילה לנושא
        public static void AddWordForSubject(WordForSubjectDto w)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.WordForSubject_tbl.Add(w.DtoToDal());
                db.SaveChanges();
            }
        }
        //הוספת איש קשר
        public static void AddContactMan(ContactManDto c)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.ContactMan_tbl.Add(c.DtoToDal());
                db.SaveChanges();
            }
        }
        //הוספת איש קשר לנושא
        public static void AddContactForSubject(ContactForSubjectDto c)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.ContactForSubject_tbl.Add(c.DtoToDal());
                db.SaveChanges();
            }
        }
        //הוספת מייל
        public static void AddEmail(EmailDto c)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.Email_tbl.Add(c.DtoToDal());
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }
        //הוספת משתמש
        public static void AddUser(UserDto u)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.User_tbl.Add(u.DtoToDal());
                db.SaveChanges();
                //איתחול נתונים בשביל משתמש חדש
                //שיש לו 0 מיילים ו-0 מילים
                int userId = GetUserByMail(u.mail).userId;
                EmailsData.DataDict.Add(userId, new Data(0, 0));
            }
        }

        //הוספת טעות לאלגוריתם
        public static void AddAlgorithmMistake(AlgorithmMistakeDto a)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.AlgorithmMistake_tbl.Add(a.DtoToDal());
                db.SaveChanges();
            }
        }


        //####### update tables ########

        //הוספה לטבלת עידכון נושא המייל
        public static void AddUpdateSubject(UpdateSubjectDto us)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.UpdateSubject_tbl.Add(us.DtoToDal());
                db.SaveChanges();
            }
        }

        //הוספה לטבלת עידכון משפט מפתח
        public static void AddUpdateKeySentence(UpdateKeySentenceDto uks)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.UpdateKeySentence_tbl.Add(uks.DtoToDal());
                db.SaveChanges();
            }
        }


        //הוספה לטבלת עידכון משפט מפתח למילים נרדפות
        public static void AddUpdateKeySentenceForSemilar(UpdateKeySentenceForSemilarDto uksfs)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                db.UpdateKeySentenceForSemilar_tbl.Add(uksfs.DtoToDal());
                db.SaveChanges();
            }
        }



        //update
        //===========================================================================

        //מקבלת קוד מילה וקוד נושא ומשקל
        //מעדכנת את המשקל למילה של הנושא
        public static void UpdateWeightWordForSubject(int wordId, int subId, double weight)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                WordForSubject_tbl wordForSubject = db.WordForSubject_tbl.FirstOrDefault(x => x.wordId == wordId && x.subjectId == subId);
                wordForSubject.wordWeight = weight;
                db.SaveChanges();
            }
        }


        //מקבלת קוד איש קשר וקוד נושא ומשקל
        //מעדכנת את המשקל לאיש קשר של הנושא
        public static void UpdateWeightContactForSubject(int conId, int subId, double weight)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                ContactForSubject_tbl conForSubject = db.ContactForSubject_tbl.FirstOrDefault(x => x.contactManID == conId && x.subjectId == subId);
                conForSubject.contactWeight = weight;
                db.SaveChanges();
            }
        }


        //מקבלת קוד טעות אלגוריתם ומספר הטעויות
        //מעדכנת את מספר הטעויות לרשומה המתאימה לקוד המתקבל
        public static void UpdateAmountAlgorithmMistakes(int mistakeId, int newAmount)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                AlgorithmMistake_tbl algorithmMistake = db.AlgorithmMistake_tbl.FirstOrDefault(x => x.id == mistakeId);
                algorithmMistake.AmountOfMistakes = newAmount;
                db.SaveChanges();
            }
        }




        //מקבלת קוד מייל ונושא חדש 
        //ומעדכנת את נושא המייל לנושא שהתקבל
        //דואגת לשנות גם משפט מפתח ולעדכן את כל הטבלאות בשינוי הנושא
        public static void UpdateEmailSubject(int mailId, string newSubjectName)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                //המייל שהנושא שלו מתעדכן
                EmailDto email = GetEmailById(mailId);
                //קוד הנושא החדש
                int? newSubjectId = GetSubjectIdBySubAndUser(newSubjectName, email.toUserId);
                //אם המייל קיים וגם הנושא החדש קיים
                if (email != null && newSubjectId != null)
                {
                    //תאריך נוכחי
                    DateTime today = DateTime.Now;
                    //הפרש בין שתי תאריכים
                    TimeSpan diffOfDates = today - email.dateReceived;
                    //בדיקה האם אנחנו בתוך השבוע
                    if (diffOfDates.Days < 8)
                    {

                        //קוד הנושא הישן
                        int? oldSubjetId = email.subjectMail;
                        SubjectDto oldSubjet = null;
                        //יכול להיות שלמייל אין נושא כי הוא נמחק
                        if (oldSubjetId != null)
                            oldSubjet = GetSubjectByID((int)oldSubjetId);
                        //הנושא החדש
                        SubjectDto newSubject = GetSubjectByID((int)newSubjectId);

                        //ריסט לכל האלגוריתם
                        //רשימה של מילים שהשתתפו בניתוח של המייל והכמות שלהם
                        List<UpdateSubjectDto> listToUpdate = GetListForUpdateSubject(mailId);
                        //מעבר על כל המילים
                        foreach (UpdateSubjectDto word in listToUpdate)
                        {
                            //קוד המילה במאגר
                            int wordId = GetIDByWord(word.word);
                            //אם המילה נמחקה
                            if (wordId == -1)
                            {
                                AddWord(new WordDto() { wordName = word.word });
                                wordId = GetIDByWord(word.word);
                            }

                            //מציאת הרשומה בטבלת הטעויות של האלגוריתם עבור המילה ושתי הנושאים
                            AlgorithmMistakeDto mistake = GetMistakeByWordAndTwoSubjets(word.word, oldSubjetId, (int)newSubjectId);
                            //אם לא קיים תוסיף לטבלה עם ערך 1 במספר הטעויות
                            if (mistake == null)
                            {
                                if (oldSubjetId != null) //רק אם יש נושא ישן
                                {
                                    AlgorithmMistakeDto am = new AlgorithmMistakeDto();
                                    am.word = word.word;
                                    am.newSubject = (int)newSubjectId;
                                    am.oldSubject = (int)oldSubjetId;
                                    am.AmountOfMistakes = 1;
                                    AddAlgorithmMistake(am);
                                    mistake = GetMistakeByWordAndTwoSubjets(word.word, (int)oldSubjetId, (int)newSubjectId);
                                }
                            }
                            else
                            {// אם הרשומה קיימת אני מוסיפה לכמות הטעויות +כמות הפעמים שהופיעה במייל עד תקרה של 20
                                if (mistake.AmountOfMistakes < 20)
                                {
                                    UpdateAmountAlgorithmMistakes(mistake.id, mistake.AmountOfMistakes + word.amount);

                                    mistake.AmountOfMistakes++; //(כדי שזה יתעדכן בקוד (בגלל שאני לא עושה שליפה נוספת
                                }
                            }


                            // מכאן טיפול נושא החדש ####

                            //מציאת המילה לנושא החדש
                            WordForSubjectDto wordForNewSub = GetWordForSubjectByWordAndSub(wordId, (int)newSubjectId);
                            //יכול להיות שהמילה עלתה לאבות
                            //ויכול להיות שמלכתחילה היא היתה משותפת אצל האבא
                            if (wordForNewSub == null)
                            {
                                //לעלות באבות
                                SubjectDto temp = newSubject;
                                Boolean isfound = false;
                                while (temp.parentID != null)
                                {
                                    temp = GetSubjectByID((int)temp.parentID);
                                    //בדיקה האם המילה שייכת לאבא
                                    wordForNewSub = GetWordForSubjectByWordAndSub(wordId, (int)temp.subjectId);
                                    if (wordForNewSub != null)
                                    {
                                        //רק אם המילה משותפת אצל האבא אז אני מסמנת שמצאתי אותה
                                        if (wordForNewSub.isCommon)
                                            isfound = true;
                                        break;
                                    }
                                }
                                // כלומר מצאתי את המילה משותפת אצל האבא
                                if (isfound)
                                {
                                    // מציאת מספר צאצאיו של אותו נושא
                                    int numOfChildren = GetNumOfChildrenByPID(temp.subjectId, GetDictOfNumOfChildren(email.toUserId));
                                    // אם אין נושא ישן מעלה רק אחד כי אין תיעוד על מספר הטעויות
                                    if (oldSubjetId == null)
                                        db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == newSubjectId && x.wordId == wordForNewSub.wordId).wordWeight += (word.amount / numOfChildren);

                                    // מעלה במשקל של המילה לנושא את את אחוז הטעויות שקרו עד עכשיו כפול מספר הופעות המילה במייל לחלק במספר הצאצאים
                                    //לדוגמא מילה סווגה בטעות 3 פעמים לנושא אז לאותה מילה לנושא המתוקן נוסיף למשקל שלה 0.03 אחוז כפול מספר הפעמים שהמילה הופיעה במייל חלקי מספר הצאצאים של הנושא
                                    else
                                        db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == newSubjectId && x.wordId == wordForNewSub.wordId).wordWeight += ((wordForNewSub.wordWeight * mistake.AmountOfMistakes / 100 * word.amount + 1) / numOfChildren);
                                }

                                else
                                    // המילה לא נמצאה כמשותפת באבות ולכן תוסיף אותה למאגר
                                    AddWordForSubject(new WordForSubjectDto() { subjectId = (int)newSubjectId, wordId = wordId, wordWeight = word.amount, isCommon = false });
                            }
                            else
                            {
                                // אם אין נושא ישן מעלה רק את מספר הפעמים שהופיעה במייל כי אין תיעוד על מספר הטעויות
                                if (oldSubjetId == null)
                                    db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == newSubjectId && x.wordId == wordForNewSub.wordId)
                                        .wordWeight += word.amount;
                                //במקרה והמילה היתה קשורה לנושא החדש
                                //מעלה במשקל של המילה לנושא את את אחוז הטעויות שקרו עד עכשיו כפול מספר הופעות המילה במייל
                                //לדוגמא מילה סווגה בטעות 3 פעמים לנושא אז לאותה מילה לנושא המתוקן 
                                //נוסיף למשקל שלה 0.03 אחוז כפול מספר הפעמים שהמילה הופיעה במייל
                                else
                                    db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == newSubjectId && x.wordId == wordForNewSub.wordId)
                                        .wordWeight += (wordForNewSub.wordWeight * mistake.AmountOfMistakes / 100 * word.amount + 1);
                            }

                            // מכאן טיפול בנושא הישן ###



                            //אם הנושא הקודם קיים
                            if (oldSubjet != null)
                            {
                                //מציאת הקשירות בין המילה לנושא הישן
                                WordForSubjectDto wordForOldSub = GetWordForSubjectByWordAndSub(wordId, (int)oldSubjetId);
                                //וגם המילה קשורה לנושא הישן
                                if (wordForOldSub != null)
                                {
                                    //הורדה ממשקל המילה לנושא את את אחוז הטעויות שקרו עד עכשיו כפול מספר הופעות המילה במייל
                                    //לדוגמא מילה סווגה בטעות 3 פעמים לנושא אז לאותה מילה לנושא נוריד מהמשקל שלה 0.03 אחוז כפול מספר הפעמים שהמילה הופיעה במייל

                                    //אם ברגע שאני מורידה מהמשקל את מספר המופעים כפול אחוז הטעוית של המילה המשקל שווה לאפס אן לפחות אני ימחק את הקשירות של המילה לנושא הישן
                                    if ((wordForOldSub.wordWeight - mistake.AmountOfMistakes / 100 * wordForOldSub.wordWeight * word.amount - 1) <= 0)
                                        DeleteWordFromSub((int)oldSubjetId, wordId);
                                    else
                                    {
                                        // הורדה מהמשקל את סך ההופעות כפול אחוז הטעויות
                                        db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == oldSubjetId && x.wordId == wordForOldSub.wordId)
                                            .wordWeight -= (mistake.AmountOfMistakes / 100 * wordForOldSub.wordWeight * word.amount + 1);
                                        db.SaveChanges();
                                    }
                                }


                                //אם המילה כבר לא קשורה לנושא יכול להיות שהיא עלתה באבות שלו בגלל הפונקציה שמוצאת מילים משותפות ומעלה אותם לאבות
                                //ויכול להיות שמלכתחילה היא היתה משותפת אצל האבא
                                else
                                {
                                    //לעלות באבות
                                    SubjectDto temp = oldSubjet;
                                    Boolean isfound = false;
                                    while (temp.parentID != null)
                                    {
                                        temp = GetSubjectByID((int)temp.parentID);
                                        //בדיקה האם המילה שייכת לאבא
                                        wordForOldSub = GetWordForSubjectByWordAndSub(wordId, (int)temp.subjectId);
                                        if (wordForOldSub != null)
                                        {
                                            //רק אם המילה משותפת אצל האבא אז אני מסמנת שמצאתי אותה
                                            if (wordForOldSub.isCommon)
                                                isfound = true;
                                            break;
                                        }
                                    }
                                    if (isfound)
                                    {
                                        // מציאת מספר צאצאיו של אותו נושא
                                        int numOfChildren = GetNumOfChildrenByPID(temp.subjectId, GetDictOfNumOfChildren(email.toUserId));
                                        //בדיקה האם זה מתאפס 
                                        if ((wordForOldSub.wordWeight - ((mistake.AmountOfMistakes / 100 * wordForOldSub.wordWeight * word.amount - 1) / numOfChildren)) <= 0)
                                            DeleteWordFromSub((int)oldSubjetId, wordId);
                                        else
                                        {
                                            // הורדה מהמשקל את סך ההופעות כפול אחוז הטעויות לחלק למספר הצאצים של הנושא בגלל שזה משותף
                                            db.WordForSubject_tbl.FirstOrDefault(x => x.subjectId == oldSubjetId && x.wordId == wordForOldSub.wordId)
                                                .wordWeight -= ((mistake.AmountOfMistakes / 100 * wordForOldSub.wordWeight * word.amount + 1) / numOfChildren);
                                            db.SaveChanges();
                                        }
                                    }

                                    //במקרה והוא לא מצא את המילה באבות אז המילה כבר אינה קשורה לנושא הישן
                                    //או שמשקלה לנושא התאפס או שעלתה באבות שלו והתגלתה כלא רלוונטית לניתוח
                                    //במקרה כזה אני לא עושה כלום כי המילה כבר לא משפיעה על הנושא
                                }

                            }
                        }
                        db.SaveChanges();

                        //מציאת השולח - איש הקשר
                        int contactManId = GetcontactIdByEmail(email.fromAddressMail);
                        //אם הוא לא קיים אני מוסיפה אותו
                        if (contactManId == -1)
                        {
                            AddContactMan(new ContactManDto() { email = email.fromAddressMail, describeContact = email.fromName });
                            contactManId = GetcontactIdByEmail(email.fromAddressMail);
                        }
                        // מציאת הקשירות של האיש קשר לנושא החדש
                        ContactForSubjectDto contact1 = GetContactForSubByIdAndSub(contactManId, (int)newSubjectId);
                        // אם לא קיימת קשירות לנושא אני מוסיפה עם משקל של 0
                        if (contact1 == null)
                            AddContactForSubject(new ContactForSubjectDto() { subjectId = (int)newSubjectId, contactManID = contactManId, contactWeight = 0 });
                        else
                        {
                            //אם קיימת קשירות אני מוסיפה למשקל של האיש קשר לנושא החדש עוד אחד
                            db.ContactForSubject_tbl.FirstOrDefault(x => x.subjectId == (int)newSubjectId && x.contactManID == contactManId).contactWeight++;
                            db.SaveChanges();
                        }
                        //אם הנושא הקודם קיים 
                        if (oldSubjetId != null)
                        {
                            //מציאת הקשירות עם הנושא הישן
                            ContactForSubjectDto contact2 = GetContactForSubByIdAndSub(contactManId, (int)oldSubjetId);
                            //אם הקשירות קיימת
                            if (contact2 != null)
                            {
                                //אם הורדה של אחד תאפס את המשקל של האיש קשר לנושא הישן אז תמחק את הקשר בין האיש קשר לנושא
                                if (contact2.contactWeight <= 1)
                                    DeleteContactForSub(contactManId, (int)oldSubjetId);
                                else
                                {
                                    //הורדה מהמשקל של האיש קשר לנושא הישן אחד
                                    db.ContactForSubject_tbl.FirstOrDefault(x => x.subjectId == oldSubjetId && x.contactManID == contactManId).contactWeight--;
                                    db.SaveChanges();
                                }
                            }

                        }



                        //מציאת משפט מפתח קשור לפי הנושא החדש
                        string newkeySentence = FindRelatedSentence.FindRelatedSentenceForUpdateSubject(email, (int)newSubjectId); 
                        //מציאת המייל שעידכן את הנושא שלו
                        Email_tbl email_Tbl = db.Email_tbl.FirstOrDefault(x => x.mailID == email.mailID);
                        //עידכון השינויים
                        if (email_Tbl != null)
                        {
                            email_Tbl.RelatedSentence = newkeySentence;
                            email_Tbl.subjectMail = newSubjectId;
                            db.SaveChanges();
                        }
                    }
                }
            }
        }




        public static void UpdateSubjectColor(int subId, string color)
        {

            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {
                Subject_tbl subject = db.Subject_tbl.FirstOrDefault(x => x.subjectId == subId);
                if (subject != null)
                {
                    subject.color = color;
                    db.SaveChanges();
                }
            }
        }

        //delete
        //===========================================================================



        //פונקציה המקבלת קוד מילה
        //מוחקת אותה מהמאגר
        //דואגת למחוק את כל הקשירויות שלה לנושאים
        public static void DeleteWord(int wordID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                //מחיקת כל הקשירויות לנושאים 
                db.WordForSubject_tbl.RemoveRange(db.WordForSubject_tbl.Where(x => x.wordId == wordID).ToList());
                db.SaveChanges();

                //מחיקת המילה בעצמה
                db.Word_tbl.Remove(db.Word_tbl.FirstOrDefault(x => x.wordId == wordID));
                db.SaveChanges();
            }
        }

        //מקבלת קוד איש קשר 
        //מוחקת אותו מהמאגר כולל מכל הטבלאות שקשורות אליו
        public static void DeleteContact(int conId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                // db.ContactForUser_tbl.RemoveRange(db.ContactForUser_tbl.Where(x => x.contactManID == conId).ToList());
                db.ContactForSubject_tbl.RemoveRange(db.ContactForSubject_tbl.Where(x => x.contactManID == conId).ToList());
                db.SaveChanges();
                db.ContactMan_tbl.Remove(db.ContactMan_tbl.FirstOrDefault(x => x.contactManId == conId));
                db.SaveChanges();
            }
        }
        //פונקציה המקבלת קוד מילה וקוד נושא
        //מוחקת 
        public static void DeleteWordFromSub(int subId, int wordID)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                //בדיקה האם המילה קשורה לנושא
                WordForSubject_tbl sfw = db.WordForSubject_tbl.FirstOrDefault(x => x.wordId == wordID && x.subjectId == subId);
                if (sfw != null)
                {
                    db.WordForSubject_tbl.Remove(sfw);

                }
                db.SaveChanges();
            }
        }




        //פונקציה המקבלת קוד נושא
        //ומוחקת אותו מהמאגר
        //דואגת למחוק את כל הדברים שקשורים אליו כמו מילים ואנשי קשר וכו
        public static void DeleteSubject(int subId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                //בדיקה האם הנושא קיים
                SubjectDto subject = GetSubjectByID(subId);
                if (subject != null)
                {
                    //מציאת כל המילים המשותפות של הנושא
                    List<WordForSubjectDto> commonWords = GetCommonWordsForSubject(subId);
                    //מציאת כל הילדים שלו
                    List<int> childrenLst = GetSubjectsIDByParentId(subId);
                    foreach (int childId in childrenLst)
                    {
                        //מציאת הילדים של הילד
                        List<int> grandsonLst = GetSubjectsIDByParentId(childId);
                        foreach (WordForSubjectDto commonWord in commonWords)
                        {
                            //הוספת המילה לבן
                            WordForSubjectDto newWFS = new WordForSubjectDto()
                            {
                                subjectId = childId,
                                wordId = commonWord.wordId,
                                wordWeight = (double)GetWeightWordForSubjectByWordAndSub(subId, commonWord.wordId),
                                isCommon = false
                            };
                            if (grandsonLst.Count > 0)
                            {
                                //אם יש לו ילדים תייצג את המילה כמשותפת
                                newWFS.isCommon = true;
                            }
                            db.WordForSubject_tbl.Add(newWFS.DtoToDal());
                        }
                        //שינוי האבא
                        db.Subject_tbl.First(x => x.subjectId == childId).parentID = subject.parentID;
                    }
                    db.SaveChanges();

                    //מחיקת כל הקשירויות עם האנשי קשר
                    db.ContactForSubject_tbl.RemoveRange(db.ContactForSubject_tbl.Where(x => x.subjectId == subId).ToList());
                    //מציאת כל המילים של הנושא
                    List<WordForSubject_tbl> listWfs = db.WordForSubject_tbl.Where(x => x.subjectId == subId).ToList();
                    //מעבר על כל מילה
                    foreach (WordForSubject_tbl wfs in listWfs)
                    {
                        WordForSubject_tbl w = listWfs.FirstOrDefault(x => x.wordId == wfs.wordId);
                        //מורידה את הקשירות שלה לנושא
                        db.WordForSubject_tbl.Remove(w);
                        db.SaveChanges();
                        //אם אין לה עוד קשירויות עם נושאים אחרים
                        if (GetWordsForSubjectByWord(wfs.wordId).Count == 0)
                            //תמחק את המילה הזאת מהמאגר
                            DeleteWord(wfs.wordId);
                        db.SaveChanges();
                    }

                    //שליפת כל המיילים של המשתמש
                    IQueryable<Email_tbl> emails = db.Email_tbl.Where(x => x.subjectMail == subId);
                    foreach (Email_tbl email in emails)
                    {
                        // מחיקת הנושא מכל המיילים שקשורים לנושא
                        if (email.subjectMail == (int)subId)
                            email.subjectMail = null;
                    }

                   

                    // מחיקה מהטבלאות הזמניות
                    db.AlgorithmMistake_tbl.RemoveRange(db.AlgorithmMistake_tbl.Where(x => x.newSubject == subId || x.oldSubject == subId));
                    db.UpdateKeySentenceForSemilar_tbl.RemoveRange(db.UpdateKeySentenceForSemilar_tbl.Where(x => x.subjectId == subId));
                    db.UpdateKeySentence_tbl.RemoveRange(db.UpdateKeySentence_tbl.Where(x => x.subjectId == subId));
                    db.Subject_tbl.Remove(db.Subject_tbl.FirstOrDefault(x => x.subjectId == subId));
                    db.SaveChanges();
  
                }
            }
        }


        //מקבלת קוד איש קשר וקוד נושא
        //מוחקת את הקשירות של האיש קשר מהנושא
        public static void DeleteContactForSub(int conId, int subId)
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                ContactForSubject_tbl cfs = db.ContactForSubject_tbl.FirstOrDefault(x => x.contactManID == conId && x.subjectId == subId);
                if (cfs != null)
                {
                    db.ContactForSubject_tbl.Remove(cfs);
                    db.SaveChanges();

                    List<ContactForSubject_tbl> cfsLst = db.ContactForSubject_tbl.Where(x => x.contactManID == conId).ToList();
                    if (cfsLst.Count() == 0)
                    {
                        DeleteContact(conId);
                        db.SaveChanges();
                    }

                }
            }
        }


        // מוחקת את כל הרשומות של המיילים שעבר עליהם שבוע מטבלאות שנשמרו לעידכון נושא מייל
        public static void DeleteAfterWeek()
        {
            using (TextClassificationProjectEntities db = new TextClassificationProjectEntities())
            {

                //מחיקת כל הרשומות למיילים שהתקבלו לפני יותר משבוע 
                db.UpdateKeySentenceForSemilar_tbl.RemoveRange(db.UpdateKeySentenceForSemilar_tbl.Where(email => (DateTime.Now - GetEmailById(email.mailID).dateReceived).Days > 7).ToList());
                db.UpdateKeySentence_tbl.RemoveRange(db.UpdateKeySentence_tbl.Where(email => (DateTime.Now - GetEmailById(email.mailID).dateReceived).Days > 7).ToList());
                db.UpdateSubject_tbl.RemoveRange(db.UpdateSubject_tbl.Where(email => (DateTime.Now - GetEmailById(email.mailID).dateReceived).Days > 7).ToList());
                db.SaveChanges();
            }
        }
    }



    public class Validation
    {
        public Validation()
        {
            User = null;
            Status = 404;
            ExistMail = false;
        }
        public UserDto User { get; set; }
        public Boolean ExistMail { get; set; }
        public int Status { get; set; }
    }
}


