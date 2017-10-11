using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices2DB_PoC.Entities
{
    public class LandingPageSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Url { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public ICollection<LandingPageDetail> LandingPageDetails { get; set; }
        = new List<LandingPageDetail>();

    }
}
