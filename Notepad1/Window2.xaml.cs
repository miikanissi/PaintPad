using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        Double resultValue = 0.0;
        double a = 0.0;
        double b = 0.0;
        String operationPerformed = "";
        bool isOperationPerformed = false;

        public Window2()
        {
            InitializeComponent();
        }

        private void button_click(object sender, RoutedEventArgs e)
        {
            if ((textBox_Result.Text == "0") || (isOperationPerformed))
            {
                textBox_Result.Clear();
            }

            isOperationPerformed = false;
            Button button = (Button)sender;
            textBox_Result.Text = textBox_Result.Text + button.Content;
        }

        private void operator_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            operationPerformed = "" + button.Content;
            resultValue = Double.Parse(textBox_Result.Text);
            labelCurrentOperation.Content = resultValue + " " + operationPerformed;
            isOperationPerformed = true;

        }


        private void cbutton_click(object sender, RoutedEventArgs e)
        {
            textBox_Result.Text = "0";
            resultValue = 0;
            labelCurrentOperation.Content = "";
        }


        private void value_click(object sender, RoutedEventArgs e)
        {

            switch (operationPerformed)
            {
                case "+":
                    textBox_Result.Text = (resultValue + Double.Parse(textBox_Result.Text)).ToString();
                    break;

                case "-":
                    textBox_Result.Text = (resultValue - Double.Parse(textBox_Result.Text)).ToString();
                    break;
                case "*":
                    textBox_Result.Text = (resultValue * Double.Parse(textBox_Result.Text)).ToString();
                    break;
                case "/":
                    textBox_Result.Text = (resultValue / Double.Parse(textBox_Result.Text)).ToString();
                    break;
                default:
                    break;
            }
        }


    
}
}
