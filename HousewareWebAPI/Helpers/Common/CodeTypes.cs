namespace HousewareWebAPI.Helpers.Common
{
    public static class CodeTypes
    {
        // Infomation
        public readonly static CodeDescription Success = new(000, "Successfully!");

        // Error value
        public readonly static CodeDescription Err_CustomValidation = new(101, "Validation error!");
        public readonly static CodeDescription Err_IdNotMatch = new(102, "ID not match!");
        public readonly static CodeDescription Err_LackVal = new(103, "Lack of value!");
        public readonly static CodeDescription Err_IncorrectVal = new(104, "Input value is not correct");

        // Error data
        public readonly static CodeDescription Err_Exist = new(201, "Already exists!");
        public readonly static CodeDescription Err_NotExist = new(202, "Not exists!");
        public readonly static CodeDescription Err_NotFound = new(203, "Not found!");

        // Access control
        public readonly static CodeDescription Err_AccessDenied = new(301, "Access denied");
        public readonly static CodeDescription Err_Unauthorized = new(302, "Unauthorized");

        // Other error
        public readonly static CodeDescription Err_Exception = new(901, "Exception error!");
        public readonly static CodeDescription Err_AccFail = new(902, "Execution actions failed!");
        public readonly static CodeDescription Err_Unknown = new(999, "Error message from server. Please contact Trung Dung to more information!");

    }
}
