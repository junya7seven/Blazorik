using AuthDbExtension;
using Microsoft.EntityFrameworkCore;

namespace Blazorik.WebApi
{
    public class UserDbcontext : ApplicationDbContext<UserDbcontext>
    {
        public UserDbcontext(DbContextOptions<UserDbcontext> options) : base(options) { }
    }
}
