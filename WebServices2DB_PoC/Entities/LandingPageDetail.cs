using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices2DB_PoC.Entities
{
    public class LandingPageDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(70)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        public string Description { get; set; }

        [ForeignKey("LandingPageSummaryId")]
        public LandingPageSummary LandingPageSummary { get; set; }
        public int LandingPageSummaryId { get; set; }
    }
}
