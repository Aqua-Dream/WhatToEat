using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for option.xaml
    /// </summary>
    /// 
    
    public partial class option : UserControl, INotifyPropertyChanged
    {
        private int m_weight;
        private bool m_checked;
        private string m_name;
        public int Weight
        {
            get { return m_weight; }
            set
            {
                if (m_weight == value)
                    return;
                m_weight = value;
                OnPropertyChanged("Weight");
            }
        }
        public bool Checked
        {
            get { return m_checked; }
            set
            {
                if (m_checked == value)
                    return;
                m_checked = value;
                OnPropertyChanged("Checked");
            }
        }
        public string FoodName
        {
            get { return m_name; }
            set
            {
                if (m_name == value)
                    return;
                m_name = value;
                OnPropertyChanged("Name");
            }
        }
        public option()
        {
            InitializeComponent();
            foodPanel.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class WeightConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (int.TryParse(strValue, out int weight) && weight > 0 && weight < 1000)
                return weight;
            return 1;
        }
    }


}
