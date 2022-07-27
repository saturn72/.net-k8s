namespace Admin.Backend.Domain
{
    public record UpdateContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "update";
        public TModel? ToUpdate { get; init; }
        public TModel? Before { get; set; }
        public TModel? After { get; set; }
    }
}
