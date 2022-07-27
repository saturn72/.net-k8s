using EndpointQueryService.Domain;
using System.ComponentModel;

namespace EndpointQueryService.Services
{
    public abstract record ActionContext
    {
        private string? _error;

        public abstract string ActionName { get; }

        public object? Data { get; set; }
        public EndpointInfo? Endpoint { get; set; }

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
        public string? UserId { get; set; }
    }
}
