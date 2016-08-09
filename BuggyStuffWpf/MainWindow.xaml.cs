using BuggyStuff.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BuggyStuffWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThreadPool.SetMinThreads(20, 20);
        }

        private void btnKillCpu_Click(object sender, RoutedEventArgs e)
        {
            // Hang the CPU, but don't hang the UI
            Task.Run(() => Performance.KillCpu());
        }

        private void btnMemoryLeak_Click(object sender, RoutedEventArgs e)
        {
            new MemoryLeak().Show();
            //BigProduct product = Memory.CreateNewProduct();
            //Memory.CacheProduct(product, null);
        }

        private void btnGc_Click(object sender, RoutedEventArgs e)
        {
            Memory.CauseGC1And2();
        }

        private void btnUiUtilization_Click(object sender, RoutedEventArgs e)
        {
            new CustomAnimationExample().Show();
        }

        private void btnUnevenWorkload_Click(object sender, RoutedEventArgs e)
        {
            Concurrency.UnevenWorkload();
            MessageBox.Show("Done");
        }

        private void btnOversubscription_Click(object sender, RoutedEventArgs e)
        {
            Concurrency.Oversubscription();
            MessageBox.Show("Done");
        }

        private void btnLockConvoy_Click(object sender, RoutedEventArgs e)
        {
            Concurrency.LockConvoy();
            MessageBox.Show("Done");
        }
    }
}
