using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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