using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    class Guest
    {
        public string Name { get; set; }
        public string PassportNum { get; set; }
        public Stay HotelStay { get; set; }
        public Membership Member { get; set; }
        public bool IsCheckedIn { get; set; }

        public Guest() { }
        public Guest(string name, string passportNum, Stay hotelStay, Membership member)
        {
            Name = name;
            PassportNum = passportNum;
            HotelStay = hotelStay;
            Member = member;
            IsCheckedIn = false;
        }

        public override string ToString()
        {
            return $"Name: {Name} \tPassport No: {PassportNum} \tStay: {HotelStay} \tMembership: {Member}";
        }
    }
}
