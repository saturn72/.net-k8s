namespace Admin.Backend
{
    internal sealed class Defaults
    {
        
        internal sealed class Pagination
        {
            internal const uint MaxPageSize = 1000;
        }

        internal sealed class Account
        {
            internal sealed class IndexNames
            {
                internal const string SubjectId = "subject-id";
                internal const string Name = "name";
            }
        }

        internal sealed class Datasource
        {
            internal sealed class IndexNames
            {
                internal const string Id = "id";
            }
        }

        internal sealed class Endpoint
        {
            public const string PathDelimiter = "/";
        }
    }
}
