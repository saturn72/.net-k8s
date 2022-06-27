namespace Admin.Backend
{
    internal sealed class EventKeys
    {
        internal sealed class Endpoint
        {
            internal const string Created = "endpoint.created";
        }
        
        internal sealed class Datasource
        {
            internal const string Created = "datasource.created";
        }

        internal sealed class Account
        {
            internal const string Created = "account.created";
            internal const string Updated = "account.updated";
            internal const string Deleted = "account.deleted";
        }
    }
}
