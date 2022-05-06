using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using Google.Cloud.Firestore;

namespace EndpointQueryService.Data.Endpoints
{
    public class FirestoreEndpointRepository : IEndpointRepository
    {
        private const string DocumentPathDelilmieter = "-";

        private readonly FirestoreDb _db;
        private readonly Func<CollectionReference> _getCollection;
        private readonly ILogger<FirestoreEndpointRepository> _logger;

        public FirestoreEndpointRepository(
            FirestoreDb db,
            ILogger<FirestoreEndpointRepository> logger)
        {
            _db = db;
            _getCollection = () => _db.Collection("endpoints");
            _logger = logger;
        }

        public async Task AddOrUpdateEndpointEntryByPath(EndpointInfo info)
        {
            if (!info.IsValid())
                throw new ArgumentException(nameof(info));

            var path = info.Path.Replace(EndpointInfo.PathDelimiter, DocumentPathDelilmieter);
            var doc = _getCollection().Document(path);
            var dSnap = await doc.GetSnapshotAsync();

            var d = new
            {
                idProperty = info.IdProperty,
                searchableBy = info.SearchableBy.ToArray(),
                account = info.Account,
                name = info.Name,
                version = info.Version,
                subVersion = info.SubVersion,
            };
            var wr = await doc.SetAsync(d);

            if (dSnap.UpdateTime == wr.UpdateTime)
                _logger.LogError("Failed to set document for endpoint: {info}", info);
        }

        public Task<EndpointInfo> GetEndpointByPathAsync(string path)
        {
            _getCollection().as
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EndpointEntry>> GetEntries(GetEntriesContext context)
        {
            throw new NotImplementedException();
        }

        private FirestoreModel ToFirestoreModel(EndpointInfo info)
        {
            throw new NotImplementedException();
        }
        private EndpointInfo FromFirestoreModel(FirestoreModel model)
        {
            throw new NotImplementedException();
        }
        internal class FirestoreModel
        {

        }
    }
}
