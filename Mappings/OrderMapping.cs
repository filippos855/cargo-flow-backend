using cargo_flow_backend.Entities;
using cargo_flow_backend.Mappings;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order order)
    {
        if (order == null)
            return null;

        return new OrderDto
        {
            Id = order.Id,
            Number = order.Number,
            CreatedDate = order.CreatedDate,
            Company = order.Company.ToDto(),
            DeliveryPerson = order.DeliveryPerson.ToDto(),
            Address = order.Address,
            Status = order.Status.ToDto(),
            TripId = order.Trip?.Id,
            TripNumber = order.Trip?.Number
        };
    }

    public static Order ToEntity(this OrderCreateRequest req, Company company, Person person, DictionaryItem status, Trip? trip)
    {
        return new Order
        {
            Number = "",
            CreatedDate = DateTime.Now,
            Company = company,
            DeliveryPerson = person,
            Address = req.Address,
            Status = status,
            Trip = trip
        };
    }

    public static void UpdateEntity(this OrderUpdateRequest req, Order entity, Company company, Person person, DictionaryItem status, Trip? trip)
    {
        entity.Company = company;
        entity.DeliveryPerson = person;
        entity.Address = req.Address;
        entity.Status = status;
        entity.Trip = trip;
    }
}
