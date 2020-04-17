using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ethosIQ_NGCC_Shared
{
    public class AgentState
    {
        public string Id;
        public string State;
        
        public AgentState(string Id, string State)
        {
            this.Id = Id;
            this.State = State;
        }
    }
}
