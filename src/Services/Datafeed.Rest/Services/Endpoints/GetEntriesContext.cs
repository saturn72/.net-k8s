using System.ComponentModel;

namespace Datafeed.Rest.Services.Endpoints
{
    public record GetEntriesContext : AccountEndpointVersionContext
    {
        [DefaultValue(0)]
        public int OffSet { get; set; } = 0;

        [DefaultValue(100)]
        public int PageSize { get; set; } = 100;
    }
}
