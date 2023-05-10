using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwarf2Lx200Adapter
{
    public class TelescopeData
    {
        public double RightAscension { get; set; }
        public double Declination { get; set; }
        public TelescopeData()
        {
            RightAscension = 0;
            Declination = 0;
        }

        public TelescopeData(double Ra, double Dec)
        {
            RightAscension = Ra;
            Declination = Dec;
        }
    }

}
