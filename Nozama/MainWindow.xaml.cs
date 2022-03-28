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
using System.Data;
using MySql.Data.MySqlClient;

namespace Nozama
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection contact = new Connection();
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable gr;
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
                command = new MySqlCommand($"SELECT Haslo FROM konta WHERE Login='{login}'",contact.connection);
                command.ExecuteNonQuery();
                
                MySqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                if (!dataReader.HasRows) { MessageBox.Show("Zły logon."); }
                else if(hasło == dataReader.GetString(0))
                {
                    MessageBox.Show("Udało się zalogować");
                    //NOWE OKNO
                }
                else
                {
                    MessageBox.Show("Złe hasło");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnRejestruj_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_paczki = Convert.ToInt32(txtIdPaczki.Text);

                contact.connection.Open();
                command = new MySqlCommand($"SELECT Status FROM status WHERE ID_Statusu='{id_paczki}'", contact.connection);
                command.ExecuteNonQuery();

                MySqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                if (!dataReader.HasRows) { MessageBox.Show("Brak paczki o podanym ID"); }
                else
                {
                    MessageBox.Show(dataReader.GetString(0));
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
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
