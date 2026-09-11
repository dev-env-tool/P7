using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO
{
    public class RatingDto
    {
        public int Id { get; set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }



        [Range(0.0001, double.MaxValue, ErrorMessage = "The order number is not greater than 0 and smaller than 1.79 E+308.")]
        [CustomValidation(ErrorMessage = "The OrderNumber is not a double.")]
        [DefaultValue("1")]
        public string? OrderNumber { get; set; }
    }
}