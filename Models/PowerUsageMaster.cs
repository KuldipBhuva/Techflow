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
    
    public partial class PowerUsageMaster
    {
        public int PUID { get; set; }
        public Nullable<int> CompID { get; set; }
        public string DataSuite { get; set; }
        public string RackID { get; set; }
        public string PowerFeeds { get; set; }
        public string AmpereRating { get; set; }
        public Nullable<decimal> PowerContracted { get; set; }
        public Nullable<decimal> PowerUsed { get; set; }
        public string OverUnder { get; set; }
        public Nullable<System.DateTime> LastReading { get; set; }
    }
}
