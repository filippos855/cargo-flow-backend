using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Responses;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Mappings
{
    public static class TripMapping
    {
        public static TripDto ToDto(this Trip trip)
        {
            if (trip == null)
                return null;

            return new TripDto
            {
                Id = trip.Id,
                Number = trip.Number,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Status = trip.Status.ToDto(),
                TransportCompany = trip.TransportCompany.ToDto(),
                Driver = trip.Driver?.ToDto(),
                TractorUnit = trip.TractorUnit?.ToDto(),
                Trailer = trip.Trailer?.ToDto(),
                Orders = trip.Orders?.Select(o => o.ToDto()).ToList()
            };
        }

        public static Trip ToEntity(this TripCreateRequest request,
                                    DictionaryItem status,
                                    Company transportCompany,
                                    Person? driver = null,
                                    FleetVehicle? tractorUnit = null,
                                    FleetVehicle? trailer = null,
                                    List<Order>? orders = null)
        {
            return new Trip
            {
                Number = request.Number,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = status,
                TransportCompany = transportCompany,
                Driver = driver,
                TractorUnit = tractorUnit,
                Trailer = trailer,
                Orders = orders
            };
        }

        public static void UpdateEntity(this TripUpdateRequest request, Trip trip,
                                        DictionaryItem status,
                                        Company transportCompany,
                                        Person? driver = null,
                                        FleetVehicle? tractorUnit = null,
                                        FleetVehicle? trailer = null,
                                        List<Order>? orders = null)
        {
            trip.Number = request.Number;
            trip.StartDate = request.StartDate;
            trip.EndDate = request.EndDate;
            trip.Status = status;
            trip.TransportCompany = transportCompany;
            trip.Driver = driver;
            trip.TractorUnit = tractorUnit;
            trip.Trailer = trailer;
            trip.Orders = orders;
        }
    }
}
