using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;


namespace P7CreateRestApi.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {

        private static P7Referential? _context;

        public RuleNameRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RuleName>> GetAllRuleNames()
        {
            return await _context!.RuleNames.ToListAsync();
        }

        public async Task<IEnumerable<RuleName>> GetRuleNameById(int id)
        {
            return await _context!.RuleNames.Where(r => r.Id == id)
                                  .ToListAsync();
        }

        public async Task CreateRuleName(RuleName ruleName)
        {
            if (ruleName != null)
            {
                await _context!.RuleNames.AddAsync(ruleName);
                await _context!.SaveChangesAsync();
            }
        }

        public async Task UpdateRuleName(RuleName ruleName)
        {
            var existingRuleName = await _context.RuleNames.FindAsync(ruleName.Id);

            if (existingRuleName != null)
            {
                _context.Entry(existingRuleName).CurrentValues.SetValues(ruleName);
                await _context!.SaveChangesAsync();
            }
        }


        public async Task DeleteRuleNameById(int id)
        {
            RuleName ruleName = _context!.RuleNames.First(r => r.Id == id);

            if (ruleName != null)
            {
                _context!.RuleNames.Remove(ruleName);
                await _context.SaveChangesAsync();
            }
        }
    }
}