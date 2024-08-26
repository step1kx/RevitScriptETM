using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitScriptETM
{
    public class TaskItems
    {
        public int TaskNumber { get; set; }
        public string FromSection { get; set; }
        public string ToSection { get; set; }
        public string TaskIssuer { get; set; }
        public bool TaskCompleted { get; set; }
        public string TaskHandler { get; set; }
        public bool TaskApproval { get; set; }
        public string WhoApproval { get; set; }
        public string ScreenShot { get; set; }
        public string TaskDescription { get; set; }
    }
}
