using EFCore.Entities;

namespace EFDataService.Repositories.Interfaces
{
    public interface IPromoCodeRepository
    {
        PromoCode Get(string promoCode, string serviceId);
        void Apply(PromoCode promoCode);

    }
}
