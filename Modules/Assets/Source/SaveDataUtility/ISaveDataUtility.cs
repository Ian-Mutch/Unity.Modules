using System.Threading;
using System.Threading.Tasks;

namespace Modules
{
    public interface ISaveDataUtility
    {
        Task<WriteDataResult> WriteAsync(SaveData data, CancellationToken cancellationToken = default);
        Task<ReadDataResult> ReadAsync(CancellationToken cancellationToken = default);
    }
}
