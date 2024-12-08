using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Bll.BuildStructs;
using Outlook = Microsoft.Office.Interop.Outlook;
using Dto;

namespace Bll
{
    public class OutlookConnecting
    {
        Outlook.Items Items = null;

        //פונקציה ראשית לניהול קבלת מייל
        public void StartConection()
        {
            Outlook.Application myApp = new Outlook.Application();
            Outlook.NameSpace mapiNameSpace = myApp.GetNamespace("MAPI");
            // לתיקיה זו נכנסים כל המיילים
            Outlook.MAPIFolder myInbox = mapiNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            //יצירת אירוע בעת קבלת מייל
            this.Items = myInbox.Items;
            this.Items.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(RecivedMail);
        }


        //פונקציה המופעלת אוטומטית בכל הגעת מייל חדש
        //הפונקציה מקבלת את המייל בתור פרמטר
        public void RecivedMail(object Item)
        {
            //המרת האובייקט המתקבל לאוביקט מסוג מייל כדי שיהיה אפשר לשלוף ממנו את הפרטים
            Outlook.MailItem mail = (Outlook.MailItem)Item;

            //יצירת אוביקט מחלקה שיצרתי לצורך ניתוח המייל
            EmailDto newMail = new EmailDto();
            newMail.IdInOutLook = mail.EntryID;
            newMail.dateReceived = DateTime.Now.Date;
            newMail.body = mail.Body;
            newMail.title = mail.Subject;
            newMail.fromAddressMail = mail.SenderEmailAddress;
            newMail.fromName = mail.SenderName;
            newMail.toUserId = ClassDB.GetUserByMail(mail.ReceivedOnBehalfOfName).userId;

            //שליחה למחלקה אשר מנתחת את המייל ומחזירה אוביקט המייצג את הניתוח
            BuildWordsStruct buildWordsStruct = new BuildWordsStruct(newMail);

            //מציאת נושא המייל לפי מה שקיבלתי מהניתוח
            int subjectId = FindSubject.FindMailSubject(newMail.toUserId,
                buildWordsStruct.SubjetWordsAmount, buildWordsStruct.SearchTree, buildWordsStruct.ContactmanWeight);

            string content = newMail.title + "\r\n" + newMail.body;
            //מציאת משפט קשור
            string keySentence = FindRelatedSentence.FindMailRelatedSentence(content,
                subjectId, buildWordsStruct.MarksWordsForSub, buildWordsStruct.MarksSemilarsForSub);
           
            //עידכון נושא ומשפט מפתח למייל
            newMail.subjectMail = subjectId;
            newMail.RelatedSentence = keySentence;

            //עידכון בדאטה
            UpdateInDB.UpdateEmail(newMail, buildWordsStruct);

            //כל 30 מיילים מופעלת הפונקציה לעידכון מילים משותפות
            if (ClassDB.GetEmailAmount() % 30 == 0)
                UpdateCommonWords.UpdateLevelWord(newMail.toUserId);
        }
    }
}