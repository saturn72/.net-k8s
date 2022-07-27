using System.ComponentModel;

namespace Admin.Backend.Domain
{
    public record PaginationContext<TModel> : ContextBase
    {
        public override string EntityName => typeof(TModel).Name;
        public override string ActionName => "read-page";
        public bool IsAsc { get; init; }
        public uint PageSize { get; init; } = 100;
        public uint Offset { get; init; }
        public string? OrderBy { get; init; }
        public IEnumerable<TModel>? Data { get; set; }
    }
}
