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

namespace Nozama
{
    /// <summary>
    /// Logika interakcji dla klasy PracownkZmianaStatusuOkno.xaml
    /// </summary>
    public partial class PracownkZmianaStatusuOkno : Window
    {
        public PracownkZmianaStatusuOkno()
        {
            InitializeComponent();
        }
        public PracownkZmianaStatusuOkno(int status) : this()
        {
            if (status == 3)//jezeli stary status to WDrodze
            {
                radObebrane.IsEnabled = false;
                radWDrodze.IsEnabled = false;
            }
            else if (status == 2)//jezeli stary status to Odebrane
            {
                radObebrane.IsEnabled = false;
                radDostarczone.IsEnabled = false;
            }
            else if (status == 4)//jezeli stary status to Zaakceptowane
            {
                radDostarczone.IsEnabled = false;
                radWDrodze.IsEnabled = false;
            }
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (radDostarczone.IsChecked == false && radObebrane.IsChecked == false && radWDrodze.IsChecked == false)
            {
                MessageBox.Show("Uwaga!!! Nie zmieniono statusu.");
                PracownikOkno.nowyStatus = -1;
            }
            else
            {
                if (radWDrodze.IsChecked == true) { PracownikOkno.nowyStatus = 3; }
                if (radObebrane.IsChecked == true) { PracownikOkno.nowyStatus = 2; }
                if (radDostarczone.IsChecked == true) { PracownikOkno.nowyStatus = 5; }
            }
            this.Close();
        }
    }
}
