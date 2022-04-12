﻿using MySql.Data.MySqlClient;
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
        private DataTable dostepneZLEC = new DataTable();
        private DataTable zaakceptowaneZLEC = new DataTable();
        private int IDKuriera;
        public PracownikOkno()
        {
            InitializeComponent();
            try
            {
                MainWindow.contact.connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT zamowienie.ID_Zamowienia,ad1.Miejscowosc,ad2.Miejscowosc,paczka.Wysokosc,paczka.Szerokosc,paczka.Glebokosc,paczka.Waga FROM zamowienie,klienci as kl1,klienci as kl2,adres as ad1,adres as ad2,paczka,aktualny_status WHERE zamowienie.Paczka_ID=paczka.ID_Paczki AND zamowienie.Nadawca_ID=kl1.ID_Klienta AND kl1.Adres_ID=ad1.ID_Adresu AND zamowienie.Odbiorca_ID=kl2.ID_Klienta AND kl2.Adres_ID=ad2.ID_Adresu AND zamowienie.ID_Zamowienia=aktualny_status.Zamowienia_ID AND aktualny_status.Status_ID=1;", MainWindow.contact.connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dostepneZLEC);
                dtaDostepneZlecenia.ItemsSource = dostepneZLEC.DefaultView;
                MainWindow.contact.connection.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        public PracownikOkno(string nazwaKonta, int id) : this()
        {
            lblNazwaUzytkownika.Content = nazwaKonta;
            MainWindow.contact.connection.Open();
            var command = new MySqlCommand($"SELECT ID_Kuriera FROM kurier WHERE kurier.Konto_ID={id}; ", MainWindow.contact.connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            IDKuriera = (int)dataReader.GetValue(0);
            dataReader.Close();
            MySqlCommand command1 = new MySqlCommand($"SELECT zamowienie.ID_Zamowienia,ad1.Miejscowosc,ad2.Miejscowosc,paczka.Wysokosc,paczka.Szerokosc,paczka.Glebokosc,paczka.Waga,status.Status,zamowienie.Cena FROM zamowienie,klienci as kl1,klienci as kl2,adres as ad1,adres as ad2,paczka,aktualny_status,status WHERE zamowienie.Paczka_ID=paczka.ID_Paczki AND zamowienie.Nadawca_ID=kl1.ID_Klienta AND kl1.Adres_ID=ad1.ID_Adresu AND zamowienie.Odbiorca_ID=kl2.ID_Klienta AND kl2.Adres_ID=ad2.ID_Adresu AND zamowienie.ID_Zamowienia=aktualny_status.Zamowienia_ID AND aktualny_status.Status_ID!=1 AND aktualny_status.Status_ID!=5 and zamowienie.Kurier_ID={IDKuriera} AND aktualny_status.Status_ID=status.ID_Statusu; ", MainWindow.contact.connection);
            command1.ExecuteNonQuery();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter(command1);
            adapter1.Fill(zaakceptowaneZLEC);
            dtaPrzyjeteZlecenia.ItemsSource = zaakceptowaneZLEC.DefaultView;
            MainWindow.contact.connection.Close();
        }
        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void UstawTytułyKolumnDostepnymZleceniom()
        {
            dtaDostepneZlecenia.Columns[0].Header = " ID ";
            dtaDostepneZlecenia.Columns[1].Header = " Z kąd ";
            dtaDostepneZlecenia.Columns[2].Header = " Dokąd ";
            dtaDostepneZlecenia.Columns[3].Header = " Długość ";
            dtaDostepneZlecenia.Columns[4].Header = " Szerokość ";
            dtaDostepneZlecenia.Columns[5].Header = " Głębokość ";
            dtaDostepneZlecenia.Columns[6].Header = " Waga ";
        }
        public void UstawTytułyKolumnPrzyjetymZleceniom()
        {
            dtaPrzyjeteZlecenia.Columns[0].Header = " ID ";
            dtaPrzyjeteZlecenia.Columns[1].Header = " Z kąd ";
            dtaPrzyjeteZlecenia.Columns[2].Header = " Dokąd ";
            dtaPrzyjeteZlecenia.Columns[3].Header = " Długość ";
            dtaPrzyjeteZlecenia.Columns[4].Header = " Szerokość ";
            dtaPrzyjeteZlecenia.Columns[5].Header = " Głębokość ";
            dtaPrzyjeteZlecenia.Columns[6].Header = " Waga ";
            dtaPrzyjeteZlecenia.Columns[7].Header = " Status ";
            dtaPrzyjeteZlecenia.Columns[8].Header = " Cena ";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UstawTytułyKolumnDostepnymZleceniom();
            UstawTytułyKolumnPrzyjetymZleceniom();
        }
        public static int nowyStatus;
        private void btnZmienStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtaPrzyjeteZlecenia.SelectedItem == null) //Jeżeli nie jest wybrany wiersz wyrzuć wyjątek
                {
                    throw new InvalidOperationException("Brak wybranego wiersza");
                }
                int zaznaczonyIndexZaakceptowanego = dtaPrzyjeteZlecenia.SelectedIndex;
                int idZaznaczonegoZamowieniaZaakceptowanego = (int)dostepneZLEC.Rows[zaznaczonyIndexZaakceptowanego][0];
                
                //MySqlCommand command = new MySqlCommand("", MainWindow.contact.connection); //Zmień status
            }
            catch(InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private double cenaZa1km = 0.05;
        private double cenaZa1kg = 0.15;
        private double cenaZaMetrSzescienny = 100;
        public decimal ObliczCeneDostawy(double x,double y,double z, double masa, double dystans)
        {
            decimal cenaKoncowa = 0;
            cenaKoncowa += (decimal)(dystans * cenaZa1km);
            cenaKoncowa += (decimal)(masa * cenaZa1kg);
            cenaKoncowa += (decimal)((x * y * z / 1000000) * cenaZaMetrSzescienny);
            return cenaKoncowa;
        }
        public void CzyscDostepneZleceniaGrid()
        {
            DataTable empty = new DataTable();
            dtaDostepneZlecenia.ItemsSource = empty.DefaultView;
        }
        public void CzyscPrzyjeteZleceniaGrid()
        {
            DataTable empty = new DataTable();
            dtaPrzyjeteZlecenia.ItemsSource = empty.DefaultView;
        }
        private void btnZaakceptuj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtaDostepneZlecenia.SelectedItem == null) //Jeżeli nie jest wybrany wiersz wyrzuć wyjątek
                {
                    throw new InvalidOperationException("Nie zaznaczono wiersza");
                }
                else
                {
                    int zaznaczonyIndex = dtaDostepneZlecenia.SelectedIndex;
                    int idZaznaczonegoZamowienia = (int)dostepneZLEC.Rows[zaznaczonyIndex][0];
                    var rand = new Random();
                    var losowyDystans = rand.NextDouble() * 1000;//Na potrzeby projektu jest tu losowana odległość w km od miejsca odbioru paczki do miejsca dostawy
                    double wymiarX = (double)dostepneZLEC.Rows[zaznaczonyIndex][3];
                    double wymiarY = (double)dostepneZLEC.Rows[zaznaczonyIndex][4];
                    double wymiarZ = (double)dostepneZLEC.Rows[zaznaczonyIndex][5];
                    double waga = (double)dostepneZLEC.Rows[zaznaczonyIndex][6];
                    decimal cenaZamowienia = Math.Floor(ObliczCeneDostawy(wymiarX, wymiarY, wymiarZ, waga, losowyDystans));
                    MainWindow.contact.connection.Open();

                    MySqlCommand command = new MySqlCommand($"UPDATE `zamowienie` SET `Kurier_ID` = '{IDKuriera}', `Dystans` = '{losowyDystans}', `Cena` = '{cenaZamowienia}' WHERE `zamowienie`.`ID_Zamowienia` = {idZaznaczonegoZamowienia}; ", MainWindow.contact.connection);
                    command.ExecuteNonQuery();
                    command = new MySqlCommand($"UPDATE aktualny_status SET aktualny_status.Status_ID=4 WHERE aktualny_status.Zamowienia_ID={idZaznaczonegoZamowienia}", MainWindow.contact.connection);
                    command.ExecuteNonQuery();

                    CzyscPrzyjeteZleceniaGrid();
                    CzyscDostepneZleceniaGrid();

                    command = new MySqlCommand($"SELECT zamowienie.ID_Zamowienia,ad1.Miejscowosc,ad2.Miejscowosc,paczka.Wysokosc,paczka.Szerokosc,paczka.Glebokosc,paczka.Waga,status.Status,zamowienie.Cena FROM zamowienie,klienci as kl1,klienci as kl2,adres as ad1,adres as ad2,paczka,aktualny_status,status WHERE zamowienie.Paczka_ID=paczka.ID_Paczki AND zamowienie.Nadawca_ID=kl1.ID_Klienta AND kl1.Adres_ID=ad1.ID_Adresu AND zamowienie.Odbiorca_ID=kl2.ID_Klienta AND kl2.Adres_ID=ad2.ID_Adresu AND zamowienie.ID_Zamowienia=aktualny_status.Zamowienia_ID AND aktualny_status.Status_ID!=1 AND aktualny_status.Status_ID!=5 and zamowienie.Kurier_ID={IDKuriera} AND aktualny_status.Status_ID=status.ID_Statusu; ", MainWindow.contact.connection);
                    command.ExecuteNonQuery();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    zaakceptowaneZLEC.Clear();
                    adapter.Fill(zaakceptowaneZLEC);
                    dtaPrzyjeteZlecenia.ItemsSource = zaakceptowaneZLEC.DefaultView;

                    command = new MySqlCommand("SELECT zamowienie.ID_Zamowienia,ad1.Miejscowosc,ad2.Miejscowosc,paczka.Wysokosc,paczka.Szerokosc,paczka.Glebokosc,paczka.Waga FROM zamowienie,klienci as kl1,klienci as kl2,adres as ad1,adres as ad2,paczka,aktualny_status WHERE zamowienie.Paczka_ID=paczka.ID_Paczki AND zamowienie.Nadawca_ID=kl1.ID_Klienta AND kl1.Adres_ID=ad1.ID_Adresu AND zamowienie.Odbiorca_ID=kl2.ID_Klienta AND kl2.Adres_ID=ad2.ID_Adresu AND zamowienie.ID_Zamowienia=aktualny_status.Zamowienia_ID AND aktualny_status.Status_ID=1;", MainWindow.contact.connection);
                    command.ExecuteNonQuery();
                    MySqlDataAdapter adapter1 = new MySqlDataAdapter(command);
                    dostepneZLEC.Clear();
                    adapter1.Fill(dostepneZLEC);
                    dtaDostepneZlecenia.ItemsSource = dostepneZLEC.DefaultView;

                    UstawTytułyKolumnDostepnymZleceniom();
                    UstawTytułyKolumnPrzyjetymZleceniom();

                    MainWindow.contact.connection.Close();
                }
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
