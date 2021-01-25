using EFCore.Entities;
using System.Collections.Generic;
using EFDataService.Model;

namespace EFDataService.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        IEnumerable<Service> Get(SearchArgument arg);
    }
}
