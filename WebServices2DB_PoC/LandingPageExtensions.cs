using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices2DB_PoC.Entities;

namespace WebServices2DB_PoC
{
    public static class LandingPageExtensions
    {
        public static void EnsureSeedDataForContext(this LandingPageContext context)
        {
            if (context.LandingPageSummaries.Any())
                return;

            var landingPageSummaries = new List<LandingPageSummary>()
            {
                new LandingPageSummary()
                {
                    Name = "Google",
                    Url = "mail.google.com",
                    Description = "My google account.",
                    LandingPageDetails = new List<LandingPageDetail>()
                    {
                        new LandingPageDetail()
                        {
                            UserId = "Sri@gmail.com",
                            Password = "password123",
                            Description = "My first google account"
                        },
                        new LandingPageDetail()
                        {
                            UserId = "Sri_2@gmail.com",
                            Password = "123password",
                            Description = "My second google account"
                        },
                    }
                },
                new LandingPageSummary()
                {
                    Name = "Apple",
                    Url = "mail.apple.com",
                    Description = "My apple account.",
                    LandingPageDetails = new List<LandingPageDetail>()
                    {
                        new LandingPageDetail()
                        {
                            UserId = "Sri@apple.com",
                            Password = "passwdeececord123",
                            Description = "My first apple account"
                        },
                        new LandingPageDetail()
                        {
                            UserId = "Sri_2@apple.com",
                            Password = "123passcdfvfeword",
                            Description = "My second apple account"
                        },
                    }
                },
                new LandingPageSummary()
                {
                    Name = "Bank Of America",
                    Url = "mailbofa.google.com",
                    Description = "My Bofa account.",
                    LandingPageDetails = new List<LandingPageDetail>()
                    {
                        new LandingPageDetail()
                        {
                            UserId = "Srilaks@gmail.com",
                            Password = "passwordwerd123",
                            Description = "My first Bofa account"
                        },
                        new LandingPageDetail()
                        {
                            UserId = "Srilaks_2@gmail.com",
                            Password = "123edfrpassword",
                            Description = "My second Bofa account"
                        },
                    }
                }
            };

            context.LandingPageSummaries.AddRange(landingPageSummaries);
            context.SaveChanges();
        }
    }
}
