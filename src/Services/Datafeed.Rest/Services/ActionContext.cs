using Datafeed.Rest.Domain;
using System.ComponentModel;

namespace Datafeed.Rest.Services
{
    public abstract record ActionContext
    {
        private string? _error;

        public abstract string ActionName { get; }

        public object? Data { get; set; }
        public Template? Template { get; set; }

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
