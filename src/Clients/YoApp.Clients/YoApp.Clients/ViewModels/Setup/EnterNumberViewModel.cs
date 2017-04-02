using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoApp.Clients.Helpers;
using YoApp.Clients.Manager;
using YoApp.Clients.Pages.Modals;
using YoApp.Clients.Pages.Setup;

namespace YoApp.Clients.ViewModels.Setup
{
    public class EnterNumberViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _selectedCountry;
        public int SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                MatchCallingCode();
            }
        }

        private string _callingCode;
        public string CallingCode
        {
            get { return _callingCode; }
            set
            {
                _callingCode = value;
                OnPropertyChanged();
            }
        }

        public Command CompleteCommand { get; private set; }
        public List<string> Countries { get; private set; }
        public string UserPhoneNumber { get; set; }

        private readonly CountriesRepository _countryCodesRepository;
        private List<CountryViewModel> _countriesFromRepo;
        private readonly IVerificationManager _verificationManager;
        private readonly IPageService _pageService;

        public EnterNumberViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _verificationManager = App.Managers.Resolve<IVerificationManager>();
            _countryCodesRepository = new CountriesRepository();
            InitCountriesList();

            CompleteCommand = new Command(async () => await ExecuteRequest());
        }

        private async Task ExecuteRequest()
        {
            var result = await _pageService.DisplayAlert("Phone number verification",
                $"+{CallingCode} {UserPhoneNumber}\n Is this phone number correct?",
                "Yes",
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
            _countriesFromRepo = _countryCodesRepository.Countries;

            foreach (var country in _countriesFromRepo)
                Countries.Add($"{country.ToStringWithEmojiFlag()}");

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
            await _pageService.Navigation.PushModalAsync(new LoadingModalPage("Please wait."));

            var response = await _verificationManager
                                .RequestVerificationCodeAsync(CallingCode,
                                UserPhoneNumber);

            await _pageService.Navigation.PopModalAsync();

            if (!response)
            {
                await _pageService.DisplayAlert("Error",
                    "Sorry, could not send you an SMS.\nPlease try later again.",
                    "Ok");
            }
            else
            {
                await _pageService.Navigation
                    .PushAsync(new WaitVerificationPage($"{CallingCode}{UserPhoneNumber}"));
            }
        }
    }
}
