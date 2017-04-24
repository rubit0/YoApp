using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using YoApp.Clients.Core;
using YoApp.Clients.Forms;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Setup;

namespace YoApp.Clients.ViewModels.Setup
{
    public class EnterNumberViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _selectedCountry;
        public int SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                MatchCallingCode();
            }
        }

        private string _callingCode;
        public string CallingCode
        {
            get => _callingCode;
            set
            {
                _callingCode = value;
                OnPropertyChanged();
            }
        }

        public Command CompleteCommand { get; }
        public List<string> Countries { get; private set; }
        public string UserPhoneNumber { get; set; }

        private readonly CountriesContext _countryContext;
        private List<CountryViewModel> _countriesFromRepo;
        private readonly IVerificationManager _verificationManager;
        private readonly IPageService _pageService;
        private readonly IUserDialogs _userDialogs;

        public EnterNumberViewModel(IPageService pageService, IVerificationManager verificationManager, IUserDialogs userDialogs)
        {
            _pageService = pageService;
            _verificationManager = verificationManager;
            _userDialogs = userDialogs;

            _countryContext = new CountriesContext();
            InitCountriesList();

            CompleteCommand = new Command(async () => await ExecuteRequest());
        }

        private async Task ExecuteRequest()
        {
            var result = await _pageService.DisplayAlert("Verifying phone number",
                $"+{CallingCode} {UserPhoneNumber}\n\nIs this OK or do you want to edit the number?",
                "Ok",
                "Edit");

            if (result)
                await StartRequest();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MatchCallingCode()
        {
            CallingCode = _countriesFromRepo[SelectedCountry].CallingCode;
        }

        private void InitCountriesList()
        {
            Countries = new List<string>();
            _countriesFromRepo = _countryContext.Countries;

            foreach (var country in _countriesFromRepo)
                Countries.Add($"{country.Name}");

            var userRegion = System.Globalization.RegionInfo
                .CurrentRegion
                .TwoLetterISORegionName;

            SelectedCountry = _countriesFromRepo
                .TakeWhile(c => string.CompareOrdinal(c.CountryCode, userRegion) != 0)
                .Count();

            MatchCallingCode();
        }

        private async Task StartRequest()
        {
            _userDialogs.ShowLoading("Connecting.");
            var response = await _verificationManager
                                .RequestVerificationCodeAsync(CallingCode,
                                UserPhoneNumber);
            _userDialogs.HideLoading();

            if (!response)
                await _pageService.DisplayAlert("Service Error",
                    "Sorry, could not send you an SMS.\nPlease try later again.",
                    "Ok");
            else
                await _pageService.Navigation
                    .PushAsync(new VerificationPage($"{CallingCode}{UserPhoneNumber}"));
        }
    }
}
