using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BTIDataBaseProj.Helpers
{
    public class TextParsingRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string)
            {
                return new ValidationResult(false, "");
            }

            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingGroup owner)
        {
            return base.Validate(value, cultureInfo, owner);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            return base.Validate(value, cultureInfo, owner);
        }
    }
}
