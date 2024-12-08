using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dto;




namespace Bll.BuildStructs
{
    public class BuildWordsStruct
    {
        public List<string> WordsForAnalysis { get; set; } // המילים "הנקיות" לניתוח
        public Dictionary<int, double?> ContactmanWeight { get; set; } // משקל לכל נושא עבור איש הקשר של המייל
        public Dictionary<string, int> WordsAmount { get; set; } //מילון בו לכל מילה יש את כמות הפעמים בו היא מופיעה במייל  
        public List<string> ExistWords { get; set; } //מילים שנמצאות בדאטה
        public List<string> NotExistWords { get; set; } //מילים שלא נמצאות בדאטה

        //מילון של נושאים ולכל נושא יש מילון של מילים ששיכות אליו ונתונים עליהם - משקל והאם מילה משותפת
        public Dictionary<int, Dictionary<string, WordForSub>> MarksWordsForSub { get; set; }

        //מילון של נושאים ולכל נושא יש מילון של מילים שלא היו בדאטה והיאו להם מילים נרדפות ששיכות אליו ונתונים עליהם 
        //משקל והאם מילה משותפת וכמות המילים-
        public Dictionary<int, Dictionary<string, semilarWordForSub>> MarksSemilarsForSub { get; set; }

        //מילון של מילים שלא נמצאות בדאטה לכל מילה יש מילון של מילים נרדפות ואחוז הדימיון שלהם למילה
        public Dictionary<string, Dictionary<string, double>> SemilarsWords { get; set; }

        //מילון שהמפתחות בו יהיו הנושאים והערכים יהיו כמות המילים שהשתתפו בניתוח וקשורות לנושא
        public Dictionary<int, int> SubjetWordsAmount { get; set; }
        public Dictionary<int, FindNode> SearchTree { get; set; }   //עץ החיפוש בשביל האלגוריתם


