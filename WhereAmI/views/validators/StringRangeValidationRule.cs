using System.Globalization;
using System.Windows.Controls;

namespace WhereAmI.views.validators
{
    public class StringRangeValidationRule : ValidationRule
    {
        private int _minimumLength = -1;
        private int _maximumLength = -1;
        private string _errorMessage;
        
        public int MinimumLength
        {
            get { return _minimumLength; }
            set { _minimumLength = value; }
        }
        
        public int MaximumLength
        {
            get { return _maximumLength; }
            set { _maximumLength = value; }
        }
        
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        
        public override ValidationResult Validate(object value, 
            CultureInfo cultureInfo)
        {
            string inputString = (value ?? string.Empty).ToString();
            if (inputString.Length < this.MinimumLength || inputString.Trim().Length==0)
                 return new ValidationResult(false, Properties.Resources.ErrMsgRequired);
            if (this.MaximumLength > 0 && inputString.Length > this.MaximumLength)
                return new ValidationResult(false, string.Format(Properties.Resources.ErrMsgStringTooLong, this._maximumLength));
            
            return new ValidationResult(true, null);
        }
    }
}
