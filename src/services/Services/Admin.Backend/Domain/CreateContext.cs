namespace Admin.Backend.Domain
{
    public record CreateContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "create";
        public TModel? ToCreate { get; init; }
        public TModel? Created { get; set; }
    }
}
