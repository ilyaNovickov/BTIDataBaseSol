using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BTIDataBaseProj.Helpers
{
    /// <summary>
    /// Информация о здании
    /// </summary>
    public class BuildingInfo : INotifyPropertyChanged
    {
        private BuildingsTable building = null;
        private string kadastr = "";
        private string address = "";
        private string district = "";
        private int land = 0;
        private int year = 0;
        private string material = "";
        private string @base = "";
        private string comments = null;
        private int wear = 0;
        private int flow = 0;
        private int line = 0;
        private int square = 0;
        private byte[] pic = null;
        private int flats = 0;
        private bool elevator = false;

        public BuildingInfo() { }

        public BuildingInfo(BuildingsTable building)
        {
            BuildingsTable = building;
        }

        /// <summary>
        /// Кадастр здания
        /// </summary>
        public string Kadastr
        {
            get => kadastr;
            set
            {
                kadastr = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Адресс здания
        /// </summary>
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Район 
        /// </summary>
        public string District
        {
            get => district;
            set
            {
                district = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Площадь земельного участка
        /// </summary>
        public int Land
        {
            get => land;
            set
            {
                land = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Год постройки
        /// </summary>
        public int Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Материал стен
        /// </summary>
        public string Material
        {
            get => material;
            set
            {
                material = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Материал фундамента
        /// </summary>
        public string Base
        {
            get => @base;
            set
            {
                @base = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Примечания к зданию
        /// </summary>
        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Износ в процентах
        /// </summary>
        public int Wear
        {
            get => wear;
            set
            {
                wear = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кол-во этажей 
        /// </summary>
        public int Flow
        {
            get => flow;
            set
            {
                flow = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Расстояние от центра города
        /// </summary>
        public int Line
        {
            get => line;
            set
            {
                line = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Площадь квартир
        /// </summary>
        public int Square
        {
            get => square;
            set
            {
                square = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Изображение здания
        /// </summary>
        public byte[] Picture
        {
            get => pic;
            set
            {
                pic = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кол-во квартир
        /// </summary>
        public int Flats
        {
            get => flats;
            set
            {
                flats = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Наличие лифта
        /// </summary>
        public bool Elevator
        {
            get => elevator;
            set
            {
                elevator = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Связанная модель здания из БД
        /// </summary>
        public BuildingsTable BuildingsTable
        {
            get => building;
            set
            {
                if (value == null)
                {
                    ClearProperties();
                    building = value;
                    goto End;
                }
                building = value;
                SetProperties(value);
End:
                OnPropertyChanged();
            }
        }

        //Установка свойств информации о здании
        private void SetProperties(BuildingsTable buildingsTable)
        {
            Kadastr = buildingsTable.Kadastr;
            Address = buildingsTable.Address;
            District = buildingsTable.District;
            Land = buildingsTable.Land;
            Year = buildingsTable.Year;
            Material = buildingsTable.Material;
            Base = buildingsTable.Base;
            Comments = buildingsTable.Comments;
            Wear = buildingsTable.Wear;
            Flow = buildingsTable.Flow;
            Line = buildingsTable.Line;
            Square = buildingsTable.Square;
            Picture = buildingsTable.Picture;
            Flats = buildingsTable.Flats;
            Elevator = buildingsTable.Elevator;
        }

        //Сброс свойств в состояние по умолчанию
        private void ClearProperties()
        {
            Kadastr = string.Empty;
            Address = string.Empty;
            District = string.Empty;
            Land = 0;
            Year = 0;
            Material = string.Empty;
            Base = string.Empty;
            Comments = null;
            Wear = 0;
            Flow = 0;
            Line = 0;
            Square = 0;
            Picture = null;
            Flats = 0;
            Elevator = false;
        }

        /// <summary>
        /// Сброс свойств
        /// </summary>
        public void Clear()
        {
            ClearProperties();

            BuildingsTable = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
