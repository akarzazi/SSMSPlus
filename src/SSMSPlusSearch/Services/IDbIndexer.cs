using System.Threading.Tasks;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusSearch.Entities;

namespace SSMSPlusSearch.Services
{
    public interface IDbIndexer
    {
        Task<int> DbExistsAsync(DbConnectionString dbConnectionString);
        Task<int> IndexAsync(DbConnectionString dbConnectionString);
        Task<int> ReIndexAsync(DbConnectionString dbConnectionString);
    }
}