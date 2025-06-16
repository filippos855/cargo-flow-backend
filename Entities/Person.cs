using System.ComponentModel.DataAnnotations;

namespace cargo_flow_backend.Entities
{
    public class Person
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Cnp { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public ICollection<Order>? DeliveryOrders { get; set; }
        public ICollection<Trip>? DrivenTrips { get; set; }
        public ICollection<Company>? ContactForCompanies { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
