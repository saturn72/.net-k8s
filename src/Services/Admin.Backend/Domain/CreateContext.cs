namespace Admin.Backend.Domain
{
    public record CreateContext<TModel> : ContextBase<TModel>
    {
        public override string ActionName => "create";
        public TModel? Created { get; set; }
        public object? Response { get; set; }
    }
}
