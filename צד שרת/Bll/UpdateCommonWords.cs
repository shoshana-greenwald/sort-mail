using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public static class UpdateCommonWords
    {
        //מאפיין מחלקה סטטי המכיל את רשימת הנושאים אותם שולחים לפונקציית ChiSequre לבדוק האם המילה משותפת לכל הנושאים
        static List<Statistic> a = new List<Statistic>();


        //מקבלת קוד משתמש
        //מעדכנת את המילים שקשורות לנושאים שלו לפי הצורך כמילים משותפות
        public static void UpdateLevelWord(int userId)
        {
            BinaryTree t = new BinaryTree(); //בניית עץ נושאים
            t = BuildTree(ClassDB.GetSubjectDict(userId));
            List<WordDto> lWords = ClassDB.GetAllWords();//מילים במאגר לעדכון 
            foreach (WordDto item in lWords)
            {
                //מילון המכיל לכל נושא כמה המילה הופיע בו וכמה לא
                Dictionary<int, Statistic> dicStatistic = ClassDB.GetStatisForWord(item.wordId); // שליחה לפונקצייה המעדכנת את המילה
                UpdateJointWord(t.Root, dicStatistic, item.wordId);
                a.Clear();
            }
        }


        //פונקציה המחזירה את עץ הנושאים המיוצג באמצעות עץ בינראי
        //מקבלת מילון המכיל את כול הנושאים ועבור כל נושא יש קוד וקוד של אביו
        //המילון ממוין בסדר עולה לפי הרמה של הנושא בעץ
        public static BinaryTree BuildTree(Dictionary<int, int?> subDic)
        {
            //לשלוח מילון ממוין לפי הרמה

            BinaryTree tree = new BinaryTree();
            //האיבר הראשון יהיה השורש
            tree.Root = new BinaryNode(subDic.ElementAt(0).Key, subDic.ElementAt(0).Value);
            //עוברים על כל הנושאים
            for (int i = 1; i < subDic.Count; i++)
            {
                //יצירת צומת חדשה
                BinaryNode node = new BinaryNode(subDic.ElementAt(i).Key, subDic.ElementAt(i).Value);
                //שליחה לפונקציה המוצאת את צומת האב
                BinaryNode parent = GetParentNode(tree.Root, node);

                //אם זה נושא ללא אבא
                //מכניסים אותו מימין לשורש
                if (parent == null)
                {
                    node.Parent = tree.Root;
                    //אם לשורש יש כבר אחים
                    if (tree.Root.Right != null)
                    {
                        node.Right = tree.Root.Right;
                        node.Right.Parent = node;
                    }
                    tree.Root.Right = node;
                }
                else
                {   //אם זה אבא שלו סימן שהוא בן ראשון לכן תכניס אותו לצד שמאל
                    if (parent.SubjectID == node.ParentSub)
                        parent.Left = node;
                    //אם זה לא אבא שלו זה האח הראשון שלו לכן תכניס אותו לימינו
                    else
                    {
                        //אם יש לו עוד אחים
                        if (parent.Right != null)
                        {
                            node.Right = parent.Right;
                            node.Right.Parent = node;
                        }
                        parent.Right = node;
                    }
                    node.Parent = parent;
                }
            }
            return tree;
        }


        //פונקציה המקבלת שורש עץ וצומת ומחזירה את האבא של הצומת שנשלחה
        public static BinaryNode GetParentNode(BinaryNode tree, BinaryNode node)
        {
            //אם העץ ריק
            if (tree == null)
                return null;
            //מצא את אבא שלו
            if (tree.SubjectID == node.ParentSub)
            {
                //במקרה שאין לאבא עוד בנים
                if (tree.Left == null)
                    return tree;
                //במקרה שיש עוד בנים תחזיר את הבן הראשון שלו
                return tree.Left;
            }
            //תחפש את האבא שלו בצד שמאל וימין של העץ
            BinaryNode l = GetParentNode(tree.Left, node);
            BinaryNode r = GetParentNode(tree.Right, node);
            if (l != null)
                return l;
            return r;
        }


        //פונקציה סטטית המקבלת עץ נושאים, רשימה המציינת לכל נושא כמה פעמים מופיע/לא באותם הנושאים, מילה לעדכון
        public static void UpdateJointWord(BinaryNode n, Dictionary<int, Statistic> dicStatistic, int idWord)
        {
            if (n == null) return;
            UpdateJointWord(n.Left, dicStatistic, idWord);
            UpdateJointWord(n.Right, dicStatistic, idWord);
            a.Add(dicStatistic[n.SubjectID]);
            //במצב בו המילה הגיע לראש העץ ז"א שהיא משותפת לכל הנושאים וע"כ היא מייצגת מילה שלא משפיע למציאת נושא
            if (n.Parent == null)
            {
                if (ChiSquare.Chi(a) > 0.05)//אם יחודי
                    ClassDB.DeleteWord(idWord);
            }
            else
            {
                if (n.Parent.Left == n)//אם הגעתי משמאל
                {
                    if (a.Count > 1)//ויש לי כמה אחים
                    {
                        if (ChiSquare.Chi(a) > 0.05) //מילה משותפת
                        {
                            //אוביקט מילה לנושא
                            WordForSubjectDto wordfs = new WordForSubjectDto();
                            wordfs.wordId = idWord;
                            wordfs.isCommon = true; //!משותף
                            double weight = 0;
                            //במצב בו במילה הייתה משותפת בעבר לבנים של אותו נושא
                            //מוסיפה גם את האבא לרשימה
                            a.Add(dicStatistic[(int)n.ParentSub]);
                            //עוברת על האבא והבנים שלו
                            foreach (Statistic sub in a)
                            {
                                //בודקת האם המילה בכלל קיימת בנושא הזה
                                double? w = ClassDB.GetWeightWordForSubjectByWordAndSub(sub.subjectId, idWord);
                                if (w != null)
                                {
                                    //מוסיפה את המשקל 
                                    weight += (double)w;
                                    //הפונקציה מוחקת את המילה מהנושא 
                                    ClassDB.DeleteWordFromSub(sub.subjectId, idWord);
                                }
                            }
                            wordfs.wordWeight = weight;
                            ClassDB.AddWordForSubject(wordfs);
                            // במצב בו הוחלט כי המילה משותפת לנושא ממשיכים לבדוק האם המילה משותפת גם לאחיו. אנו מעדכנים את האובייקט המייצג
                            // את כמות הופעות המילה באותו נושא ומוסיפים לו את כמות הופעות המילה בנושאי בניו.
                            //מוציאה את האבא
                            a.Remove(dicStatistic[(int)n.ParentSub]);
                            foreach (var item in a)
                            {
                                dicStatistic[n.Parent.SubjectID].No += item.No;
                                dicStatistic[n.Parent.SubjectID].Yes += item.Yes;
                            }
                        }
                    } //במצב בו יש רק נושא בן יחיד כך שאין אפשרות לבדוק אותו יחד עם אחיו לכן נבדוק אותו עם אביו
                    else
                    {
                        a.Add(dicStatistic[n.Parent.SubjectID]);
                        double x = ChiSquare.Chi(a);
                        if (ChiSquare.Chi(a) > 0.05) //מילה משותפת
                        {
                            //אוביקט מילה לנושא
                            WordForSubjectDto wordfs = new WordForSubjectDto();
                            wordfs.subjectId = n.Parent.SubjectID;
                            wordfs.wordId = idWord;
                            wordfs.isCommon = true;//!משותף
                            double weight = 0;
                            foreach (Statistic sub in a)
                            {
                                //בודקת האם המילה בכלל קיימת בנושא הזה
                                double? w = ClassDB.GetWeightWordForSubjectByWordAndSub(sub.subjectId, idWord);
                                if (w != null)
                                {
                                    //מוסיפה את המשקל 
                                    weight += (double)w;
                                    //הפונקציה מוחקת את המילה מהנושא 
                                    ClassDB.DeleteWordFromSub(sub.subjectId, idWord);
                                }
                            }
                            wordfs.wordWeight = weight;
                            ClassDB.AddWordForSubject(wordfs);

                            //מוסיפה את מספר ההופעות של הבן
                            dicStatistic[n.Parent.SubjectID].No += a[0].No;
                            dicStatistic[n.Parent.SubjectID].Yes += a[0].Yes;
                        }
                    }
                    a.Clear();
                }
            }
        }



    }











    public class BinaryNode
    {
        public BinaryNode(int id, int? parentID)
        {
            this.SubjectID = id;
            this.ParentSub = parentID;
            this.Parent = null;
            this.Right = null;
            this.Left = null;
            this.Level = ClassDB.GetSubjectLevel(SubjectID);
        }

        public BinaryNode Parent { get; set; }
        public BinaryNode Right { get; set; }
        public BinaryNode Left { get; set; }
        public int Level { get; set; }
        public int SubjectID { get; set; }
        public int? ParentSub { get; set; }

    }

    public class BinaryTree
    {
        public BinaryTree()
        {
            this.Root = null;
        }
        public BinaryNode Root { get; set; }
    }



}
