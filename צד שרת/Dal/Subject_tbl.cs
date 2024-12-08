//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subject_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subject_tbl()
        {
            this.AlgorithmMistake_tbl = new HashSet<AlgorithmMistake_tbl>();
            this.AlgorithmMistake_tbl1 = new HashSet<AlgorithmMistake_tbl>();
            this.ContactForSubject_tbl = new HashSet<ContactForSubject_tbl>();
            this.Email_tbl = new HashSet<Email_tbl>();
            this.Subject_tbl1 = new HashSet<Subject_tbl>();
            this.UpdateKeySentence_tbl = new HashSet<UpdateKeySentence_tbl>();
            this.UpdateKeySentenceForSemilar_tbl = new HashSet<UpdateKeySentenceForSemilar_tbl>();
            this.WordForSubject_tbl = new HashSet<WordForSubject_tbl>();
        }
    
        public int subjectId { get; set; }
        public string subjectName { get; set; }
        public Nullable<int> parentID { get; set; }
        public int userId { get; set; }
        public string color { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlgorithmMistake_tbl> AlgorithmMistake_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlgorithmMistake_tbl> AlgorithmMistake_tbl1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactForSubject_tbl> ContactForSubject_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Email_tbl> Email_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subject_tbl> Subject_tbl1 { get; set; }
        public virtual Subject_tbl Subject_tbl2 { get; set; }
        public virtual User_tbl User_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UpdateKeySentence_tbl> UpdateKeySentence_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UpdateKeySentenceForSemilar_tbl> UpdateKeySentenceForSemilar_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WordForSubject_tbl> WordForSubject_tbl { get; set; }
    }
}
