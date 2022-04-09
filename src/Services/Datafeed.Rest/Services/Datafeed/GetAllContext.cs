using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Datafeed
{
    public record GetAllContext
    {
        public Account? Account { get; internal set; }
        public IAsyncEnumerable<object>? Data { get; set; }
        public string? Endpoint { get; internal set; }
        public int? OffSet { get; internal set; }
        public int? PageSize { get; internal set; }
        public string? Version { get; internal set; }
    }
}
