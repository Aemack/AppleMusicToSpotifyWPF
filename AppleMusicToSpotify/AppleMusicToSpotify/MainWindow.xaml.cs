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

namespace AppleMusicToSpotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        private void AppleToSpotifyClicked(object sender, RoutedEventArgs e)
        {
            vm.AppleToSpotifyClicked();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            vm.Hyperlink_RequestNavigate(sender, e);
        }
    }
}
