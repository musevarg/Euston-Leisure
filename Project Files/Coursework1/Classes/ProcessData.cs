using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Windows;

namespace Coursework1.Classes
{
    public class ProcessData
    {
        private List<Hashtag> TrendsList = new List<Hashtag>();
        private List<Mention> MentionsList = new List<Mention>();
        private List<SIR> SIRList = new List<SIR>();
        private List<URL> URLQuarantine = new List<URL>();
        private int MessagesReadFromFile = 0;

        //Use Header to detect message type and call the appropriate validation and sanitize method
        public void DetectMessageType(String header, String message, bool fromTextFile)
        {
            if (Regex.IsMatch(header, @".\d{9}"))
            {
                Char type = header.Trim()[0];
                if(type.Equals('S') || type.Equals('E') || type.Equals('T'))
                {
                    switch (type.ToString().ToUpper().ToCharArray()[0])
                    {

                        case 'S':
                            if (ValidSMS(message))
                            {
                                WriteJsonToFile(SanitizeSMS(header, message));
                                if (fromTextFile)
                                    MessagesReadFromFile++;
                            }
                            else
                            {
                                if (!fromTextFile)
                                {
                                    MessageBox.Show("Invalid text message.\nThe phone number must in the form of an international phone number (+440142322121) and the message must be no longer than 140 characters.", "Invalid SMS");
                                }
                            }
                            break;

                        case 'E':
                            if (ValidEmail(message))
                            {
                                WriteJsonToFile(SanitizeEmail(header, message));
                                if (fromTextFile)
                                    MessagesReadFromFile++;
                            }
                            else
                            {
                                if (!fromTextFile)
                                {
                                    MessageBox.Show("Invalid email message.\nThe message must be sent from a valid email address and the message must be no longer than 1028 characters.", "Invalid Email");
                                }
                            }
                            break;

                        case 'T':
                            if (ValidTweet(message))
                            {
                                WriteJsonToFile(SanitizeTweet(header, message));
                                if (fromTextFile)
                                    MessagesReadFromFile++;
                            }
                            else
                            {
                                if (!fromTextFile)
                                {
                                    MessageBox.Show("Invalid Twitter message.\nUser ID must be in the form @ID and no longer than 15 characters and message must be no longer than 140 characters.", "Invalid Tweet");
                                }
                            }

                            break;

                        default:
                            if (!fromTextFile)
                            {
                                MessageBox.Show("The header provided is not valid. Please try again.", "Invalid Message Header");
                            }
                            break;
                    }
                    UpdateGUI();
                }
                else
                {
                    if (!fromTextFile)
                    {
                        MessageBox.Show("The header provided is not valid. Please try again.", "Invalid Message Header");
                    }
                }
 
            }
            else
            {
                if (!fromTextFile)
                {
                    MessageBox.Show("The header provided is not valid. Please try again.", "Invalid Message Header");
                }
            }
        }

        //Check whether SMS message is valid (valid international phone number and length smaller or equal to 140 characters)
        public bool ValidSMS(string message)
        {
            string[] split = message.Split(new char[] { ' ' }, 2);

            if (Regex.Match(split[0].Trim(), @"\+\d{1,}").Success)
            {
                if (split[1].Trim().Length > 140)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            } 
        }

        //Sanitise SMS message
        public Message SanitizeSMS(String header, String message)
        {
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = split[0].Trim();
            String body = split[1].Trim();

            body = ExpandAbbreviations(body);

            Message sms = new Message()
            {
                Type = "SMS",
                ID = header,
                Sender = sender,
                Body = body
            };

            return sms;
        }

