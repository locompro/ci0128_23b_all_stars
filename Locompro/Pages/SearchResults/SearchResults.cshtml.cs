using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Castle.Core.Internal;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Models;
using Microsoft.Extensions.Configuration;

namespace Locompro.Pages.SearchResults
{
    /// <summary>
    /// Page model for the search results page
    /// </summary>
    public class SearchResultsModel : PageModel
    {
        /// <summary>
        /// Service that handles the advanced search modal
        /// </summary>
        private readonly AdvancedSearchModalService _advancedSearchServiceHandler;

        private SearchService _searchService;

        /// <summary>
        /// Service that handles the locations data
        /// </summary>
        private CountryService _countryService;

        /// <summary>
        /// Configuration to get the page size
        /// </summary>
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Buffer for page size according to Paginated List and configuration
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// Paginated list of products found
        /// </summary>
        public PaginatedList<Item> displayItems { get; set; }

        /// <summary>
        /// List of all items found
        /// </summary>
        private List<Item> items;

        /// <summary>
        /// Amount of items found
        /// </summary>
        public double itemsAmount { get; set; }

        /// <summary>
        /// Name of product that was searched
        /// </summary>
        public string productName { get; set; }

        public string provinceSelected { get; set; }
        public string cantonSelected { get; set; }
        public string categorySelected { get; set; }
        public long minPrice { get; set; }
        public long maxPrice { get; set; }
        public string modelSelected { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="countryService"></param>
        /// <param name="advancedSearchServiceHandler"></param>
        /// <param name="configuration"></param>
        public SearchResultsModel(CountryService countryService,
            AdvancedSearchModalService advancedSearchServiceHandler,
            IConfiguration configuration,
            SearchService searchService)
        {
            this._searchService = searchService;
            this._advancedSearchServiceHandler = advancedSearchServiceHandler;
            this._countryService = countryService;
            this.Configuration = configuration;
            this._pageSize = Configuration.GetValue("PageSize", 4);

            // this.OnTestingCreateTestingItems();
        }

        /// <summary>
        /// Gets the items to be displayed in the search results
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="query"></param>
        /// <param name="province"></param>
        /// <param name="canton"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="category"></param>
        /// <param name="model"></param>
        public async Task OnGetAsync(int? pageIndex,
            string query,
            string province,
            string canton,
            long minValue,
            long maxValue,
            string category,
            string model)
        {
            this.provinceSelected = province;
            this.cantonSelected = canton;
            this.minPrice = minValue;
            this.maxPrice = maxValue;
            this.categorySelected = category;
            this.modelSelected = model;
            string queryString = Request.Query["query"];
            if (queryString.IsNullOrEmpty())
            {
                this.productName = query;
            }

            // this.productName = query;
            // this.items =
            //     (await _searchService.SearchItems(productName, province, canton, minValue, maxValue, category, model))
            //     .ToList();
            //
            // this.itemsAmount = items.Count;
            //
            // this.displayItems = PaginatedList<Item>.Create(items, pageIndex ?? 1, _pageSize);
        }


        /// <summary>
        /// Returns the view component for the advanced search modal
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetAdvancedSearch()
        {
            // generate the view component
            var viewComponentResult = ViewComponent("AdvancedSearch", this._advancedSearchServiceHandler);

            // return it for it to be integrated
            return viewComponentResult;
        }

        /// <summary>
        /// Updates the cantons and province selected for the advanced search modal
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetUpdateProvince(string province)
        {
            // update the model with all cantons in the given province
            await this._advancedSearchServiceHandler.ObtainCantonsAsync(province);

            // prevent the json serializer from looping infinitely
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // generate the json file with the cantons
            var cantonsJson = JsonConvert.SerializeObject(this._advancedSearchServiceHandler.Cantons, settings);

            // specify the content type as a json file
            Response.ContentType = "application/json";

            // send to client
            return Content(cantonsJson);
        }

        void OnTestingCreateTestingItems()
        {
            this.items = new List<Item>
            {
                new Item(
                    "2020-10-10",
                    "sombrero",
                    1000,
                    "sombrereria",
                    "San Jose",
                    "San Jose",
                    "sombrero de paja"),
                new Item("2020-10-10",
                    "zapatos",
                    1000,
                    "zapateria",
                    "San Jose",
                    "San Jose",
                    "zapatos de cuero"),
                new Item("2020-10-10",
                    "camisas",
                    1000,
                    "camiseria",
                    "San Jose",
                    "San Jose",
                    "camisas de algodon"),
                new Item("2020-10-10",
                    "pantalones",
                    1000,
                    "pantaloneria",
                    "San Jose",
                    "San Jose",
                    "pantalones de mezclilla"),
                new Item(
                    "2020-10-10",
                    "sombrero",
                    1000,
                    "sombrereria",
                    "San Jose",
                    "San Jose",
                    "sombrero de paja"),

                new Item(
                    "2020-10-10",
                    "zapatos",
                    1000,
                    "zapateria",
                    "San Jose",
                    "San Jose",
                    "zapatos de cuero"),

                new Item(
                    "2021-03-15",
                    "camisa",
                    500,
                    "tienda de ropa",
                    "Heredia",
                    "Heredia",
                    "camisa de algodón"),

                new Item(
                    "2021-02-28",
                    "televisor",
                    2000,
                    "electrodomésticos",
                    "Cartago",
                    "Cartago",
                    "televisor LED de 55 pulgadas"),

                new Item(
                    "2021-01-05",
                    "laptop",
                    1200,
                    "tecnología",
                    "San Jose",
                    "San Jose",
                    "laptop ultrabook"),

                new Item(
                    "2021-04-20",
                    "bicicleta",
                    350,
                    "deportes",
                    "Alajuela",
                    "Alajuela",
                    "bicicleta de montaña"),

                new Item(
                    "2021-06-10",
                    "refrigeradora",
                    900,
                    "electrodomésticos",
                    "Heredia",
                    "Heredia",
                    "refrigeradora de acero inoxidable"),

                new Item(
                    "2021-08-22",
                    "reloj",
                    300,
                    "joyería",
                    "Puntarenas",
                    "Puntarenas",
                    "reloj de pulsera"),

                new Item(
                    "2020-11-17",
                    "mueble de salón",
                    750,
                    "muebles",
                    "San Jose",
                    "San Jose",
                    "mueble de salón moderno"),

                new Item(
                    "2021-09-02",
                    "teléfono móvil",
                    800,
                    "tecnología",
                    "Cartago",
                    "Cartago",
                    "teléfono móvil Android"),

                new Item(
                    "2021-07-30",
                    "cámara DSLR",
                    1100,
                    "electrónica",
                    "Alajuela",
                    "Alajuela",
                    "cámara réflex digital"),

                new Item(
                    "2020-12-05",
                    "tabla de surf",
                    350,
                    "deportes acuáticos",
                    "Puntarenas",
                    "Puntarenas",
                    "tabla de surf para principiantes"),

                new Item(
                    "2021-04-05",
                    "silla de oficina",
                    150,
                    "muebles",
                    "Heredia",
                    "Heredia",
                    "silla ergonómica para oficina"),

                new Item(
                    "2021-03-12",
                    "café gourmet",
                    12,
                    "cafetería",
                    "San Jose",
                    "San Jose",
                    "café molido de alta calidad"),

                new Item(
                    "2021-02-14",
                    "guitarra acústica",
                    300,
                    "instrumentos musicales",
                    "Cartago",
                    "Cartago",
                    "guitarra acústica de concierto"),

                new Item(
                    "2021-08-28",
                    "juego de mesa",
                    25,
                    "juguetes",
                    "Alajuela",
                    "Alajuela",
                    "juego de mesa familiar"),

                new Item(
                    "2021-07-01",
                    "caña de pescar",
                    40,
                    "deportes",
                    "Puntarenas",
                    "Puntarenas",
                    "caña de pescar telescópica"),

                new Item(
                    "2021-06-15",
                    "batidora",
                    60,
                    "electrodomésticos",
                    "Heredia",
                    "Heredia",
                    "batidora de mano"),

                new Item(
                    "2021-09-10",
                    "silla de playa",
                    25,
                    "muebles de exterior",
                    "San Jose",
                    "San Jose",
                    "silla plegable para la playa"),

                new Item(
                    "2020-11-30",
                    "tenis deportivos",
                    75,
                    "tienda de deportes",
                    "Cartago",
                    "Cartago",
                    "tenis deportivos para correr")
            };
        }
    }
}