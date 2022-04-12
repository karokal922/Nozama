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
    /// Logika interakcji dla klasy KlientOkno.xaml
    /// </summary>
    public partial class KlientOkno : Window
    {
        public KlientOkno()
        {
            InitializeComponent();
        }

        /* IsFormEmpty() iteruje pomiędzy elementami gridów z danymi i sprawdza czy któryś z TextBoxów nie jest pusty */
        private bool IsFormEmpty()
        {
            foreach(UIElement elem in grdPaczka.Children)
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
            /* Na razie jest tak brzydko w dwóch pętlach bo nie wiem jak połączyć UiElementCollection */
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

            return false;
        }

        /* Validate() funkcja zbiorcza do sprawdzania poprawności wprowadzonych danych */
        private void Validate()
        {
            if (IsFormEmpty())
            {
                throw new ArgumentNullException("Proszę uzupełnić wszystkie dane żeby nadać paczkę");
            }
            string message;
            if (!IsPackageValid(out message))
            {
                throw new ArgumentOutOfRangeException(message);
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
            mes = "";
            return true;
        }

        private void btnWyslij_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }

            string wysokosc = txbWysokosc.Text;
            string szerokosc = txbSzerokosc.Text;
            string glebokosc = txbGlebokosc.Text;
            string waga = txbWaga.Text;

            string miejscowosc = txbMiejscowosc.Text;
            string kod_pocztowy = txbKodP.Text;
            string ulica = txbUlica.Text;
            string nr_budynku = txbNrB.Text;
            string nr_mieszkania = txbNrM.Text;

        }
    }
}
