using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BadgerysCreekHotel.Models
{
    public class Customer
    {
        [Key, Required]
        [EmailAddress]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }
        [StringLength(20, MinimumLength = 2)]
        public string Surname { get; set; }
        [StringLength(20, MinimumLength = 2)]
        public string GivenName { get; set; }
        [RegularExpression(@"^[0-9]{4}$")]
        public string Postcode { get; set; }
        public ICollection<Booking> TheBookings { get; set; }
    }
}
