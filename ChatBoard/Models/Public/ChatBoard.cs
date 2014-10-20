using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatBoard.Models.Public
{
    // https://www.junnark.com/Content/BlogImages/LinkChatroomTables.jpg
    public class ChatBoard
    {
        [ScaffoldColumn(false)]
        public int BoardID { get; set; }

        [Display(Name = "Topic")]
        public string Topic { get; set; }


        public virtual ICollection<UserInTheBoard> UsersInTheBoard { get; set; }

        public virtual ICollection<PinnedAnswer> PinnedAnswers { get; set; }
    }
}