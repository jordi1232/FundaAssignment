using System.Net.Http;
using System.Windows;
using FundaAssignment.Services;
using FundaAssignment.ViewModels;

namespace FundaAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HttpClient _httpClient;
        private const string KEY = "76666a29898f491480386d966b75f949";

        private MakelaarAantalViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            LoggingService.Instance.SetTextbox(txtDebug);

            _httpClient = new HttpClient();
            _viewModel = (MakelaarAantalViewModel)base.DataContext;
        }

        private void btnAmsterdamRequest_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartRequesting(["Amsterdam"]);
        }

        private void btnTuinRequest_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartRequesting(["Amsterdam", "Tuin"]);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Stop();
        }
    }
}