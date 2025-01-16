using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bll
{
    public static class HebrewNlp
    {


        //פונקציה המקבלת רשימה של מילים ומחזירה רשימה של מילות הבסיס שלהן
        public static List<string> NlpForWords(List<string> words)
        {
            List<string> lst = new List<string>();
            foreach (string word in words)
            {
                //מציאת מילת הבסיס
                string baseWord = NlpSingleWord(word);
                if (baseWord != null) //זאת לא בעיה אם תיהיה אותה מילה הרבה פעמים
                    lst.Add(baseWord);
            }
            return lst;
        }


        //פונקציה המקבלת מילון של מילים נרדפות ומחזירה מילון עם מילות הבסיס שלהן
        public static Dictionary<string, double> NlpForSimilarWords(Dictionary<string, double> words)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> word in words)
            {
                //מציאת מילת הבסיס
                string baseWord = NlpSingleWord(word.Key);
                //אם זה עדיין לא נמצא במילון
                if (baseWord != null && !dict.ContainsKey(baseWord))
                    dict.Add(baseWord, word.Value);
            }
            return dict;
        }


        // nlp מקבלת מילה ומחזירה את מילת הבסיס שלה על ידי 
        public static string NlpSingleWord(string word)
        {
            //בדיקה האם כבר ניתחנו את המילה בעבר
            string bw = ClassDB.GetBaseWordByWord(word);
            if (bw != null)
                return bw;

            //אם המילה כבר נמצאת במאגר לא צריך לנקות אותה
            if (ClassDB.GetIDByWord(word) != -1)
                return word;
            //מחזיר מלא אופציות למילת בסיס
            //לוקחת את ההצעה הראשונה כי אין חוקיות
            HebrewNLP.Morphology.MorphInfo wordInfo = AnalyzeWord(word);
            if (wordInfo == null)
                return null;
            string baseWord = wordInfo.BaseWord;
            HebrewNLP.Morphology.PartOfSpeech partOfSpeech = wordInfo.PartOfSpeech;

            //לא את כל הסוגים- מחזירים רק את מה שיעזור בניתוח- 
            //אחרי התבוננות ארוכה---- שמות עצם פעלים ותארים
            //אבל אם זה נמצא בדאטה כן תחזיר את המילה
            if (partOfSpeech != HebrewNLP.Morphology.PartOfSpeech.NOUN && partOfSpeech != HebrewNLP.Morphology.PartOfSpeech.VERB 
                && partOfSpeech != HebrewNLP.Morphology.PartOfSpeech.ADJECTIVE && ClassDB.GetIDByWord(word) == -1)
                return null;

            //החלפת אותיות סופיות כי האנאלפי לא מחזיר עם אותיות סופיות
                 if (baseWord.EndsWith("כ")) baseWord = baseWord.Substring(0, baseWord.Length - 1) + "ך";
            else if (baseWord.EndsWith("מ")) baseWord = baseWord.Substring(0, baseWord.Length - 1) + "ם";
            else if (baseWord.EndsWith("נ")) baseWord = baseWord.Substring(0, baseWord.Length - 1) + "ן";
            else if (baseWord.EndsWith("פ")) baseWord = baseWord.Substring(0, baseWord.Length - 1) + "ף";
            else if (baseWord.EndsWith("צ")) baseWord = baseWord.Substring(0, baseWord.Length - 1) + "ץ";

            //הכנסת המילה המנותחת למאגר
            ClassDB.AddBaseWord(new Dto.BaseWordDto() { word = word, @base = baseWord });

            return baseWord;
        }


        //מקבלת מילה
        //מתחברת לספרייה ומחזירה מילת בסיס
        public static HebrewNLP.Morphology.MorphInfo AnalyzeWord(string word)
         {
            // nlp איתחול 
            HebrewNLP.HebrewNLP.Password = "YOUR_PASSWORD"; // אתה צריך להכניס לכאן את הסיסמה האישית שלך לחשבון 
            //מחזיר מלא אופציות למילת בסיס
            //לוקחת את ההצעה הראשונה כי אין חוקיות
            List<List<List<HebrewNLP.Morphology.MorphInfo>>> wordInfo = HebrewNLP.Morphology.HebrewMorphology.AnalyzeText(word);
            if (wordInfo!=null)
                if (wordInfo[0] != null)
                    if (wordInfo[0][0] != null)
                        return wordInfo[0][0][0];
            return null;
        }
        

        //מקבלת טקסט 
        //מתחברת לספריה ומחזירה רשימה של הטקסט מחולק למשפטים
        public static List<string> SpliteToSentences(string words)
        {
            HebrewNLP.HebrewNLP.Password = "YOUR_PASSWORD";   // אתה צריך להכניס לכאן את הסיסמה האישית שלך לחשבון 
            //שליחה לפומקציה שמחלקת טקסט למשפטים
            List<string> sentencesList = HebrewNLP.Sentencer.Sentences(words);
            return sentencesList;
        }
    }
}
