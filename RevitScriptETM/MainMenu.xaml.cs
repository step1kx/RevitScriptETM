using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitScriptETM
{
    public class TaskItem
    {
        public int TaskNumber { get; set; }
        public string FromSection { get; set; }
        public string ToSection { get; set; }
        public string TaskIssuer { get; set; }
        public bool TaskCompleted { get; set; }
        public string TaskHandler { get; set; }
        public string ScreenShot { get; set; }
        public string TaskDescription { get; set; }
    }
    public partial class MainMenu : Window
    {
        public ObservableCollection<TaskItem> TaskItems { get; set; }

        public MainMenu()  
        {
            InitializeComponent();  
            TaskItems = new ObservableCollection<TaskItem>
            {
                new TaskItem { TaskNumber = 1, TaskCompleted = true },
                new TaskItem { TaskNumber = 2, TaskCompleted = true },
                new TaskItem { TaskNumber = 3, TaskCompleted= true }
            };
            DataContext = this;

            Console.WriteLine("TaskItems count: " + TaskItems.Count);
        }
    }
}
