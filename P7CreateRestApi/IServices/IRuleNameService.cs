using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IServices
{
    public interface IRuleNameService
    {
        Task<IEnumerable<RuleNameDto>> GetAllRuleNamesDto();
        Task<IEnumerable<RuleNameDto>> GetRuleNameDtoById(int id);
        Task CreateRuleNameWithRuleNameDto(RuleNameDto RuleNameDto);
        Task UpdateRuleNameWithRuleNameDto(RuleNameDto RuleNameDto);
        Task DeleteRuleNameById(int id);
        Task<RuleNameDto> MapRuleNameToRuleNameDto(RuleName RuleName);
        Task<RuleName> MapRuleNameDtoToRuleName(RuleNameDto RuleNameDto);

    }
}