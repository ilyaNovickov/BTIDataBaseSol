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
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;

namespace BTIDataBaseProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private float minScale = 0.02f;
        private float maxScale = 10.0f;
        private float scale = 1f;

        public event PropertyChangedEventHandler PropertyChanged;

        public float Scale
        {
            get => scale;
            private set
            {
                scale = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Scale)));
            }
        }

        BTIDataBaseEntities1 contex = new BTIDataBaseEntities1();
        CollectionViewSource buildingsViewSourse;
        CollectionViewSource flatsViewSourse;
        BuildingInfo buildingInfo = new BuildingInfo();
        FlatInfo flatInfo = new FlatInfo();

        public MainWindow()
        {
            InitializeComponent();

            buildingsViewSourse = ((CollectionViewSource)(FindResource("buildingViewSourse")));
            flatsViewSourse = ((CollectionViewSource)(FindResource("flatsViewSourse")));
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
            aboutFlatGrid.DataContext = flatInfo;

            contex.BuildingsTable.Load();
            buildingsViewSourse.Source = contex.BuildingsTable.Local;
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

        private void flatsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(flatsDataGrid.SelectedItem is FlatsTable))
                return;

            flatInfo.FlatsTable = (FlatsTable)flatsDataGrid.SelectedItem;
        }

        #region forBuilding
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

            BuildingsTable building = new BuildingsTable()
            {
                Kadastr = buildingInfo.Kadastr,
                Address = buildingInfo.Address,
                District = buildingInfo.District,
                Land = buildingInfo.Land,
                Year = buildingInfo.Year,
                Material = buildingInfo.Material,
                Base = buildingInfo.Base,
                Flow = buildingInfo.Flow,
                Line = buildingInfo.Line,
                Square = buildingInfo.Square,
                Flats = buildingInfo.Flats,
                Comments = buildingInfo.Comments,
                Wear = buildingInfo.Wear,
                Picture = buildingInfo.Picture,
                Elevator = buildingInfo.Elevator
            };

            contex.BuildingsTable.Add(building);
            buildingsViewSourse.View.Refresh();
            contex.SaveChanges();
        }

        private void removeBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null && buildingInfo.Kadastr != buildingInfo.BuildingsTable.Kadastr)
            {
                MessageBox.Show("Запис здания не выбрана");
                return;
            }

            contex.BuildingsTable.Remove(buildingInfo.BuildingsTable);
            buildingInfo.Clear();
            buildingsViewSourse.View.Refresh();
            contex.SaveChanges();
        }

        private void updateBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null && buildingInfo.Kadastr != buildingInfo.BuildingsTable.Kadastr)
            {
                MessageBox.Show("Запис здания не выбрана");
                return;
            }

            if (buildingInfo.BuildingsTable.Kadastr != buildingInfo.Kadastr)
            {
                MessageBox.Show("Кадастр здания изменить нельзя");
                return;
            }

            buildingInfo.BuildingsTable.District = buildingInfo.District;
            buildingInfo.BuildingsTable.Address = buildingInfo.Address;
            buildingInfo.BuildingsTable.Wear = buildingInfo.Wear;
            buildingInfo.BuildingsTable.Picture = buildingInfo.Picture;
            buildingInfo.BuildingsTable.Year = buildingInfo.Year;
            buildingInfo.BuildingsTable.Material = buildingInfo.Material;
            buildingInfo.BuildingsTable.Comments = buildingInfo.Comments;
            buildingInfo.BuildingsTable.Base = buildingInfo.Base;
            buildingInfo.BuildingsTable.Elevator = buildingInfo.Elevator;
            buildingInfo.BuildingsTable.Flats = buildingInfo.Flats;
            buildingInfo.BuildingsTable.Flow = buildingInfo.Flow;
            buildingInfo.BuildingsTable.Square = buildingInfo.Square;
            buildingInfo.BuildingsTable.Line = buildingInfo.Line;
            buildingInfo.BuildingsTable.Land = buildingInfo.Land;
            //buildingInfo.BuildingsTable.Kadastr = buildingInfo.Kadastr;

            buildingsViewSourse.View.Refresh();
            contex.SaveChanges();
        }

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            ofd.Filter = "Изображения (*.png, *.jpeg, *.jpg, *.bmp)|*.png;*.jpeg;*.jpg;*.bmp";

            bool? res = ofd.ShowDialog();

            if (res.HasValue && res.Value)
            {
                FileStream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                MemoryStream memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                stream.Close();
                buildingInfo.Picture = memoryStream.ToArray();
                memoryStream.Close();
            }
        }
        

        #region forImage
        private double a => (minScale * maxScale - Math.Pow(1d, 2d)) / (minScale - 2d * 1d + maxScale);
        private double b => Math.Pow((1d - minScale), 2d) / (minScale - 2d * 1d + maxScale);
        private double c => 2d * Math.Log((maxScale - 1d) / (1d - minScale));

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Scale = (float)(a + b * Math.Exp(c * slider.Value));
        }
        #endregion

        #endregion
        #region aboutFlats
        private void addFlatButtin_Click(object sender, RoutedEventArgs e)
        {
            if (flatInfo.FlatId != -1 && flatInfo.FlatsTable != null)
            {
                MessageBoxResult res = MessageBox.Show("Информация о квартире ссылается на существующею квартиру\nДобавить новую квартиру?", "Внимание", MessageBoxButton.YesNo);

                if (res != MessageBoxResult.Yes)
                    return;
            }

            #region checkKadastr
            {
                IEnumerable<string> kadastrs = from building in contex.BuildingsTable
                                               select building.Kadastr;

                if (!kadastrs.Contains(flatInfo.BuildingKadastr))
                {
                    MessageBox.Show("Не указан кадастр здания или здания с таким кадастром не существует\nИзменити кадастр здания для квартиры");
                    return;
                }
            }
            #endregion
            
            FlatsTable flatsTable = new FlatsTable()
            {
                BuildingKadastr = flatInfo.BuildingKadastr,
                Flat = flatInfo.Flat,
                Storey = flatInfo.Storey,
                Level = flatInfo.Level,
                Dwell = flatInfo.Dwell,
                Balcony = flatInfo.Balcony,
                Height = flatInfo.Height,
                Branch = flatInfo.Branch,
                Rooms = flatInfo.Rooms,
                SquareFlat = flatInfo.SquareFlat,
            };

            contex.FlatsTable.Add(flatsTable);
            buildingsViewSourse.View.Refresh();
            contex.SaveChanges();
        }

        private void removeFlatButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatInfo.FlatsTable == null)// && flatInfo.FlatId != buildingInfo.BuildingsTable.Kadastr)
            {
                MessageBox.Show("Запис квартиры не выбрана");
                return;
            }

            contex.FlatsTable.Remove(flatInfo.FlatsTable);
            flatInfo.Clear();
            buildingsViewSourse.View.Refresh();
            contex.SaveChanges();
        }

        

        private void updateFlatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private int flatErrorsCount = 0;
        private void AboutFlatTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addFlatButton.IsEnabled = false;
                removeFlatButton.IsEnabled = false;
                updateFlatButton.IsEnabled = false;
                flatErrorsCount++;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                flatErrorsCount--;
                if (flatErrorsCount == 0)
                {
                    addFlatButton.IsEnabled = true;
                    removeFlatButton.IsEnabled = true;
                    updateFlatButton.IsEnabled = true;
                }
            }
        }
#endregion
    }
}
