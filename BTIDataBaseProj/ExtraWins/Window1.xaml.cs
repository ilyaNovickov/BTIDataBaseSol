using BTIDataBaseProj.Helpers;
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

namespace BTIDataBaseProj.ExtraWins
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class StartWin : Window
    {
        private List<string> startForms = new List<string>()
        {
            "Стандартное подключение", 
            "Подключение по паролю", 
            "Подключение по аунтификации"
        };

        public StartWin()
        {
            InitializeComponent();

            formatsComboBox.ItemsSource = startForms;
            formatsComboBox.SelectedIndex = 0;
        }

        public string ConnectionString
        {
            get; private set;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (formatsComboBox.SelectedIndex)
            {
                case 0:
                case 2:
                    passwordTextBox.IsEnabled = false;
                    userTextBox.IsEnabled = false;
                    break;
                case 1:
                    passwordTextBox.IsEnabled = true;
                    userTextBox.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            switch (formatsComboBox.SelectedIndex)
            {
                case 0:
                    ConnectionString = null;
                    break;
                case 1:
                    ConnectionString = ConnectionCreator.GetConnection(userTextBox.Text, passwordTextBox.Password);
                    break;
                case 2:
                    ConnectionString = ConnectionCreator.GetConnection();
                    break;
                default:
                    break;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
