using Admin.Backend.Domain;

namespace Admin.Backend.Services.Account
{
    public interface IAccountStore
    {
        Task<AccountDomainModel> Create(AccountDomainModel toCreate);
        Task<AccountDomainModel?> GetAccountBySubjectId(string subjectId);
        Task<AccountDomainModel?> GetAccountByName(string name);
        Task<AccountDomainModel?> GetAccountById(string id);
        Task<AccountDomainModel?> UpdateAccount(AccountDomainModel toUpdate);
        Task<AccountDomainModel?> DeleteAccount(string accountId);
    }
}
