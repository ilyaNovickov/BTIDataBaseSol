using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace BTIDataBaseProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //BTIDataBaseEntities contex = new BTIDataBaseEntities();
        //CollectionViewSource buildingsViewSourse;
        //CollectionViewSource flatsViewSourse;

        public MainWindow()
        {
            InitializeComponent();

            //buildingsViewSourse = ((CollectionViewSource)(FindResource("buildingsTableViewSource")));
            //flatsViewSourse = ((CollectionViewSource)(FindResource("buildingsTableFlatsTableViewSource")));

            //DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Data.CollectionViewSource buildingsTableViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource")));
            //// Загрузите данные, установив свойство CollectionViewSource.Source:
            //// buildingsTableViewSource.Source = [универсальный источник данных]

            //contex.BuildingsTable.Load();

            //buildingsViewSourse.Source = null;//contex.BuildingsTable.Local;

            //contex.FlatsTable.Load();

            //flatsViewSourse.Source = contex.FlatsTable.Local;

            ////buildingsTableFlatsTableViewSource
        }

        private void buildingsTableDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //buildingsViewSourse.View?.Refresh();



            //if (flatsTableDataGrid.SelectedItem is FlatsTable flat)
            //{
            //    buildingsViewSourse.Source = from b in contex.BuildingsTable.ToList() where b.Kadastr == flat.BuildingKadastr select b;
            //        }
        }
    }
}
