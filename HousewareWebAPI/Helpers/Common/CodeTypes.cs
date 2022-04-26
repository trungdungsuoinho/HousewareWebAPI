﻿namespace HousewareWebAPI.Helpers.Common
{
    public static class CodeTypes
    {
        public readonly static CodeDescription Success = new (0, "Successfully!");
        public readonly static CodeDescription Err_Exception = new (1, "Exception error!");
        public readonly static CodeDescription Err_CustomValidation = new(2, "Validation error!");
        public readonly static CodeDescription Err_Unknown = new(999, "Error message from server. Please contact Trung Dung to more information!");

        public readonly static CodeDescription Err_Exist = new(3, "Already exists!");
        public readonly static CodeDescription Err_NotExist = new(4, "Not exists!");
        public readonly static CodeDescription Err_NotFound = new(5, "Not found!");
        public readonly static CodeDescription Err_IdNotMatch = new(6, "ID not match!");

    }
}
