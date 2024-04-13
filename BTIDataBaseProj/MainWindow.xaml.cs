using System;
using System.CodeDom.Compiler;
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
using Xceed.Wpf.AvalonDock;
using BTIDataBaseProj.Helpers;

namespace BTIDataBaseProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BTIDataBaseEntities contex = new BTIDataBaseEntities();
        //CollectionViewSource buildingsViewSourse;
        //CollectionViewSource flatsViewSourse;
        BuildingInfo buildingInfo = new BuildingInfo();

        public MainWindow()
        {
            InitializeComponent();

            //DataContext = this;
        }

        ~MainWindow()
        {
            contex?.Dispose();
        }

        public BuildingInfo BuildingInfo
        {
            get => buildingInfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aboutBuildingGrid.DataContext = buildingInfo;
            //System.Windows.Data.CollectionViewSource buildingsTableViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource")));
            //// Загрузите данные, установив свойство CollectionViewSource.Source:
            //// buildingsTableViewSource.Source = [универсальный источник данных]

            //contex.BuildingsTable.Load();

            //buildingsViewSourse.Source = null;//contex.BuildingsTable.Local;

            //contex.FlatsTable.Load();

            //flatsViewSourse.Source = contex.FlatsTable.Local;

            ////buildingsTableFlatsTableViewSource
            ///

            contex.BuildingsTable.Load();

            buildingsDataGrid.ItemsSource = contex.BuildingsTable.Local.ToBindingList<BuildingsTable>();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //bool v = zxc.IsVisible;
            //zxc.IsVisible = !zxc.IsVisible;
        }

        private void buildingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(buildingsDataGrid.SelectedItem is BuildingsTable))
                return;

            buildingInfo.BuildingsTable = (BuildingsTable)buildingsDataGrid.SelectedItem;
        }

        private void addBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.Kadastr == "" || buildingInfo.Kadastr == null)
            {
                MessageBox.Show("Не указан кадастр здания");
                return;
            }
            else if ((from b in contex.BuildingsTable.Local where b.Kadastr == buildingInfo.Kadastr select b).Count() != 0)
            {
                MessageBox.Show("Здание с таким кадастром уже указано");
                return;
            }

            contex.AddBuilding(buildingInfo.Kadastr, buildingInfo.Address, buildingInfo.District, buildingInfo.Land, buildingInfo.Year,
                buildingInfo.Material, buildingInfo.Base, buildingInfo.Flow, buildingInfo.Line, buildingInfo.Square, buildingInfo.Flats, buildingInfo.Comments,
                buildingInfo.Wear, buildingInfo.Picture, buildingInfo.Elevator);
            //update ....
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
