using Microsoft.EntityFrameworkCore;
using WaRazorDB.Models;

namespace WaRazorDB.Data
{
    public class TareaDbContext : DbContext 
    {
        public TareaDbContext(DbContextOptions<TareaDbContext> options) : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }


        protected TareaDbContext()
        {
        }
    }
}
