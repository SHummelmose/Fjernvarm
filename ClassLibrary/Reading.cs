//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reading
    {
        public int ID { get; set; }
        public int KilowattHours { get; set; }
        public int CubicMeters { get; set; }
        public int Hours { get; set; }
        public string Year { get; set; }
        public int GaugeID { get; set; }
    
        public virtual Gauge Gauge { get; set; }
    }
}
