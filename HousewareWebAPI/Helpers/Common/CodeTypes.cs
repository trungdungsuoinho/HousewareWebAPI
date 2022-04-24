namespace HousewareWebAPI.Helpers.Common
{
    public static class CodeTypes
    {
        public readonly static CodeDescription Success = new (0, "Successfully!");
        public readonly static CodeDescription Err_Exception = new (1, "Exception error!");
        public readonly static CodeDescription Err_Unknown = new(999, "Error message from server. Please contact Trung Dung to more information!");
        public readonly static CodeDescription Err_Exist = new(2, "The object with this code already exists!");
    }
}
