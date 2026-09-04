using P7CreateRestApi.Domain;

namespace P7CreateRestApi.IRepositories
{
    public interface IRuleNameRepository
    {
        Task<IEnumerable<RuleName>> GetAllRuleNames();
        Task<IEnumerable<RuleName>> GetRuleNameById(int id);
        Task CreateRuleName(RuleName ruleName);
        Task UpdateRuleName(RuleName ruleName);
        Task DeleteRuleNameById(int id);

    }
}