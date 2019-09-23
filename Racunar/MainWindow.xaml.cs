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

            // za deselektovanje operatora (operacije)
            Operatori SelektovanOperater = Operatori.Ne_Selektovan;

        // proverava da li je upisano nesto posle selektovanja operatora
        bool IsOcupied = true,

        // proverava da li firstnum ima upisanu vrednost
        CheckForFirstNum = true,

        // proverava da li je vec pritisnut taster jednako
        afterJednko = true,

        //proverava da li je tacka stavljena
        afterDot = false,

        operationPerformed = false,
        operationPerformed1 = false;
            
            // za uzimanje vrednosti iz sendera 
            Button NumbersButtons;

        public MainWindow()
        {
            
            InitializeComponent();
        }
        // event za brojeve

        private void Numbers_Click (object sender , RoutedEventArgs e)
        {
            //posle pritiska tastera jednako  i ako nije pritisnut zarez stavlja sve na pocetne vrednosti
            // deselektuje operaciju
            if (operationPerformed) { }
             else if (afterJednko && !afterDot)
                {
                    PocetneVrednostiZaOperatore();
                    SelektovanOperater = Operatori.Ne_Selektovan;
                }
            // 
            afterDot = false;
            afterJednko = false;

            // setuje font labela u odnostu na broj cifara
            SetLabelFontSize(ResultLabel);

            //setuje da je nesto upisano za prvi br
            CheckForFirstNum = true;

            // posle greske i kucanja br enejbluje dugmice ponovo
            if (Plus.IsEnabled == false)
            {
                OmoguciDugmicePoslegreske(); 
            }

            // cekuje da li je bilo greske i ako je bilo vraca font... na pocetne vrednosti
            if (ResultLabel.Foreground == Brushes.Red)
            {
               PocetneVrednostiZaOperatore();
            }

            // ukoliko je nesto upisano brise reultLabel za kucanje drugog broja
            if (IsOcupied)
            {
                ResultLabel.Content = "";
            }
            IsOcupied = false;

            // kastujemo sender objekat u dugme 
            NumbersButtons = (Button)sender;

            //ako je nula na ekranu brisemo je i upisujemo br
            if (ResultLabel.Content.ToString() == "0")
            {
                ResultLabel.Content = $"{NumbersButtons.Content}";
            }
            //ako nije dodajemo br na postojeci
            else if(ResultLabel.Content.ToString().Length<=15)
            {
                ResultLabel.Content = $"{ResultLabel.Content}{NumbersButtons.Content}";
            }
        }
        // ac vraca na pocetne vrednosti
        private void AcButton_Click(object sender, RoutedEventArgs e)
        {
            OmoguciDugmicePoslegreske();
            PocetneVrednostiZaOperatore();
            ResultLabel.Content = "0";
            operationPerformed = false;
            operationPerformed1 = false;

        }
        // mennja znak mnozenjem sa -1
        private void PlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultLabel.Content.ToString(),out firstNum))
            {
                ResultLabel.Content = firstNum * (-1);
            }
        }

        private void Tacka_Click(object sender, RoutedEventArgs e)
        {
            //proverava da li je vec zarez u labelu
            if (!afterJednko)
            {
                if (!ResultLabel.Content.ToString().Contains(","))
                {
                    ResultLabel.Content = $"{ResultLabel.Content},";
                }
            }
            //ako je pritisnu samo zarez posle jednako dodaje nulu ispred zareza
            else
            {
                PocetneVrednostiZaOperatore();
                if (!ResultLabel.Content.ToString().Contains(","))
                {
                    ResultLabel.Content = 0;
                    ResultLabel.Content = $"{ResultLabel.Content},";
                    afterDot = true;
                }
            }
        }
        // nisam zavrsio
        private void Procenat_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultLabel.Content.ToString(), out secnum))
            {
                secnum = secnum / 100;
                ResultLabel.Content = secnum;
            }
            
        }

        private void Operacije_Click(object sender,RoutedEventArgs e)
        {
            afterJednko = false;
            CheckForFirstNum = true;
            if (operationPerformed)
            {
                Jednako_Click(sender, e);

            }

            //puni firstnum sa vrednoscu
            if (double.TryParse(ResultLabel.Content.ToString(), out firstNum))
              //setuje operaciju i ispisuje prvi broj u gornji label
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
            operationPerformed = true;
            
        }

        

        private void Jednako_Click(object sender, RoutedEventArgs e)
        {
            //proverava da li je prvi br im vrednost ako ima puni secnum
            if (CheckForFirstNum && firstNum != 0)
            {
                double.TryParse(ResultLabel.Content.ToString(), out secnum);
            }
            else if (firstNum == 0 && SelektovanOperater != Operatori.Ne_Selektovan )
            {
                double.TryParse(ResultLabel.Content.ToString(), out secnum);
            }

            //deljenje sa nulom, ima exeption ali nisam ga upotrebio
            // izmenicu to kasnije
            if ((SelektovanOperater == Operatori.Deljenje && secnum == 0))
            {
                OnemoguciDugmicePoslegreske();
                PocetneVrednostiZaOperatore();
                ResultLabel.Foreground = Brushes.Red;
                ResultLabel.FontSize = 6;
                ResultLabel.Content = "ERROR DEVIDE BY ZERO NOT DEFINED  ";
            }
            
            //proverava sta je odabrano od operacije i izvrsava ispisuje resultat
            //imao sam problem sa countom i length  pa sam rezultat ubacio u double pa u label content
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
            //ako result nije nulla, nije selektovan operator result je 0
            if (result != 0 && !ResultLabel.Content.ToString().Contains("ERROR"))
            {
                ResultLabel.Content = result.ToString("R");
            }
            
            
            SetLabelFontSize(ResultLabel);
            
            
            
            afterJednko = true;
            //out of range result , ne znam da li ima exeption bas za ovaj slucaj moram da proverim 
            if (double.TryParse(ResultLabel.Content.ToString(), out infinity))
            {
                if (double.IsPositiveInfinity(infinity) || double.IsNegativeInfinity(infinity))
                {
                    OutOfRange(infinity, ResultLabel);

                }
            }



            OperatorLabel.Content = "";
            FirstNUmLabel.Content = "";
            //nakon jednako ako secnum nije nula puni first num vrednoscu ispisanom na ekranu 
            if (secnum != 0)
            {
                firstNum = double.Parse(ResultLabel.Content.ToString());
            }

            CheckForFirstNum = false;
            operationPerformed = false;
           

        }


        public enum Operatori
        {
            Sabiranje,
            oduzimanje,
            Deljenje,
            Mnozenje,
            Ne_Selektovan
        }
        // pomeranje windowsa
        private void MoveWindowsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        
        //close
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            
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
                if (label.Content.ToString().Count() >= 17 && label.Content.ToString().Count() <= 20)
                {
                    label.FontSize = 10;
                }
                if (label.Content.ToString().Count() > 20 && label.Content.ToString().Contains("E"))
                {
                    label.FontSize = 9;
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

        /*public void HundredSeparator(Label label)
        {
            if (!label.Content.ToString().Contains("ERROR"))
            {
                double result = Convert.ToDouble(label.Content);
                label.Content = result.ToString("R");
            }
        }*/
    }

       

       
       
}
