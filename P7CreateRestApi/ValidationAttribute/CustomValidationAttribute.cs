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

public class CustomValidationAttributeForByte : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var byteValue = value.ToString();
        int outputvalue;
        bool success = int.TryParse(byteValue, out (outputvalue));
        if (success && outputvalue < 256)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}