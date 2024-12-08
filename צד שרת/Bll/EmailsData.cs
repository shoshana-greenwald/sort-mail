using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public static class EmailsData
    {
        //Data מילון של משתמשים במערכת, לכל משתמש אוביקט מסוג 
        public static Dictionary<int, Data> DataDict = new Dictionary<int, Data>();
    }


    public class Data
    {
        public int EmailsAmount { get; set; } // מספר האימיילים שקיבל המשתמש עד כה
        public int wordsAmount { get; set; } // מספר המילים שהכילו
        public Data(int emails,int words)
        {
            this.EmailsAmount = emails;
            this.wordsAmount = words;
        }
    }
}
