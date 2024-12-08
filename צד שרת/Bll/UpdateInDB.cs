using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Bll.BuildStructs;

namespace Bll
{
    public static class UpdateInDB
    {
        //פונקציה המקבלת מייל ומבנים המסמלים את ניתוחו
        //ומעדכנת בדאטה את מה שצריך לעדכן
        public static void UpdateEmail(EmailDto email, BuildWordsStruct buildWordsStruct)
        {
            //הכנסת המייל למאגר
            ClassDB.AddEmail(email);
            //עידכון המילים שנמצאות במאגר
            foreach (string existword in buildWordsStruct.ExistWords)
            {
                int wordId = ClassDB.GetIDByWord(existword);
                //לא בהכרח שהמילה קשורה לנושא הזה!
                WordForSubjectDto wordForSub = ClassDB.GetWordForSubjectByWordAndSub(wordId, (int)email.subjectMail);
                if (wordForSub != null)
                {
                    //המישקל המקורי של המילה לנושא
                    double wordWeight = (double)wordForSub.wordWeight;
                    //כמות ההופעות של המילה במייל
                    int wordAmount = buildWordsStruct.WordsAmount[existword];
                    ClassDB.UpdateWeightWordForSubject(wordId, (int)email.subjectMail, wordWeight + wordAmount);
                }
                //כלומר המילה נמצאת במאגר אך לא קשורה לנושא
                else
                {
                    //צריך לבדוק האם המילה משותפת ונמצאת אצל אחד מאבותיו
                    //אם כן צריך לעלות את הציון אצל האבא

                    //הנושא של המייל
                    SubjectDto sub = ClassDB.GetSubjectByID((int)email.subjectMail);
                    //נושא האב
                    SubjectDto subParent = ClassDB.GetSubjectByID(sub.parentID);
                    Boolean isCommon = false;
                    //כל עוד יש אבא לנושא
                    while (subParent!=null)
                    {
                        //תבדוק האם האא קשור למילה
                        wordForSub = ClassDB.GetWordForSubjectByWordAndSub(wordId, subParent.subjectId);
                        if (wordForSub != null)
                        {
                            //ואם הוא קשור למילה - האם היא משותפת אצלו
                            if (wordForSub.isCommon == true)
                                isCommon = true;
                            break;
                        }

                        // תמשיך לסבא וכו
                        subParent = ClassDB.GetSubjectByID(subParent.parentID);
                    }
                    //במקרה והמילה משותפת אצל אחד מהאבות צריך להוסיף את הציון לאבא
                    if (isCommon)
                        ClassDB.UpdateWeightWordForSubject(wordId, subParent.subjectId, (double)wordForSub.wordWeight + buildWordsStruct.WordsAmount[existword]);
                    //אם לא אז צריך להוסיף לנושא את המילה והציון יהיה מספר ההופעות של המילה במייל הזה
                    else ClassDB.AddWordForSubject(new WordForSubjectDto((int)email.subjectMail, wordId, buildWordsStruct.WordsAmount[existword], false));
                }
            }
            //הכנסת המילים החדשות למאגר עם משקל אחד
            foreach (string notExistword in buildWordsStruct.NotExistWords)
            {
                ClassDB.AddWord(new WordDto() { wordName = notExistword }); //הכנסת המילה
                int wordId = ClassDB.GetIDByWord(notExistword); //קוד המילה
                ClassDB.AddWordForSubject(new WordForSubjectDto((int)email.subjectMail, wordId, buildWordsStruct.WordsAmount[notExistword],false)); 
            }



            //מציאת איש הקשר
            int cmID = ClassDB.GetcontactIdByEmail(email.fromAddressMail);
            if (cmID == -1) // אם לא קיים
            {
                ClassDB.AddContactMan(new ContactManDto() { email = email.fromAddressMail, describeContact = email.fromAddressMail });
                cmID = ClassDB.GetcontactIdByEmail(email.fromAddressMail);
            }



            //מציאת איש  קשר לנושא
            ContactForSubjectDto cfs = ClassDB.GetContactForSubByIdAndSub(cmID, (int)email.subjectMail);
            if (cfs == null)
                ClassDB.AddContactForSubject(new ContactForSubjectDto() { contactManID = cmID, subjectId = (int)email.subjectMail, contactWeight = 1 });
            else
                ClassDB.UpdateWeightContactForSubject(cmID, (int)email.subjectMail, (double)++cfs.contactWeight);



            //שמירת פרטים על ניתוח המייל בשלוש טבלאות שישמרו במשך שבוע ויעזרו במקרה של שינוי נושא המייל

            email.mailID = ClassDB.GetEmailByUserAndOutlookId(email.toUserId, email.IdInOutLook).mailID;

            //הכנסה לטבלה ראשונה עבור עידכון נושא
            UpdateSubjectDto upSubject = new UpdateSubjectDto();
            //שומר בטבלה עבור כל מילה כמה פעמים היא היתה במייל
            foreach (KeyValuePair<string, int> word in buildWordsStruct.WordsAmount)
            {
                upSubject.mailID = email.mailID;
                upSubject.word = word.Key;
                upSubject.amount = word.Value;
                ClassDB.AddUpdateSubject(upSubject);
            }

            //הכנסה לטבלה שנייה עבור עידכון משפט מפתח
            UpdateKeySentenceDto upSentence = new UpdateKeySentenceDto();
            foreach (KeyValuePair<int, Dictionary<string, WordForSub>> sub in buildWordsStruct.MarksWordsForSub)
            {
                foreach (KeyValuePair<string, WordForSub> word in sub.Value)
                {
                    upSentence.mailID = email.mailID;
                    upSentence.subjectId = sub.Key;
                    upSentence.word = word.Key;
                    upSentence.wordWeight = word.Value.Weight;
                    upSentence.isCommon = word.Value.IsCommon;
                    ClassDB.AddUpdateKeySentence(upSentence);
                }
            }

            //הכנסה לטבלה שלישית עבור עידכון משפט מפתח למילים שלא היו בדאטה והביאו להם מילים נרדפות
            UpdateKeySentenceForSemilarDto upSentenceForSem = new UpdateKeySentenceForSemilarDto();
            foreach (KeyValuePair<int, Dictionary<string, semilarWordForSub>> sub in buildWordsStruct.MarksSemilarsForSub)
            {
                foreach (KeyValuePair<string, semilarWordForSub> word in sub.Value)
                {
                    upSentenceForSem.mailID = email.mailID;
                    upSentenceForSem.subjectId = sub.Key;
                    upSentenceForSem.word = word.Key;
                    upSentenceForSem.specialWeight = word.Value.SpecialWeight;
                    upSentenceForSem.specialAmount = word.Value.SpecialAmount;
                    upSentenceForSem.commonWeight = word.Value.CommonWeight;
                    upSentenceForSem.commonAmount = word.Value.CommonAmount;
                    ClassDB.AddUpdateKeySentenceForSemilar(upSentenceForSem);
                }
            }
        }

    }
}


