using Microsoft.EntityFrameworkCore;
using MyAppIGTI.Models;

namespace MyAppIGTI.Data
{
    public class DBMyAppContext : DbContext
    {
        public DBMyAppContext(DbContextOptions<DBMyAppContext> options) 
                : base (options)
        {
            
        } 

        public DbSet<ProfileTestModel> TabProfileTest { get; set; }

        public DbSet<ResultTestModel> TabResultTest { get; set; }

    }
}
