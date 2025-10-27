//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;

//namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
//{
//    public static class ApplicationDbInitializer
//    {
//        public static async Task SeedDefaultDataAsync(IServiceProvider services)
//        {
//                try
//                {
//                    var context = services.GetRequiredService<ApplicationDbContext>();

//                    await SeedTranslationsAsync(context);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("An error occurred while seeding the database.");
//                    Console.WriteLine(ex.Message);
//                }
//        }

//        private static async Task SeedTranslationsAsync(ApplicationDbContext context)
//        {
//            var englishTranslation = await context.OrganizationTranslation
//                .FirstOrDefaultAsync(t => t.LanguageCode == "en" && t.IsDefault);

//            if (englishTranslation == null)
//            {
//                englishTranslation = new OrganizationTranslation
//                {
//                    LanguageCode = "en",
//                    IsDefault = true,
//                    JsonData = TranslationData.English,
//                    CreationDate = DateTime.UtcNow
//                };
//                await context.OrganizationTranslation.AddAsync(englishTranslation);
//            }
//            else
//            {
//                englishTranslation.JsonData = TranslationData.English;
//            }

//            var arabicTranslation = await context.OrganizationTranslation
//                .FirstOrDefaultAsync(t => t.LanguageCode == "ar" && t.IsDefault);

//            if (arabicTranslation == null)
//            {
//                arabicTranslation = new OrganizationTranslation
//                {
//                    LanguageCode = "ar",
//                    IsDefault = true,
//                    JsonData = TranslationData.Arabic,
//                    CreationDate = DateTime.UtcNow
//                };
//                await context.OrganizationTranslation.AddAsync(arabicTranslation);
//            }
//            else
//            {
//                arabicTranslation.JsonData = TranslationData.Arabic;
//            }

//            await context.SaveChangesAsync();
//        }
//    }
//}
