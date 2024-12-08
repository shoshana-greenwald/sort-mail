using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll.BuildStructs;

namespace Bll
{
    public static class FindSubject
    {
        public static Dictionary<int, FindNode> SearchTree; //עץ חיפוש
        public static Dictionary<int, double?> ContactmanWeight; //מבנה לאנשי קשר

        public static int FindMailSubject(int user, Dictionary<int, int> subjectWordsAmount, Dictionary<int, FindNode> searchTree, Dictionary<int, double?> contactmanWeight)
        {
            //השמה במשתנים הסטטים של המחלקה
            SearchTree = searchTree;
            ContactmanWeight = contactmanWeight;

            //בתחילה המילון שהתקבל מסמל יער של עצים וכאן מוצאים את הצומת עם הציון המקסימלי שממנה יתחיל החיפוש
            int maxNode = FindTreeRoot();
            //חיפוש מהצומת המקסימלית
            int maxSubject = FindSubectByTree(maxNode);

            //כאן ישנה התייחסות לנושא חדש שהמשקלים שלו עדיין נמוכים
            //אך אם כמות המילים שלו גוברת על כמות המילים של הנושא הנבחר אז הוא יהיה הנושא למייל
            //כל זאת בתנאי שהנושא שנבחר הוא לא נושא חדש בעצמו

            if (EmailsData.DataDict.ContainsKey(user))
            {
                //בדיקה האם הנושא הנוכחי אינו חדש
                if (ClassDB.GetTotalWordsForSub(maxSubject) >= (EmailsData.DataDict[user].wordsAmount / EmailsData.DataDict[user].EmailsAmount * 200))
                {
                    //מילון המכיל לכל נושא את כמות המילים שלו במאגר ממוין בסדר יורד לפי כמות המילים 
                    Dictionary<int, int> dict = subjectWordsAmount.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);

                    //מעבר על כל נושא
                    foreach (var sub in dict)
                    {
                        //בגלל שהמילון ממוין בסדר יורד אם הגעת לכמות מילים שקטנה מהכמות של הנושא הנבחר תחזיר את הנושא הנבחר
                        if (sub.Value <= dict[maxSubject])
                            return maxSubject;

                        //אם הוא הגיע לפה סימן שכמות המילים של הנושא הנוכחי יותר גדולה מהנושא הנבחר
                        //אם כמות המילים יותר גדולה וגם הנושא הוא נושא חדש אז תחזיר את הנושא הנוכחי
                        //נושא חדש זה נושא שמספר המילים שיש עליו במאגר קטן מממוצע של מילים למייל כפול 200
                        if (ClassDB.GetTotalWordsForSub(sub.Key) <= (EmailsData.DataDict[user].wordsAmount / EmailsData.DataDict[user].EmailsAmount * 200))
                            return sub.Key;
                    }
                }
            }
            return maxSubject;
        }



        // פונקציה שמוצאת את תת העץ המקסימלי שממנו ימשיך החיפוש
        // היא עושה זאת ע"י ששולחת את כל הנושאים לפונקציה שמתחתיה
        private static int FindTreeRoot()
        {
            //FindMaxNode :איתחול רשימה של הקודים של כל הנושאים בשביל להשתמש בפונקיה 
            List<int> subIdList = new List<int>();
            //מעבר על כל העץ
            foreach (var sub in FindSubject.SearchTree)
                subIdList.Add(sub.Key);
            //החזרת הבן המקסימלי - הציון לא נדרש
            return FindMaxNode(subIdList).maxSub;
        }



        //מקבלת רשימת נושאים 
        //מגלה את הנושא המקסימלי ע"י חישוב משקלי המילים בשילוב האיש קשר
        private static (int maxSub, double maxMark) FindMaxNode(List<int> nodes)
        {
            double maxMark = 0;
            int maxSub = 0;

            //מעבר כל כל הצמתים של הנושאים שנשלחו
            foreach (int subject in nodes)
            {
                //חישוב משקל לנושא מסוים לפי משקלי המילים המשותפות והמיוחדות
                double mark = (FindSubject.SearchTree[subject].CommonWordsWeight + FindSubject.SearchTree[subject].SpecialWordsWeight) * 0.85;
                //מוסיפים את משקל איש הקשר
                mark += (double)FindSubject.ContactmanWeight[subject] * 0.15;
                //אם זה גובר על המקסימום
                if (mark > maxMark)
                {
                    maxMark = mark;
                    maxSub = subject;
                }
            }
            return (maxSub, maxMark);  //  ------>  נושא בעל הציון המקסימלי
        }




        //פונקציה רקורסיבית לגילוי הנושא המקסימלי מתת עץ
        //מקבלת את קוד האבא ומוצאת את הצאצא המקסימלי מהאבא והלאה
        private static int FindSubectByTree(int parentSub)
        {
            //הבנים של האבא שנשלח
            List<int> children = FindSubject.SearchTree[parentSub].ChildrenNodes;
            //במקרה שאין לו בנים
            if (children.Count == 0)
                return parentSub;
            //משקל של האבא
            double parentWeight = (double)((FindSubject.SearchTree[parentSub].SpecialWordsWeight * 0.85) + (FindSubject.ContactmanWeight[parentSub] * 0.15));
            //מציאת הבן עם המשקל המקסימלי מביו כל הבנים
            (int maxSonId, double maxSonMark) = FindMaxNode(children);
            //אם האב גובר על הבן
            if (parentWeight > maxSonMark)
                return parentSub;
            //אם הבן גובר שולחים את קוד הבן המקסימלי שיתחיל לחפש ממנו
            else return FindSubectByTree(maxSonId);
        }

    }



}
