using System;
using System.Net;

namespace HousewareWebAPI.Helpers.Models
{
    public class GenPayURLRequest
    {
        public string Vnp_Version { get; set; } = "2.1.0";
        public string Vnp_Command { get; set; } = "pay";
        public string Vnp_TmnCode { get; set; }
        public int Vnp_Amount { get; set; }
        public DateTime Vnp_CreateDate { get; set; }
        public string Vnp_CurrCode { get; set; } = "VND";
        public string Vnp_IpAddr { get; set; }
        public string Vnp_Locale { get; set; } = "vn";
        public string Vnp_OrderInfo { get; set; }
        public string Vnp_OrderType { get; set; } = "230000";
        public string Vnp_ReturnUrl { get; set; }
        public string Vnp_TxnRef { get; set; }
        public string GenParamRoute()
        {
            return
                "vnp_Amount=" + WebUtility.UrlEncode(Vnp_Amount.ToString()) +
                "&vnp_Command=" + WebUtility.UrlEncode(Vnp_Command) +
                "&vnp_CreateDate=" + WebUtility.UrlEncode(Vnp_CreateDate.ToString("yyyyMMddHHmmss")) +
                "&vnp_CurrCode=" + WebUtility.UrlEncode(Vnp_CurrCode) +
                "&vnp_IpAddr=" + WebUtility.UrlEncode(Vnp_IpAddr) +
                "&vnp_Locale=" + WebUtility.UrlEncode(Vnp_Locale) +
                "&vnp_OrderInfo=" + WebUtility.UrlEncode(Vnp_OrderInfo) +
                "&vnp_OrderType=" + WebUtility.UrlEncode(Vnp_OrderType) +
                "&vnp_ReturnUrl=" + WebUtility.UrlEncode(Vnp_ReturnUrl) +
                "&vnp_TmnCode=" + WebUtility.UrlEncode(Vnp_TmnCode) +
                "&vnp_TxnRef=" + WebUtility.UrlEncode(Vnp_TxnRef) +
                "&vnp_Version=" + WebUtility.UrlEncode(Vnp_Version);
        }
    }
    public class CodeIPNURLRequest
    {
        public string Vnp_TmnCode { get; set; }
        public int Vnp_Amount { get; set; }
        public string Vnp_BankCode { get; set; }
        public string Vnp_BankTranNo { get; set; }
        public string Vnp_CardType { get; set; }
        public string Vnp_PayDate { get; set; }
        public string Vnp_OrderInfo { get; set; }
        public string Vnp_TransactionNo { get; set; }
        public string Vnp_ResponseCode { get; set; }
        public string Vnp_TransactionStatus { get; set; }
        public string Vnp_TxnRef { get; set; }
        public string Vnp_SecureHashType { get; set; }
        public string Vnp_SecureHash { get; set; }

        public string GenParamRoute()
        {
            return
                "vnp_Amount=" + WebUtility.UrlEncode(Vnp_Amount.ToString()) +
                "&vnp_BankCode=" + WebUtility.UrlEncode(Vnp_BankCode) +
                "&vnp_BankTranNo=" + WebUtility.UrlEncode(Vnp_BankTranNo) +
                "&vnp_CardType=" + WebUtility.UrlEncode(Vnp_CardType) +
                "&vnp_OrderInfo=" + WebUtility.UrlEncode(Vnp_OrderInfo) +
                "&vnp_PayDate=" + WebUtility.UrlEncode(Vnp_PayDate) +
                "&vnp_SecureHashType=" + WebUtility.UrlEncode(Vnp_SecureHashType) +
                "&vnp_TmnCode=" + WebUtility.UrlEncode(Vnp_TmnCode) +
                "&vnp_TransactionNo=" + WebUtility.UrlEncode(Vnp_TransactionNo) +
                "&vnp_TransactionStatus=" + WebUtility.UrlEncode(Vnp_TransactionStatus) +
                "&vnp_TxnRef=" + WebUtility.UrlEncode(Vnp_TxnRef) +
                "&vnp_ResponseCode=" + WebUtility.UrlEncode(Vnp_ResponseCode);
        }
    }

    public class IPNVNPayResponse
    {
        public string RspCode { get; set; }
        public string Message { get; set; }
    }
}
