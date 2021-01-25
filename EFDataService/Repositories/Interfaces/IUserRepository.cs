using EFCore.Entities;
using EFDataService.Model;

namespace EFDataService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(string email);

        UserDTO Login(LoginArgument arg);
    }
}
