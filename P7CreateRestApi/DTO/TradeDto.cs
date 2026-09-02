using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO
{
    public class TradeDto
    {
        public int TradeId { get; set; }
        public string Account { get; set; }
        public string AccountType { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d))+$", ErrorMessage = "The BuyQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The BuyQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? BuyQuantity { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d))+$", ErrorMessage = "The SellQuantity is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The SellQuantity is not greater than 0 and smaller than 1.79 E+308.")]
        public double? SellQuantity { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d))+$", ErrorMessage = "The BuyPrice is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The BuyPrice is not greater than 0 and smaller than 1.79 E+308.")]
        public double? BuyPrice { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d))+$", ErrorMessage = "The SellPrice is not a number.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The SellPrice is not greater than 0 and smaller than 1.79 E+308.")]
        public double? SellPrice { get; set; }

        [RegularExpression("^([0-9]{4})-((01|02|03|04|05|06|07|08|09|10|11|12|(?:J(anuary|u(ne|ly))|February|Ma(rch|y)|A(pril|ugust)|(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)|" +
        "(JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER)|(September|October|November|December)|(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)|" +
        "(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)))|(january|february|march|april|may|june|july|august|september|october|november|december))-([0-3][0-9])\\s([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$", 
        ErrorMessage = "The TradeDate does not follow DateTimeFormat.")]
        public DateTime? TradeDate { get; set; }
        public string TradeSecurity { get; set; }
        public string TradeStatus { get; set; }
        public string Trader { get; set; }
        public string Benchmark { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }

        [RegularExpression("^([0-9]{4})-((01|02|03|04|05|06|07|08|09|10|11|12|(?:J(anuary|u(ne|ly))|February|Ma(rch|y)|A(pril|ugust)|(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)|" +
        "(JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER)|(September|October|November|December)|(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)|" +
        "(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)))|(january|february|march|april|may|june|july|august|september|october|november|december))-([0-3][0-9])\\s([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$",
        ErrorMessage = "The CreationDate does not follow DateTimeFormat.")]
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }

        [RegularExpression("^([0-9]{4})-((01|02|03|04|05|06|07|08|09|10|11|12|(?:J(anuary|u(ne|ly))|February|Ma(rch|y)|A(pril|ugust)|(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)|" +
        "(JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER)|(September|October|November|December)|(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)|" +
        "(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)))|(january|february|march|april|may|june|july|august|september|october|november|december))-([0-3][0-9])\\s([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$",
        ErrorMessage = "The RevisionDate does not follow DateTimeFormat.")]
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; }
    }
}