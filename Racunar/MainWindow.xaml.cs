using System;
using System.Collections.Generic;
using System.Globalization;
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
using Racunar.Classes;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Racunar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private double firstNum,secnum,infinity,result;

        Operatori SelektovanOperater = Operatori.Ne_Selektovan;

        bool IsOcupied = true,check = true,afterjednako = true;

       

        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Numbers_Click (object sender , RoutedEventArgs e)
        {
            if (afterjednako)
            {
                PocetneVrednostiZaOperatore();
                SelektovanOperater = Operatori.Ne_Selektovan;

            }

            afterjednako = false;


            SetLabelFontSize(ResultLabel);
            check = true;

            if (Plus.IsEnabled == false)
            {
                OmoguciDugmicePoslegreske(); 
            }
            if (ResultLabel.Foreground == Brushes.Red)
            {
               PocetneVrednostiZaOperatore();
            }
            int selectedItem = 0;
            if (IsOcupied)
            {
                ResultLabel.Content = "";
            }
            IsOcupied = false;
            if (sender == Jedan) selectedItem = 1;
            if (sender == Dva) selectedItem = 2;
            if (sender == Tri) selectedItem = 3;
            if (sender == Cetri) selectedItem = 4;
            if (sender == Pet) selectedItem = 5;
            if (sender == Sest) selectedItem = 6;
            if (sender == Sedam) selectedItem = 7;
            if (sender == Osam) selectedItem = 8;
            if (sender == Devet) selectedItem = 9;
            if (sender == Nula) selectedItem = 0;

            if (ResultLabel.Content.ToString() == "0")
            {
                ResultLabel.Content = $"{selectedItem}";
            }
            else if(ResultLabel.Content.ToString().Length<=15)
            {
                
                ResultLabel.Content = $"{ResultLabel.Content.ToString() + selectedItem}";
                
            }
        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {
            OmoguciDugmicePoslegreske();
            PocetneVrednostiZaOperatore();
            ResultLabel.Content = "0";

        }

        private void PlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultLabel.Content.ToString(),out firstNum))
            {
                ResultLabel.Content = firstNum * (-1);
            }
        }

        private void Tacka_Click(object sender, RoutedEventArgs e)
        {
            if (!ResultLabel.Content.ToString().Contains(","))
            {
                ResultLabel.Content = $"{ResultLabel.Content},";
            }
        }

        private void Procenat_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(ResultLabel.Content.ToString(),out firstNum))
            {
                ResultLabel.Content = firstNum / 100;
            }
        }

        private void Operacije_Click(object sender,RoutedEventArgs e)
        {
            afterjednako = false;
            check = true;
            if (double.TryParse(ResultLabel.Content.ToString(), out firstNum))
                
             if (sender == Podeljeno)
             {
                 OperatorLabel.Content = "/";
                 SelektovanOperater = Operatori.Deljenje;
                 FirstNUmLabel.Content = firstNum;
                 

             }
            if (sender == Puta)
            {
                OperatorLabel.Content = "*";
                SelektovanOperater = Operatori.Mnozenje;
                FirstNUmLabel.Content = firstNum;
                
            }
            if (sender == Plus)
            {
                OperatorLabel.Content = "+";
                SelektovanOperater = Operatori.Sabiranje;
                FirstNUmLabel.Content = firstNum;
                
            }
            if (sender == Minus)
            {
                OperatorLabel.Content = "-";
                SelektovanOperater = Operatori.oduzimanje;
                FirstNUmLabel.Content = firstNum;
                
            }
            
            IsOcupied = true;
           
            
        }

        private void Jednako_Click(object sender, RoutedEventArgs e)
        {

            if (check && firstNum != 0)
            {
                double.TryParse(ResultLabel.Content.ToString(), out secnum);
            }


            if ((SelektovanOperater == Operatori.Deljenje && secnum == 0))
            {
                OnemoguciDugmicePoslegreske();
                PocetneVrednostiZaOperatore();
                ResultLabel.Foreground = Brushes.Red;
                ResultLabel.FontSize = 6;
                ResultLabel.Content = "ERROR DEVIDE BY ZERO NOT DEFINED  ";
            }
            else if (SelektovanOperater == Operatori.Deljenje ||
                SelektovanOperater == Operatori.Mnozenje||
                SelektovanOperater == Operatori.oduzimanje||
                SelektovanOperater == Operatori.Sabiranje)
                {

                        switch (SelektovanOperater)
                        {
                            case Operatori.Sabiranje:
                                result = Operators.Sabiranje(firstNum, secnum);
                                ResultLabel.Content = result;
                                break;
                            case Operatori.oduzimanje:
                                result = Operators.Oduzimanje(firstNum, secnum);
                                ResultLabel.Content =
                                     result;

                                break;
                            case Operatori.Deljenje:
                                result = Operators.Deljenje(firstNum, secnum);
                                ResultLabel.Content =
                                result;
                                DeljenjeRezultujeNulu(result, ResultLabel);
                                break;
                            case Operatori.Mnozenje:
                                result = Operators.Mnozenje(firstNum, secnum);
                                ResultLabel.Content =
                                result;
                                break;
                            
                            default:
                                break;
                        }

                
            }
            if (result != 0)
            {
                ResultLabel.Content = result.ToString();
            }
            
            
            SetLabelFontSize(ResultLabel);
            
            
            
            afterjednako = true;

            if (double.TryParse(ResultLabel.Content.ToString(), out infinity))
            {
                if (double.IsPositiveInfinity(infinity) || double.IsNegativeInfinity(infinity))
                {
                    OutOfRange(infinity, ResultLabel);

                }
            }



            OperatorLabel.Content = "";
            FirstNUmLabel.Content = "";
            if (secnum != 0)
            {
                firstNum = double.Parse(ResultLabel.Content.ToString());
            }

            check = false;
           
            
        }


        public enum Operatori
        {
            Sabiranje,
            oduzimanje,
            Deljenje,
            Mnozenje,
            Ne_Selektovan
        }

        private void MoveWindowsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

       

        public void PocetneVrednostiZaOperatore()
        {
            OperatorLabel.Content = "";
            FirstNUmLabel.Content = "";
            ResultLabel.Content = "";
            ResultLabel.Foreground = Brushes.Black;
            ResultLabel.FontSize = 15;
            firstNum = 0;
            secnum = 0;

            

        }


        public void OutOfRange(double infinity, Label label)
        {
            if (double.IsInfinity(infinity))
            {
                OnemoguciDugmicePoslegreske();
                PocetneVrednostiZaOperatore();
                label.Foreground = Brushes.Red;
                label.FontSize = 6;
                label.Content = "ERROR OUT OF RANGE";
            }
            
        }

        public void OnemoguciDugmicePoslegreske()
        {
            plusMinus.IsEnabled = false;
            Procenat.IsEnabled = false;
            Podeljeno.IsEnabled = false;
            Puta.IsEnabled = false;
            Minus.IsEnabled = false;
            Plus.IsEnabled = false;
            Jednako.IsEnabled = false;
            Tacka.IsEnabled = false;

        }

        public void OmoguciDugmicePoslegreske()
        {
            plusMinus.IsEnabled = true;
            Procenat.IsEnabled = true;
            Podeljeno.IsEnabled = true;
            Puta.IsEnabled = true;
            Minus.IsEnabled = true;
            Plus.IsEnabled = true;
            Jednako.IsEnabled = true;
            Tacka.IsEnabled = true;

        }

        public void SetLabelFontSize(Label label)
        {

            if (!ResultLabel.Content.ToString().Contains("ERROR"))
            {
                if (label.Content.ToString().Count() >= 17 && label.Content.ToString().Count() <= 19)
                {
                    label.FontSize = 10;
                }
                if (label.Content.ToString().Count() > 21 && label.Content.ToString().Contains("E"))
                {
                    label.FontSize = 8;
                }

                if (label.Content.ToString().Count() > 13 && label.Content.ToString().Count() < 17)
                {
                    label.FontSize = 11;

                }
            }
            
            
        }

        public void DeljenjeRezultujeNulu(double res, Label label)
        {
            if (res == 0)
            {
                OnemoguciDugmicePoslegreske();
                PocetneVrednostiZaOperatore();
                label.Foreground = Brushes.Red;
                label.FontSize = 6;
                label.Content = "ERROR OUT OF RANGE";
            }
        }

      /*  public void HundredSeparator(Label label)
        {


            if (!label.Content.ToString().Contains(",")&& !label.Content.ToString().Contains("ERROR"))
            {
                double result = Convert.ToDouble(label.Content);
                label.Content = result.ToString("N0");
            }
            
                
            
        }*/
    }

       

       
       
}
