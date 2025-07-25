using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUMiniHotelSystem.DAL.Models
{
    public class RoomDTO
    {
        public int RoomID { get; set; }   // Nên có để đặt phòng!
        public string RoomNumber { get; set; }
        public string RoomDetailDescription { get; set; }
        public int RoomMaxCapacity { get; set; }
        public string RoomTypeName { get; set; }
        public int RoomStatus { get; set; } // 0: Available, 1: Booked
        public decimal RoomPricePerDay { get; set; }
        public string TypeDescription { get; set; }
        public string TypeNote { get; set; }

        public string RoomStatusString => RoomStatus switch
        {
            0 => "Available",
            1 => "Booked",
            _ => "Unknown"
        };
    }

}
