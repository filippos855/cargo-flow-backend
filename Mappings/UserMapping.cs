using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class UserMapping
    {
        public static UserDto ToDto(this User user)
        {
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = new DictionaryItemDto
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name,
                    Dictionary = new DictionaryDto
                    {
                        Id = user.Role.Dictionary.Id,
                        Name = user.Role.Dictionary.Name
                    }
                },
                Person = new PersonDto
                {
                    Id = user.Person.Id,
                    FullName = user.Person.FullName
                },
                IsActive = user.IsActive
            };
        }
    }
}
