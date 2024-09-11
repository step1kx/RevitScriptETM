using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitScriptETM
{
    public class FilterItems
    {
        public string FromSection { get; set; }
        public string ToSection { get; set; }
        public string TaskIssuer { get; set; }
        public int TaskCompleted { get; set; }
        public string TaskHandler { get; set; }
        public int TaskApproval { get; set; }
        public string WhoApproval { get; set; }
    }
}
