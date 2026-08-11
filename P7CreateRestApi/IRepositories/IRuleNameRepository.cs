//using Dot.Net.WebApi.Controllers;
//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;

namespace P7CreateRestApi.IRepositories
{
    public interface IRuleNameRepository
    {
        Task<IEnumerable<RuleName>> GetAllRuleNames();
        Task<IEnumerable<RuleName>> GetRuleNameById(int id);
        void CreateRuleName(RuleName ruleName);
        Task UpdateRuleName(RuleName ruleName);
        void DeleteRuleNameById(int id);

    }
}