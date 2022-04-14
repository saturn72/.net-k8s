using System.ComponentModel;

namespace Datafeed.Rest.Services
{
    public record GetAllEntriesContext : ActionContext
    {
        public override string ActionName => Consts.Template.Actions.GetAll;

        private int _offSetRequested;
        private int _offSet;
        private int _pageSizeRequested = 100;
        private int _pageSize = 100;
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
    }
}
