using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoApp.Backend.Data.Repositories
{
    public interface IUserRepository
    {
        bool IsPhoneNumberTaken(string phoneNumber);
    }
}
