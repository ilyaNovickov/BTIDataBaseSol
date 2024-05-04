using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows.Data;

namespace BTIDataBaseProj.Helpers
{
    /// <summary>
    /// Информация о помещении
    /// </summary>
    public class RoomInfo : INotifyPropertyChanged
    {
        private int roomId = -1;
        private int record = 0;
        private int squareRoom = 0;
        private string size = string.Empty;
        private string name = string.Empty;
        private string decoration = string.Empty;
        private int heightRoom = 0;
        private int socket = 0;
        private int section = 0;
        private int? flat = 0;
        private RoomsTable roomsTable = null;

        /// <summary>
        /// ID помещения
        /// </summary>
        public int RoomId 
        {
            get => roomId;
            set
            {
                roomId = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Номер помещения
        /// </summary>
        public int Record 
        {
            get => record; 
            set
            {
                record = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Площадь помещения
        /// </summary>
        public int SquareRoom 
        {
            get => squareRoom;
            set
            {
                squareRoom = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Размер помещения в плане
        /// </summary>
        public string Size 
        {
            get => size;
            set
            {
                size = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Назначение
        /// </summary>
        public string Name 
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Отделка
        /// </summary>
        public string Decoretion 
        {
            get => decoration;
            set
            {
                decoration = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Высота помещения
        /// </summary>
        public int HeightRoom
        {
            get => heightRoom;
            set
            {
                heightRoom = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кол-во розеток
        /// </summary>
        public int Socket 
        {
            get => socket;
            set
            {
                socket = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Кол-во элементов в батарее отопления
        /// </summary>
        public int Section 
        {
            get => section;
            set
            {
                section = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// ID связанной квартиры
        /// </summary>
        public Nullable<int> Flat 
        {
            get => flat ?? 0;
            set
            {
                flat = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Модель помещения в БД
        /// </summary>
        public RoomsTable RoomsTable
        {
            get => roomsTable;
            set
            {
                if (value == null)
                {
                    ClearProperties();
                    roomsTable = value;
                    goto End;
                }
                roomsTable = value;
                SetProperties(value);
End:
                OnPropertyChanged();
            }
        }

        //Установка соотведствующих свойств помещения
        private void SetProperties(RoomsTable roomsTable)
        {
            RoomId = roomsTable.RoomId;
            Record = roomsTable.Record;
            SquareRoom = roomsTable.SquareRoom;
            Size = roomsTable.Size;
            Name = roomsTable.Name;
            Decoretion = roomsTable.Decoretion;
            HeightRoom = roomsTable.HeightRoom;
            Socket = roomsTable.Socket;
            Section = roomsTable.Section;
            Flat = roomsTable.Flat;
        }

        //Сброс свойств в значение по умолчанию
        private void ClearProperties()
        {
            RoomId = -1;
            Record = 0;
            SquareRoom = 0;
            Size = string.Empty;
            Name = string.Empty;
            Decoretion = string.Empty;
            HeightRoom = 0;
            Socket = 0;
            Section = 0;
            Flat = -1;
        }

        /// <summary>
        /// Сброс свойств
        /// </summary>
        public void Clear()
        {
            ClearProperties();

            RoomsTable = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string protertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(protertyName));
        }
    }
}
