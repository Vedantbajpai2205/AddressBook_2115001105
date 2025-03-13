using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class RequestModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}
