using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf_MongoDB_Test.Validation
{

    /// <summary>
    /// See https://www.codeproject.com/Articles/1069135/Dynamic-Validation-with-FluentValidation-in-WPF-MV
    /// </summary>
    public class RequiredNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!(value is string))
            {
                return new ValidationResult(false, "Object is not string");
            }

            if( string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "String may not be empty");
            }

            return new ValidationResult(true, null);
        }
    }
}
