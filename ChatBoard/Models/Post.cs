using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatBoard.Models
{

    //public class Tag
    //{
    //    [Key]
    //    public int TagId { get; set; }
    //    public string Title { get; set; }
    //}


    //public class Board_Tag
    //{
    //    [Key, ForeignKey("BoardModel")]
    //    public int BoardId { get; set; }

    //    [Key, ForeignKey("Tag")]
    //    public int TagId { get; set; }

    //    public virtual BoardModel Board { get; set; }   // Keeps track of the board that this tag is belong to
    //    public virtual Tag Tag { get; set; }            // Keeps track of the tag
    //}


    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(128), ForeignKey("ApplicationUser")]
        public virtual string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }


    public class Comment
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}