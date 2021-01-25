using System;
using System.Collections.Generic;

namespace EFCore.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
    }
}
