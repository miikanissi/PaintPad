using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Notepad1
{
    /// Notepad and Paint app in one. Also includes a pop up window for a calculator and a language switch.
    
    public partial class MainWindow : Window
    {
        public string LangSwitch { get; private set; } = null;
        public string strokeColor = "gray";
        public string filePath;

        public int strokeSize = 2;
        public int caseSwitch = 2;
        public bool isHighlighted = false;
        public bool isEllipse = false;

        Point currentPoint = new Point();
        public Ellipse elip = new Ellipse();
         
        public MainWindow()
        {
            // This was replaced with a language switcher in LocApp.cs
            // Set the current user interface culture to the specific culture Swedish
            // System.Threading.Thread.CurrentThread.CurrentUICulture =
            //            new System.Globalization.CultureInfo("en");

            InitializeComponent();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + S shortcut for saving
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveFile();
            }
            // Ctrl + Z shortcut to undo last text box edit
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                // Determine if last operation can be undone in text box  
                if (textBox1.CanUndo == true)
                {
                    textBox1.Undo();
                }
            }
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void menuPrint_Click(object sender, RoutedEventArgs e)
        {
            // Lazy printing method. Prints the entire grid. 
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(grid, textBox1.Text);
            }

        }

        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileAs();
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();

        }

        private void menuUndo_Click(object sender, RoutedEventArgs e)
        {
            // Determine if last operation can be undone in text box.   
            if (textBox1.CanUndo == true)
            {
                textBox1.Undo();
            }
        }

        private void menuCopy_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.SelectionLength > 0)

                textBox1.Copy();
        }

        private void menuPaste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                textBox1.Paste();
            }
        }

        private void menuCut_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.SelectedText != "")
                textBox1.Cut();
        }

        private void menuFont_Click(object sender, RoutedEventArgs e)
        {
            // Opens a new window for Font changing. Uses regex to get the font number from a string into int. 
            Window1 mywindow = new Window1();

            if (mywindow.ShowDialog() == true)
            {
                if (mywindow.listBox1.SelectedItem != null)
                {
                    string curItem = mywindow.listBox1.SelectedItem.ToString();
                    string resultString = Regex.Match(curItem, @"\d+").Value;
                    int font1 = Int32.Parse(resultString);
                    //MessageBox.Show(resultString);
                    textBox1.FontSize = font1;
                }
            }
        }
        
        private void Canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Drawing on canvas. Checks if Ellipse is checked on and creates an ellipse. If not then only gets the mouse position. 
            if (!isEllipse)
            {
               if (e.ButtonState == MouseButtonState.Pressed)
                    currentPoint = e.GetPosition(Surface);
            }
            else
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    currentPoint = e.GetPosition(Surface);

                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(strokeColor);

                elip = new Ellipse
                {
                    
                    Stroke = brush,
                    StrokeThickness = strokeSize
                };
                Surface.Children.Add(elip);
            }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            TogglePen();                                                      
        }
        void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            HighlightMenu(sender);
            TogglePen();
        }
        

        private void brushSize_Click(object sender, RoutedEventArgs e)
        {
            // switch case to rotate between three brush sizes. Default is case 1. 
            switch (caseSwitch)
            {
                case 1:

                    brushSize.Icon = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri("/smaller.png", UriKind.Relative))
                    };
                    strokeSize = 2;
                    caseSwitch += 1;
                    break;

                case 2:

                    brushSize.Icon = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri("/small.png", UriKind.Relative))
                    };
                    strokeSize = 6;
                    caseSwitch += 1;
                    break;

                case 3:

                    brushSize.Icon = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri("/big.png", UriKind.Relative))
                    };
                    strokeSize = 12;
                    caseSwitch = 1;
                    break;
            }
            // Old logic to rotate between 2 brush sizes. 
            /*
            if (!isBigBrush)
            {
                brushSize.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("/big.png", UriKind.Relative))
                };
                strokeSize = 6;
                isBigBrush = true;
            }
            else
            {
                brushSize.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("/small.png", UriKind.Relative))
                };
                strokeSize = 2;
                isBigBrush = false;
            }*/
        }

        private void colorBlack_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "black";
        }

        private void colorGrey_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "gray";
        }

        private void colorWhite_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "white";
        }

        private void colorRed_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "red";
        }

        private void colorBlue_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "blue";
        }

        private void colorYellow_Click(object sender, RoutedEventArgs e)
        {
            ColorHighlight(sender);
            strokeColor = "yellow";
        }

        private void ellipse1_Click(object sender, RoutedEventArgs e)
        {
            HighlightShape(sender);
        }

        private void menuCalc_Click(object sender, RoutedEventArgs e)
        {
            // Opens the calculator 
            Window2 mywindow = new Window2();

            mywindow.ShowDialog();
        }

        private void englishCulture_Click(object sender, RoutedEventArgs e)
        {
            LangSwitch = "en";
            Close();
        }

        private void finnishCulture_Click(object sender, RoutedEventArgs e)
        {
            LangSwitch = "fi-FI";
            Close();
        }

        private void swedishCulture_Click(object sender, RoutedEventArgs e)
        {
            LangSwitch = "sv-SE";
            Close();
        }
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!isEllipse)
            {
                // If Ellipse is not on then simply draws a line on canvas where the users mouse is. 
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();

                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString(strokeColor);

                    line.Stroke = brush;
                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = e.GetPosition(Surface).X;
                    line.Y2 = e.GetPosition(Surface).Y;
                    line.StrokeThickness = strokeSize;
                    currentPoint = e.GetPosition(Surface);

                    Surface.Children.Add(line);
                }
            }
            else
            {
                // If Ellipse is on this draws an ellipse where user is moving his mouse. 
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double minX = Math.Min(e.GetPosition(Surface).X, currentPoint.X);
                    double minY = Math.Min(e.GetPosition(Surface).Y, currentPoint.Y);
                    double maxX = Math.Max(e.GetPosition(Surface).X, currentPoint.X);
                    double maxY = Math.Max(e.GetPosition(Surface).Y, currentPoint.Y);

                    Canvas.SetTop(elip, minY);
                    Canvas.SetLeft(elip, minX);

                    double height = maxY - minY;
                    double width = maxX - minX;

                    elip.Height = Math.Abs(height);
                    elip.Width = Math.Abs(width);
                }
            }
        }


        public void TogglePen()
        {
            // Uses the isHitTestVisible property of canvas to toggle drawing on and off
            // Also changes textbox into read only depending on drawing state. 
            if (Surface.IsHitTestVisible == false)
            {
                Surface.IsHitTestVisible = true;
                textBox1.IsReadOnly = true;
            }
            else
            {
                Surface.IsHitTestVisible = false;
                textBox1.IsReadOnly = false;
            }
        }
        public void SaveFile()
        {
            // Checks if file is already saved in a filepath and either triggers save as function or save function.
            if (!string.IsNullOrEmpty(filePath))
            {
                // Checks using regex if file is a png or something else. for png saves an image. 
                Regex regex = new Regex(@"\.png\s$", RegexOptions.IgnoreCase);
                Match match = regex.Match(filePath);
                if (match.Success)
                {

                    SavePNGImage();
                }
                else
                {
                    SaveTXTFile();
                }
            }


            else
            {
                SaveFileAs();
            }
        }
        public void OpenFile()
        {
            // ofd to open a file. Regex checks if file is a png or text file and treats it accordingly. 
            OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|PNG(*.png)|*.png|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                Regex regex = new Regex(@"\.png\s$", RegexOptions.IgnoreCase);
                Match match = regex.Match(filePath);
                if (match.Success)
                {
                    OpenPNGImage();
                }
                else
                {
                    textBox1.Text = File.ReadAllText(filePath);
                }
            }
        }
        public void OpenPNGImage()
        {
            // To open a png it is simply drawn into the background of canvas
            // Improvements needed here. Also logic needs to be thought better.
            // My thoughts: As you open a png file it would be useful to either be able to type text over it or have text editor disabled when editing a png.
            // Currently Text editor gets hidden since canvas background is drawn a solid image.
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(filePath, UriKind.Relative));
            Surface.Background = brush;
        }
        public void ConvertToBitmapSource(UIElement element)
        {
            // This is for drawing a bitmap image. 
            var target = new RenderTargetBitmap(
                (int)element.RenderSize.Width, (int)element.RenderSize.Height,
                96, 96, PixelFormats.Pbgra32);
            target.Render(element);

            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(target);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite(filePath))
            {
                encoder.Save(file);
            }
        }
        public void SavePNGImage()
        {
            // Converts the entire grid to a png. This includes Canvas and text below it. 
            ConvertToBitmapSource(grid);
        }

        public void SaveFileAs()
        {
            // Opens savefiledialog to do the saving as. If you save as a .png it saves an image.
            // Uses regex to determine .png
            // Perhaps having a Save File as an Image.. & Save File as .. would be better logic but I ended up with just one option to do both. 
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt | PNG (*.png) | *.png | All Files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                if (sfd.FileName != "")
                {
                    filePath = sfd.FileName;
                    Regex regex = new Regex(@"\.png\s?$", RegexOptions.IgnoreCase);
                    Match match = regex.Match(filePath);
                    if (match.Success)
                    {
                        SavePNGImage();
                    }
                    else
                    {
                        SaveTXTFile();
                    }
                }
            }
        }

        public void SaveTXTFile()
        {
            // First used streamwriter but a pre existing method for File is better.
            // This saves empty lines and tabs as well. 
            File.WriteAllText(filePath, textBox1.Text);
            /*StreamWriter writer = File.CreateText(filePath);
            writer.WriteLine(textBox1.Text);
            writer.Close();*/
        }
        public void HighlightShape(object sender)
        {
            // Selects and Highlights Ellipse when selected
            if (!isEllipse)
            {
                ellipse1.Background = null;
                ellipse1.BorderBrush = null;

                MenuItem mi = sender as MenuItem;
                var converter = new System.Windows.Media.BrushConverter();
                mi.Background = (SolidColorBrush)converter.ConvertFromString("#3D26A0DA");
                mi.BorderBrush = (SolidColorBrush)converter.ConvertFromString("#FF26A0DA");

                isEllipse = true;
            }
            else
            {
                ellipse1.Background = null;
                ellipse1.BorderBrush = null;
                isEllipse = false;
            }
        }
        public void HighlightMenu(object sender)
        {
            // Highlights Pen if selected. 
            if (!isHighlighted)
            {
                // clear previously set backgrounds...
                menuPen.Background = null;
                menuPen.BorderBrush = null;

                MenuItem mi = sender as MenuItem;
                var converter = new System.Windows.Media.BrushConverter();
                mi.Background = (SolidColorBrush)converter.ConvertFromString("#3D26A0DA");
                mi.BorderBrush = (SolidColorBrush)converter.ConvertFromString("#FF26A0DA");
                isHighlighted = true;
            }
            else
            {
                // At first I looped to clear all menuitems but I ended up wanting multiple highlights at the same time.
                /*foreach (MenuItem menuItem in menu1.Items.OfType<MenuItem>())
                {
                    menuItem.SetValue(MenuItem.BackgroundProperty, null);
                    menuItem.SetValue(MenuItem.BorderBrushProperty, null);
                }
                */
                menuPen.Background = null;
                menuPen.BorderBrush = null;

                isHighlighted = false;
            }
        }
        
        public void ColorHighlight(object sender)
        {
            // Highlights whichever color is selected. 
                colorBlack.Background = null;
                colorBlack.BorderBrush = null;
                colorGrey.Background = null;
                colorGrey.BorderBrush = null; 
                colorWhite.Background = null;
                colorWhite.BorderBrush = null;
                colorBlue.Background = null;
                colorBlue.BorderBrush = null;
                colorRed.Background = null;
                colorRed.BorderBrush = null;
                colorYellow.Background = null;
                colorYellow.BorderBrush = null;

                MenuItem mi = sender as MenuItem;
                var converter = new System.Windows.Media.BrushConverter();
                mi.Background = (SolidColorBrush)converter.ConvertFromString("#3D26A0DA");
                mi.BorderBrush = (SolidColorBrush)converter.ConvertFromString("#FF26A0DA");

        }
    }
}
