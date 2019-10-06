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

namespace WPF_TTT_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        static String serverURL = "http://145.239.95.192:4000";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            
            String message = textBoxInput.Text;

            //odoslat na server
            Uri uriPost = new Uri(serverURL + "/");

            var values = new Dictionary<string, string>
            {
            { "sprava", message }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(uriPost, content);

            var responseString = await response.Content.ReadAsStringAsync();

            MessageBox.Show(responseString);
            
        }

        private async void ButtonGet_Click(object sender, RoutedEventArgs e)
        {
            var responseString = await client.GetStringAsync(serverURL + "/");
            MessageBox.Show(responseString);
        }
    }
}
