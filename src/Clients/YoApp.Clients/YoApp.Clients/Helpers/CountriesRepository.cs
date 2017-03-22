using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using YoApp.Clients.Models;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Helpers
{
    public class CountriesRepository
    {
        public List<CountryViewModel> Countries { get; }

        public CountriesRepository()
        {
            Countries = LoadCountries();
        }

        public CountryViewModel GetCountryByCountryCode(string countryCode)
        {
            return Countries.FirstOrDefault(c => c.CountryCode == countryCode);
        }

        public CountryViewModel GetCountryByName(string name)
        {
            return Countries.FirstOrDefault(c => c.Name == name);
        }

        public Dictionary<string, CountryViewModel> GetCountriesDictionary()
        {
            var countryDictionary = new Dictionary<string, CountryViewModel>();

            foreach (var country in Countries)
            {
                countryDictionary.Add(country.Name, country);
            }

            return countryDictionary;
        }

        private List<CountryViewModel> LoadCountries()
        {
            var assembly = typeof(CountriesRepository).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("YoApp.Clients.Ressources.Countries.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<List<CountryViewModel>>(json);
            }
        }
    }
}
