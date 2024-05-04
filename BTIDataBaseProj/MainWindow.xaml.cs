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
        /*
         * Для изменения маштаба изображения
         */
        //Максимально, минимальное и текущее значение
        //маштаба в % (1f == 100%)
        private float minScale = 0.02f;
        private float maxScale = 10.0f;
        private float scale = 1f;

        /// <summary>
        /// Событие изменения свойств
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Маштаб изображения
        /// </summary>
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

        //Контекст подлючения к бд
        BTIDataBaseEntities contex = new BTIDataBaseEntities();

        //Коллекции здания, квартир выбранного здания
        //и комнат выбранной квартиры
        CollectionViewSource buildingsViewSourse;
        CollectionViewSource flatsViewSourse;
        CollectionViewSource roomsViewSourse;

        //Информация о здании/квартире/помещении
        BuildingInfo buildingInfo = new BuildingInfo();
        FlatInfo flatInfo = new FlatInfo();
        RoomInfo roomInfo = new RoomInfo();

        //Коллекции ВСЕХ здания/квартир/комнат
        //предназначено для поиска
        private ICollectionView flatsSeachCollectionView;
        private ICollectionView buildingsSeachCollectionView;
        private ICollectionView roomsSeachCollectionView;


        public MainWindow()
        {
            InitializeComponent();
            //Получение коллекций из ресурсов приложения
            buildingsViewSourse = ((CollectionViewSource)(FindResource("buildingViewSourse")));
            flatsViewSourse = ((CollectionViewSource)(FindResource("flatsViewSourse")));
            roomsViewSourse = ((CollectionViewSource)FindResource("roomsViewSourse"));
        }

        ~MainWindow()
        {
            contex?.Dispose();
        }

        //Обработка события загрузки приложения
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Привязка контекста данных с информацией
                //о здании/квартире/комнаты элементов управления
                aboutBuildingGrid.DataContext = buildingInfo;
                aboutFlatGrid.DataContext = flatInfo;
                aboutRoomGrid.DataContext = roomInfo;

                //Загрузка таблицы зданий и её привязка к элементу DataGrid
                //При привязке также привязываются квартиры выбранного здания
                //и комнаты выбранной квартиры
                contex.BuildingsTable.Load();
                buildingsViewSourse.Source = contex.BuildingsTable.Local;

                //Для поиска зданий
                #region forBuildingSeach
                //Привязка данных из БД к таблице 
                buildingsSeachCollectionView = CollectionViewSource.GetDefaultView(contex.BuildingsTable.Local);

                //Привязка фильтрации зданий по кадастру и адрессу
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

                buildingsSeachDataGrid.ItemsSource = contex.BuildingsTable.Local;
                #endregion
                //Для поиска квартир
                #region forFlatSeach
                //Привязка данных из БД к таблице 
                contex.FlatsTable.Load();

                flatsSeachCollectionView = CollectionViewSource.GetDefaultView(contex.FlatsTable.Local);

                //Привязка фильтрации квартир по ID и номеру квартиры
                flatsSeachCollectionView.Filter = (obj) =>
                {
                    if ((flatIdSeachTextBox.Text == null || flatIdSeachTextBox.Text == "") &&
                        (flatNumberSeachTextBox.Text == null || flatNumberSeachTextBox.Text == ""))
                        return true;

                    FlatsTable f = obj as FlatsTable;

                    return f.FlatId.ToString().Contains(flatIdSeachTextBox.Text) &&
                                f.Flat.ToString().Contains(flatNumberSeachTextBox.Text);
                };

                flatSeachDataGrid.ItemsSource = contex.FlatsTable.Local;
                #endregion
                //Для поиска комнат
                #region forRoomSeach
                //Привязка данных из БД к таблице 
                contex.RoomsTable.Load();

                roomsSeachCollectionView = CollectionViewSource.GetDefaultView(contex.RoomsTable.Local);

                //Привязка фильтрации комнат по ID и номеру помещения
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

                roomsSeachDataGrid.ItemsSource = contex.RoomsTable.Local;
                #endregion
            }
            catch (Exception ex)
            {
                //Вывод сообщения при неудачном подключении к БД
                //и закрытие приложения
                MessageBox.Show($"Не удалось подключиться к базе данных. Информация о ошибке:\n" +
                    $"\"{ex.Message}\"\n" +
                    $"Проверте файл *.exe.config", "Ошибка подключения к БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void mainWin_Closing(object sender, CancelEventArgs e)
        {
            contex?.Dispose();
        }

        //Изменение выбранного элемента в таблице
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
        //Для таблицы и для поля с информацией о здании
        #region forBuilding
        //Для панели управления таблицы
        #region buildingsToolBar
        private void firstBuildingButton_Click(object sender, RoutedEventArgs e)=>buildingsViewSourse.View.MoveCurrentToFirst();
        
        private void previousBuildingButton_Click(object sender, RoutedEventArgs e)=>buildingsViewSourse.View.MoveCurrentToPrevious();
        
        private void nextBuildingButton_Click(object sender, RoutedEventArgs e)=>buildingsViewSourse.View.MoveCurrentToNext();
        
        private void lastBuildingButton_Click(object sender, RoutedEventArgs e)=>buildingsViewSourse.View.MoveCurrentToLast();
        #endregion

        //Добавление здания 
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
            buildingsSeachCollectionView.Refresh();

            SaveDBChangings();

            buildingsDataGrid.SelectedItem = building;
        }

        //Удаление выбранного здания
        private void removeBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingInfo.BuildingsTable == null && buildingInfo.Kadastr != buildingInfo.BuildingsTable?.Kadastr)
            {
                MessageBox.Show("Запис здания не выбрана");
                return;
            }

            contex.BuildingsTable.Remove(buildingInfo.BuildingsTable);

            buildingsViewSourse.View.Refresh();
            buildingsSeachCollectionView.Refresh();

            SaveDBChangings();
        }

        //Обновление данных для выбранного здания
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
            //Кадастр менять нельзя так как это первичный ключ
            //buildingInfo.BuildingsTable.Kadastr = buildingInfo.Kadastr;

            SaveDBChangings();

            buildingsViewSourse.View.Refresh();
            buildingsSeachCollectionView.Refresh();
        }

        //Очистка информации с панели
        private void clearBuildingInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingsDataGrid.SelectedItem == null)
            {
                buildingInfo.Clear();
                return;
            }
            buildingsDataGrid.SelectedItem = null;
        }

        //Открытие панели с примечаниями о здании
        private void openBuildongCommentsButton_Click(object sender, RoutedEventArgs e)=>buildingCommentsPanelMenuItem_Click(sender, e);

        //Открытие панели с изображением здания
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
        //Для таблицы и для поля с информацией о квартире
        #region aboutFlats
        //Для панели управления таблицы
        #region flatToolBar
        private void firstFlatButton_Click(object sender, RoutedEventArgs e)=>flatsViewSourse.View.MoveCurrentToFirst();
        
        private void previousFlatButton_Click(object sender, RoutedEventArgs e)=>flatsViewSourse.View.MoveCurrentToPrevious();
        
        private void nextFlatButton_Click(object sender, RoutedEventArgs e)=>flatsViewSourse.View.MoveCurrentToNext();
        
        private void lastFlatButton_Click(object sender, RoutedEventArgs e)=>flatsViewSourse.View.MoveCurrentToLast();
        #endregion
        //Добавление квартиры
        private void addFlatButtin_Click(object sender, RoutedEventArgs e)
        {
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

            contex.FlatsTable.Add(flatsTable);
            flatsViewSourse.View.Refresh();
            flatsSeachCollectionView.Refresh();

            SaveDBChangings();

            flatsDataGrid.SelectedItem = flatsTable;
        }

        //Удаление квартиры выбранной
        private void removeFlatButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatInfo.FlatsTable == null)
            {
                MessageBox.Show("Запис квартиры не выбрана");
                return;
            }

            contex.FlatsTable.Remove(flatInfo.FlatsTable);

            flatsViewSourse.View.Refresh();
            flatsSeachCollectionView.Refresh();

            SaveDBChangings();
        }

        //обновление данных в выбранной квартире
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

            SaveDBChangings();

            buildingsViewSourse.View.Refresh();
            flatsViewSourse.View.Refresh();
            flatsSeachCollectionView.Refresh();

        }

        //Очистка панели с информацией
        private void flatClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (flatsDataGrid.SelectedItem == null)
            {
                flatInfo.Clear();
                return;
            }
            flatsDataGrid.SelectedItem = null;
        }

        //Добавление квартиры к выбранному зданию
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

        /// <summary>
        /// Проверка кадастра здания для добавляемого здания
        /// </summary>
        private void CheckKadastrForFlat()
        {
            IEnumerable<string> kadastrs = from building in contex.BuildingsTable
                                           select building.Kadastr;

            if (!kadastrs.Contains(flatInfo.BuildingKadastr))
                MessageBox.Show("Не указан кадастр здания или здания с таким кадастром не существует\n" +
                    "Измените кадастр здания для квартиры", "Внимание",
                    MessageBoxButton.OK ,MessageBoxImage.Warning);
        }

        //Обработка ошибок с валидацией вводимых данных
        #region forError
        //Кол-во найденных ошибок
        private int flatErrorsCount = 0;

        private void AboutFlatTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            //Нельзя добавить квартиру при наличии ошибок
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addFlatButton.IsEnabled = false;
                removeFlatButton.IsEnabled = false;
                updateFlatButton.IsEnabled = false;
                addToSelectedBuildingButton.IsEnabled = false;
                flatErrorsCount++;
            }
            //Можно добавить квартиру когда ошибок нет
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
        //Для таблицы и для поля с информацией о помещении
        #region aboutRooms
        //Для панели управления таблицы
        #region roomsToolBar
        private void firstRoomButton_Click(object sender, RoutedEventArgs e)=>roomsViewSourse.View.MoveCurrentToFirst();

        private void previousRoomButton_Click(object sender, RoutedEventArgs e)=>roomsViewSourse.View.MoveCurrentToPrevious();

        private void nextRoomButton_Click(object sender, RoutedEventArgs e)=>roomsViewSourse.View.MoveCurrentToNext();

        private void lastRoomButton_Click(object sender, RoutedEventArgs e)=>roomsViewSourse.View.MoveCurrentToLast();
        #endregion
        //Обновление данных в выбранной квартире
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

            SaveDBChangings();

            roomsViewSourse.View.Refresh();
            flatsViewSourse.View.Refresh();
            buildingsViewSourse.View.Refresh();
            roomsSeachCollectionView.Refresh();
        }

        //Удаление выбранной комнаты
        private void removeRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomInfo.RoomsTable == null && roomInfo.RoomId != roomInfo.RoomsTable?.RoomId)
            {
                MessageBox.Show("Запис комнаты не выбрана");
                return;
            }

            contex.RoomsTable.Remove(roomInfo.RoomsTable);

            roomsViewSourse.View.Refresh();
            roomsSeachCollectionView.Refresh();

            SaveDBChangings();
        }

        //Добавление комнаты
        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
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

            contex.RoomsTable.Add(rooms);
            roomsViewSourse.View.Refresh();
            roomsSeachCollectionView.Refresh();

            SaveDBChangings();

            roomsDataGrid.SelectedItem = rooms;
        }

        /// <summary>
        /// Проверка ID квартиры для добавляемой комнаты
        /// </summary>
        private void CheckFlatIdForRoom()
        {
            IEnumerable<int> flats = from flat in contex.FlatsTable
                                     select flat.FlatId;
            if (!(flats.Contains(roomInfo.Flat.Value)))
                MessageBox.Show("Не указана ID квартиры или квартира с таким ID не существует\n" +
                    "Измените ID квартиры здания для комнаты",
                     "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Очистка информации о помещении
        private void clearRoomInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomsDataGrid.SelectedItem == null)
            {
                roomInfo.Clear();
                return;
            }
            roomsDataGrid.SelectedItem = null;
        }

        //Добавления помещения к выбранной квартире
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

        //Для проверки ошибок при валидации данныхы
        #region forErrors
        //Кол-во ошибок связанных с данными помещения
        private int roomsErrorsCount = 0;

        private void RoomsTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            //Нельзя добавить помещении при наличие ошибок
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addRoomButton.IsEnabled = false;
                removeRoomButton.IsEnabled = false;
                updateRoomButton.IsEnabled = false;
                addRoomToSelectedFlatButton.IsEnabled = false;
                roomsErrorsCount++;
            }
            //Можно добавить помещении когда ошибок нет
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

        //Для меню
        #region menu
        //Сохранение БД
        private void saveDBMenuItem_Click(object sender, RoutedEventArgs e) => SaveDBChangings();
        //Закрытие приложения
        private void closeMenuItem_Click(object sender, RoutedEventArgs e) => this.Close();
        //Менюшки для таблиц с данными
        #region forTables
        private void openBuildingsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(buildingsTable);

        private void openFlatsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(flatsTable);

        private void openRoomsTableMenuItem_Click(object sender, RoutedEventArgs e) => OpenTableInAvalonDock(roomsTable);

        /// <summary>
        /// Открывает закрытое окно в панели для отображения таблиц БД
        /// </summary>
        /// <param name="content"></param>
        private void OpenTableInAvalonDock(LayoutContent content)
        {
            if (tableLayout.Children.Contains(content))
                return;

            tableLayout.Children.Add(content);
            tableLayout.SelectedContentIndex = tableLayout.Children.Count - 1;
        }
        #endregion
        //Менюшки для "Информация о ..."
        #region forVisibleofPanels
        private void buildingInfoPanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingInfoPanel.IsVisible = true;
        
        private void flatInfoPanelMenuItem_Click(object sender, RoutedEventArgs e) => flatInfoPanel.IsVisible = true;
        
        private void roomInfoMenuItem_Click(object sender, RoutedEventArgs e) => roomInfoPanel.IsVisible = true;
        
        private void buildingImagePanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingImagePanel.IsVisible = true;

        private void buildingCommentsPanelMenuItem_Click(object sender, RoutedEventArgs e) => buildingCommentsPanel.IsVisible = true;
        #endregion
        //Менюшки для поиска
        #region forSeaching
        private void openBuildingSeachMenuItem_Click(object sender, RoutedEventArgs e) => OpenSeachPanelInAvalonDock(buildingsSeachPanel);

        private void openFlatSeachMenuItem_Click(object sender, RoutedEventArgs e) => OpenSeachPanelInAvalonDock(flatsSeachPanel);

        private void openRoomSeachMenuItem_Click(object sender, RoutedEventArgs e) => OpenSeachPanelInAvalonDock(roomsSeachPanel);

        /// <summary>
        /// Открытие панелей с таблицами для поиска 
        /// </summary>
        /// <param name="content"></param>
        private void OpenSeachPanelInAvalonDock(LayoutAnchorable content)
        {
            if (extraPanel.Children.Contains(content))
                return;

            extraPanel.Children.Add(content);
            extraPanel.SelectedContentIndex = extraPanel.Children.Count - 1;
        }
        #endregion
        #endregion

        //Для поиска
        #region seach
        //Для поиска зданий
        #region buildingSeach
        //обновить таблицу для поиска
        private void updateBuildingSeachButton_Click(object sender, RoutedEventArgs e) => buildingsSeachCollectionView.Refresh();

        //Выбрать здание из окна поиска в окно главно таблицы
        private void selectBuildingOnSeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildingsSeachDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Здание не выбрано");
                return;
            }

            SelectBuildingOnSeach(((BuildingsTable)buildingsSeachDataGrid.SelectedItem).Kadastr);
        }
        #endregion
        //Для поиска квартир
        #region flatSeach
        //обновить таблицу для поиска
        private void updateFlatSeachButton_Click(object sender, RoutedEventArgs e) => flatsSeachCollectionView.Refresh();

        //Выбрать квартиру из окна поиска в окно главно таблицы
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
        #endregion
        //Для поиска помещений
        #region roomSeach
        //обновить таблицу для поиска
        private void updateRoomsSeachButton_Click(object sender, RoutedEventArgs e) => roomsSeachCollectionView.Refresh();

        //Выбрать помещение из окна поиска в окно главно таблицы
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
        #endregion

        /// <summary>
        /// Выбрать здание, найденное в таблице поиска, в главной таблице
        /// </summary>
        /// <param name="kadastr">Кадастр выбранного здания</param>
        /// <returns></returns>
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

        /// <summary>
        /// Выбрать квартиру, найденное в таблице поиска, в главной таблице
        /// </summary>
        /// <param name="flatId">ID квартиры</param>
        /// <returns></returns>
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

        /// <summary>
        /// Выбрать помещение, найденное в таблице поиска, в главной таблице
        /// </summary>
        /// <param name="roomId">ID комнаты</param>
        /// <returns></returns>
        private bool SelectRoomOnSeach(int roomId = -1)
        {
            IEnumerable<RoomsTable> rooms = from RoomsTable room in roomsDataGrid.Items
                                            where room.RoomId == roomId
                                            select room;

            if (rooms.Count() != 1)
            {
                MessageBox.Show("В таблице нет комнаты с таким Id");
                return false;
            }

            roomsDataGrid.SelectedItem = rooms.First();

            return true;
        }
        #endregion

        /// <summary>
        /// Сохранение базы данных
        /// </summary>
        private void SaveDBChangings()
        {
            try
            {
                contex.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения БД. Сообщение об ошибке:\n"+
                    $"{ex.Message}", "Ошибка сохранения БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
}