using Datafeed.Rest.Domain;
using Datafeed.Rest.Services;

namespace Datafeed.Rest.Data
{
    public interface IRepository
    {
        Task<Template> GetTemplateByPath(string path);
        Task<IEnumerable<TemplateEntry>> GetEntriesByVersion(GetAllEntriesContext context);
    }
}
