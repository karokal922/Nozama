using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy PracownikOkno.xaml
    /// </summary>
    public partial class PracownikOkno : Window
    {
        public PracownikOkno()
        {
            InitializeComponent();
            try
            {
                MySqlCommand command = new MySqlCommand("Select * from konta", MainWindow.contact.connection);
                command.ExecuteNonQuery();
                DataTable gr = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(gr);
                dtaDostepneZlecenia.ItemsSource = gr.DefaultView;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
