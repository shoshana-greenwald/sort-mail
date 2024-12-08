using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll.BuildStructs;
using Dto;

namespace Bll
{
    static class FindRelatedSentence
    {


        public static string FindMailRelatedSentence(string emailContent, int subjectId, Dictionary<int, Dictionary<string,
            BuildStructs.WordForSub>> MarksWordsForSub, Dictionary<int, Dictionary<string, BuildStructs.semilarWordForSub>> MarksSemilarsForSub)
        {
            string words = emailContent;
            //נקודה מסמלת ירידת שורה
            words = words.Replace("\r\n".ToString(), ".#@ ");
            //רק אם יש את סמל המעקב-----
            if (words.IndexOf("<https://mailtrack".ToString()) >= 0)
                words = words.Substring(0, words.IndexOf("<https://mailtrack".ToString())).Replace(".#@ .#@ ", "");
            //שליחה לפומקציה שמחלקת טקסט למשפטים
            List<string> sentencesList = HebrewNlp.SpliteToSentences(words);
            for (int j = 0; j < sentencesList.Count; j++)
                sentencesList[j] = sentencesList[j].Replace(".#@", "").Replace("#@", "");
            //מערך שיחזיק לי עבור כל משפט את ציונו
            double[] marks = new double[sentencesList.Count];
            //רשימה שתחזיק את כל האבות, הסבות וכו בעץ של הנושא שהתקבל
            List<int> parents = new List<int>();
            //הנושא שהתקבל
            int? subid = subjectId;
            //מציאת האבות
            while (subid != null)
            {
                //מוצא את האבא
                subid = ClassDB.GetSubjectByID((int)subid).parentID;
                if (subid != null)
                    parents.Add((int)subid);
                else break;
            }

            int i = 0;
            //עובר על כל המשפטים שבמייל
            foreach (string sentence in sentencesList)
            {
                //סינון תווים, חלוקת הטקסט למילים וסינון מילים ריקות
                string s = sentence;
                List<char> chars = new List<char> { '!', '"', ';', ':', '?', '.', ',', '(', ')','\t',
                    '\'', '\n', '\r', '@', '#', '$', '%', '^', '&', '*', '<', '>', '\\', '/', '+', '\'' };
                chars.ForEach(a => s = s.Replace(a.ToString(), ""));
                List<string> lst = s.Split(' ').ToList();
                lst = lst.Where(x => x != "" && x != null).ToList();

                //איתחול הציון
                marks[i] = 0;
                //מעבר על כל המילים 
                foreach (var word in lst)
                {
                    //מציאת מילת הבסיס
                    string baseWord = ClassDB.GetBaseWordByWord(word);
                    if (baseWord == null) // אם המילה לא נמצאת בטבלה אז סימן שהיא לא היתה רלוונטית לניתוח
                        continue;

                    //הוספת מישקלים בגלל האבות רק למילים משותפות
                    foreach (int parent in parents)
                    {
                        if (MarksWordsForSub.ContainsKey(parent))
                        { 
                            //בדיקה האם המילה קיימת בנושא זה והאם היא משותפת
                            if (MarksWordsForSub[parent].ContainsKey(baseWord))
                                if (MarksWordsForSub[parent][baseWord].IsCommon == true)
                                    marks[i] += MarksWordsForSub[parent][baseWord].Weight;
                            //הוספת המילים המשותפות מהמילים הנרדפות
                            if (MarksSemilarsForSub.ContainsKey(parent))
                                if(MarksSemilarsForSub[parent].ContainsKey(baseWord))
                                    marks[i] += MarksSemilarsForSub[parent][baseWord].CommonWeight / MarksSemilarsForSub[parent][baseWord].CommonAmount;
                        }
                    }


                    //הוספה לנושא עצמו מיוחד+משותף
                    if (MarksWordsForSub.ContainsKey(subjectId))
                    {
                        if (MarksWordsForSub[subjectId].ContainsKey(baseWord))
                            marks[i] += MarksWordsForSub[subjectId][baseWord].Weight;
                    }
                    if (MarksSemilarsForSub.ContainsKey(subjectId))
                    {
                        if (MarksSemilarsForSub[subjectId].ContainsKey(baseWord))
                        {
                            if (MarksSemilarsForSub[subjectId][baseWord].SpecialAmount != 0)
                                marks[i] += MarksSemilarsForSub[subjectId][baseWord].SpecialWeight / MarksSemilarsForSub[subjectId][baseWord].SpecialAmount;
                            if (MarksSemilarsForSub[subjectId][baseWord].CommonAmount != 0)
                                marks[i] += MarksSemilarsForSub[subjectId][baseWord].CommonWeight / MarksSemilarsForSub[subjectId][baseWord].CommonAmount;
                        }
                    }
                }
                i++;
            }
            //מציאת הציון המקסימלי
            double maxValue = marks.Max();
            //מציאת המשפט עם הציון המקסימלי
            int maxIndex = marks.ToList().IndexOf(maxValue);
            //החזרת המשפט
            return sentencesList[maxIndex];
        }

