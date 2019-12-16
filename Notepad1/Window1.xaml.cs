using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notepad1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            //string curItem = listBox1.SelectedItem.ToString();
            //string resultString = Regex.Match(curItem, @"\d+").Value;
            //int font1 = Int32.Parse(resultString);
            //MainWindow mywindow = new MainWindow();
            //mywindow.textBox1.FontSize = font1;
            DialogResult = true;
            Close();


        }
    }
}
