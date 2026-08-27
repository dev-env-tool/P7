using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain
{
    public class Rating
    {
        // TODO: Map columns in data table RATING with corresponding fields

        public int Id { get; set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }

        [RegularExpression("^(-?(\\d+\\.?\\d+|\\d))+$", ErrorMessage = "The order number is not a number.")]
        //[Range(0.0001, double.MaxValue, ErrorMessage = "The order number is not greater than 0 and smaller than 1.79 E+308.")]
        public byte? OrderNumber { get; set; }
    }
}