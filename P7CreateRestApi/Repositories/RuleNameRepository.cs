
//using Dot.Net.WebApi.Controllers;
//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;

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

        public void CreateRuleName(RuleName ruleName)
        {
            if (ruleName != null)
            {
                _context!.RuleNames.Add(ruleName);
                _context.SaveChanges();
            }
        }
        public async Task UpdateRuleName(RuleName ruleName)
        {
            if (ruleName != null)
            {
                _context!.Entry(ruleName).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteRuleNameById(int id)
        {
            RuleName ruleName = _context!.RuleNames.First(r => r.Id == id);

            if (ruleName != null)
            {
                _context!.RuleNames.Remove(ruleName);
                _context.SaveChanges();
            }
        }
    }
}