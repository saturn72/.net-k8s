using Dapper;
using Datafeed.Rest.Domain;
using Datafeed.Rest.Services;
using System.Data.SqlClient;

namespace Datafeed.Rest.Data
{
    public class DapperRepository : IRepository
    {
        private readonly string _connectionString;

        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString; ;
        }
        public async Task<IEnumerable<TemplateEntry>> GetEntriesByVersion(GetAllEntriesContext context)
        {
            var sql = SqlScripts.GetEntriesByVersion(context);

            using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<TemplateEntry>(sql);
        }

        public async Task<Template> GetTemplateByPath(string path)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<Template>(SqlScripts.GetTemplateByPath, new { Path = path });
        }

        private sealed class SqlScripts
        {
            private const string SelectFrom = $"SELECT FROM {nameof(Template)}s";
            internal const string GetTemplateByPath = $"{SelectFrom} WHERE [Path] = path";

            internal static string GetEntriesByVersion(GetAllEntriesContext context)
            {
                var where = $"[TemplateId] = {context.Template.Id} AND [Version] = {context.Template.Version}";
                return $"{SelectFrom} WHERE {where} " +
                    //$" ORDER BY {orderBy} {sortOrder} " +
                    $"OFFSET {context.OffSet} ROWS " +
              $"FETCH NEXT {context.PageSize} ROWS ONLY;";
            }
        }
    }
}
