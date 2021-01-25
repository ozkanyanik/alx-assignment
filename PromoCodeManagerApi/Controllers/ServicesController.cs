using EFCore.Entities;
using EFCore.Enums;
using EFDataService.Model;
using EFDataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PromoCodeManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IStringLocalizer<ServicesController> _localizer;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;
        public ServicesController(IServiceRepository serviceRepository, IPromoCodeRepository promoCodeRepository, IStringLocalizer<ServicesController> localizer)
        {
            _localizer = localizer;
            _serviceRepository = serviceRepository;
            _promoCodeRepository = promoCodeRepository;
        }


        [HttpPost]
        [Route("Search")]
        public IActionResult Search(SearchArgument arg)
        {
            try
            {
                
                var services = _serviceRepository.Get(arg).ToList();
                var userPromoCodesString = User.FindFirst("PromoCodes")?.Value;
                if (string.IsNullOrEmpty(userPromoCodesString))
                {
                    return Ok(services);
                }

                var promoCodes = JsonConvert.DeserializeObject<List<PromoCode>>(userPromoCodesString).Where(p => p.Activated);
                foreach (var pc in promoCodes)
                {
                    var s = services.FirstOrDefault(m => m.Id.Equals(pc.ServiceId));
                    if (s != null)
                    {
                        s.Status = ServiceStatus.Activated;
                    }
                }
                return Ok(services);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("ApplyPromoCode")]
        public IActionResult ApplyPromoCode(PromoCodeApplyArgument arg)
        {
            try
            {
                var promoCode = _promoCodeRepository.Get(arg.PromoCode, arg.ServiceId);

                if (promoCode == null)
                {
                    return Ok(_localizer.GetString("PromoCodeNotFound").Value);
                }

                if (!promoCode.UserId.ToString().Equals(User.FindFirst(ClaimTypes.UserData)?.Value))
                {
                    return Ok(_localizer.GetString("PromoCodeForAnotherUser").Value);
                }

                _promoCodeRepository.Apply(promoCode);

                return Ok(_localizer.GetString("Activated").Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
