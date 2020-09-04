using Microsoft.EntityFrameworkCore;

namespace crudelicious.Models
{
    public class crudeliciousContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter alongcopy
        public crudeliciousContext(DbContextOptions options) : base(options) { }
        public DbSet<Dish> Dishes { get; set; }
    }
}