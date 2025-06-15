using Microsoft.EntityFrameworkCore;
using cargo_flow_backend.Entities;

namespace cargo_flow_backend.Data
{
    public class CargoFlowDbContext : DbContext
    {
        public CargoFlowDbContext(DbContextOptions<CargoFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}