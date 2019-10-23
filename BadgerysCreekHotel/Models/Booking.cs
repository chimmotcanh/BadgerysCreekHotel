using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BadgerysCreekHotel.Models
{
    public class Booking
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [ForeignKey("Room")]
        public int RoomID { get; set; }
        [ForeignKey("Customer")]
        public string CustomerEmail { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public Room TheRoom { get; set; }
        public Customer TheCustomer { get; set; }
    }
}
