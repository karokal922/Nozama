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
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Nozama
{
    /// <summary>
    /// Logika interakcji dla klasy KlientOkno.xaml
    /// </summary>
    public partial class KlientOkno : Window
    {
        private string nazwaKonta;
        private int idKonta;

        private string imieKlienta;
        private string nazwiskoKlienta;
        private int idKlienta;

        private KlientOkno()
        {
            InitializeComponent();
            MainWindow.contact.connection.Open();
        }

        public KlientOkno(string NazwaKonta, int id) : this()
        {
            this.nazwaKonta = NazwaKonta;
            this.idKonta = id;

            string queryKlientInfo = $"SELECT k.ID_Klienta, k.Imie, k.Nazwisko FROM klienci k WHERE k.Konto_ID={this.idKonta}";
            var command = new MySqlCommand(queryKlientInfo, MainWindow.contact.connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();

            this.idKlienta = reader.GetInt32(0);
            this.imieKlienta = reader.GetString(1);
            this.nazwiskoKlienta = reader.GetString(2);

            lblNazwaUzytkownika.Content = $"{imieKlienta} {nazwiskoKlienta} [{nazwaKonta}]";

            reader.Close();
        }

        /* IsFormEmpty() iteruje pomiędzy elementami gridów z danymi i sprawdza czy któryś z TextBoxów nie jest pusty */
        private bool IsFormEmpty()
        {
            /* Na razie jest tak brzydko bo nie wiem jak połączyć UiElementCollection */
            /* UPDATE 26.04.2022 Nadal nie wiem jak to połączyć */
            foreach (UIElement elem in grdPaczka.Children)
            {
                if(elem is TextBox)
                {
                    TextBox _temp = (TextBox)elem;
                    if(_temp.Text.Length == 0)
                    {
                        return true;
                    }
                }
            }
            
            foreach (UIElement elem in grdAdres.Children)
            {
                if (elem is TextBox)
                {
                    TextBox _temp = (TextBox)elem;
                    if (_temp.Text.Length == 0)
                    {
                        return true;
                    }
                }
            }
            foreach (UIElement elem in grdKlient.Children)
            {
                if (elem is TextBox)
                {
                    TextBox _temp = (TextBox)elem;
                    if (_temp.Text.Length == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /* Validate() funkcja zbiorcza do sprawdzania poprawności wprowadzonych danych */
        private void Validate()
        {
            if (IsFormEmpty())
            {
                throw new ArgumentNullException("Proszę uzupełnić wszystkie dane żeby nadać paczkę");
            }
            string message_pack;
            if (!IsPackageValid(out message_pack))
            {
                throw new ArgumentOutOfRangeException(message_pack);
            }
            string message_address;
            if (!IsAddressValid(out message_address))
            {
                throw new FormatException(message_address);
            }
            /* Sprawdź wszystkie pola i warunki i wyrzuć wyjątek jesli coś jest źle */
        }

        /* IsPackageValid() sprawdza czy dane z textBoxow z grida z danymi paczki są poprawne */
        private bool IsPackageValid(out string mes)
        {
            double wysokosc = Convert.ToDouble(txbWysokosc.Text);
            double szerokosc = Convert.ToDouble(txbSzerokosc.Text);
            double glebokosc = Convert.ToDouble(txbGlebokosc.Text);
            double waga = Convert.ToDouble(txbWaga.Text);

            if(wysokosc > 10000.0 || szerokosc > 10000.0 || glebokosc > 10000.0)
            {
                mes = "Paczka jest zbyt duża";
                return false;
            }
            if(waga > 10000)
            {
                mes = "Paczka jest zbyt ciężka";
                return false;
            }
            mes = "";
            return true;
        }

        /* IsAddressValid() sprawdza czy dane z textBoxow z grida z danymi adresu są poprawne */
        private bool IsAddressValid(out string mes)
        {
            string patternKodPocztowy = @"[0-9]{2}-[0-9]{3}";
            Regex reg = new Regex(patternKodPocztowy);

            MatchCollection matched = reg.Matches(txbKodP.Text);

            if(matched.Count == 0)
            {
                mes = "Niepoprawny format kodu pocztowego, wpisz xx-xxx gdzie x to cyfra od 0 do 9";
                return false;
            }
            mes = "";
            return true;
        }

        private long StworzPaczke(string wysokosc, string szerokosc, string glebokosc, string waga)
        {
            /* Tworzy rekord paczki w bazie i zwraca Id paczki */

            MySqlCommand command = MainWindow.contact.connection.CreateCommand();
            command.CommandText = $"INSERT INTO paczka (ID_Paczki, Wysokosc, Szerokosc, Glebokosc, Waga) VALUES (NULL, {wysokosc}, {szerokosc}, {glebokosc}, {waga})";
            command.ExecuteNonQuery();

            return command.LastInsertedId;
        }

        private long StworzAdres(string miejscowosc, string kodPocztowy, string ulica, string nrBudynku, string nrMieszkania)
        {
            /* Tworzy rekord adresu w bazie i zwraca Id adresu */

            MySqlCommand command = MainWindow.contact.connection.CreateCommand();
            command.CommandText = $"INSERT INTO adres (ID_Adresu, Miejscowosc, Kod_pocztowy, Ulica, Nr_budynku, Nr_mieszkania) VALUES (NULL, '{miejscowosc}', '{kodPocztowy}', '{ulica}', {nrBudynku}, {nrMieszkania})";
            command.ExecuteNonQuery();

            return command.LastInsertedId;
        }

        private long StworzKlienta(string imie, string nazwisko, string numerTelefonu, long idAdres)
        {
            /* Tworzy rekord klienta w bazie i zwraca Id klienta */

            MySqlCommand command = MainWindow.contact.connection.CreateCommand();
            command.CommandText = $"INSERT INTO klienci (ID_Klienta, Konto_ID, Imie, Nazwisko, Adres_ID, Nr_kontaktowy) VALUES (NULL, NULL, '{imie}', '{nazwisko}', {idAdres}, '{numerTelefonu}')";
            command.ExecuteNonQuery();

            return command.LastInsertedId;
        }

        private void StworzZamowienie(string idOdbiorca, string idPaczka)
        {
            /* Tworzy rekord zamówienia*/

            MySqlCommand command = MainWindow.contact.connection.CreateCommand();
            command.CommandText = $"INSERT INTO zamowienie (ID_Zamowienia, Nadawca_ID, Odbiorca_ID, Paczka_ID, Kurier_ID, Dystans, Data_odbioru, Data_dostawy, Cena) VALUES (NULL, {this.idKlienta}, {idOdbiorca}, {idPaczka}, NULL, NULL, NULL, NULL, NULL)";

            command.ExecuteNonQuery();
        }

        private void btnWyslij_Click(object sender, RoutedEventArgs e)
        {
            bool Validated = true;
            try
            {
                Validate();
            }
            catch (Exception except)
            {
                Validated = false;
                MessageBox.Show(except.Message);
            }

            if (Validated)
            {

                string wysokosc = txbWysokosc.Text;
                string szerokosc = txbSzerokosc.Text;
                string glebokosc = txbGlebokosc.Text;
                string waga = txbWaga.Text;

                long idPaczki = StworzPaczke(wysokosc, szerokosc, glebokosc, waga);

                string miejscowosc = txbMiejscowosc.Text;
                string kod_pocztowy = txbKodP.Text;
                string ulica = txbUlica.Text;
                string nr_budynku = txbNrB.Text;
                string nr_mieszkania = txbNrM.Text;

                long idAdresu = StworzAdres(miejscowosc, kod_pocztowy, ulica, nr_budynku, nr_mieszkania);

                string odbiorcaImie = txbImie.Text;
                string odbiorcaNazwisko = txbNazwisko.Text;
                string odbiorcaNumer = txbNumerKontaktowy.Text;

                long idOdbiorcy = StworzKlienta(odbiorcaImie, odbiorcaNazwisko, odbiorcaNumer, idAdresu);

                StworzZamowienie(idOdbiorcy.ToString(), idPaczki.ToString());

                MessageBox.Show("Zamówienie zostało pomyślnie utworzone");
            }

        }

        private void btnOdswiezPaczki_Click(object sender, RoutedEventArgs e)
        {
            /* Pobierz paczki które użytkownik wysłał */
            var command = new MySqlCommand($"SELECT z.Nadawca_ID, z.Odbiorca_ID FROM zamowienie z WHERE z.Nadawca_ID = {idKonta}", MainWindow.contact.connection);
            MySqlDataReader reader =  command.ExecuteReader();
            while (reader.Read())
            {
                
            }
            reader.Close();
        }
    }
}
