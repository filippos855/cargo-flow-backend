// CargoFlowDbContext.cs - configurat complet cu Fluent API
using cargo_flow_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Data
{
    public class CargoFlowDbContext : DbContext
    {
        public CargoFlowDbContext(DbContextOptions<CargoFlowDbContext> options) : base(options) { }

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
            // === Company ===
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Code).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Cui).HasMaxLength(10);
                entity.Property(c => c.Address).HasMaxLength(200);

                entity.HasOne(c => c.ContactPerson)
                      .WithMany(p => p.ContactForCompanies)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // === Dictionary ===
            modelBuilder.Entity<Dictionary>(entity =>
            {
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            });

            // === DictionaryItem ===
            modelBuilder.Entity<DictionaryItem>(entity =>
            {
                entity.Property(di => di.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(di => di.Dictionary)
                      .WithMany(d => d.Items)
                      .HasForeignKey(di => di.DictionaryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === FleetVehicle ===
            modelBuilder.Entity<FleetVehicle>(entity =>
            {
                entity.Property(fv => fv.Identifier).IsRequired().HasMaxLength(50);
                entity.Property(fv => fv.ItpExpiration).IsRequired();
                entity.Property(fv => fv.RcaExpiration).IsRequired();
                entity.HasOne(fv => fv.Type).WithMany();
            });

            // === Invoice ===
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(i => i.Number).IsRequired().HasMaxLength(50);
                entity.Property(i => i.IssueDate).IsRequired();
                entity.Property(i => i.DueDate).IsRequired();
                entity.Property(i => i.Amount).HasPrecision(18, 2);
                entity.Property(i => i.Currency).IsRequired().HasMaxLength(10);

                entity.HasOne(i => i.InvoiceType).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(i => i.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(i => i.Company).WithMany(c => c.Invoices).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(i => i.Order).WithMany(o => o.Invoices).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(i => i.Trip).WithMany(t => t.Invoices).OnDelete(DeleteBehavior.SetNull);
            });

            // === Order ===
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.Number).IsRequired().HasMaxLength(50);
                entity.Property(o => o.CreatedDate).IsRequired();
                entity.Property(o => o.Address).IsRequired().HasMaxLength(200);

                entity.HasOne(o => o.Company).WithMany(c => c.Orders).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.DeliveryPerson).WithMany(p => p.DeliveryOrders).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Trip).WithMany(t => t.Orders).OnDelete(DeleteBehavior.SetNull);
            });

            // === Person ===
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Cnp).IsRequired().HasMaxLength(13);
                entity.Property(p => p.Phone).HasMaxLength(20);
                entity.Property(p => p.Email).HasMaxLength(100);
            });

            // === Trip ===
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.Property(t => t.Number).IsRequired().HasMaxLength(50);
                entity.Property(t => t.StartDate).IsRequired();

                entity.HasOne(t => t.Status).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.TransportCompany).WithMany(c => c.TransportedTrips).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.Driver).WithMany(p => p.DrivenTrips).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(t => t.TractorUnit).WithMany(f => f.AsTractorUnitInTrips).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(t => t.Trailer).WithMany(f => f.AsTrailerInTrips).OnDelete(DeleteBehavior.SetNull);
            });

            // === User ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.IsActive).HasDefaultValue(true);

                entity.HasOne(u => u.Role).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Person).WithMany(p => p.Users).OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
