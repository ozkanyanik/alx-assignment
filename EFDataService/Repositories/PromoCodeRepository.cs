using EFCore;
using EFCore.Entities;
using EFDataService.Repositories.Interfaces;
using System.Linq;

namespace EFDataService.Repositories
{
    public class PromoCodeRepository : IPromoCodeRepository
    {
        protected readonly EFCoreContext Context;

        public PromoCodeRepository(EFCoreContext context)
        {
            this.Context = context;
        }
        public PromoCode Get(string promoCode, string serviceId)
        {
            return Context.PromoCodes.FirstOrDefault(m => !m.Activated &&  m.Code.Equals(promoCode) && m.ServiceId.ToString().Equals(serviceId));
        }

        public void Apply(PromoCode promoCode)
        {
            promoCode.Activated = true;
            Context.PromoCodes.Update(promoCode);
            Context.SaveChanges();
        }
    }
}