        //Check whether Email message contains a valid email address and that the message body is smaller or equal to 1028 characters
        public bool ValidEmail(string message)
        {
            Match email = Regex.Match(message, @"((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])");
            if (email.Success)
            {
                int index = email.Index + email.Length + 20;
                if (message.Substring(index, message.Length - index).Length > 1028)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        //Sanitise E-Mail message
        public Message SanitizeEmail(String header, String message)
        {
            Match address = Regex.Match(message, @"((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])");
            int index = address.Index + address.Length + 21;
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = address.Value;
            String subject = message.Substring((address.Index + address.Length),21).Trim();
            String body = message.Substring(index, message.Length - index).Trim();

            FindSIR(subject, body);
            body = QuarantineURLs(sender, body);

            Message email = new Message()
            {
                Type = "Email",
                ID = header,
                Sender = sender,
                Subject = subject,
                Body = body
            };
            return email;            
        }

        //Check whether Twitter message contains a valid twitter ID and that message length is smaller or equal to 140 characters
        public bool ValidTweet(string message)
        {
            string[] split = message.Split(new char[] { ' ' }, 2);
            if(Regex.IsMatch(split[0].Trim(), @"@\w+"))
            {
                if (split[0].Trim().Length > 15 || split[1].Trim().Length > 140)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        //Sanitise Twitter message
        public Message SanitizeTweet(String header, String message)
        {
            String[] split = message.Split(new char[] { ' ' }, 2);
            String sender = split[0].Trim();
            String body = split[1].Trim();

            FindHashtags(body);
            FindTwitterIDs(body);
            body = ExpandAbbreviations(body);

            Message tweet = new Message()
            {
                Type = "Tweet",
                ID = header,
                Sender = sender,
                Body = body
            };

            return tweet;
        }

        //Find hashtags in twitter messages
        private void FindHashtags(String message)
        {
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(message);
            bool hashExists;

            foreach (var match in matches)
            {
                hashExists = false;
                Hashtag h = new Hashtag(match.ToString(), 1);
                foreach (Hashtag hash in TrendsList)
                {
                    if (hash.Tag.Trim().Equals(h.Tag.Trim()))
                    {
                        hash.Count++;
                        hashExists = true;
                        break;
                    }
                }

                if (!hashExists)
                {
                    TrendsList.Add(h);
                }
            }
       
        }

        //Find Twitter IDs in twitter messages
        private void FindTwitterIDs(String message)
        {
            var regex = new Regex(@"@\w+");
            var matches = regex.Matches(message);
            bool idExists;

            foreach (var match in matches)
            {
                idExists = false;
                Mention t = new Mention(match.ToString(), 1);
                foreach (Mention tweet in MentionsList)
                {
                    if (tweet.Id.Trim().Equals(t.Id.Trim()))
                    {
                        tweet.Count++;
                        idExists = true;
                        break;
                    }
                }

                if (!idExists)
                {
                    MentionsList.Add(t);
                }
            }
        }

        //Find SIR (Significant Incident Report) in email messages
        private void FindSIR(String subject, String message)
        {
            String[] incidents = { "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };

            if (subject.Contains("SIR"))
            {
                string date = Regex.Match(subject, @"(\d{2}[/]+\d{2}[/]+\d{2})").Value;
                string centrecode = Regex.Match(message, @"(\d{2}[-]+\d{3}[-]+\d{2})").Value;
                string incident = "";
                foreach (String s in incidents)
                {
                    if (message.Contains(s))
                    {
                        incident = s;
                    }
                }
                SIRList.Add(new SIR(date, centrecode, incident));
            }
        }

        //Quarantine URLs in email messages
        private String QuarantineURLs(string sender, string message)
        {
            var regex = new Regex(@"(((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:\/~\+#]*[\w\-\@?^=%&amp;\/~\+#])?)");
            var matches = regex.Matches(message);
            string newmessage = message;

            foreach (var match in matches)
            {
                String date = DateTime.Now.ToLongDateString();
                String url = match.ToString();
                URL u = new URL(date, sender, url);
                URLQuarantine.Add(u);
                newmessage = message.Replace(match.ToString(), "<URL Quarantined>");
                message = newmessage;
            }
            return newmessage;
        }

        //Expand textspeak abbreviations in SMS and Twitter messages
        private String ExpandAbbreviations(String message)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Coursework1.Res.textwords.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new char[] { ',' }, 2);
                    if (Regex.Match(message, @"\b" + values[0].Trim() + @"\b").Success)
                    {
                        message = Regex.Replace(message, @"\b" + values[0].Trim() + @"\b", values[0] + " <" + values[1] + ">");
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
                    (window as MainWindow).mentionsList.Items.Clear();
                    (window as MainWindow).trendsList.Items.Clear();

                    (window as MainWindow).dataGrid1.DataContext = null;
                    (window as MainWindow).dataGrid2.DataContext = null;

                    (window as MainWindow).dataGrid1.DataContext = SIRList;
                    (window as MainWindow).dataGrid2.DataContext = URLQuarantine;

                    foreach (Mention tid in MentionsList)
                    {
                        (window as MainWindow).mentionsList.Items.Add(tid.Id);
                    }
                    TrendsList.Sort((x, y) => y.Count.CompareTo(x.Count));
                    foreach (Hashtag h in TrendsList)
                    {
                        (window as MainWindow).trendsList.Items.Add(h.Tag + " (" + h.Count + ")");
                    }                    
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

        //Open file dialog to select a csv or text file
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

        //Read CSV file
        private void ReadCSV(string path)
        {
            int lines = 0;
            try
            {
                using (var reader = new StreamReader(@path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        lines++;
                        var values = line.Split(new char[] { ',' }, 2);
                        try
                        {
                            DetectMessageType(values[0].ToString().Trim(), values[1].ToString().Trim(), true);
                        }
                        catch
                        {
                            MessageBox.Show("There was an error reading:\n" + path + "\n\nTry again with a valid CSV file.", "Error Reading File");
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
            UpdateGUI();
            if (MessagesReadFromFile != 0)
            {
                MessageBox.Show(MessagesReadFromFile + " message(s) were processed. " + (lines-MessagesReadFromFile) + " message(s) were invalid.", "Done!");
            }
            else
            {
                MessageBox.Show("No valid messages could be found in this file. Please try again with another file.", "Error");
            }
            MessagesReadFromFile = 0;
        }

        private void WriteJsonToFile(Object message)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Message));
            jsonSerializer.WriteObject(stream1, message);

            string jsonFile = "messages.json";
            StreamWriter file;

            StreamReader sr = new StreamReader(stream1);
            stream1.Position = 0;

            if (File.Exists(jsonFile))
            {
                if (new FileInfo(jsonFile).Length != 0)
                {
                    string text = File.ReadAllText(jsonFile);
                    text = text.Replace(']', ',');
                    using (file = new StreamWriter(jsonFile, false))
                    {
                        file.Write(text + sr.ReadToEnd() + "]");
                    }
                }
                else
                {
                    using (file = new StreamWriter(jsonFile, false))
                    {
                        file.Write("[" + sr.ReadToEnd() + "]");
                    }
                }
            }
            else
            {
                using (file = new StreamWriter(jsonFile, true))
                {
                    file.Write("[" + sr.ReadToEnd() + "]");
                }
            }
        }
    }
}