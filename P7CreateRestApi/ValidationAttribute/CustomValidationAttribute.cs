using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Globalization;


public class CustomValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var doubleValue = value.ToString();
        double outputvalue;
        bool success = double.TryParse(doubleValue, out(outputvalue));
        if (success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}