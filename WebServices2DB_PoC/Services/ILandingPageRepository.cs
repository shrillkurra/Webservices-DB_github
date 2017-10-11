using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices2DB_PoC.Entities;

namespace WebServices2DB_PoC.Services
{
    public interface ILandingPageRepository
    {
        // V2
        // Interface definition for the method
        bool AccountExists(int accountId);

        IEnumerable<LandingPageSummary> GetLandingPageSummaries();
        LandingPageSummary GetLandingPageSummary(int id, bool includeDetails);
        IEnumerable<LandingPageDetail> GetLandingPageDetails(int id);
        LandingPageDetail GetLandingPageDetail(int id, int detailId);

        void AddIndividualAccountForSummaryAccount(int mainAccountId, LandingPageDetail landingPageDetail);
        void DeleteIndividualAccountForSummaryAccount(LandingPageDetail landingPageDetail);
        bool Save();
    }
}
