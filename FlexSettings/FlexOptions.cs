using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexSettings
{
    public class FlexOptions
    {
        public string Coin = "eth";

        public string IP { get; set; }
        public string Address { get; set; }
        public long PayoutLimit { get; set; }
        public long GasLimit { get; set; }
        public int Interval { get; set; }
        public bool Debug { get; set; }

        public override string ToString()
        {
            return "address=" + Address
                + "&payoutLimit=" + PayoutLimit
                + "&maxFeePrice=" + GasLimit
                + "&ipAddress=" + IP
                + "&coin=" + Coin;
        }
    }
}
