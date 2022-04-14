using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services
{
    public interface ITemplateService
    {
        Task GetEntries(GetAllEntriesContext context);
        Task<Template> GetTemplateByPath(string path);
    }
}
