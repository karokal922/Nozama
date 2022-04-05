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
                MainWindow.contact.connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT zamowienie.ID_Zamowienia,ad1.Miejscowosc,ad2.Miejscowosc,paczka.Wysokosc,paczka.Szerokosc,paczka.Glebokosc,paczka.Waga FROM zamowienie,klienci as kl1,klienci as kl2,adres as ad1,adres as ad2,paczka,aktualny_status WHERE zamowienie.Paczka_ID=paczka.ID_Paczki AND zamowienie.Nadawca_ID=kl1.ID_Klienta AND kl1.Adres_ID=ad1.ID_Adresu AND zamowienie.Odbiorca_ID=kl2.ID_Klienta AND kl2.Adres_ID=ad2.ID_Adresu AND zamowienie.ID_Zamowienia=aktualny_status.Zamowienia_ID AND aktualny_status.Status_ID=1;", MainWindow.contact.connection);
                command.ExecuteNonQuery();
                DataTable gr = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(gr);
                dtaDostepneZlecenia.ItemsSource = gr.DefaultView;
                MainWindow.contact.connection.Close();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtaDostepneZlecenia.Columns[0].Header = " ID ";
            dtaDostepneZlecenia.Columns[1].Header = " Z kąd ";
            dtaDostepneZlecenia.Columns[2].Header = " Dokąd ";
            dtaDostepneZlecenia.Columns[3].Header = " Wysokość ";
            dtaDostepneZlecenia.Columns[4].Header = " Szerokość ";
            dtaDostepneZlecenia.Columns[5].Header = " Głębokość ";
            dtaDostepneZlecenia.Columns[6].Header = " Waga ";
        }

        private void btnZmienStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtaPrzyjeteZlecenia.SelectedItem == null) //Jeżeli nie jest wybrany wiersz wyrzuć wyjątek
                {
                    throw new InvalidOperationException("Brak wybranego wiersza");
                }

                string _nowyStatus = Microsoft.VisualBasic.Interaction.InputBox("Podaj nowy status zamówienia\n1: Nadane\n2: W trakcie\n3: Odebrane", "Zmień Status", "1");
                int _nowyStatusNumer = Convert.ToInt32(_nowyStatus);

                if(Enum.IsDefined(typeof(Typy.EnumStatus), _nowyStatusNumer))
                {
                    Typy.EnumStatus nowyStatusNumer = (Typy.EnumStatus)_nowyStatusNumer;
                }
                else
                {
                    throw new Exception("DUPA");
                }

                //MySqlCommand command = new MySqlCommand("", MainWindow.contact.connection); //Zmień status

            }
            catch(InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch(FormatException)
            {
                MessageBox.Show("Wartość statusu jest nieprawidłowa, musi być liczbą całkowitą");
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
