namespace CargoFlow.Backend.Models.Responses
{
    public class DashboardStatsDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ActiveTrips { get; set; }
        public int ExpiringFleet { get; set; }
        public int UnpaidInvoices { get; set; }
        public int ActiveClients { get; set; }
    }
}