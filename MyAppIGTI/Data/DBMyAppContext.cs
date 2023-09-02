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

        /* -1 = Running
         *  0 - Not Running
         *  1 - Success
         *  2 - Fail
        */

    }
}
