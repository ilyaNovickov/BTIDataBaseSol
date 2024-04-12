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

        public BuildingInfo() { }
        public BuildingInfo(BuildingsTable building)
        {
            BuildingsTable = building;
        }

        public string Kadastr { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public int Land { get; set; }
        public int Year { get; set; }
        public string Material { get; set; }
        public string Base { get; set; }
        public string Comments { get; set; }
        public int Wear { get; set; }
        public int Flow { get; set; }
        public int Line { get; set; }
        public int Square { get; set; }
        public byte[] Picture { get; set; }
        public int Flats { get; set; }
        public bool Elevator { get; set; }

        public  BuildingsTable BuildingsTable 
        {
            get => building;
            set
            {
                if (value == null)
                {
                    Clear();
                    return;
                }
                building = value;
                SetProperties(value);
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

        public void Clear()
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

            BuildingsTable = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
