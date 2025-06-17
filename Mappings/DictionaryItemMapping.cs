using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class DictionaryItemMapping
    {
        public static DictionaryItemDto ToDto(this DictionaryItem item)
        {
            return new DictionaryItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Dictionary = new DictionaryDto
                {
                    Id = item.Dictionary.Id,
                    Name = item.Dictionary.Name
                }
            };
        }
    }
}