        public BuildWordsStruct(EmailDto email)
        {
            //איתחול המבנים
            this.MarksWordsForSub = new Dictionary<int, Dictionary<string, WordForSub>>();
            this.MarksSemilarsForSub = new Dictionary<int, Dictionary<string, semilarWordForSub>>();
            this.SemilarsWords = new Dictionary<string, Dictionary<string, double>>();
            this.WordsAmount = new Dictionary<string, int>();
            this.ExistWords = new List<string>();
            this.NotExistWords = new List<string>();
            this.SearchTree = new Dictionary<int, FindNode>();
            this.SubjetWordsAmount = new Dictionary<int, int>();
            this.ContactmanWeight = new Dictionary<int, double?>();


            // קבלת כל הנושאים שנמצאים בדאטה ואיתחולם במבנים הנדרשים
            Dictionary<int, SubjectDto> userSubjects = ClassDB.GetDictOfAllSubjectsByUserId(email.toUserId);


            foreach (KeyValuePair<int, SubjectDto> sub in userSubjects)
            {
                this.MarksWordsForSub.Add(sub.Key, new Dictionary<string, WordForSub>());
                this.MarksSemilarsForSub.Add(sub.Key, new Dictionary<string, semilarWordForSub>());
                this.SearchTree.Add(sub.Key, new FindNode(sub.Key));
                this.SubjetWordsAmount.Add(sub.Key, 0);
                this.ContactmanWeight.Add(sub.Key, 0);
            }

            //מילון ובו עבור כל מילה יש רשימה בה נמצאים כל הקשרים שלה לנושאים של המשתמש
            //שלפתי את כל המילים עכשיו כדי לחסוך בזמן ריצה
            Dictionary<int, List<WordForSubjectDto>> wordsForUserSubjects = ClassDB.GetWordsForSubjectByListOfSubs(userSubjects);

            //המילים לניתוח לאחר "ניקוי" פסיקים וכו
            List<string> wordList = CleanWords(email.title, email.body);


            // מציאת מילות הבסיס שלהם
            this.WordsForAnalysis = HebrewNlp.NlpForWords(wordList);

            //יצירת סרד עבור כל מילה שלא נמצאת במאגר 
            Dictionary<string, Task<Dictionary<string, double>>> tasks = new Dictionary<string, Task<Dictionary<string, double>>>();

            foreach (string w in this.WordsForAnalysis)
            {
                //אם המילה לא נמצאת במאגר וגם לא ניתחתי אותה עדיין
                if (ClassDB.GetIDByWord(w) == -1 && !tasks.ContainsKey(w))
                {
                    //תיצור סרד חדש ותימצא את המילים הנרדפות של המילה
                    Task<Dictionary<string, double>> task = Task.Run(() =>
                    {
                        return HebrewNlp.NlpForSimilarWords(Synonyms.GetSimilarWords(w));
                    });
                    tasks.Add(w, task);
                }
            }

            //עוברים על כל מילה במייל
            foreach (string word in this.WordsForAnalysis)
            {
                //בדיקה אם כבר ניתחנו מילה זו
                if (!this.WordsAmount.ContainsKey(word))//אם המילה לא נמצאה
                    this.WordsAmount.Add(word, 1);
                else
                {
                    this.WordsAmount[word]++;//המילה כבר נמצאת לכן צריך לעלות לה את המונה
                    continue;//עוברים למילה הבאה
                }

                //בדיקה האם המילה נמצאת במאגר
                int wordId = ClassDB.GetIDByWord(word);
                if (wordId == -1)//המילה לא נמצאת במאגר
                {
                    this.NotExistWords.Add(word);//הוספה לרשימה
                    // nlp קבלת מילים נרדפות למילה ומציאת מילות הבסיס שלהם על ידי  
                    //כל זה נעשה בסרד 
                    Dictionary<string, double> semilarWords = tasks[word].Result;
                    //הוספה למילון
                    this.SemilarsWords.Add(word, semilarWords);
                    //פונקציה להוספת משקלים למבנים ממילים נרדפות
                    this.AddWeightForSimilar(word, semilarWords, wordsForUserSubjects);
                }
                else
                {//המילה כבר נמצאת במאגר
                    //בדיקה האם יש לה קשר לנושאים של המשתמש
                    if (wordsForUserSubjects.ContainsKey(wordId))
                    {
                        this.ExistWords.Add(word);
                        //פונקציה להוספת משקלים לעץ הנושאים
                        this.AddWeight(wordId, word, wordsForUserSubjects[wordId]);
                    }
                }
            }
            //טיפול באיש קשר
            //מציאת כל הנושאים בהם איש הקשר קיים
            int contactId = ClassDB.GetcontactIdByEmail(email.fromAddressMail);
            List<ContactForSubjectDto> contactForSubjects = ClassDB.GetContactsForSubsByContact(contactId, userSubjects);
            foreach (ContactForSubjectDto conForSub in contactForSubjects)
            {
                //עידכון המשקלים במבנה של האנשי קשר
                this.ContactmanWeight[conForSub.subjectId] = conForSub.contactWeight;
            }


            //מכאן בניית עץ החיפוש


            //מעבר על כל המילים שיש להם קשירות לנושא
            //לכל נושא יש מילון של מילים ומשקליהם וכו
            foreach (KeyValuePair<int, Dictionary<string, WordForSub>> sub in this.MarksWordsForSub)
            {
                foreach (WordForSub wordForSub in sub.Value.Values)
                {
                    if (wordForSub.IsCommon == true)//מילה משותפת
                    {
                        this.SearchTree[sub.Key].CommonWordsWeight += wordForSub.Weight * this.WordsAmount[wordForSub.Word];
                        //מוסיפים את כמות הפעמים שהמילה הופיעה במייל גם לכל הצאצאים כי היא מילה משותפת
                        AddSubjectWordAmount(sub.Key, this.WordsAmount[wordForSub.Word]);
                    }
                    else
                        this.SearchTree[sub.Key].SpecialWordsWeight += wordForSub.Weight * this.WordsAmount[wordForSub.Word];
                    //מעלים למקום של הנושא את סך הופעות המילה במייל
                    this.SubjetWordsAmount[sub.Key] += this.WordsAmount[wordForSub.Word];

                }
            }

            //כנ"ל רק למבנה של המילים שלא היו בדאטה והביאו להם מילים נרדפות
            foreach (KeyValuePair<int, Dictionary<string, semilarWordForSub>> sub in this.MarksSemilarsForSub)
            {
                foreach (semilarWordForSub wordForSub in sub.Value.Values)
                {
                    if (wordForSub.CommonAmount != 0) //אם היו מילים נרדפות שקימות במאגר בתור מילים משותפות 
                    {
                        this.SearchTree[sub.Key].CommonWordsWeight += (wordForSub.CommonWeight / wordForSub.CommonAmount)
                                                  * this.WordsAmount[wordForSub.Word];
                        //מוסיפים את כמות הפעמים שהמילה הופיעה במייל גם לכל הצאצאים כי היא מילה משותפת
                        AddSubjectWordAmount(sub.Key, this.WordsAmount[wordForSub.Word]);
                    }


                    if (wordForSub.SpecialAmount != 0)//אם היו מילים נרדפות שקימות במאגר בתור מילים מיוחדות
                        this.SearchTree[sub.Key].SpecialWordsWeight += (wordForSub.SpecialWeight / wordForSub.SpecialAmount)
                            * this.WordsAmount[wordForSub.Word];

                    //מעלים למקום של הנושא את סך הופעות המילה במייל
                    this.SubjetWordsAmount[sub.Key] += this.WordsAmount[wordForSub.Word];
                }
            }

            //זה נכון!!!!!!!!!
            //הוספת נתונים עבור המשך האלגוריתם
            //EmailsData.DataDict[email.toUserId].EmailsAmount++;
            //EmailsData.DataDict[email.toUserId].wordsAmount += wordList.Count;

        }

