using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using Google.Cloud.Firestore;

namespace EndpointQueryService.Data.Endpoints
{
    public class FirestoreEndpointRepository : IEndpointRepository
    {
        private const string DocumentPathDelilmieter = "-";

        private readonly FirestoreDb _db;
        private readonly Func<string, DocumentReference> _getDocument;
        private readonly ILogger<FirestoreEndpointRepository> _logger;

        public FirestoreEndpointRepository(
            FirestoreDb db,
            ILogger<FirestoreEndpointRepository> logger)
        {
            _db = db;
            _getDocument = (path) => _db.Collection("endpoints").Document(path);
            _logger = logger;
        }

        public async Task AddOrUpdateEndpointEntryByPath(EndpointInfo info)
        {
            if (!info.IsValid()) throw new ArgumentException(nameof(info));

            var path = info.Path.Replace(EndpointInfo.PathDelimiter, DocumentPathDelilmieter);
            var doc = _getDocument(path);
            var dSnap = await doc.GetSnapshotAsync();

            var wr = await doc.SetAsync(info);
            
            if (dSnap.UpdateTime == wr.UpdateTime)
                _logger.LogError("Failed to set document for endpoint: {info}", info);
        }

        public async Task<EndpointInfo> GetEndpointByPath(string path)
        {
            var doc = _getDocument(path);
            var s = await doc.GetSnapshotAsync();
            if (!s.Exists) return null;

            throw new NotImplementedException();
        }

        public Task<IEnumerable<EndpointEntry>> GetEntriesPage(GetEntriesContext context)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EndpointEntry>> GetEntriesById(string path, IEnumerable<string> ids)
        {
            var doc = _getDocument(path);

            throw new NotImplementedException();
        }
    }
}
