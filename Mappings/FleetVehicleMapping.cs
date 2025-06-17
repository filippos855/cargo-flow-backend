using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class FleetVehicleMapping
    {
        public static FleetVehicleDto ToDto(this FleetVehicle vehicle)
        {
            return new FleetVehicleDto
            {
                Id = vehicle.Id,
                Identifier = vehicle.Identifier,
                Type = new DictionaryItemDto
                {
                    Id = vehicle.Type.Id,
                    Name = vehicle.Type.Name,
                    Dictionary = new DictionaryDto
                    {
                        Id = vehicle.Type.Dictionary.Id,
                        Name = vehicle.Type.Dictionary.Name
                    }
                },
                ItpExpiration = vehicle.ItpExpiration,
                RcaExpiration = vehicle.RcaExpiration,
                IsAvailable = vehicle.IsAvailable
            };
        }

        public static FleetVehicle ToEntity(this FleetVehicleCreateRequest request, DictionaryItem type)
        {
            return new FleetVehicle
            {
                Identifier = request.Identifier,
                Type = type,
                ItpExpiration = request.ItpExpiration,
                RcaExpiration = request.RcaExpiration,
                IsAvailable = request.IsAvailable
            };
        }

        public static void UpdateEntity(this FleetVehicleUpdateRequest request, FleetVehicle vehicle, DictionaryItem type)
        {
            vehicle.Identifier = request.Identifier;
            vehicle.Type = type;
            vehicle.ItpExpiration = request.ItpExpiration;
            vehicle.RcaExpiration = request.RcaExpiration;
            vehicle.IsAvailable = request.IsAvailable;
        }
    }
}
