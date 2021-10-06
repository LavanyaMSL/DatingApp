using Microsoft.EntityFrameworkCore;
using API.Entitites;

namespace API.Data
{
    public class IuserRepository: DbContext
    {
     public IuserRepository(DbContextOptions options):base(options)
     {

     }  
     public DbSet<AppUser> Users { get; set; } 
    }
}