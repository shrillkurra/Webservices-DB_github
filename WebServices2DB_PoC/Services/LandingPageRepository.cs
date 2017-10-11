using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices2DB_PoC.Entities;

namespace WebServices2DB_PoC.Services
{
    public class LandingPageRepository : ILandingPageRepository
    {
        private LandingPageContext _context;

        public LandingPageRepository(LandingPageContext context)
        {
            _context = context;
        }

        // V2
        // Helper method to check the existence of a high-level account
        public bool AccountExists(int accountId)
        {
            return _context.LandingPageSummaries.Any(acc => acc.Id == accountId);
        }

        public LandingPageDetail GetLandingPageDetail(int id, int detailId)
        {
            return _context.LandingPageDetails.Where(c => c.LandingPageSummaryId == id && c.Id == detailId).FirstOrDefault();
        }

        public IEnumerable<LandingPageDetail> GetLandingPageDetails(int id)
        {
            return _context.LandingPageDetails.Where(c => c.LandingPageSummaryId == id).ToList();
        }

        public IEnumerable<LandingPageSummary> GetLandingPageSummaries()
        {
            return _context.LandingPageSummaries.OrderBy(c => c.Name).ToList();
        }

        public LandingPageSummary GetLandingPageSummary(int id, bool includeDetails)
        {
            if (includeDetails)
            {
                return _context.LandingPageSummaries
                    .Include(c => c.LandingPageDetails)
                    .Where(c => c.Id == id).FirstOrDefault();
            }
            return _context.LandingPageSummaries.Where(o => o.Id == id).FirstOrDefault();
        }

        // Version 2
        public void AddIndividualAccountForSummaryAccount(int mainAccountId, LandingPageDetail landingPageDetail)
        {
            var curSummaryAccount = GetLandingPageSummary(mainAccountId, false);
            curSummaryAccount.LandingPageDetails.Add(landingPageDetail);
            Save();
        }

        public void DeleteIndividualAccountForSummaryAccount(LandingPageDetail landingPageDetail)
        {
            _context.LandingPageDetails.Remove(landingPageDetail);

        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
