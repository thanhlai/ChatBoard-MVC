//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatBoard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoggedInUser
    {
        public string LoggedInUserId { get; set; }
        public string UserId { get; set; }
        public string BoardId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Board Board { get; set; }
    }
}
