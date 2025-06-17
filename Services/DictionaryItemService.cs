using cargo_flow_backend.Data;
using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace cargo_flow_backend.Services
{
    public class DictionaryItemService
    {
        private readonly CargoFlowDbContext _context;

        public DictionaryItemService(CargoFlowDbContext context)
        {
            _context = context;
        }

        public async Task<DictionaryItem?> GetItemByIdAsync(int id)
        {
            return await _context.DictionaryItems
                .Include(i => i.Dictionary)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<DictionaryItem>> GetItemsByQueryAsync(DictionaryItemQuery query)
        {
            var items = _context.DictionaryItems
                .Include(i => i.Dictionary)
                .Where(i => i.Dictionary.Name == query.DictionaryName);

            return await items.ToListAsync();
        }
    }
}
