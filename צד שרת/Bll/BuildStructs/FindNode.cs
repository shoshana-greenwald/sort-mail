using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.BuildStructs
{
    public class FindNode
    {
        public FindNode(int subId)
        {
            this.SubjectId = subId;

            //אבא של הנושא
            this.ParentNode = ClassDB.GetSubjectByID(subId).parentID;

            //רשימת ילדים
            this.ChildrenNodes = ClassDB.GetSubjectsIDByParentId(subId);
            this.SpecialWordsWeight = 0;
            this.CommonWordsWeight = 0;
        }

        public int SubjectId { get; set; }
        public double CommonWordsWeight { get; set; }
        public double SpecialWordsWeight { get; set; }
        public List<int> ChildrenNodes { get; set; }
        public int? ParentNode { get; set; }


    }
}
