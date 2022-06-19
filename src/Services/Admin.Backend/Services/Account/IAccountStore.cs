using Admin.Backend.Domain;

namespace Admin.Backend.Services.Account
{
    public interface IAccountStore
    {
        Task Create(CreateContext<AccountDomainModel> context);
        Task<AccountDomainModel> GetByName(string name);
    }
}
