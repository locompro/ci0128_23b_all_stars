using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Castle.Core.Internal;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace Locompro.Pages.SearchResults
{
    public class ItemDisplayInfo
    {
        public string lastSubmissionDate { get; set; }
        public string productName { get; set; }
        public double productPrice { get; set; }
        public string productStore { get; set; }
        public string cantonLocation { get; set; }
        public string provinceLocation { get; set; }
        public string productDescription { get; set; }

        public ItemDisplayInfo(string lastSubmissionDate,
                string productName,
                double productPrice,
                string productStore,
                string cantonLocation,
                string provinceLocation,
                string productDescription)
        {
            this.lastSubmissionDate = lastSubmissionDate;
            this.productName = productName;
            this.productPrice = productPrice;
            this.productStore = productStore;
            this.cantonLocation = cantonLocation;
            this.provinceLocation = provinceLocation;
            this.productDescription = productDescription;
        }
    };


    public class SearchResultsModel : PageModel
    {

        private AdvancedSearchModalService advancedSearchServiceHandler;

        private CountryService countryService;

        private readonly IConfiguration Configuration;

        private int pageSize;
        public string productName { get; set; }

        public PaginatedList<ItemDisplayInfo> displayItems { get; set; }

        private List<ItemDisplayInfo> items;

        public double itemsAmount { get; set; }

        public string provinceSelected { get; set; }
        public string cantonSelected { get; set; }
        public string categorySelected { get; set; }
        public long minPrice { get; set; }
        public long maxPrice { get; set; }
        public string modelSelected { get; set; }

        public SearchResultsModel(CountryService countryService,
                AdvancedSearchModalService advancedSearchServiceHandler,
                IConfiguration configuration)
        {
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;
            this.countryService = countryService;
            this.Configuration = configuration;
            this.pageSize = Configuration.GetValue("PageSize", 4);

            this.OnTestingCreateTestingItems();
        }

        public void OnGetAsync(int? pageIndex,
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
                queryString = "";
                this.productName = query;
            }            

            this.productName = query;
            Console.WriteLine(this.productName);

            this.itemsAmount = items.Count;

            /*
           List<Product> products = (await this.productService.GetAll()).ToList();

           foreach (Product product in products)
           {
               this.items.Add(new ItemDisplayInfo("0/0/0", product.Name, 1000, "Generic Store", "Generic Canton", "Generic Province", "Generic Description"));
           }

           this.itemsAmount = items.Count;
           */

            this.displayItems = PaginatedList<ItemDisplayInfo>.Create(items, pageIndex ?? 1, pageSize);
        }

        public IActionResult OnGetAdvancedSearch()
        {
            // generate the view component
            var viewComponentResult = ViewComponent("AdvancedSearch", this.advancedSearchServiceHandler);

            // return it for it to be integrated
            return viewComponentResult;
        }

        public async Task<IActionResult> OnGetUpdateProvince(string province)
        {
            // update the model with all cantons in the given province
            await this.advancedSearchServiceHandler.ObtainCantonsAsync(province);

            // prevent the json serializer from looping infinitely
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // generate the json file with the cantons
            var cantonsJson = JsonConvert.SerializeObject(this.advancedSearchServiceHandler.cantons, settings);

            // specify the content type as a json file
            Response.ContentType = "application/json";

            // send to client
            return Content(cantonsJson);
        }

        void OnTestingCreateTestingItems()
        {
            this.items = new List<ItemDisplayInfo>
                {
                new ItemDisplayInfo(
                    "2020-10-10",
                    "sombrero",
                    1000,
                    "sombrereria",
                    "San Jose",
                    "San Jose",
                    "sombrero de paja"),
                new ItemDisplayInfo("2020-10-10",
                    "zapatos",
                    1000,
                    "zapateria",
                    "San Jose",
                    "San Jose",
                    "zapatos de cuero"),
                new ItemDisplayInfo("2020-10-10",
                    "camisas",
                    1000,
                    "camiseria",
                    "San Jose",
                    "San Jose",
                    "camisas de algodon"),
                new ItemDisplayInfo("2020-10-10",
                    "pantalones",
                    1000,
                    "pantaloneria",
                    "San Jose",
                    "San Jose",
                    "pantalones de mezclilla"),
                new ItemDisplayInfo(
                    "2020-10-10",
                    "sombrero",
                    1000,
                    "sombrereria",
                    "San Jose",
                    "San Jose",
                    "sombrero de paja"),

                new ItemDisplayInfo(
                    "2020-10-10",
                    "zapatos",
                    1000,
                    "zapateria",
                    "San Jose",
                    "San Jose",
                    "zapatos de cuero"),

                new ItemDisplayInfo(
                    "2021-03-15",
                    "camisa",
                    500,
                    "tienda de ropa",
                    "Heredia",
                    "Heredia",
                    "camisa de algodón"),

                new ItemDisplayInfo(
                    "2021-02-28",
                    "televisor",
                    2000,
                    "electrodomésticos",
                    "Cartago",
                    "Cartago",
                    "televisor LED de 55 pulgadas"),

                new ItemDisplayInfo(
                    "2021-01-05",
                    "laptop",
                    1200,
                    "tecnología",
                    "San Jose",
                    "San Jose",
                    "laptop ultrabook"),

                new ItemDisplayInfo(
                    "2021-04-20",
                    "bicicleta",
                    350,
                    "deportes",
                    "Alajuela",
                    "Alajuela",
                    "bicicleta de montaña"),

                new ItemDisplayInfo(
                    "2021-06-10",
                    "refrigeradora",
                    900,
                    "electrodomésticos",
                    "Heredia",
                    "Heredia",
                    "refrigeradora de acero inoxidable"),

                new ItemDisplayInfo(
                    "2021-08-22",
                    "reloj",
                    300,
                    "joyería",
                    "Puntarenas",
                    "Puntarenas",
                    "reloj de pulsera"),

                new ItemDisplayInfo(
                    "2020-11-17",
                    "mueble de salón",
                    750,
                    "muebles",
                    "San Jose",
                    "San Jose",
                    "mueble de salón moderno"),

                new ItemDisplayInfo(
                    "2021-09-02",
                    "teléfono móvil",
                    800,
                    "tecnología",
                    "Cartago",
                    "Cartago",
                    "teléfono móvil Android"),

                new ItemDisplayInfo(
                    "2021-07-30",
                    "cámara DSLR",
                    1100,
                    "electrónica",
                    "Alajuela",
                    "Alajuela",
                    "cámara réflex digital"),

                new ItemDisplayInfo(
                    "2020-12-05",
                    "tabla de surf",
                    350,
                    "deportes acuáticos",
                    "Puntarenas",
                    "Puntarenas",
                    "tabla de surf para principiantes"),

                new ItemDisplayInfo(
                    "2021-04-05",
                    "silla de oficina",
                    150,
                    "muebles",
                    "Heredia",
                    "Heredia",
                    "silla ergonómica para oficina"),

                new ItemDisplayInfo(
                    "2021-03-12",
                    "café gourmet",
                    12,
                    "cafetería",
                    "San Jose",
                    "San Jose",
                    "café molido de alta calidad"),

                new ItemDisplayInfo(
                    "2021-02-14",
                    "guitarra acústica",
                    300,
                    "instrumentos musicales",
                    "Cartago",
                    "Cartago",
                    "guitarra acústica de concierto"),

                new ItemDisplayInfo(
                    "2021-08-28",
                    "juego de mesa",
                    25,
                    "juguetes",
                    "Alajuela",
                    "Alajuela",
                    "juego de mesa familiar"),

                new ItemDisplayInfo(
                    "2021-07-01",
                    "caña de pescar",
                    40,
                    "deportes",
                    "Puntarenas",
                    "Puntarenas",
                    "caña de pescar telescópica"),

                new ItemDisplayInfo(
                    "2021-06-15",
                    "batidora",
                    60,
                    "electrodomésticos",
                    "Heredia",
                    "Heredia",
                    "batidora de mano"),

                new ItemDisplayInfo(
                    "2021-09-10",
                    "silla de playa",
                    25,
                    "muebles de exterior",
                    "San Jose",
                    "San Jose",
                    "silla plegable para la playa"),

                new ItemDisplayInfo(
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
