using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using YoApp.Clients.ViewModels.Setup;

namespace YoApp.Clients.Core
{
    /// <summary>
    /// Loads country emoji flags from a json ressource.
    /// </summary>
    public class CountriesContext
    {
        public List<CountryViewModel> Countries { get; }

        public CountriesContext()
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
            return Countries.ToDictionary(c => c.Name);
        }

        private List<CountryViewModel> LoadCountries()
        {
            var assembly = typeof(CountriesContext).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("YoApp.Clients.Ressources.Countries.json") 
                ?? throw new InvalidOperationException("Could not locate Countries.json as embedded ressource");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<List<CountryViewModel>>(json);
            }
        }
    }
}
