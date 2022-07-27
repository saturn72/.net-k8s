using Admin.Backend.Domain;

namespace Admin.Backend.Services.Account
{
    public interface IAccountService
    {
        Task CreateAccount(CreateContext<AccountDomainModel> context);
        Task GetAccountBySubjectId(ReadByIndexContext<AccountDomainModel> context);
        Task GetAccountByName(ReadByIndexContext<AccountDomainModel> context);
        Task UpdateAccount(UpdateContext<AccountDomainModel> context);
        Task DeleteAccountById(DeleteContext<AccountDomainModel> context);
        Task<string> GetAccountDeletionToken(string subjectId, string accountId);
    }
}
