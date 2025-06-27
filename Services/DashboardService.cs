using cargo_flow_backend.Services;
using CargoFlow.Backend.Models.Responses;

namespace CargoFlow.Backend.Services
{
    public class DashboardService
    {
        private readonly OrderService _orderService;
        private readonly TripService _tripService;
        private readonly FleetVehicleService _fleetService;
        private readonly InvoiceService _invoiceService;
        private readonly CompanyService _companyService;

        public DashboardService(
            OrderService orderService,
            TripService tripService,
            FleetVehicleService fleetService,
            InvoiceService invoiceService,
            CompanyService companyService)
        {
            _orderService = orderService;
            _tripService = tripService;
            _fleetService = fleetService;
            _invoiceService = invoiceService;
            _companyService = companyService;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var totalOrders = await _orderService.CountAllAsync();
            var pendingOrders = await _orderService.CountPendingAsync();
            var activeTrips = await _tripService.CountActiveAsync();
            var expiringFleet = await _fleetService.CountItpExpiringInDaysAsync(7);
            var unpaidInvoices = await _invoiceService.CountUnpaidAsync();
            var activeClients = await _companyService.CountActiveAsync();

            return new DashboardStatsDto
            {
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                ActiveTrips = activeTrips,
                ExpiringFleet = expiringFleet,
                UnpaidInvoices = unpaidInvoices,
                ActiveClients = activeClients
            };
        }
    }
}
