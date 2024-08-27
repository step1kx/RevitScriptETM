using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitScriptETM
{
    public class TaskItems
    {
        public int taskNumber { get; set; }
        public string fromSection { get; set; }
        public string toSection { get; set; }
        public string taskIssuer { get; set; }
        public int taskCompleted { get; set; }
        public string taskHandler { get; set; }
        public int taskApproval { get; set; }
        public string whoApproval { get; set; }
        public string screenShot { get; set; }
        public string taskDescription { get; set; }

        public TaskItems(int TaskNumber, string FromSection, string ToSection,
        string TaskIssuer, int TaskCompleted, string TaskHandler,
        int TaskApproval, string WhoApproval, string ScreenShot, string TaskDecription)
        {
            taskNumber = TaskNumber;
            fromSection = FromSection;
            toSection = ToSection;
            taskIssuer = TaskIssuer;
            taskCompleted = TaskCompleted;
            taskHandler = TaskHandler;
            taskApproval = TaskApproval;
            whoApproval = WhoApproval;
            screenShot = ScreenShot;
            taskDescription = TaskDecription;
                

        }
    }

    
}
