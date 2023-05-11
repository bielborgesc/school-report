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

namespace SchoolReport
{
    /// <summary>
    /// Lógica interna para StudentScreen.xaml
    /// </summary>
    public partial class StudentScreen : Window
    {
        public StudentScreen()
        {
            InitializeComponent();
        }

        public void SaveBtn(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
