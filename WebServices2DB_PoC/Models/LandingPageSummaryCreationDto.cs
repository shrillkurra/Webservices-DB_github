using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices2DB_PoC.Models
{
    public class LandingPageSummaryCreationDto
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public string Description { get; set; }

        public int GetLandingPageDetailsCount
        {
            get
            {
                return LandingPageDetails.Count;
            }
        }
        public ICollection<LandingPageDetailDto> LandingPageDetails { get; set; }
        = new List<LandingPageDetailDto>();
    }
}
