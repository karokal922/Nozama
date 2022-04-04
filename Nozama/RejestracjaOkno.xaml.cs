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
using System.Data;
using MySql.Data.MySqlClient;

namespace Nozama
{
    public partial class RejestracjaOkno : Window
    {
        MySqlCommand command;
        MySqlCommand select;
        public RejestracjaOkno()
        {
            InitializeComponent();
        }
        private void btnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtImie.Text != "" && txtKodPocztowy.Text != "" && txtMiejscowosc.Text != "" && txtNazwisko.Text != "" &&
                    txtNrBudynku.Text != "" && txtNrKontaktowy.Text != ""  && txtLogin.Text != "" && txtHaslo.Password != "" && txtHasloPowtorka.Password != "")
                {
                    string ulica=""; 
                    int nrMieszkania=0;
                    string imie = txtImie.Text;
                    string nazwisko = txtNazwisko.Text;
                    string miejscowosc = txtMiejscowosc.Text;
                    if (txtUlica.Text != "")
                    {
                        ulica = txtUlica.Text;
                    }
                    string kodPocztowy = txtKodPocztowy.Text;
                    string login = txtLogin.Text;
                    string haslo = txtHaslo.Password;
                    string hasloPowtorka = txtHasloPowtorka.Password;
                    int nrKontaktowy = Convert.ToInt32(txtNrKontaktowy.Text);
                    int nrBudynku = Convert.ToInt32(txtNrBudynku.Text);
                    if (txtNrMieszkania.Text != "")
                    {
                        nrMieszkania = Convert.ToInt32(txtNrMieszkania.Text);
                    }

                    if (txtNrKontaktowy.Text.Length != 9)
                    {
                        throw new Exception("Zły numer kontaktowy (9 cyfr).");
                    }
                    if (kodPocztowy.Length>6 || !kodPocztowy.Contains("-") || kodPocztowy.IndexOf("-") != 2)
                    {
                        throw new Exception("Zły kod pocztowy (np: 12-345).");
                    }
                    if (haslo.Length < 7)
                    {
                        throw new Exception("Hasło minimum 8 znaków.");
                    }
                    if (haslo != hasloPowtorka)
                    {
                        throw new Exception("Hasła się nie zgadzają.");
                    }

                    MainWindow.contact.connection.Open();
                    select = new MySqlCommand($"SELECT `ID_Konta` FROM `konta` WHERE Login='{login}'", MainWindow.contact.connection);
                    select.ExecuteNonQuery();
                    MySqlDataReader dataReader = select.ExecuteReader();
                    dataReader.Read();
                    if (dataReader.HasRows) { throw new Exception("Podany login juz istnieje."); }
                    dataReader.Close();

                    //INSERT do `konta`
                    command = new MySqlCommand($"INSERT INTO `konta` (`ID_Konta`, `Czy_Pracownik`, `Login`, `Haslo`) VALUES (NULL, '', '{login}', '{haslo}');", MainWindow.contact.connection);
                    command.ExecuteNonQuery();

                    //Znalezienie ID_Konta nowego uzytkownika
                    select.ExecuteNonQuery();
                    MySqlDataReader dataReader1 = select.ExecuteReader();
                    dataReader1.Read();
                    int idNowegoKonta = Convert.ToInt32(dataReader1.GetString(0));
                    dataReader1.Close();

                    //INSERT do `adres` 
                    command = new MySqlCommand($"INSERT INTO `adres` (`ID_Adresu`, `Miejscowosc`, `Kod_pocztowy`, `Ulica`, `Nr_budynku`, `Nr_mieszkania`) VALUES ('', '{miejscowosc}', '{kodPocztowy}', '{ulica}', '{nrBudynku}', '{nrMieszkania}');", MainWindow.contact.connection);
                    command.ExecuteNonQuery();

                    //Znalezienie ID_Adresu nowego adresu
                    select = new MySqlCommand($"SELECT `ID_Adresu` FROM `adres` WHERE Miejscowosc='{miejscowosc}' AND Kod_pocztowy='{kodPocztowy}' AND Ulica='{ulica}' AND Nr_budynku='{nrBudynku}' AND Nr_mieszkania='{nrMieszkania}'", MainWindow.contact.connection);
                    select.ExecuteNonQuery();
                    MySqlDataReader dataReader2 = select.ExecuteReader();
                    dataReader2.Read();
                    int idNowegoAdresu = Convert.ToInt32(dataReader2.GetString(0));
                    dataReader2.Close();

                    //INSERT do `klienci`
                    command = new MySqlCommand($"INSERT INTO `klienci` (`ID_Klienta`, `Konto_ID`, `Imie`, `Nazwisko`, `Adres_ID`, `Nr_kontaktowy`) VALUES (NULL, '{idNowegoKonta}', '{imie}', '{nazwisko}', '{idNowegoAdresu}', '{nrKontaktowy}');", MainWindow.contact.connection);
                    command.ExecuteNonQuery();
                    MainWindow.contact.connection.Close();
                    this.Close();
                }
                else
                {
                    throw new Exception("Uzupełnij dane");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnWstecz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