        //פונקציה למציאת משפט קשור חדש לאחר ששינו את נושא המייל
        public static string FindRelatedSentenceForUpdateSubject(EmailDto email, int newSubjectId)
        {
            //בניית מבנים בשביל האלגוריתם של מציאת משפט מפתח
            //כל הנתונים נשמרו בדאטה למשך שבוע מקבלת המייל
            
            //מבנה ראשון
            //מילון בו לכל נושא של אותו משתמש יש מילון מילים שקשורות לנושא ואת משקליהם
            Dictionary<int, Dictionary<string, WordForSub>> MarksWordsForSub = new Dictionary<int, Dictionary<string, WordForSub>>();
            //כל המילים שהשתתפו בניתוח המייל
            List<UpdateKeySentenceDto> lstWords = ClassDB.GetWordsLstToUpdateKeySentenceByMailId(email.mailID);
            foreach (UpdateKeySentenceDto w in lstWords)
            {
                //בניית מבנה עבור כל מילה
                BuildStructs.WordForSub wfs = new BuildStructs.WordForSub();
                //קוד המילה בדאטה
                int wordId = ClassDB.GetIDByWord(w.word);
                //אם המילה לא נמצאת בדאטה כרגע אז אל תתיחס אליה כי מציאת משפט המפתח הוא לפי המצב הנוכחי
                if (wordId != -1)
                {
                    // בניית מבנה עבור המילה
                    wfs.Word = w.word;
                    wfs.Weight = w.wordWeight;
                    wfs.IsCommon = w.isCommon;

                    if (!MarksWordsForSub.ContainsKey(w.subjectId))
                        MarksWordsForSub.Add(w.subjectId, new Dictionary<string, WordForSub>());
                    // הכנסה למילון

                    //כאן הוא נופל$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ זה לא נכון
                    if (!MarksWordsForSub[w.subjectId].ContainsKey(w.word))
                        MarksWordsForSub[w.subjectId].Add(w.word, wfs);
                }
            }

            //מבנה שני
            //מילון בו לכל נושא יש רשימת מילים שלא היו קיימות בדאטה והיאו להם מילים נרדפות לכל מילה יש משקל וכמות עבור המילים המיוחדות והמשותפות
            Dictionary<int, Dictionary<string, semilarWordForSub>> MarksSemilarsForSub = new Dictionary<int, Dictionary<string, semilarWordForSub>>();
            List<UpdateKeySentenceForSemilarDto> lstSemiWords = ClassDB.GetSemilarWordsLstToUpdateKeySentenceByMailId(email.mailID);
            foreach (UpdateKeySentenceForSemilarDto w in lstSemiWords)
            {
                // כנ"ל
                BuildStructs.semilarWordForSub wfs = new BuildStructs.semilarWordForSub();
                int wordId = ClassDB.GetIDByWord(w.word);
                if (wordId != -1)
                {
                    wfs.Word = w.word;
                    wfs.CommonWeight = w.commonWeight;
                    wfs.CommonAmount = w.commonAmount;
                    wfs.SpecialWeight = w.specialWeight;
                    wfs.SpecialAmount = w.specialAmount;
                    if (!MarksSemilarsForSub.ContainsKey(w.subjectId))
                        MarksSemilarsForSub.Add(w.subjectId, new Dictionary<string, semilarWordForSub>());
                    MarksSemilarsForSub[w.subjectId].Add(w.word, wfs);
                }
            }
            //מציאת משפט מפתח חדש לפי הנושא החדש
            return FindRelatedSentence.FindMailRelatedSentence(email.title + "\r\n" + email.body, newSubjectId, MarksWordsForSub, MarksSemilarsForSub);
        }
    }
}
