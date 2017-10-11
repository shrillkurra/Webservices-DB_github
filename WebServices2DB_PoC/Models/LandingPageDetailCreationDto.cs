using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices2DB_PoC.Models
{
    public class LandingPageDetailCreationDto
    {
        public string UserId { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }
    }
}
