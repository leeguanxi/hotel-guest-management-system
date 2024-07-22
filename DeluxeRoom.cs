using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P08_Team9_PRG2Assignment
{
    class DeluxeRoom: Room
    {
        public bool additionalBed { get; set; }
        
        public DeluxeRoom(): base() { }

        public DeluxeRoom(int rn,string bc,double dr,bool ia) : base(rn, bc, dr, ia)
        { 
            additionalBed = false;
        }
        public override double CalculateCharges()
        {
            
            if (additionalBed == true)
            {
                return dailyRate + 25;
            }
            else
            {
                return dailyRate;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\t Additional Bed:" + additionalBed;
        }

    }
}
