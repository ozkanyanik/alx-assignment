using EFCore;
using EFCore.Entities;
using EFDataService.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using EFDataService.Model;

namespace EFDataService.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        protected readonly EFCoreContext Context;

        public ServiceRepository(EFCoreContext context)
        {
            this.Context = context;
        }
        public IEnumerable<Service> Get(SearchArgument arg)
        {
            if (arg.Count == 0)
            {
                arg.Count = 5;
            }

            return !string.IsNullOrEmpty(arg.Name)
                ? Context.Services.Where(m => m.ServiceName.Contains(arg.Name)).Skip(arg.Index).Take(arg.Count)
                : Context.Services.Skip(arg.Index).Take(arg.Count);
        }
    }
}
