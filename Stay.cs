using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    class Stay
    {
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public List<Room> roomList { get; set; } = new List<Room>(); // list that stores rooms belong to a guest

        public Stay() { }
        public Stay(DateTime checkinDate, DateTime checkoutDate)
        {
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }

        public void AddRoom(Room r)
        {
            roomList.Add(r);
        }

        public double CalculateTotal()
        {
            int daystayed = CheckoutDate.Subtract(CheckinDate).Days;
            double total = 0;
            foreach(Room r in roomList)
            {
                double roomcharge = r.CalculateCharges();
                total += roomcharge * daystayed;
            }
            return total;
            
        }

        public override string ToString()
        {
            return $"Check in: {CheckinDate} \tCheck out: {CheckoutDate}";
        }
    }
}
