using System.Threading.Tasks;
using YoApp.Data.Repositories;

namespace YoApp.Data
{
    public interface IUnitOfWork
    {
        void Complete();
        Task CompleteAsync();
    }
}
