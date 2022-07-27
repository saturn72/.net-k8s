namespace Admin.Backend.Domain
{
    public record DeleteContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "delete-by-id";
        public string? IdToDelete { get; init; }
        public TModel? ToDelete { get; set; }
        public TModel? Deleted { get; set; }
        public string? DeleteToken { get; init; }
    }
}
