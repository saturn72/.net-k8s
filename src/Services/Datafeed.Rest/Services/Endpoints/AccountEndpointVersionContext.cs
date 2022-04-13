using Datafeed.Rest.Domain;
using System.ComponentModel;

namespace Datafeed.Rest.Services.Endpoints
{
    public record AccountEndpointVersionContext
    {
        private string? _error;

        public object? Data { get; set; }
        public Account? Account { get; set; }
        public AccountEndpoint? Endpoint { get; set; }
        public string? Version { get; set; }

        [DefaultValue(false)]
        public bool IsError { get; private set; }
        public string? ErrorDescription { get; set; }

        public string? Error
        {
            get => _error;
            set
            {
                _error = value;
                IsError = true;
            }
        }
    }
}
