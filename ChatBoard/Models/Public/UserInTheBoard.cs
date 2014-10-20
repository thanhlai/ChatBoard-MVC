using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatBoard.Models.Public
{
    public class UserInTheBoard
    {
        [ScaffoldColumn(false)]
        public int UserID { get; set; }


        [Required, MaxLength(25), Display(Name = "Username")]
        public string Username { get; set; }

        public int BoardID { get; set; }
        
    }
}