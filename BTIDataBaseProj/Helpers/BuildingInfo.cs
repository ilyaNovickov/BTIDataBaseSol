using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BTIDataBaseProj.Helpers
{
    public class BuildingInfo : INotifyPropertyChanged
    {
        private BuildingsTable building = null;
        private string kadastr = "";
        private string address="";
        private string district = "";
        private int land = 0;
        private int year = 0;
        private string material = "";
        private string @base ="";
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

        public string Kadastr 
        {
            get => kadastr;
            set
            {
                kadastr = value;
                OnPropertyChanged();
            }
        }
        public string Address 
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }
        public string District 
        {
            get => district;
            set
            {
                district = value;
                OnPropertyChanged();
            }
        }
        public int Land 
        {
            get => land;
            set
            {
                land = value;
                OnPropertyChanged();
            }
        }
        public int Year 
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged();
            }
        }
        public string Material
        {
            get => material;
            set
            {
                material = value;
                OnPropertyChanged();
            }
        }
        public string Base 
        {
            get => @base;
            set
            {
                @base = value;
                OnPropertyChanged();
            }
        }
        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged();
            }
        }
        public int Wear 
        {
            get => wear;
            set
            {
                wear = value;
                OnPropertyChanged();
            }
        }
        public int Flow 
        {
            get => flow;
            set
            {
                flow = value;
                OnPropertyChanged();
            }
        }
        public int Line 
        {
            get => line;
            set
            {
                line = value;
                OnPropertyChanged();
            }
        }
        public int Square 
        {
            get => square;
            set
            {
                square = value;
                OnPropertyChanged();
            }
        }
        public byte[] Picture 
        {
            get => pic;
            set
            {
                pic = value;
                OnPropertyChanged();
            }
        }
        public int Flats 
        {
            get => flats;
            set
            {
                flats = value;
                OnPropertyChanged();
            }
        }
        public bool Elevator 
        {
            get => elevator;
            set
            {
                elevator = value;
                OnPropertyChanged();
            }
        }

        public  BuildingsTable BuildingsTable 
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

        public void Clear()
        {
            ClearProperties();

            BuildingsTable = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
