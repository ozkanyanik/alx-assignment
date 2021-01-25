using EFCore;
using EFCore.Entities;
using EFDataService.Helper;
using EFDataService.Repositories;
using EFDataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFDataService
{
    public static class EFDataServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddEFDataService(this IServiceCollection services)
        {
            services.AddEFCore();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
            return services;
        }

        public static IApplicationBuilder UseEFCore(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null)
            {
                return app;
            }

            var context = serviceScope.ServiceProvider.GetRequiredService<EFCoreContext>();
            if (context.Database.EnsureCreated())
            {
                InsertDummyData(context);
            }
           
            return app;
        }

        private static void InsertDummyData(EFCoreContext context)
        {
            #region Adding Users
            var userList = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "john.doe@algroup.org",
                    Password = SecurityHandler.EncryptString(SecurityHandler.DefaultKey,"1234"),
                    Fullname = "John Doe"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "hiro.hamata@algroup.org",
                    Password = SecurityHandler.EncryptString(SecurityHandler.DefaultKey,"1234"),
                    Fullname = "Hiro Hamata"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "jane.brown@algroup.org",
                    Password = SecurityHandler.EncryptString(SecurityHandler.DefaultKey,"1234"),
                    Fullname = "Jane Brown"
                }

            };
            context.AddRange(userList);
            #endregion

            #region Adding Services
            var serviceNameList = new string[]
            {
                "Web Operation",
                "Web Dynamics",
                "Mobile Fiber",
                "Mobile Nexus",
                "Mobile Revolution",
                "Web Spread",
                "Cloud Post",
                "Web Foster",
                "Web Agile",
                "Web Design",
                "Application Build",
                "Web Vortex",
                "Cloud Storage",
                "Cloud Development",
                "Cloud Email Service",
                "Web Scope",
                "Project Management",
                "Cloud Signal",
                "B2C Service",
                "B2B Service",
                "Mobile Development",
                "Application Design",
                "Database Development",
                "Backup Operations",
                "Agile",
                "Scrum",
                "Service Consultant",
                "Web Application Development",
                "Cloud Service Operations",
                "CI/CD Implementation"
            };
            var serviceList = serviceNameList.Select(
                sn => new Service
                {
                    Id = Guid.NewGuid(), 
                    ServiceName = sn, 
                    Description = $"Description of the {sn}"
                }).ToList();

            context.AddRange(serviceList);
            #endregion

            #region Adding Promo Codes

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var promoCodeList = new List<PromoCode>();
            for (var x = 0; x < 10; x++)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < 9; i++)
                {
                    var random = new Random();
                    sb.Append(chars[random.Next(0, chars.Length)].ToString());
                }

                var s = serviceList.OrderBy(x => new Random().Next()).Take(1).FirstOrDefault();
                var u = userList.OrderBy(x => new Random().Next()).Take(1).FirstOrDefault();

                promoCodeList.Add(new PromoCode
                {
                    Id = Guid.NewGuid(),
                    ServiceId = s.Id,
                    UserId = u.Id,
                    Code = sb.ToString(),
                    Bonus = 100
                });

            }
            context.AddRange(promoCodeList);
            #endregion

            context.SaveChanges();
        }
    }
}
