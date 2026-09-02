using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO
{
    public class BidListDto
    {
        // TODO: Map columns in data table BIDLIST with corresponding fields

        public int BidListId { get; set; }
        public string Account { get; set; }
        public string BidType { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The BidQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The BidQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? BidQuantity { get; set; } = 1;

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The AskQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The AskQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? AskQuantity { get; set; } = 1;

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The Bid is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The Bid is not greater than 0 and smaller than 1.79 E+308.")]
        public double? Bid { get; set; } = 1;

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The Ask is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The Ask is not greater than 0 and smaller than 1.79 E+308.")]
        public double? Ask { get; set; } = 1;
        public string Benchmark { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The BidListDate does not suit the DateTime format.")]
        public string? BidListDate { get; set; }
        public string Commentary { get; set; }
        public string BidSecurity { get; set; }
        public string BidStatus { get; set; }
        public string Trader { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The CreationDate does not suit the DateTime format.")]
        public string? CreationDate { get; set; }
        public string RevisionName { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The RevisionDate does not suit the DateTime format.")]
        public string? RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }
    }
}