        //פונקציה רקורסיבית המקבלת קוד נושא וכמות מילה משותפת במייל
        //מוסיפה את הכמות לכל הצאצאים שמתחת לאותו נושא בגלל שהמילה משותפת
        private void AddSubjectWordAmount(int sub, int amount)
        {
            List<int> children = this.SearchTree[sub].ChildrenNodes;
            while (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    this.SubjetWordsAmount[children[i]] += amount;
                    AddSubjectWordAmount(children[i], amount);
                }
            }
        }


        // פונקציה המקבלת כותרת וגוף המייל 
        //ומחזירה רשימה של מילים "נקיות" לניתוח
        public List<string> CleanWords(string title, string body)
        {
            string words = title + " " + body;
            // (השמטת סמל מעקב המיילים (אם קיים  
            int trackingIndex = words.IndexOf("<https://mailtrack".ToString());
            if (trackingIndex >= 0)
                words = words.Substring(0, trackingIndex);
            //הורדת סימני פיסוק
            List<char> chars = new List<char> { '!', '"', ';', ':', '?', '.', ',', '(', ')', '\'', '\n', '\r','\t',
                '@', '#', '$', '%', '^', '&', '*', '<', '>', '\\', '/', '+', '\'', '{', '}', '[', ']', '-', '~', '`' };
            chars.ForEach(a => words = words.Replace(a.ToString(), " "));
            //חלוקת המילים לרשימה
            List<string> lst = words.Split(' ').ToList();
            //בדיקה האם יש מילים ריקות
            lst = lst.Where(x => x != "" && x != null).ToList();
            return lst;
        }






        //פונקציה מקבלת מילה ורשימת כל הקשירויות שלה עם כל הנושאים
        //מוסיפה במקום של הנושאים משקל לפי הקשירות  
        private void AddWeight(int wordId, string word, List<WordForSubjectDto> wordsForSubject)
        {
            // כל הקשירויות שלה עם כל הנושאים
            foreach (WordForSubjectDto wordForSub in wordsForSubject)
            {
                //הוספת המילה ומשקלה למבנה
                this.MarksWordsForSub[wordForSub.subjectId].Add(word, new WordForSub(word, (double)wordForSub.wordWeight));

                //בדיקה האם המילה משותפת
                if (wordForSub.isCommon)//מילה משותפת
                    //אם זה מילה משותפת צריך להראות שזה משותף
                    this.MarksWordsForSub[wordForSub.subjectId][word].IsCommon = true;
            }
        }




