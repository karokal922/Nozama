﻿using System;
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
using System.Data;
using MySql.Data.MySqlClient;

namespace Nozama
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Connection contact = new Connection();
        MySqlCommand command;

        public MainWindow()
        {
            InitializeComponent();
            contact.Connect();
        }

        private void btnLoguj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = txtLogin.Text;
                string hasło = pasHaslo.Password;
                contact.connection.Open();
                command = new MySqlCommand($"SELECT Haslo FROM konta WHERE Login='{login}'", contact.connection);

                MySqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                if (!dataReader.HasRows)
                {
                    MessageBox.Show("Zły login.");
                    dataReader.Close();
                    contact.connection.Close();
                }
                else if (hasło == dataReader.GetString(0))
                {
                    dataReader.Close();
                    command = new MySqlCommand($"SELECT Czy_Pracownik FROM konta WHERE Login='{login}' AND Haslo='{hasło}'", contact.connection);
                    dataReader = command.ExecuteReader();
                    dataReader.Read();
                    if (dataReader.GetBoolean(0) == false)
                    {
                        KlientOkno klientOkno = new KlientOkno();
                        this.Visibility = Visibility.Hidden;
                        klientOkno.ShowDialog();
                        txtLogin.Text = "";
                        pasHaslo.Password = "";
                        this.Visibility = Visibility.Visible;
                    }
                    else if (dataReader.GetBoolean(0) == true)
                    {
                        contact.connection.Close();
                        PracownikOkno pracownikOkno = new PracownikOkno();
                        this.Visibility = Visibility.Hidden;
                        pracownikOkno.lblNazwaUzytkownika.Content = login;
                        pracownikOkno.ShowDialog();
                        txtLogin.Text = "";
                        pasHaslo.Password = "";
                        this.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        throw new Exception("Błąd sprawdzania czy to klient czy pracownik");
                    }
                }
                else
                {
                    MessageBox.Show("Złe hasło");
                }
                dataReader.Close();
                contact.connection.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnSprawdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_paczki = Convert.ToInt32(txtIdPaczki.Text);

                contact.connection.Open();
                command = new MySqlCommand($"SELECT Status FROM status s,aktualny_status a,zamowienie z WHERE s.ID_Statusu=a.Status_ID AND a.Zamowienia_ID=z.ID_Zamowienia AND ID_Zamowienia='{id_paczki}'", contact.connection);
                command.ExecuteNonQuery();

                MySqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                if (!dataReader.HasRows) { MessageBox.Show("Brak paczki o podanym ID"); }
                else
                {
                    MessageBox.Show("Status twojego zamówienia: " + dataReader.GetString(0));
                }
                contact.connection.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnRejestruj_Click(object sender, RoutedEventArgs e)
        {
            RejestracjaOkno rejestracja = new RejestracjaOkno();
            this.Visibility = Visibility.Hidden;
            rejestracja.ShowDialog();
            this.Visibility = Visibility.Visible;
        }



        /*private void btnPoka_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contact.connection.Open();
                //command = new MySqlCommand("INSERT INTO `konta` (`ID_Konta`, `Login`, `Haslo`) VALUES (NULL, 'aaaaa', '$2y$10$db.aw2RhBdNSx4HXOX.eouCeKgV4XoGtuvFQKk8ZgshKpiingP3i.')",contact.connection);
                //command.ExecuteNonQuery();
                command = new MySqlCommand("Select * from konta",contact.connection);
                command.ExecuteNonQuery();
                gr = new DataTable();
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(gr);
                dtaPlace.ItemsSource = gr.DefaultView;

                contact.connection.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }*/
    }
}
