using MahApps.Metro.Controls;
using Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private void LaunchGit(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start(new ProcessStartInfo
            //{
            //    FileName = "http://www.github.com",
            //    UseShellExecute = true
            //});
            System.Diagnostics.Process.Start("http://www.github.com");
        }
    }
}
