using Humanizer;

namespace Web_Doan_2023.Models
{
    public class isStatus
    {
        public const string WaitingForApproval = "Waiting for approval";
        public const string warehouseimported = "warehouse imported";
        public const string WaitingForDelivery = "Waiting for delivery";
        public const string Approve = "Your order has been approved";
        public const string Cancel = "Cancel";
        public const string Success = "Success";

        public const string waitForConfirmation = "wait for confirmation";//1 Chờ xác nhận
        public const string Confirmed = "Confirmed";//2 đả xác nhận
        public const string PackingGoods = "Packing goods";//3 đang gói hàng
        public const string SwitchToA_DedicatedTransportUnit = "Switch to a dedicated transport unit";//3 đang gói hàng
        public const string DeliveryInProgress = "Delivery In Progress";//4 đang giao hàng
        public const string DeliverySuccessful = "Delivery Successful";//5 giao thanh cong

    }
}
