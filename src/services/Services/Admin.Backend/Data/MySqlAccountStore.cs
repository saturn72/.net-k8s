using Admin.Backend.Domain;
using Admin.Backend.Services.Account;
using Dapper;
using MySql.Data.MySqlClient;

namespace Admin.Backend.Data
{
    public class MySqlAccountStore : IAccountStore
    {
        private readonly string _connectionString;
        public MySqlAccountStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<AccountDomainModel?> Create(AccountDomainModel toCreate)
        {
            using var con = new MySqlConnection(_connectionString);

            var result = await con.ExecuteAsync(Dapper.Account.Insert, toCreate);
            return result == 1 ?
                await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountByAccountName, toCreate) :
                default;
        }

        public async Task<AccountDomainModel?> GetAccountById(string id)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountById, new { id });
        }

        public async Task<AccountDomainModel?> GetAccountBySubjectId(string subjectId)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountBySubjectId, new { CreatedByUserId = subjectId });
        }

        public async Task<AccountDomainModel?> GetAccountByName(string name)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountByAccountName, new { name });
        }

        public async Task<AccountDomainModel?> UpdateAccount(AccountDomainModel toUpdate)
        {
            using var con = new MySqlConnection(_connectionString);

            var result = await con.ExecuteAsync(Dapper.Account.Update, toUpdate);
            return result == 1 ?
                await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountById, toUpdate) :
                default;
        }

        public async Task<AccountDomainModel?> DeleteAccount(string accountId)
        {
            var d = new { id = accountId };
            using var con = new MySqlConnection(_connectionString);

            var res = await con.QuerySingleOrDefaultAsync<AccountDomainModel>(Dapper.Account.GetAccountById, d);

            var result = await con.ExecuteAsync(Dapper.Account.DeleteById, d);
            return result == 1 ? res : default;
        }
    }
}
