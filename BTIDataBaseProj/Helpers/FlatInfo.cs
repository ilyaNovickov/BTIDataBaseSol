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

namespace BTIDataBaseProj.Helpers
{
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

        public int FlatId 
        {
            get => flatId;
            set
            {
                flatId = value;
                OnPropertyChanged();
            }
        }
        public int Flat 
        {
            get => flat;
            set
            {
                flat = value;
                OnPropertyChanged();
            }
        }
        public int Storey 
        {
            get => storey; 
            set
            {
                storey = value;
                OnPropertyChanged();
            }
        }
        public int Rooms 
        {
            get => rooms;
            set
            {
                rooms = value;
                OnPropertyChanged();
            }
        }
        public bool Level 
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }
        public int SquareFlat 
        {
            get => squareFlat;
            set
            {
                squareFlat = value;
                OnPropertyChanged();
            }
        }
        public int Dwell 
        {
            get => dwell;
            set
            {
                dwell = value;
                OnPropertyChanged();
            }
        }
        public int Branch 
        {
            get => branch;
            set
            {
                branch = value;
                OnPropertyChanged();
            }
        }
        public int Balcony 
        {
            get => balcony;
            set
            {
                balcony = value;
                OnPropertyChanged();
            }
        }
        public int Height 
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }
        public string BuildingKadastr 
        {
            get => buildingKadastr;
            set
            {
                buildingKadastr = value;
                OnPropertyChanged();
            }
        }

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

        private void SetProperties(FlatsTable buildingsTable)
        {
            FlatId = buildingsTable.Flat;
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
