﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BTIDataBaseProj.Helpers
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource buildingsTableViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // buildingsTableViewSource.Source = [универсальный источник данных]
            System.Windows.Data.CollectionViewSource buildingsTableViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource1")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // buildingsTableViewSource1.Source = [универсальный источник данных]
            System.Windows.Data.CollectionViewSource buildingsTableViewSource2 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource2")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // buildingsTableViewSource2.Source = [универсальный источник данных]
            System.Windows.Data.CollectionViewSource buildingsTableViewSource3 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("buildingsTableViewSource3")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // buildingsTableViewSource3.Source = [универсальный источник данных]
        }
    }
}
