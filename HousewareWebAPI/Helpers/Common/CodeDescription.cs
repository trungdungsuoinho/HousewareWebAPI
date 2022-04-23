namespace HousewareWebAPI.Helpers.Common
{
    public class CodeDescription
    {
        public int ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public CodeDescription()
        {
        }

        public CodeDescription(int resultCode, string resultDescription)
        {
            ResultCode = resultCode;
            ResultDescription = resultDescription;
        }
    }
}
