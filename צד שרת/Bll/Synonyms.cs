using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    // אחרי התבוננות לקחתי את המילים שהדימיון שלהם למילה עולה על 50 אחוז והשכיחות שלהם בטוויטר גדולה מ100
    static class Synonyms
    {
        //מקבלת מילה
        // מחזירה את כל המילים הנרדפות שלה במילון: מילה-אחוז דימיון
        public static Dictionary<string, double> GetSimilarWords(string word)
        {   
            Dictionary<string, double> similarWords = new Dictionary<string, double>();

            string htmlCode;
            using (WebClient client = new WebClient())
            {
                try
                {
                var htmlData = client.DownloadData("https://u.cs.biu.ac.il/~yogo/tw2v/similar/" + word);
                htmlCode = Encoding.UTF8.GetString(htmlData);
                }
                catch (Exception)
                {
                    return similarWords;
                }
            }


            if (htmlCode.Contains("is unknown, sorry"))
                return similarWords;

            string[] s = htmlCode.Split(new string[] { "/td>" }, StringSplitOptions.None);

            for (int z = 0, i = 3; z < 20; z++, i += 3)
            {
                if (i + 2 > s.Length)
                    return similarWords;
                int frequencyS = s[i + 2].IndexOf("'>");
                frequencyS += 2;
                int frequencyE = s[i + 2].IndexOf("<", frequencyS);
                int frequency = (int.Parse)(s[i + 2].Substring(frequencyS, frequencyE - frequencyS));
                if (frequency < 100) //אם כמות הפעמים שהמילה הופיעה טוויטר קטנה ממאה אל תחזיר אותה
                    continue;
                int startN1 = s[i].IndexOf("color");
                int startN = s[i].IndexOf(">", startN1);
                startN++;
                int endN = s[i].IndexOf("<", startN);
                double percent = (double.Parse)(s[i].Substring(startN, endN - startN));
                // אם אחוז הדימיון של המילה הנרדפת למילה המקורית קטן מחמישים אל תחזיר אותה
                if (percent < 50)
                    // המילים ממוניות על פי אחוז הדמיון מהגדול לקטן אז גם המילים הבאות יהיו קטנות מ50
                    break;        
                int cut = s[i + 1].IndexOf("/similar");
                cut += 9;
                string wordd = s[i + 1].Substring(cut);
                wordd = wordd.Substring(0, wordd.Length - 6);
                int end = wordd.IndexOf(">");
                wordd = wordd.Substring(0, end - 1);
                similarWords.Add(wordd, percent); //  ------>  <אחוז דימיון , מילה>  
            }
            return similarWords;
        }

    }
}
