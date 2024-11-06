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
        public int TaskTaken { get; set; }
        public int TaskCompleted { get; set; }
        public string TaskHandler { get; set; }
        public int TaskApproval { get; set; }
        public string WhoApproval { get; set; }
        public string ScreenShot { get; set; }
        public string TaskDescription { get; set; }
        public string TaskExplanation { get; set; }
        public string TaskView { get; set; }
        public string WhoTaken { get; set; }
       
        public TaskItems(int taskNumber, string fromSection, string toSection,
                         string taskIssuer, int taskCompleted, string taskHandler,
                         int taskApproval, string whoApproval, string screenShot, string taskDescription, string taskView, int taskTaken, string taskExplanation, string whoTaken)
        {
            TaskNumber = taskNumber;
            FromSection = fromSection;
            ToSection = toSection;
            TaskIssuer = taskIssuer;
            TaskTaken = taskTaken;
            TaskCompleted = taskCompleted;
            TaskHandler = taskHandler;
            TaskApproval = taskApproval;
            WhoApproval = whoApproval;
            ScreenShot = screenShot;
            TaskDescription = taskDescription;
            TaskView = taskView;
            TaskExplanation = taskExplanation;
            WhoTaken = whoTaken;
        }
    }
}
