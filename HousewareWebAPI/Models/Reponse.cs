using HousewareWebAPI.Helpers.Common;

namespace HousewareWebAPI.Models
{
    public class Reponse
    {
        public int ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public object Result { get; set; }

        public Reponse()
        {
        }

        public void SetCode(CodeDescription code)
        {
            ResultCode = code.ResultCode;
            ResultDescription = code.ResultDescription;
        }

        public void SetResult(object value)
        {
            Result = value;
        }

        public Reponse(CodeDescription code, object jsresult = null)
        {
            ResultCode = code.ResultCode;
            ResultDescription = code.ResultDescription;
            SetResult(jsresult);
        }
    }
}
