using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    class Membership
    {
        public string Status { get; set; }
        public int Points { get; set; }

        public Membership() { }
        public Membership(string status, int points)
        {
            Status = status;
            Points = points;
        }
        public void EarnPoints(double p)
        {
            Points += (int)(p / 10);
            if (Points >= 200)
            {
                Status = "Gold";
            }
            else if (Points >= 100)
            {
                Status = "Silver";
            }
        }
        public bool RedeemPoints(int point)
        {
            if (Status == "Silver" || Status == "Gold")
            {
                Points -= point;
                return true;
            }
            else return false;
        }
        public override string ToString()
        {
            return $"Status: {Status} \tPoints: {Points}";
        }
    }
}
