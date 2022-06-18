namespace Admin.Backend.Domain
{
    public abstract record ContextBase
    {
        public abstract string EntityName { get; }
        public abstract string ActionName { get; }
        private string? _error;
        public bool IsError { get; private set; }
        public string? ErrorDescription { get; set; }
        public string? UserError { get; private set; }
        public string? Error
        {
            get => _error;
            set
            {
                _error = value;
                IsError = true;
            }
        }
        public void SetErrors(string error, string userError, string? description = null)
        {
            Error = error;
            UserError = userError;
            ErrorDescription = description;
        }
        public string? UserId { get; set; }
    }
    public abstract record ContextBase<TModel> : ContextBase
    {
        public TModel? Model { get; init; }
    }
}
