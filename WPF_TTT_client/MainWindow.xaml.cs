using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace WPF_TTT_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public System.Timers.Timer timer = new System.Timers.Timer(200);
        static String serverURL = "http://145.239.95.192:4000";
        string lastMessage = "";        

        public MainWindow()
        {
            InitializeComponent();
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(getMessage);
            timer.AutoReset = true;
        }

        public async void getMessage(object source, System.Timers.ElapsedEventArgs e)
        {
            //zavolat get
            var responseString = await client.GetStringAsync(serverURL + "/");
            //MessageBox.Show(responseString);
            if (responseString != lastMessage)
            {
                var json = responseString;
                ChatMessage obj = JsonConvert.DeserializeObject<ChatMessage>(json);
                this.Dispatcher.Invoke(() =>
                {
                    textBlockMessages.Text += UnixTimeStampToDateTime(obj.time/1000).ToShortTimeString() + " = " +  obj.user + ": " + obj.sprava + '\n';
                });
                lastMessage = responseString;
            }

        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {

            String message = textBoxInput.Text;
            String user = textBoxName.Text;

            //odoslat na server
            Uri uriPost = new Uri(serverURL + "/");

            var values = new Dictionary<string, string>
            {
            { "sprava", message },
            { "user", user }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(uriPost, content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
