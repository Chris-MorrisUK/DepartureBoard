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

namespace DarwinFeed
{
    /// <summary>
    /// Interaction logic for DestinationRow.xaml
    /// </summary>
    public partial class DestinationRow : UserControl
    {
        public DestinationRow()
        {
            InitializeComponent();
            this.Loaded += DestinationRow_Loaded;
        }

        void DestinationRow_Loaded(object sender, RoutedEventArgs e)
        {
            grdOutter.ColumnDefinitions[0].MaxWidth = this.ActualWidth * 0.7;
        }

  

        
    }
}
