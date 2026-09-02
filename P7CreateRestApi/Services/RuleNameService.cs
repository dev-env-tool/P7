using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;


namespace P7CreateRestApi.Services
{
    public class RuleNameService : IRuleNameService
    {
        private readonly IRuleNameRepository _iRuleNameRepository;
        private readonly IMapper _mapper;


        public RuleNameService(IRuleNameRepository iRuleNameRepository, IMapper iMapper)
        {
            _iRuleNameRepository = iRuleNameRepository;
            _mapper = iMapper;
        }
        public async Task<IEnumerable<RuleNameDto>> GetAllRuleNamesDto()
        {
            Task<IEnumerable<RuleName>> ruleNames = _iRuleNameRepository!.GetAllRuleNames();
            List<RuleNameDto> listOfRuleNameDtos = new List<RuleNameDto>();
            foreach (RuleName ruleName in await ruleNames)
            {
                RuleNameDto ruleNameDto = await MapRuleNameToRuleNameDto(ruleName);
                listOfRuleNameDtos.Add(ruleNameDto);
            }

            return listOfRuleNameDtos;
        }

        public async Task<IEnumerable<RuleNameDto>> GetRuleNameDtoById(int id)
        {
            Task<IEnumerable<RuleName>> ruleNames = _iRuleNameRepository!.GetRuleNameById(id);
            List<RuleNameDto> listOfRuleNameDtos = new List<RuleNameDto>();
            foreach (RuleName ruleName in await ruleNames)
            {
                RuleNameDto ruleNameDto = await MapRuleNameToRuleNameDto(ruleName);
                listOfRuleNameDtos.Add(ruleNameDto);
            }

            return listOfRuleNameDtos;
        }

        public async Task CreateRuleNameWithRuleNameDto(RuleNameDto ruleNameDto)
        {
            RuleName ruleName = await MapRuleNameDtoToRuleName(ruleNameDto);
            if (ruleNameDto != null)
            {
                await _iRuleNameRepository.CreateRuleName(ruleName);
            }
        }

        public async Task UpdateRuleNameWithRuleNameDto(RuleNameDto ruleNameDto)
        {
            RuleName ruleName = await MapRuleNameDtoToRuleName(ruleNameDto);
            if (ruleNameDto != null)
            {
                await _iRuleNameRepository.UpdateRuleName(ruleName);
            }
        }


        public async Task DeleteRuleNameById(int id)
        {
            await _iRuleNameRepository.DeleteRuleNameById(id);
        }

        public async Task<RuleNameDto> MapRuleNameToRuleNameDto(RuleName ruleName)
        {
            RuleNameDto ruleNameDto = _mapper.Map<RuleName, RuleNameDto>(ruleName);
            return ruleNameDto;
        }
        public async Task<RuleName> MapRuleNameDtoToRuleName(RuleNameDto ruleNameDto)
        {
            RuleName ruleName = _mapper.Map<RuleNameDto, RuleName>(ruleNameDto);
            return ruleName;
        }



    }
}