        //פונקציה המקבלת מילה ואת רשימת המילים הנרדפות לה ורשימה המכילה את כל הקשירויות בין המילים לנושאים 
        // ומוצאת את כל הנושאים שצריך להוסיף להם משקל בגללה
        private void AddWeightForSimilar(string baseWord, Dictionary<string, double> words, Dictionary<int, List<WordForSubjectDto>> allWordsForSubjects)
        {
            //מעבר על כל המילים הנרדפות
            foreach (KeyValuePair<string, double> word in words)// ------>  <שכיחות , מילה> 
            {
                //בדיקה האם המילה הנרדפת נמצאת במאגר
                int wordId = ClassDB.GetIDByWord(word.Key);

                //אם המילה לא נמצאת במאגר
                if (wordId == -1) continue;// ממשיך למילה הבאה

                //המילה נמצאת במאגר
                if (!allWordsForSubjects.ContainsKey(wordId))
                    continue;// המילה אינה קשורה לנושאים של המשתמש

                //המילה נמצאת במאגר וקשורה לנושא של המשתמש
                // כל הקשירויות של המילה עם כל הנושאים
                List<WordForSubjectDto> wordsForSubject = allWordsForSubjects[wordId];
                //מעבר על כל הקשר
                foreach (WordForSubjectDto wordForSub in wordsForSubject)
                {
                    //המבנה הכללי
                    if (!this.MarksSemilarsForSub[wordForSub.subjectId].ContainsKey(baseWord))
                    {//אם המילה עדיין לא קיימת במבנה
                        this.MarksSemilarsForSub[wordForSub.subjectId].Add(baseWord, new semilarWordForSub(baseWord));
                    }

                    //בדיקה האם המילה משותפת
                    if (wordForSub.isCommon)//מילה משותפת
                    {
                        //הוספת המשקל למילים המשותפות כפול אחוז הדימיון למילה המקורית
                        this.MarksSemilarsForSub[wordForSub.subjectId][baseWord].CommonWeight += (double)wordForSub.wordWeight * word.Value / 100;
                        this.MarksSemilarsForSub[wordForSub.subjectId][baseWord].CommonAmount++; //העלאת המונה של מספר המילים
                    }
                    else//המילה מיוחדת
                    {
                        //הוספת המשקל למילים המיוחדות כפול אחוז הדימיון למילה המקורית
                        this.MarksSemilarsForSub[wordForSub.subjectId][baseWord].SpecialWeight += (double)wordForSub.wordWeight * word.Value / 100;
                        this.MarksSemilarsForSub[wordForSub.subjectId][baseWord].SpecialAmount++;//העלאת המונה של מספר המילים
                    }
                }
            }
        }



    }//BuildFindObject




    public class WordForSub
    {
        //מבנה של מילה לנושא
        public WordForSub(string wordName)
        {
            this.Word = wordName;
            this.Weight = 0;
            this.IsCommon = false;
        }
        public WordForSub(string wordName, double weight) : this(wordName)
        {
            this.Weight = weight;
        }
        public WordForSub()
        {

        }
        public string Word { get; set; } // המילה
        public double Weight { get; set; } // משקלה
        public Boolean IsCommon { get; set; } // ??האם משותפת
    }

    public class semilarWordForSub
    {
        // (מבנה של מילה לנושא (מילה שלא נמצאת בדאטה 
        public semilarWordForSub(string wordName)
        {
            this.Word = wordName;
            this.SpecialWeight = 0;
            this.SpecialAmount = 0;
            this.CommonWeight = 0;
            this.CommonAmount = 0;
        }
        public semilarWordForSub()
        {

        }
        public string Word { get; set; } // המילה
        public double SpecialWeight { get; set; } // משקל מילים נרדפות מיוחדות שלה
        public int SpecialAmount { get; set; } // כמות המילים הנרדפות המיוחדות
        public double CommonWeight { get; set; } // משקל מילים נרדפות משותפות שלה
        public int CommonAmount { get; set; } // כמות המילים הנרדפות המשותפות
    }
}