using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    class StandardRoom:Room
    {
        public bool requireWifi { get; set; }
        public bool requireBreakfast { get; set; }

        public StandardRoom() : base() { }

        public StandardRoom(int rn, string bc,double dr, bool ia) : base(rn, bc, dr, ia)
        {
            requireBreakfast = false;
            requireWifi = false;
        }

        public override double CalculateCharges()
        {
            double addOnCharge;
            if (requireWifi == true && requireBreakfast == true)
            {
                addOnCharge = 30;
            }
            else
            {
                if (requireWifi == true)
                {
                    addOnCharge = 10;
                }
                else if (requireBreakfast == true)
                {
                    addOnCharge = 20;
                }
                else
                {
                    addOnCharge = 0;
                }
            }
            return dailyRate + addOnCharge;
        }

        public override string ToString()
        {
            return base.ToString() + "\tRequire Wifi:" + requireWifi 
                + "\tRequire Breakfast:" + requireBreakfast;
        }
    }
}
