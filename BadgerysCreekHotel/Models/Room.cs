using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BadgerysCreekHotel.Models
{
    public class Room
    {
        [Key][Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [RegularExpression(@"^[G.1.2.3]$")]
        public string Level { get; set; }
        [RegularExpression(@"^[1-3]$")]
        public int BedCount { get; set; }
        [Range(50, 300)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public ICollection<Booking> TheBookings { get; set; }
    }
}
