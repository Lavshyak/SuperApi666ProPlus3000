using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperApi666ProPlus3000.BackendModels;

namespace SuperApi666ProPlus3000.DbContexts
{
    public class MainDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            
        }

        /*
 public virtual DbSet<(User)TUser> Users { get; set; } = default!; //IdentityDbContext-> IdentityDbContext -> IdentityUserContext.Users

 from IdentityDbContext-> IdentityDbContext:
 public virtual DbSet<TUserClaim> UserClaims { get; set; } = default!;
 public virtual DbSet<TUserLogin> UserLogins { get; set; } = default!;
 public virtual DbSet<TUserToken> UserTokens { get; set; } = default!;
*/

        public DbSet<Poop> Poops { get; set; } = default!;

        //public DbSet<User> Users { get; set; } //удалить
    }
}










