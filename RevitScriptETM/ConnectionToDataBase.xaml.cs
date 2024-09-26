using System;
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
    /// <summary>
    /// Логика взаимодействия для ConnectionToDataBase.xaml
    /// </summary>
    public partial class ConnectionToDataBase : Window
    {
        public ConnectionToDataBase()
        {
            InitializeComponent();
        }

        private void CheckConnectionToDataBase()
        {
            
        }

        //private void MovingWin(object sender, EventArgs e)
        //{
        //    if (Mouse.LeftButton == MouseButtonState.Pressed)
        //    {
        //        ConnectionToDataBase.Window.DragMove();
        //    }
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;//!!!
            Close();
        }
    }
}
