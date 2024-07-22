using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    abstract class Room
    {
        public int roomNumber { get; set; }
        public string bedConfiguration { get; set; }
        public double dailyRate { get; set; }
        public bool isAvail { get; set; }

        public Room() { }
        public Room(int rn, string bc,double dr,bool ia)
        {
            roomNumber = rn;
            bedConfiguration = bc;
            dailyRate = dr;
            isAvail = ia;
        }

        public abstract double CalculateCharges();

        public override string ToString()
        {
            return "Room Number:" + roomNumber + "\tBed Configuration:" + bedConfiguration
                + "\tDaily Rate:" + dailyRate + "\tAvailability:" + isAvail;
        }

    }
}
