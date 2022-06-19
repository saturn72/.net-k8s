using Admin.Backend.Domain;

namespace Admin.Backend.Services.Account
{
    public interface IAccountService
    {
        Task CreateAccount(CreateContext<AccountDomainModel> context);
    }
}
