namespace EndpointQueryService.Services
{
    public record GetEntriesContext : ActionContext
    {
        public override string ActionName => Consts.Endpoint.Actions.PageRead;
        public int PageNumberRequested { get; init; } = 1;
        public int PageNumberReturned { get; set; } = 1;
        public int? PageSize => (Data as IEnumerable<object>)?.Count();


        //private IEnumerable<string>? _ids;
        //private IEnumerable<KeyValuePair<string, string>>? _filter;
        //public IEnumerable<string>? Ids
        //{
        //    get => _ids;
        //    init
        //    {
        //        if (_filter != null)
        //            throw new InvalidOperationException($"Filter cannot be set when {nameof(Filter)} initialized");

        //        _ids = value;
        //    }
        //}
        //public IEnumerable<KeyValuePair<string, string>>? Filter
        //{
        //    get => _filter;
        //    init
        //    {
        //        if (_ids != null)
        //            throw new InvalidOperationException($"Filter cannot be set when {nameof(Ids)} initialized");
        //        _filter = value;
        //    }
        //}
    }
}
