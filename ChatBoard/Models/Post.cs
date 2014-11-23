﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
/*
 * @author: Team Virus
 * @project: Chat Board
 * @setID: 4D
 * @courseID: COMP 4952
 * @instructors: Mirela Gutica & Medhat Elmasry 
 * **/
namespace ChatBoard.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateCreated { get; set; }
        [Required]
        public string Content { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public int Loves { get; set; }
        public string Owner { get; set; }   // use for in-site search optimization
        [MaxLength(128), ForeignKey("ApplicationUser")]
        public virtual string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        //public ICollection<Message> Messages { get; set; }
    }


    //public class Message
    //{
    //    public int Id { get; set; }
    //    [Column(TypeName = "DateTime2")]
    //    public DateTime DateCreated { get; set; }
    //    [Required]
    //    public string Content { get; set; }
    //    [MaxLength(128), ForeignKey("ApplicationUser")]
    //    public virtual string UserId { get; set; }
    //    public virtual ApplicationUser ApplicationUser { get; set; }
    //    public int PostId { get; set; }
    //    public Post Post { get; set; }
    //}

    //public class Tag
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public int PostId { get; set; }
    //    public Post Post { get; set; }
    //}
}