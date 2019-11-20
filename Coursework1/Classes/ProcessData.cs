using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace Coursework1.Classes
{
    class ProcessData
    {

        //Use Header to detect message type and call the appropriate sanitize method
        public void DetectMessageType(String header, String message)
        {
            Char type = header.Trim()[0];
            //Console.WriteLine(type + ", MESSAGE: " + message);

            switch(type.ToString().ToUpper().ToCharArray()[0]) {

                case 'S':
                    SanitizeSMS(header, message);
                    break;

                case 'E':
                    SanitizeEmail(header, message);
                    break;

                case 'T':
                    SanitizeTweet(header, message);
                    break;

                default:
                    break;
            }
        }

        //Sanitise SMS message
        public void SanitizeSMS(String header, String message)
        {
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = split[0].Trim();
            String body = split[1].Trim();

            //Console.WriteLine(header + "\n" + sender + "\n" + body);

            body = ExpandAbbreviations(body);
            MessageSMS sms = new MessageSMS("SMS", header, sender, body);
            WriteJsonToFile(sms);
        }

        //Sanitise E-Mail message
        public void SanitizeEmail(String header, String message)
        {
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = split[0].Trim();
            String subject = split[1].Trim().Substring(0,20);
            String body = split[1].Trim().Substring(21, split[1].Trim().Length - 21);

            //Console.WriteLine(header + "\n" + sender + "\n" + subject + "\n" + body);

            FindSIR(subject, body);
            body = QuarantineURLs(sender, body);

            MessageEmail email = new MessageEmail("Email", header, sender, subject, body);
            WriteJsonToFile(email);
        }

        //Sanitise Tweet message
        public void SanitizeTweet(String header, String message)
        {
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = split[0].Trim();
            String body = split[1].Trim();

            //Console.WriteLine(header + "\n" + sender + "\n" + body);

            MessageTweet tweet = new MessageTweet("Tweet", header, sender, body);
            FindHashtags(body);
            FindTwitterIDs(body);
            WriteJsonToFile(tweet);
        }

        //Find hashtags
        public void FindHashtags(String message)
        {
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(message);
            bool hashExists;

            foreach (var match in matches)
            {
                hashExists = false;
                Hashtag h = new Hashtag(match.ToString(), 1);
                foreach (Hashtag hash in Global._trends)
                {
                    if (hash.tag.Trim().Equals(h.tag.Trim()))
                    {
                        hash.count++;
                        hashExists = true;
                        break;
                    }
                }

                if (!hashExists)
                {
                    Global._trends.Add(h);
                }
            }

            /*Console.WriteLine("Hashtag founds:");
            foreach (Hashtag h in Global._trends)
            {
                Console.WriteLine(h.tag + ", " + h.count);
            }*/
        }

        //Find Twitter IDs
        public void FindTwitterIDs(String message)
        {
            var regex = new Regex(@"@\w+");
            var matches = regex.Matches(message);
            bool idExists;

            foreach (var match in matches)
            {
                idExists = false;
                TwitterID t = new TwitterID(match.ToString(), 1);
                foreach (TwitterID tweet in Global._mentions)
                {
                    if (tweet.id.Trim().Equals(t.id.Trim()))
                    {
                        tweet.count++;
                        idExists = true;
                        break;
                    }
                }

                if (!idExists)
                {
                    Global._mentions.Add(t);
                }
            }

            /*Console.WriteLine("Twitter IDs found:");
            foreach (TwitterID t in Global._mentions)
            {
                Console.WriteLine(t.id + ", " + t.count);
            }*/
        }

        //Find SIR (Significant Incident Report)
        public void FindSIR(String subject, String message)
        {
            String[] incidents = { "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };

            if (subject.Contains("SIR"))
            {
                string[] split = subject.Split(new char[] { ' ' });
                string date = split[1];
                string centrecode = message.Trim().Substring(19, 10);
                string incident = "";
                foreach (String s in incidents)
                {
                    if (message.Contains(s))
                    {
                        incident = s;
                    }
                }
                Global._SIR.Add(new SIR(date, centrecode, incident));
            }
            

            /*Console.WriteLine("Significant Incident Reports:");
            foreach (SIR s in Global._SIR)
            {
                Console.WriteLine(s.Date + ", " + s.CentreCode + ", " + s.Incident);
            }*/
        }

        //Quarantine URLs
        public String QuarantineURLs(string sender, string message)
        {
            var regex = new Regex(@"(((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:\/~\+#]*[\w\-\@?^=%&amp;\/~\+#])?)");
            var matches = regex.Matches(message);
            string newmessage = "";

            foreach (var match in matches)
            {
                String date = DateTime.Now.ToLongDateString();
                String url = match.ToString();
                URL u = new URL(date, sender, url);
                Global._URLQuarantine.Add(u);
                newmessage = message.Replace(match.ToString(), "<URL Quarantined>");
                message = newmessage;
            }

            /*Console.WriteLine("URLs Quarantined:");
            foreach(URL u in Global._URLQuarantine)
            {
                Console.WriteLine(u.Date + ", " + u.Sender + ", " + u.Url);
            }*/

            return newmessage;
        }

        //Expand textspeak abbreviations
        public String ExpandAbbreviations(String message)
        {
            using (var reader = new StreamReader(@"textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new char[] { ',' }, 2);
                    if (message.Contains(values[0].Trim()) || message.Contains(values[0].Trim().ToLower()))
                    {
                        message = message.Replace(values[0], values[0] + " <" + values[1] + ">");
                    }
                }
            }
            return message;
        }

        //Update GUI
        public void UpdateGUI()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    //(window as MainWindow).sirList.Items.Clear();
                    (window as MainWindow).mentionsList.Items.Clear();
                    (window as MainWindow).trendsList.Items.Clear();
                    //(window as MainWindow).urlList.Items.Clear();
                    (window as MainWindow).dataGrid1.DataContext = null;
                    (window as MainWindow).dataGrid2.DataContext = null;

                    (window as MainWindow).dataGrid1.DataContext = Global._SIR;
                    (window as MainWindow).dataGrid2.DataContext = Global._URLQuarantine;

                    /*foreach (SIR s in Global._SIR)
                    {
                        (window as MainWindow).sirList.Items.Add(s.Date + "\t" + s.CentreCode + "\t" + s.Incident);
                    }*/
                    foreach (TwitterID tid in Global._mentions)
                    {
                        (window as MainWindow).mentionsList.Items.Add(tid.id);
                    }
                    foreach (Hashtag h in Global._trends)
                    {
                        (window as MainWindow).trendsList.Items.Add(h.tag + "\t" + h.count);
                    }
                    /*foreach (URL u in Global._URLQuarantine)
                    {
                        (window as MainWindow).urlList.Items.Add(u.sender + "\t" + u.url);
                    }*/
                    
                }
            }
        }

        //Clear textboxes
        public void ClearTextBoxes()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).headerTxtBox.Clear();
                    (window as MainWindow).messasgeTxtBox.Clear();
                }
            }
        }

        //open file dialog to select a csv file
        public void OpenTextFile()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        Title = "Browse Text Files",

                        CheckFileExists = true,
                        CheckPathExists = true,

                        DefaultExt = "txt",
                        Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv",
                        FilterIndex = 2,
                        RestoreDirectory = true,

                        ReadOnlyChecked = true,
                        ShowReadOnly = true
                    };

                    if (openFileDialog1.ShowDialog() == true)
                    {
                        ReadCSV(openFileDialog1.FileName);
                    }
                
                }
            }
        }

        //read CSV file
        public void ReadCSV(string path)
        {
            using (var reader = new StreamReader(@path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new char[] { ',' }, 2);
                    try
                    {
                        DetectMessageType(values[0].ToString().Trim(), values[1].ToString().Trim());
                    }
                    catch
                    {
                        MessageBox.Show("There was an error reading:\n"+ path +"\n\nTry again with a valid csv file.", "Error Reading File");
                        break;
                    }
                }
            }
            UpdateGUI();
        }

        //Write message in JSON format in a JSON file
        public void WriteJsonToFile(Object message)
        {
            string output = JsonConvert.SerializeObject(message);
            string jsonFile = "messages.json";
            StreamWriter file;

            if (File.Exists(jsonFile))
            {
                //File.WriteAllText(jsonFile, String.Empty);
                string text = File.ReadAllText(jsonFile);
                text = text.Replace(']', ',');
                //Console.WriteLine(text);
                using (file = new StreamWriter(jsonFile, false))
                {
                    file.Write(text + output + "]");
                }
            }
            else
            {
                using (file = new StreamWriter(jsonFile, true))
                {
                    file.Write("[" + output + "]");
                }
            }
        }

    }
}
