using AtecaWPF.Services;

namespace AtecaWPF.ViewModels
{
    public partial class CalendarViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;


        public CalendarViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;

        }
    }
}
