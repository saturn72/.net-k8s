using System.ComponentModel;

namespace EndpointQueryService.Services
{
    public record GetEntriesContext : ActionContext
    {
        public override string ActionName => Consts.Endpoint.Actions.GetAll;

        private int _offSetRequested;
        private int _offSet;
        private int _pageSizeRequested = 100;
        private int _pageSize = 100;
        private IEnumerable<string>? _ids;
        private IEnumerable<KeyValuePair<string, string>>? _filter;

        public int OffSetRequested
        {
            get => _offSetRequested;
            set
            {
                _offSetRequested = value;
                _offSet = _offSetRequested != 0 ? _offSetRequested / 100 * 100 : _offSetRequested;
            }
        }
        public int OffSet => _offSet;

        [DefaultValue(100)]
        public int PageSizeRequested
        {
            get => _pageSizeRequested;
            set
            {
                _pageSizeRequested = value;

                if (_pageSizeRequested <= 100)
                {
                    _pageSize = 100;
                }
                else if (_pageSizeRequested > 1000)
                {
                    _pageSize = 1000;
                }
                else
                {
                    _pageSize = _pageSizeRequested / 100 * 100;
                }
            }
        }
        public int PageSize => _pageSize;
        public IEnumerable<string>? Ids
        {
            get => _ids;
            init
            {
                if (_filter != null)
                    throw new InvalidOperationException($"Filter cannot be set when {nameof(Filter)} initialized");

                _ids = value;
            }
        }
        public IEnumerable<KeyValuePair<string, string>>? Filter
        {
            get => _filter;
            init
            {
                if (_ids != null)
                    throw new InvalidOperationException($"Filter cannot be set when {nameof(Ids)} initialized");
                _filter = value;
            }
        }
    }
}
