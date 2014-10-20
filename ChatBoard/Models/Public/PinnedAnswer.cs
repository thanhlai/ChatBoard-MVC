using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatBoard.Models.Public
{
    public class PinnedAnswer
    {
        [ScaffoldColumn(false)]
        public int PinnedAnswerID { get; set; }

        public int BoardID { get; set; }

        //Optional
        public int UserID { get; set; }
        
        [Required]
        public string UserName { get; set; }
    }
}