//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobWork
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourtUser
    {
        public int Id { get; set; }
        public Nullable<int> ActiveId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<bool> ReadyForLogin { get; set; }
        public Nullable<bool> Logined { get; set; }
        public Nullable<bool> Authorized { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
    }
}
