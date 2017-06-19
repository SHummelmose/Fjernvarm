using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LoginVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Måler ID er påkrævet")]
        public int GaugeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Password skal udfyldes")]
        [MaxLength(8, ErrorMessage = "Maks 8 karakterer")]
        public string Password { get; set; }
    }

    public class ReadingVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Kilowatt-timer er påkrævet")]
        public int kWh { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Kubikmeter er påkrævet")]
        public int CubicMeters { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Timer er påkrævet")]
        public int Hours { get; set; }
    }
}