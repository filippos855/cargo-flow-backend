using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class PersonMapping
    {
        public static PersonDto ToDto(this Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                FullName = person.FullName,
                Cnp = person.Cnp,
                Phone = person.Phone,
                Email = person.Email
            };
        }

        public static Person ToEntity(this PersonCreateRequest request)
        {
            return new Person
            {
                FullName = request.FullName,
                Cnp = request.Cnp,
                Phone = request.Phone,
                Email = request.Email
            };
        }

        public static void UpdateEntity(this PersonUpdateRequest request, Person person)
        {
            person.FullName = request.FullName;
            person.Cnp = request.Cnp;
            person.Phone = request.Phone;
            person.Email = request.Email;
        }
    }
}