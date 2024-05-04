using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows.Controls;

namespace BTIDataBaseProj.Helpers
{
    /// <summary>
    /// Информация о квартире
    /// </summary>
    public class FlatInfo : INotifyPropertyChanged
    {
        private FlatsTable flatTable = null;
        private int flatId = -1;
        private int flat = 0;
        private int storey = 0;
        private int rooms = 0;
        private bool level = false;
        private int squareFlat = 0;
        private int dwell = 0;
        private int branch = 0;
        private int balcony = 0;
        private int height = 0;
        private string buildingKadastr = "";

        /// <summary>
        /// Id квартиры
        /// </summary>
        public int FlatId 
        {
            get => flatId;
            set
            {
                flatId = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Номер квартиры
        /// </summary>
        public int Flat 
        {
            get => flat;
            set
            {
                flat = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Номер этажа
        /// </summary>
        public int Storey 
        {
            get => storey; 
            set
            {
                storey = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кол-во комнат
        /// </summary>
        public int Rooms 
        {
            get => rooms;
            set
            {
                rooms = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Двух уровневая
        /// </summary>
        public bool Level 
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Общая площадь квартиры
        /// </summary>
        public int SquareFlat 
        {
            get => squareFlat;
            set
            {
                squareFlat = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Жилая площадь
        /// </summary>
        public int Dwell 
        {
            get => dwell;
            set
            {
                dwell = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Вспомогательная площадь
        /// </summary>
        public int Branch 
        {
            get => branch;
            set
            {
                branch = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Площадб балкона
        /// </summary>
        public int Balcony 
        {
            get => balcony;
            set
            {
                balcony = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Высота квартиры
        /// </summary>
        public int Height 
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кадастр здания, где находится эта квартира
        /// </summary>
        public string BuildingKadastr 
        {
            get => buildingKadastr;
            set
            {
                buildingKadastr = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Связанная модель квартиры из БД
        /// </summary>
        public FlatsTable FlatsTable
        {
            get => flatTable;
            set
            {
                if (value == null)
                {
                    ClearProperties();
                    flatTable = value;
                    goto End;
                }
                flatTable = value;
                SetProperties(value);
End:
                OnPropertyChanged();
            }
        }

        //Установка соотведствующих свойств квартиры
        private void SetProperties(FlatsTable buildingsTable)
        {
            FlatId = buildingsTable.FlatId;
            BuildingKadastr = buildingsTable.BuildingKadastr;
            Flat = buildingsTable.Flat;
            Storey = buildingsTable.Storey;
            Level = buildingsTable.Level;
            Dwell = buildingsTable.Dwell;
            Balcony = buildingsTable.Balcony;
            Height = buildingsTable.Height;
            Branch = buildingsTable.Branch;
            Rooms = buildingsTable.Rooms;
            SquareFlat = buildingsTable.SquareFlat;
        }

        //Сброс свойств в значение по умолчанию
        private void ClearProperties()
        {
            FlatId = -1;
            BuildingKadastr = string.Empty;
            Flat = 0;
            Storey = 0;
            Level = false;
            Dwell = 0;
            Balcony = 0;
            Height = 0;
            Branch = 0;
            Rooms = 0;
            SquareFlat = 0;
        }

        /// <summary>
        /// Сброс свойств
        /// </summary>
        public void Clear()
        {
            ClearProperties();

            FlatsTable = null;
        }

        public FlatInfo() 
        { 

        }

        public FlatInfo(FlatsTable flatsTable)
        {
            FlatsTable = flatTable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
