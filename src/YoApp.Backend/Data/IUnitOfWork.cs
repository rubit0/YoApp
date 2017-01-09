using System.Threading.Tasks;
using YoApp.Backend.Data.Repositories;

namespace YoApp.Backend.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        void Complete();
        Task CompleteAsync();
    }
}
