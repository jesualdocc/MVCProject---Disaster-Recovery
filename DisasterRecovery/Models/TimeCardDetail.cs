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
    
    public partial class TimeCardDetail
    {
        public int IdTimeCard { get; set; }
        public int IdJobList { get; set; }
        public Nullable<int> Hours { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual JobList JobList { get; set; }
        public virtual TimeCard TimeCard { get; set; }
    }
}
