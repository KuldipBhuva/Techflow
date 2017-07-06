//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuotationMaster
    {
        public int QID { get; set; }
        public Nullable<int> DataCenter { get; set; }
        public string CableSource { get; set; }
        public string CableDest { get; set; }
        public string CableType { get; set; }
        public string TerminationSource { get; set; }
        public string TerminationDest { get; set; }
        public Nullable<System.DateTime> ReqDelDate { get; set; }
        public Nullable<bool> IsPremium { get; set; }
        public Nullable<bool> IsPatchPanel { get; set; }
        public string PatchPanel { get; set; }
        public Nullable<bool> IsMediaConverter { get; set; }
        public string MediaConverter { get; set; }
        public string Comments { get; set; }
        public string ServiceReq { get; set; }
        public Nullable<System.DateTime> DateForQuote { get; set; }
        public Nullable<System.DateTime> DateForService { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> TicketID { get; set; }
        public string Prefix { get; set; }
    }
}