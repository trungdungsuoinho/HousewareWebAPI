using System.ComponentModel.DataAnnotations;

namespace HousewareWebAPI.Helpers.Attribute
{
    public class MyRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Field [{0}] is required", name);
        }
    }

    public class MyMaxLengthAttribute : MaxLengthAttribute
    {
        public MyMaxLengthAttribute(int length) : base(length)
        {
            base.ErrorMessageResourceName = "MaximumLength";
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Maximum length of field [{0}] is {1}", name, Length);
        }
    }

    public class MyMinLengthAttribute : MinLengthAttribute
    {
        public MyMinLengthAttribute(int length) : base(length)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Minimum length of field [{0}] is {1}", name, Length);
        }
    }
}
