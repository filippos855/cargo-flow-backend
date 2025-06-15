using cargo_flow_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Data
{
    public class CargoFlowDbContext : DbContext
    {
        public CargoFlowDbContext(DbContextOptions<CargoFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<FleetVehicle> FleetVehicles { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryItem> DictionaryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DictionaryItem <-> Dictionary
            modelBuilder.Entity<DictionaryItem>()
                .HasOne(di => di.Dictionary)
                .WithMany(d => d.Items)
                .HasForeignKey(di => di.DictionaryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - Trip
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Trip)
                .WithMany(t => t.Orders)
                .OnDelete(DeleteBehavior.SetNull);

            // Order - Company
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Company)
                .WithMany(c => c.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - DeliveryPerson
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryPerson)
                .WithMany(p => p.DeliveryOrders)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - Status
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice - Order
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Invoices)
                .OnDelete(DeleteBehavior.SetNull);

            // Invoice - Trip
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Trip)
                .WithMany(t => t.Invoices)
                .OnDelete(DeleteBehavior.SetNull);

            // Invoice - Company
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Company)
                .WithMany(c => c.Invoices)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice - Status
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice - Type
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.InvoiceType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);

            // Trip - TransportCompany
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.TransportCompany)
                .WithMany(c => c.TransportedTrips)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip - Driver
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany(p => p.DrivenTrips)
                .OnDelete(DeleteBehavior.SetNull);

            // Trip - TractorUnit
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.TractorUnit)
                .WithMany(f => f.AsTractorUnitInTrips)
                .OnDelete(DeleteBehavior.NoAction);

            // Trip - Trailer
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Trailer)
                .WithMany(f => f.AsTrailerInTrips)
                .OnDelete(DeleteBehavior.SetNull);

            // Trip - Status
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Company - ContactPerson
            modelBuilder.Entity<Company>()
                .HasOne(c => c.ContactPerson)
                .WithMany(p => p.ContactForCompanies)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // User - Person
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}