using Microsoft.EntityFrameworkCore;
using SiswaApi.Models;

namespace SiswaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Siswa> Siswas { get; set; }
    }
}
