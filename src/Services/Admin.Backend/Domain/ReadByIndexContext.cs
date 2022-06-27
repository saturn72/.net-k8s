namespace Admin.Backend.Domain
{
    public record ReadByIdContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "read-by-id";
        public string? Id { get; init; }
        public TModel? Read { get; set; }
    }
    public record ReadByIndexContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "read-by-index";
        public string? Index { get; init; }
        public string? IndexName { get; init; }
        public TModel? Read { get; set; }
    }
}
