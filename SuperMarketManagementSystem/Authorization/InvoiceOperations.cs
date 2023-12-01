using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SuperMarketManagementSystem.Authorization
{
    public class InvoiceOperations
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = "Create" };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = "Read" };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = "Update" };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = "Delete" };
        public static OperationAuthorizationRequirement Approve = new OperationAuthorizationRequirement { Name = "Approved" };
        public static OperationAuthorizationRequirement Reject = new OperationAuthorizationRequirement { Name = "Rejected" };
    }
    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string ApprovedOperationName = "Approved";
        public static readonly string RejectedOperationName = "Rejected";

        public static readonly string InvoiceManagersRole = "InvoiceManager";
    }
}
