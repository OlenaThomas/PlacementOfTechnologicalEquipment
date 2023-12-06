using Microsoft.EntityFrameworkCore;
using placementOfTechnologicalEquipment.Models;

namespace PlacementOfTechnologicalEquipment.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ProductionRoom> ProductionRooms { get; set; }
        public DbSet<TechnologicalMachine> TechnologicalMachines { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
