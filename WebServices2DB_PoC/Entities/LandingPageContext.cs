using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices2DB_PoC.Entities
{
    public class LandingPageContext : DbContext
    {
        public DbSet<LandingPageSummary> LandingPageSummaries { get; set; }

        public DbSet<LandingPageDetail> LandingPageDetails { get; set; }

        public LandingPageContext(DbContextOptions<LandingPageContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
