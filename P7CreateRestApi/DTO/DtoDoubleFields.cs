using Swashbuckle.AspNetCore.SwaggerGen;

namespace P7CreateRestApi.DTO
{
    public class DtoDoubleFields
    {
        public ICollection<string> dtoDoubleMemberNames { get; set; } = new List<string>
        {
            "$.bidQuantity","$.askQuantity","$.bid","$.ask","$.bid","$.bid","$.bid","$.bid",
        };




    }
}
