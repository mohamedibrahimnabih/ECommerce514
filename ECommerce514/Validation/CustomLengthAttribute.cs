using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ECommerce514.Validation
{
    public class CustomLengthAttribute : ValidationAttribute
    {
        private readonly int _length;

        public CustomLengthAttribute(int length)
        {
            _length = length;
        }

        public override bool IsValid(object? value)
        {
            if(value is string v)
            {
                if (v.Length > _length)
                    return true;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
