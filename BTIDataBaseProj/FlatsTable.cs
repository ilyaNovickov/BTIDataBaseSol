//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTIDataBaseProj
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class FlatsTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlatsTable()
        {
            this.RoomsTable = new ObservableCollection<RoomsTable>();
        }
    
        public int FlatId { get; set; }
        public int Flat { get; set; }
        public int Storey { get; set; }
        public int Rooms { get; set; }
        public bool Level { get; set; }
        public int SquareFlat { get; set; }
        public int Dwell { get; set; }
        public int Branch { get; set; }
        public int Balcony { get; set; }
        public int Height { get; set; }
        public string BuildingKadastr { get; set; }
    
        public virtual BuildingsTable BuildingsTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<RoomsTable> RoomsTable { get; set; }
    }
}
