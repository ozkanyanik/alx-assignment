using EFCore.Entities;
using EFDataService.Model;
using EFDataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PromoCodeManagerApi.Controllers;
using System;
using System.Security.Claims;

namespace RestApi.UnitTests.Controllers
{
    [TestFixture]
    public class ServicesControllerTests
    {
        private Mock<IServiceRepository> _serviceRepository;
        private Mock<IPromoCodeRepository> _promoCodeRepository;
        private Mock<IStringLocalizer<ServicesController>> _localizer;
        private ServicesController controller;
        [SetUp]
        public void Setup()
        {
            
            _serviceRepository = new Mock<IServiceRepository>();
            _promoCodeRepository = new Mock<IPromoCodeRepository>();
            _localizer = new Mock<IStringLocalizer<ServicesController>>();

            #region Mock PromoCodes

            //Mock Data For Logged User
            _promoCodeRepository.Setup(x => x.Get("A", "1"))
                .Returns(new PromoCode
                {
                    ServiceId = Guid.Empty,
                    Activated = false,
                    Bonus = 1,
                    Code = "A",
                    Id = Guid.Empty,
                    UserId = Guid.Empty
                });

            //Mock Data For Another User
            _promoCodeRepository.Setup(x => x.Get("B", "1"))
                .Returns(new PromoCode
                {
                    ServiceId = Guid.Empty,
                    Activated = false,
                    Bonus = 1,
                    Code = "A",
                    Id = Guid.Empty,
                    UserId = Guid.NewGuid()
                });
            #endregion

            #region Mock Claims

            //Mock Data of Logged User (Fake User Claims)
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "John Doe"),
                    new Claim(ClaimTypes.UserData, Guid.Empty.ToString()),
                    new Claim("PromoCodes", JsonConvert.SerializeObject( new PromoCode
                    {
                        ServiceId = Guid.Empty,
                        Activated = false,
                        Bonus = 1,
                        Code = "A",
                        Id = Guid.Empty,
                        UserId = Guid.Empty
                    })),
                }, "mock"));

            controller = new ServicesController(_serviceRepository.Object, _promoCodeRepository.Object, _localizer.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
            #endregion

            #region Mock Data For Resources (DisplayMessages)

            var key = "PromoCodeNotFound";
            var message = "Promo Code not found or has been used before!!";

            var localizedString = new LocalizedString(key, message);
            _localizer.Setup(_ => _[key]).Returns(localizedString);

            key = "PromoCodeForAnotherUser";
            message = "You can not use this Promo Code since it is assigned to another user!";

            localizedString = new LocalizedString(key, message);
            _localizer.Setup(_ => _[key]).Returns(localizedString);

            key = "Activated";
            message = "Promo code activated";

            localizedString = new LocalizedString(key, message);
            _localizer.Setup(_ => _[key]).Returns(localizedString);
            #endregion
        }

        [Test]
        public void ApplyPromoCode_WrongPromocode_ReturnsMessage_PromoCodeNotFound()
        {
            var promoCode = new PromoCodeApplyArgument
            {
                ServiceId = "1",
                PromoCode = "1"
            };
            var result = controller.ApplyPromoCode(promoCode) as OkObjectResult;
            var expextedMessage = "Promo Code not found or has been used before!!";
            
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expextedMessage, result.Value);
        }

        [Test]
        public void ApplyPromoCode_CorrectPromocode_ReturnsMessage_PromoCodeForAnotherUser()
        {
            var promoCode = new PromoCodeApplyArgument
            {
                ServiceId = "1",
                PromoCode = "B"
            };
            var result = controller.ApplyPromoCode(promoCode) as OkObjectResult;
            var expextedMessage = "You can not use this Promo Code since it is assigned to another user!";

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expextedMessage, result.Value);
        }

        [Test]
        public void ApplyPromoCode_CorrectPromocode_ReturnsMessage_Activated()
        {
            var promoCode = new PromoCodeApplyArgument
            {
                ServiceId = "1",
                PromoCode = "A"
            };
            var result = controller.ApplyPromoCode(promoCode) as OkObjectResult;
            var expextedMessage = "Promo code activated";

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expextedMessage, result.Value);
        }
    }
}
