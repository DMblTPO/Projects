using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Qalsql.Models.Db
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Group { get; set; }

        [Required]
        public TrainingMode TrainingMode { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public enum TrainingMode
    {
        [Display(Name="Evening")]
        Evening = 0,
        [Display(Name="Day")]
        Day = 1,
        [Display(Name="Weekend")]
        Weekend = 2
    }

    public partial class QalSqlContext : IdentityDbContext<ApplicationUser>
    {
        public QalSqlContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
        }

        public static QalSqlContext Create()
        {
            return new QalSqlContext();
        }
    }
}