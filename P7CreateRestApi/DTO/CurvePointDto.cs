using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO
{
    public class CurvePointDto
    {
        // TODO: Map columns in data table CURVEPOINT with corresponding fields
        public int Id { get; set; }
        public byte? CurveId { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The AsOfDate does not suit the DateTime format.")]
        public string? AsOfDate { get; set; }


        [Range(0.0001, double.MaxValue, ErrorMessage = "The Term is not greater than 0 and smaller than 1.79 E+308.")]
        [CustomValidation(ErrorMessage = "The Term is not a double.")]
        [DefaultValue("1")]
        public string? Term { get; set; }


        [Range(0.0001, double.MaxValue, ErrorMessage = "The CurvePointValue is not greater than 0 and smaller than 1.79 E+308.")]
        [CustomValidation(ErrorMessage = "The CurvePointValue is not a double.")]
        [DefaultValue("1")]
        public string? CurvePointValue { get; set; }

        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2500", ErrorMessage = "The CreationDate does not suit the DateTime format.")]
        public string? CreationDate { get; set; }
    }



}

