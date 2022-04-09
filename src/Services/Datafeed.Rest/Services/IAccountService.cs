using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services
{
    public interface IAccountService
    {
        Task<Account> GetAccountByName(string accountName);
    }
}
