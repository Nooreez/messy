using Microsoft.EntityFrameworkCore;

namespace Messenger.Models
{
    public class MessContext : DbContext
    {
        public MessContext(DbContextOptions<MessContext> options): base(options){}

        public DbSet<Users> Users { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<FriendsRequest> FriendsRequest { get; set; }
        public DbSet<Friends> Friends { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}
