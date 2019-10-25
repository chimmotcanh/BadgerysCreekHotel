using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BadgerysCreekHotel.Models.ViewModels
{
    public class BookARoom
    {
        [ForeignKey("Room")]
        public int RoomID { get; set; }
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CheckIn { get; set; }
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CheckOut { get; set; }
    }
}
