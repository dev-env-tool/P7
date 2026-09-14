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

        [Range(0, 256, ErrorMessage = "The OrderNumber is not a byte as its value not equal or greater than 0 and smaller or equal to 256.")]
        [CustomValidationAttributeForByte(ErrorMessage = "The OrderNumber is not a byte.")]
        [DefaultValue("1")]
        public string? OrderNumber { get; set; }
    }
}