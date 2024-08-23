﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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

        public MainMenu()
        {
            InitializeComponent();
        }


    }
}
