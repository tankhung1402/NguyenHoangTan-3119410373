using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class NationalInsuranceService : INationalInsuranceService
    {
        private decimal NIRate;
        private decimal NIC;

        public decimal NIContribution(decimal totalAmount)
        {
            if(totalAmount < 719)
            {
                NIRate = .0m;
                NIC = 0m;
            }
            else if(totalAmount >= 719 && totalAmount < 4176)
            {
                NIRate = .12m;
                NIC = (totalAmount - 719) * NIRate;
            }
            else if (totalAmount > 4176)
            {
                NIRate = .20m;
                NIC = ((4176 - 719 ) * .12m) + ((totalAmount - 4176) * NIRate);
            }
            return NIC;
        }
    }
}
