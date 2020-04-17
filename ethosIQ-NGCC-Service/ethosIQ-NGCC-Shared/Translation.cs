using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ethosIQ_NGCC_Shared
{
    public class Translation
    {
        public string PrimaryID;
        public string SecondaryID;

        public Translation(string PrimaryID, string SecondaryID)
        {
            this.PrimaryID = PrimaryID;
            this.SecondaryID = SecondaryID;
        }
    }
}
