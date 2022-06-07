namespace HousewareWebAPI.Helpers.Common
{
    public class GlobalVariable
    {
        // Role
        public const string RoleCustomer = "RoleCustomer";
        public const string RoleAdmin = "RoleAdmin";

        // Payment Type
        public const string PayOnline = "ONL";
        public const string PayCod = "COD";

        // Payment Status
        public const string OrderCreate = "CRE";
        public const string OrderDoing = "DOI";
        public const string OrderDone = "DON";

        // GHN
        public const string GHNToNameDefault = "Khách hàng";
        public static string GHNContent(string orderId) => string.Format("Housware gửi đơn hàng {0}", orderId);
    }
}
