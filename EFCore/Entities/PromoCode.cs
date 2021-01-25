using System;

namespace EFCore.Entities
{
    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public double Bonus { get; set; }
        public bool Activated { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
    }
}
