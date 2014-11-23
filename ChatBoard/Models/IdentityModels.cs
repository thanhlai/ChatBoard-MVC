using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
/*
 * @author: Team Virus
 * @project: Chat Board
 * @setID: 4D
 * @courseID: COMP 4952
 * @instructors: Mirela Gutica & Medhat Elmasry 
 * **/
namespace ChatBoard.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [StringLength(25)]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }


        [StringLength(25)]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        public byte[] Avatar { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        
        //public virtual ICollection<Message> Messages { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

}