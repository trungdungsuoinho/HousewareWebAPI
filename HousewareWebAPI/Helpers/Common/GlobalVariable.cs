namespace HousewareWebAPI.Helpers.Common
{
    public class GlobalVariable
    {
        // Orther
        public const int StepWidth = 10;

        // Role
        public const string RoleCustomer = "RoleCustomer";
        public const string RoleAdmin = "RoleAdmin";

        // Payment Type
        public const string PayOnline = "ONL";
        public const string PayCod = "COD";

        // Payment Status
        public const string OrderOrdered = "ODED";
        public const string OrderPaymenting = "PAIN";
        public const string OrderProcessing = "PRSS";
        public const string OrderDoing = "DOIN";
        public const string OrderDone = "DONE";
        public const string OrderCancel = "CANL";

        // GHN
        public const string GHNToNameDefault = "Khách hàng";
        public static string GHNContent(string orderId) => string.Format("Housware gửi đơn hàng {0}", orderId);

        // VNPay
        public static string VNPContent(string orderId) => string.Format("Housware thanh toán đơn hàng {0}", orderId);

        // Discount
        public enum DiscountRange
        {
            Global,
            Classification,
            Category,
            Product
        }

        public enum DiscountType
        {
            Direct,
            Percentage
        }
    }
}
