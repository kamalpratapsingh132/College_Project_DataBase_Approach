//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace College_Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teacher
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Teach_Subject { get; set; }
        public string Mob_No { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public string city { get; set; }
        public string Address { get; set; }
        public Nullable<int> ST_ID { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
