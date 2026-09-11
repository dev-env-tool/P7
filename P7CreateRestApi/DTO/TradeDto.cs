using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO
{

    public class TradeDto
    {
        public int TradeId { get; set; }
        public string Account { get; set; }
        public string AccountType { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The BuyQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The BuyQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? BuyQuantity { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The SellQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The SellQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? SellQuantity { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The BuyPrice is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The BuyPrice is not greater than 0 and smaller than 1.79 E+308.")]
        public double? BuyPrice { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d+?\\,))+$", ErrorMessage = "The SellPrice is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The SellPrice is not greater than 0 and smaller than 1.79 E+308.")]
        public double? SellPrice { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The TradeDate does not suit the DateTime format.")]
        public string? TradeDate { get; set; }
        public string TradeSecurity { get; set; }
        public string TradeStatus { get; set; }
        public string Trader { get; set; }
        public string Benchmark { get; set; }
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
    }
}