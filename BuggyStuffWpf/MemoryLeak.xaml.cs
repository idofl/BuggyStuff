using BuggyStuff.Shared;
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
using System.Windows.Shapes;

namespace BuggyStuffWpf
{
    /// <summary>
    /// Interaction logic for MemoryLeak.xaml
    /// </summary>
    public partial class MemoryLeak : Window
    {       
        public MemoryLeak()
        {

            BigProduct product = Memory.CreateNewProduct();
            Memory.CacheProduct(product, ProductRemoved);
            InitializeComponent();
        }

        private void ProductRemoved(string key)
        {
            // Do something...
        }
    }
}
