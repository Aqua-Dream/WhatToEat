using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random;
        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            ReadConfig();
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            FoodPanel.Children.Clear();
            ResultLabel.Content = "未知";
            ResultLabel.Foreground = Brushes.Red;
            ReadConfig();
        }
        private void Button_Roll(object sender, RoutedEventArgs e)
        {
            var names = new List<string>();
            var weights = new List<int>();
            var n = 0;
            foreach(option food in FoodPanel.Children)
            {
                if(food.Checked)
                {
                    names.Add(food.FoodName);
                    weights.Add(food.Weight);
                    n++;
                }
            }
            if(n == 0)
            {
                MessageBox.Show("没有想吃的吗？");
                return;
            }
            int sum = weights.Sum();
            if(sum < 0)
            {
                MessageBox.Show("你是怎么做到的？");
                return;
            }
            int r = random.Next(1, sum);
            sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += weights[i];
                if (r <= sum)
                {
                    if ((string)ResultLabel.Content == names[i] && ResultLabel.Foreground == Brushes.Blue)
                        ResultLabel.Foreground = Brushes.Green;
                    else
                    {
                        ResultLabel.Content = names[i];
                        ResultLabel.Foreground = Brushes.Blue;
                    }
                    return;
                }
            }
            MessageBox.Show("你是怎么做到的？");
        }
        private void ReadConfig()
        {
            var line_count = 0;
            var err_lines = new string[5];
            var p = 0;
            StreamReader food_file = null;
            try 
            {
                food_file = File.OpenText("food.txt");
                while (!food_file.EndOfStream)
                {
                    line_count++;
                    string s = food_file.ReadLine();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        var arr = s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        int weight = 1;
                        if (arr.Length != 3 || !int.TryParse(arr[2], out weight) || (arr[1] != "y" && arr[1] != "n") || weight <= 0 || weight > 999)
                        {
                            if (p < 5)
                                err_lines[p++] = line_count.ToString();
                        }
                        else
                        {
                            var food = new option() { Weight = weight, Checked = arr[1] == "y", FoodName = arr[0] };
                            FoodPanel.Children.Add(food);
                        }
                    }
                }
                if (p > 0)
                    MessageBox.Show("配置文件第" + string.Join(",", err_lines.Where(s=>!string.IsNullOrEmpty(s))) + "行有错误！");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (food_file != null)
                    food_file.Dispose();
            }
        }


    }
}
