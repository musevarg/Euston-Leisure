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
using Coursework1.Classes;

namespace Coursework1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ProcessData pd = new ProcessData();
            TestSoftware(pd);
            
            //Send message button handler
            sendBtn.Click += (sender, args) =>
            {
                if (!headerTxtBox.Text.Equals(new char[] { ' ' }))
                {
                    if (!messasgeTxtBox.Text.Equals(new char[] { ' ' }))
                    {
                        try
                        {
                            pd.DetectMessageType(headerTxtBox.Text, messasgeTxtBox.Text);
                            pd.UpdateGUI();
                        }
                        catch
                        {
                            MessageBox.Show("Invalid message format. Please try again.", "Wrong Message Formatting");
                            pd.ClearTextBoxes();
                        }
                    }
                }
            };

            //Clear textboxes button handler
            clearBtn.Click += (sender, args) =>
            {
                pd.ClearTextBoxes();
            };

            //Open text file button handler
            txtFileBtn.Click += (sender, args) =>
            {
                pd.OpenTextFile();
            };

        }

        private static void TestSoftware(ProcessData pd)
        {
            pd.DetectMessageType("S1234567", "+447493169621 The message.");
            pd.DetectMessageType("E54214", "test@napier.ac.uk This is a test.      This is the rest of the message. It is cool.");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #TestHastag A real blast!");
            pd.DetectMessageType("Z5555555", "HOOOHOSdfklsdkf");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #TestHastag A real blast!");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #TestHastag  #Youpee A real blast!");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #Youpee A real blast!");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #TestHastag A real blast!");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! @HongKong @Marmiton");
            pd.DetectMessageType("T4587888", "@JeanRochefort This app is so cool! #TestHashtag A real blast!");
            pd.DetectMessageType("T4587888", "@JeanRochefort This @Pingu app is so cool! This is for you @Pingu #TestHashtag A real blast!");
            pd.DetectMessageType("E666", "infocrime@gov.uk SIR 20/10/19             Sport Centre Code: 66-666-66  Nature of incident: Customer Attack  The attack happened in the afternoon after a drunk customer provoked an older woman for pissing in the pediluve. A staff member who www.wirdo.be attempted to help was drowned in the pediluve.");
            pd.DetectMessageType("S1234567", "+447493169621 The message.");
            pd.DetectMessageType("S1234567", "+447493169621 The message.");
            pd.DetectMessageType("S1234567", "+447493169621 The message.");
            pd.DetectMessageType("E666", "money@supermarket.co.uk SIR 25/11/18             Sport Centre Code: 98-987-87  Nature of incident: Theft of Properties  The attack happened in the afternoon after a drunk customer https://www.facebook.com/ provoked an older www.sedhna.com woman for pissing in the pediluve. http://gainalotofmoney.com/ A staff member who attempted to help stole an ice cream and a spoon and ran away with them.");
            pd.UpdateGUI();
        }
}
}
