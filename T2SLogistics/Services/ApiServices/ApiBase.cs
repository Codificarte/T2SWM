namespace T2SLogistics.Services.ApiServices
{
    public class ApiBase
    {
        public const string AuthLoginKey = "Auth/login";
        public const string AuthResetPasswordKey = "Auth/reset-passwordv2";
        public const string PhcOrdersKey = "PhcOrders/GetOrdersProd";
        public const string PhcOrdersByCodeKey = "PhcOrders/GetOrdersProd/{0}";

        public const string GetCustomerOrdersKey = "PhcOrders/GetCustomerOrders";
        public const string CreateUsersAppTimerKey= "UsersAppTimer/create";
        public const string ProductionEntriesKey = "ProductionEntries";
        public const string AddSeprationItemsKey = "PhcOrders/AddReadSeparationItems";
        public const string AddSeprationItemKey = "PhcOrders/AddReadSeparationItem";

        public const string AddExpeditionKey = "PhcOrders/AddExpeditionReadItems";
        public const string GetRefProductKey = "PhcOrders/GetRefProduct/{0}";
        public const string CloseExpeditionKey = "PhcOrders/CloseExpedition";
        public const string DeleteExpeditionKey = "PhcOrders/ExpeditionReadItems/Delete/{0}";
        public const string CancelReadExpeditionKey = "PhcOrders/CancelReadExpedition/{0}";


    }
}
