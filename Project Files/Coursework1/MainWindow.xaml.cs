using System.Windows;
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
            
            //Send message button handler
            sendBtn.Click += (sender, args) =>
            {
                if (!headerTxtBox.Text.Equals(new char[] { ' ' }))
                {
                    if (!messasgeTxtBox.Text.Equals(new char[] { ' ' }))
                    {
                        try
                        {
                            pd.DetectMessageType(headerTxtBox.Text, messasgeTxtBox.Text, false);
                        }
                        catch
                        {
                            MessageBox.Show("Invalid message format. Please try again.", "Wrong Message Formatting");
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
}
}
