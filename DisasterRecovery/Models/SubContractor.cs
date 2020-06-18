//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisasterRecovery.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubContractor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubContractor()
        {
            this.JobLists = new HashSet<JobList>();
            this.TimeCards = new HashSet<TimeCard>();
            this.Users = new HashSet<User>();
        }
    
        public int IdSubContractor { get; set; }
        public string SubContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> SubContractorStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobList> JobLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeCard> TimeCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
