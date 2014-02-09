using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WhereAmI.views.validators
{
    public class PlaceNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string newName = (string)value;
            if (newName.Length == 0)
                return new ValidationResult(false, null);
            var otherNames = DataManager.Instance.context.Places.Local
                .Where(p => p.Name == newName);

            if (otherNames.Count() != 0)
                return new ValidationResult(false, string.Format(Properties.Resources.ErrMsgNameAlreadyPresent, value));
            
            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}