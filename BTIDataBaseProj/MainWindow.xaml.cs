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
using System.Runtime.Remoting.Contexts;
using Xceed.Wpf.AvalonDock.Layout;
using System.Collections.ObjectModel;

namespace BTIDataBaseProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region forScale
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
        #endregion

        BTIDataBaseEntities1 contex = new BTIDataBaseEntities1();
        CollectionViewSource buildingsViewSourse;
        CollectionViewSource flatsViewSourse;
        CollectionViewSource roomsViewSourse;
        BuildingInfo buildingInfo = new BuildingInfo();
        FlatInfo flatInfo = new FlatInfo();
        RoomInfo roomInfo = new RoomInfo();

        private ICollectionView flatsSeachCollectionView;
        private ICollectionView buildingsSeachCollectionView;
        private ICollectionView roomsSeachCollectionView;

        public MainWindow()
        {
            InitializeComponent();

            buildingsViewSourse = ((CollectionViewSource)(FindResource("buildingViewSourse")));
            flatsViewSourse = ((CollectionViewSource)(FindResource("flatsViewSourse")));
            roomsViewSourse = ((CollectionViewSource)FindResource("roomsViewSourse"));
        }

        ~MainWindow()
        {
            contex?.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aboutBuildingGrid.DataContext = buildingInfo;
            aboutFlatGrid.DataContext = flatInfo;
            aboutRoomGrid.DataContext = roomInfo;
            
            contex.BuildingsTable.Load();
            buildingsViewSourse.Source = contex.BuildingsTable.Local;
            #region forBuildingSeach
            ObservableCollection<BuildingsTable> buildingsObsevableList = new ObservableCollection<BuildingsTable>(contex.BuildingsTable.Local);

            buildingsSeachCollectionView = CollectionViewSource.GetDefaultView(buildingsObsevableList);
            buildingsSeachCollectionView.Filter = (obj) =>
            {
                if ((buildingKadastrSeachTextBox.Text == null || buildingKadastrSeachTextBox.Text == "")
                && ((buildingAddressSeachTextBox.Text == null || buildingAddressSeachTextBox.Text == "")))
                {
                    return true;
                }

                BuildingsTable building = obj as BuildingsTable;

                return building.Kadastr.Contains(buildingKadastrSeachTextBox.Text) &&
                        building.Address.Contains(buildingAddressSeachTextBox.Text);
            };

            buildingsSeachDataGrid.ItemsSource = buildingsObsevableList;
#endregion
            #region forFlatSeach
            contex.FlatsTable.Load();

            ObservableCollection<FlatsTable> flatsObservableList = new ObservableCollection<FlatsTable>(contex.FlatsTable.Local);

            flatsSeachCollectionView = CollectionViewSource.GetDefaultView(flatsObservableList);
            flatsSeachCollectionView.Filter = (obj) =>
            {
                if (flatIdSeachTextBox.Text == null || flatIdSeachTextBox.Text == "")
                    return true;

                FlatsTable f = obj as FlatsTable;

                return f.FlatId.ToString().Contains(flatIdSeachTextBox.Text);
            };
            
            flatSeachDataGrid.ItemsSource = flatsObservableList;
            #endregion
            #region forRoomSeach
            contex.RoomsTable.Load();

            ObservableCollection<RoomsTable> roomsTables = new ObservableCollection<RoomsTable>(contex.RoomsTable.Local);

            roomsSeachCollectionView = CollectionViewSource.GetDefaultView(roomsTables);
            roomsSeachCollectionView.Filter = (obj) =>
            {
                if ((roomIdSeachTextBox.Text == null || roomIdSeachTextBox.Text == "")
                && ((roomRecordSeachTextBox.Text == null || roomRecordSeachTextBox.Text == "")))
                {
                    return true;
                }

                RoomsTable room = obj as RoomsTable;

                return room.RoomId.ToString().Contains(roomIdSeachTextBox.Text) &&
                        room.Record.ToString().Contains(roomRecordSeachTextBox.Text);
            };

            roomsSeachDataGrid.ItemsSource = roomsTables;
#endregion
        }



        private void mainWin_Closing(object sender, CancelEventArgs e)
        {
            contex.Dispose();
        }

        #region dataGrids Selection Changed
        private void buildingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(buildingsDataGrid.SelectedItem is BuildingsTable))
            {
                buildingInfo.Clear();
                return;
            }

            buildingInfo.BuildingsTable = (BuildingsTable)buildingsDataGrid.SelectedItem;
        }

        private void flatsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(flatsDataGrid.SelectedItem is FlatsTable))
            {
                flatInfo.Clear();
                return;
            }

            flatInfo.FlatsTable = (FlatsTable)flatsDataGrid.SelectedItem;
        }

        private void roomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(roomsDataGrid.SelectedItem is RoomsTable))
            {
                roomInfo.Clear();
                return;
            }

            roomInfo.RoomsTable = (RoomsTable)roomsDataGrid.SelectedItem;
        }
        #endregion
        #region forBuilding
        #region buildingsToolBar
        private void firstBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            buildingsViewSourse.View.MoveCurrentToFirst();
        }

        private void previousBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            buildingsViewSourse.View.MoveCurrentToPrevious();
        }

        private void nextBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            buildingsViewSourse.View.MoveCurrentToNext();
        }

        private void lastBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            buildingsViewSourse.View.MoveCurrentToLast();
        }


        #endregion
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

            //roomInfo.RoomsTable = rooms;

            contex.BuildingsTable.Add(building);
            buildingsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();

            buildingsDataGrid.SelectedItem = building;
        }

        private void removeBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null && buildingInfo.Kadastr != buildingInfo.BuildingsTable?.Kadastr)
            {
                MessageBox.Show("Запис здания не выбрана");
                return;
            }

            contex.BuildingsTable.Remove(buildingInfo.BuildingsTable);
            buildingInfo.Clear();
            buildingsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();
        }

        private void updateBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null && buildingInfo.Kadastr != buildingInfo.BuildingsTable.Kadastr)
            {
                MessageBox.Show("Запись здания не выбрана");
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

            //contex.SaveChanges();
            SaveDBChangings();

            buildingsViewSourse.View.Refresh();
            
        }

        private void clearBuildingInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingsDataGrid.SelectedItem == null)
            {
                buildingInfo.Clear();
                return;
            }
            buildingsDataGrid.SelectedItem = null;
        }

        private void openBuildongCommentsButton_Click(object sender, RoutedEventArgs e)=>buildingCommentsPanelMenuItem_Click(sender, e);

        private void openBuildingImageButton_Click(object sender, RoutedEventArgs e)=>buildingImagePanelMenuItem_Click(sender, e);

        #region extraForBuilding
        #region forImage
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

        private void clearBuildingPictureButton_Click(object sender, RoutedEventArgs e)
        {
            buildingInfo.Picture = null;
        }

        #region forImageSlider
        private double a => (minScale * maxScale - Math.Pow(1d, 2d)) / (minScale - 2d * 1d + maxScale);
        private double b => Math.Pow((1d - minScale), 2d) / (minScale - 2d * 1d + maxScale);
        private double c => 2d * Math.Log((maxScale - 1d) / (1d - minScale));

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Scale = (float)(a + b * Math.Exp(c * slider.Value));
        }
        #endregion
        #endregion
        #region forComments
        private void clearBuildingCommentsButton_Click(object sender, RoutedEventArgs e)
        {
            buildingCommentsTextBox.Clear();
        }
        #endregion
        #endregion

        #endregion
        #region aboutFlats
        #region flatToolBar
        private void firstFlatButton_Click(object sender, RoutedEventArgs e)
        {
            flatsViewSourse.View.MoveCurrentToFirst();
        }

        private void previousFlatButton_Click(object sender, RoutedEventArgs e)
        {
            flatsViewSourse.View.MoveCurrentToPrevious();
        }

        private void nextFlatButton_Click(object sender, RoutedEventArgs e)
        {
            flatsViewSourse.View.MoveCurrentToNext();
        }

        private void lastFlatButton_Click(object sender, RoutedEventArgs e)
        {
            flatsViewSourse.View.MoveCurrentToLast();
        }
        #endregion
        private void addFlatButtin_Click(object sender, RoutedEventArgs e)
        {
            //if (flatInfo.FlatId != -1 && flatInfo.FlatsTable != null)
            //{
            //    MessageBoxResult res = MessageBox.Show("Информация о квартире ссылается на существующею квартиру\nДобавить новую квартиру?", "Внимание", MessageBoxButton.YesNo);

            //    if (res != MessageBoxResult.Yes)
            //        return;
            //}

            CheckKadastrForFlat();

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

            //flatInfo.FlatsTable = flatsTable;

            contex.FlatsTable.Add(flatsTable);
            flatsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();

            flatsDataGrid.SelectedItem = flatsTable;
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
            flatsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();
        }

        private void updateFlatButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatInfo.FlatsTable == null)
            {
                MessageBox.Show("Квартира для изменения не выбрана");
                return;
            }

            CheckKadastrForFlat();

            flatInfo.FlatsTable.Dwell = flatInfo.Dwell;
            flatInfo.FlatsTable.Rooms = flatInfo.Rooms;
            flatInfo.FlatsTable.Branch = flatInfo.Branch;
            flatInfo.FlatsTable.Balcony = flatInfo.Balcony;
            flatInfo.FlatsTable.Flat = flatInfo.Flat;
            flatInfo.FlatsTable.Height = flatInfo.Height;
            flatInfo.FlatsTable.Level = flatInfo.Level;
            flatInfo.FlatsTable.Storey = flatInfo.Storey;
            flatInfo.FlatsTable.SquareFlat = flatInfo.SquareFlat;
            flatInfo.FlatsTable.BuildingKadastr = flatInfo.BuildingKadastr;

            //contex.SaveChanges();
            SaveDBChangings();

            buildingsViewSourse.View.Refresh();
            flatsViewSourse.View.Refresh();


        }

        private void flatClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatsDataGrid.SelectedItem == null)
            {
                flatInfo.Clear();
                return;
            }
            flatsDataGrid.SelectedItem = null;
        }

        private void addFlatToSelectedBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null)
            {
                MessageBox.Show("Нет выбранного здания");
                return;
            }

            flatInfo.BuildingKadastr = buildingInfo.Kadastr;

            addFlatButtin_Click(sender, e);
        }
        private void CheckKadastrForFlat()
        {
            IEnumerable<string> kadastrs = from building in contex.BuildingsTable
                                           select building.Kadastr;

            if (!kadastrs.Contains(flatInfo.BuildingKadastr))
            {
                MessageBox.Show("Не указан кадастр здания или здания с таким кадастром не существует\nИзмените кадастр здания для квартиры");
                return;
            }
        }

        #region forError
        private int flatErrorsCount = 0;
        private void AboutFlatTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addFlatButton.IsEnabled = false;
                removeFlatButton.IsEnabled = false;
                updateFlatButton.IsEnabled = false;
                addToSelectedBuildingButton.IsEnabled = false;
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
                    addToSelectedBuildingButton.IsEnabled = true;
                }
            }
        }

        #endregion

        #endregion
        #region aboutRooms
        #region roomsToolBar
        private void firstRoomButton_Click(object sender, RoutedEventArgs e)
        {
            roomsViewSourse.View.MoveCurrentToFirst();
        }

        private void previousRoomButton_Click(object sender, RoutedEventArgs e)
        {
            roomsViewSourse.View.MoveCurrentToPrevious();

        }

        private void nextRoomButton_Click(object sender, RoutedEventArgs e)
        {
            roomsViewSourse.View.MoveCurrentToNext();
        }

        private void lastRoomButton_Click(object sender, RoutedEventArgs e)
        {
            roomsViewSourse.View.MoveCurrentToLast();
        }
        #endregion
        private void updateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomInfo.RoomsTable == null)
            {
                MessageBox.Show("Комната для изменения не выбрана");
                return;
            }

            CheckFlatIdForRoom();

            roomInfo.RoomsTable.Record = roomInfo.Record;
            roomInfo.RoomsTable.Flat = roomInfo.Flat;
            roomInfo.RoomsTable.Decoretion = roomInfo.Decoretion;
            roomInfo.RoomsTable.HeightRoom = roomInfo.HeightRoom;
            roomInfo.RoomsTable.Section = roomInfo.Section;
            roomInfo.RoomsTable.Socket = roomInfo.Socket;
            roomInfo.RoomsTable.Size = roomInfo.Size;
            roomInfo.RoomsTable.SquareRoom = roomInfo.SquareRoom;
            roomInfo.RoomsTable.Name = roomInfo.Name;


            //contex.SaveChanges();
            SaveDBChangings();

            roomsViewSourse.View.Refresh();
            flatsViewSourse.View.Refresh();
            buildingsViewSourse.View.Refresh();
        }

        private void removeRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomInfo.RoomsTable == null && roomInfo.RoomId != roomInfo.RoomsTable?.RoomId)
            {
                MessageBox.Show("Запис комнаты не выбрана");
                return;
            }

            contex.RoomsTable.Remove(roomInfo.RoomsTable);
            roomInfo.Clear();
            roomsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();
        }

        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
            //if (roomInfo.Flat.Value != -1 && roomInfo.RoomsTable != null)
            //{
            //    MessageBoxResult res = MessageBox.Show("Информация о комнате ссылается на существующею комнату\nДобавить новую комнату?", "Внимание", MessageBoxButton.YesNo);

            //    if (res != MessageBoxResult.Yes)
            //        return;
            //}

            CheckFlatIdForRoom();

            RoomsTable rooms = new RoomsTable()
            {
                Flat = roomInfo.Flat,
                Decoretion = roomInfo.Decoretion,
                Socket = roomInfo.Socket,
                Section = roomInfo.Section,
                Size = roomInfo.Size,
                SquareRoom = roomInfo.SquareRoom,
                HeightRoom = roomInfo.HeightRoom,
                Name = roomInfo.Name,
                Record = roomInfo.Record,
            };

            //roomInfo.RoomsTable = rooms;

            contex.RoomsTable.Add(rooms);
            roomsViewSourse.View.Refresh();
            //contex.SaveChanges();
            SaveDBChangings();

            roomsDataGrid.SelectedItem = rooms;
        }

        private void CheckFlatIdForRoom()
        {
            IEnumerable<int> flats = from flat in contex.FlatsTable
                                     select flat.FlatId;
            if (!(flats.Contains(roomInfo.Flat.Value)))
            {
                MessageBox.Show("Не указана ID квартиры или квартира с таким ID не существует\nИзмените ID квартиры здания для комнаты");
                return;
            }
        }

        private void clearRoomInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomsDataGrid.SelectedItem == null)
            {
                roomInfo.Clear();
                return;
            }
            roomsDataGrid.SelectedItem = null;
        }

        private void addRoomToSelectedFlatButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatInfo.FlatsTable == null)
            {
                MessageBox.Show("Квартира не выбрана");
                return;
            }

            roomInfo.Flat = flatInfo.FlatId;

            addRoomButton_Click(sender, e);
        }

        #region forErrors
        private int roomsErrorsCount = 0;

        private void RoomsTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addRoomButton.IsEnabled = false;
                removeRoomButton.IsEnabled = false;
                updateRoomButton.IsEnabled = false;
                addRoomToSelectedFlatButton.IsEnabled = false;
                roomsErrorsCount++;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                roomsErrorsCount--;
                if (roomsErrorsCount == 0)
                {
                    addRoomButton.IsEnabled = true;
                    removeRoomButton.IsEnabled = true;
                    updateRoomButton.IsEnabled = true;
                    addRoomToSelectedFlatButton.IsEnabled = true;
                }
            }
        }
        #endregion

        #endregion
        #region menu
        #region forTables
        private void openBuildingsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(buildingsTable);


        private void openFlatsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(flatsTable);


        private void openRoomsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(roomsTable);


        private void OpenTableInAvalonDock(LayoutContent content)
        {
            if (tableLayout.Children.Contains(content))
                return;

            tableLayout.Children.Add(content);
            tableLayout.SelectedContentIndex = tableLayout.Children.Count - 1;
        }
        #endregion
        #region forVisibleofPanels
        private void buildingInfoPanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingInfoPanel.IsVisible = true;
        

        private void flatInfoPanelMenuItem_Click(object sender, RoutedEventArgs e) => flatInfoPanel.IsVisible = true;
        

        private void roomInfoMenuItem_Click(object sender, RoutedEventArgs e) => roomInfoPanel.IsVisible = true;
        

        private void buildingImagePanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingImagePanel.IsVisible = true;


        private void buildingCommentsPanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingCommentsPanel.IsVisible = true;

        private void saveDBMenuItem_Click(object sender, RoutedEventArgs e) => SaveDBChangings();


        #endregion

        #endregion

        private void SaveDBChangings()
        {
            try
            {
                contex.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateFlatSeachButton_Click(object sender, RoutedEventArgs e)=>flatsSeachCollectionView.Refresh();
        

        private void selectFlatOnSeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatSeachDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Квартира не выбрана");
                return;
            }

            if (!SelectBuildingOnSeach(((FlatsTable)flatSeachDataGrid.SelectedItem).BuildingKadastr))
            {
                MessageBox.Show("Здания с таким кадастром нет");
                return;
            }

            SelectFlatOnSeach(((FlatsTable)flatSeachDataGrid.SelectedItem).FlatId);
        }

        private void updateBuildingSeachButton_Click(object sender, RoutedEventArgs e)=>buildingsSeachCollectionView.Refresh();
        

        private void selectBuildingOnSeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingsSeachDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Здание не выбрано");
                return;
            }

            SelectBuildingOnSeach(((BuildingsTable)buildingsSeachDataGrid.SelectedItem).Kadastr);
        }

        private bool SelectBuildingOnSeach(string kadastr = "")
        {
            IEnumerable<BuildingsTable> buildings = from BuildingsTable building in buildingsDataGrid.Items
                                                    where building.Kadastr == kadastr
                                                    select building;

            if (buildings.Count() != 1)
            {
                MessageBox.Show("В таблице нет здания с таким кадастром");
                return false;
            }

            buildingsDataGrid.SelectedItem = buildings.First();

            return true;
        }

        private bool SelectFlatOnSeach(int flatId = -1)
        {
            IEnumerable<FlatsTable> flats = from FlatsTable flat in flatsDataGrid.Items
                                            where flat.FlatId == flatId
                                            select flat;
            if (flats.Count() != 1)
            {
                MessageBox.Show("В таблице нет квартиры с таким Id");
                return false;
            }

            flatsDataGrid.SelectedItem = flats.First();

            return true;
        }

        private void selectRoomOnSeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomsSeachDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Комната не выбрана");
                return;
            }

            if (!SelectBuildingOnSeach(((RoomsTable)roomsSeachDataGrid.SelectedItem).FlatsTable.BuildingKadastr))
            {
                MessageBox.Show("Здания с таким кадастром нет");
                return;
            }

            if (!SelectFlatOnSeach(((RoomsTable)roomsSeachDataGrid.SelectedItem).FlatsTable.FlatId))
            {
                MessageBox.Show("Квартиры с таким Id нет");
                return;
            }

            SelectRoomOnSeach(((RoomsTable)roomsSeachDataGrid.SelectedItem).RoomId);
        }

        private void updateRoomsSeachButton_Click(object sender, RoutedEventArgs e)=>roomsSeachCollectionView.Refresh();
        
        private bool SelectRoomOnSeach(int roomId = -1)
        {
            IEnumerable<RoomsTable> rooms = from RoomsTable room in roomsDataGrid.Items
                                            where room.RoomId == roomId
                                            select room;

            if (rooms.Count() != 0)
            {
                MessageBox.Show("В таблице нет комнаты с таким Id");
                return false;
            }

            roomsDataGrid.SelectedItem = rooms.First();

            return true;
        }
    }
}
