//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeITDL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLoginAudit
    {
        public System.Guid Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string PcName { get; set; }
        public string PcUserName { get; set; }
        public string PcIpAddress { get; set; }
    }
}
