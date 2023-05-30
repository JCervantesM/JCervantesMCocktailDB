using Microsoft.Ajax.Utilities;
using ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class CocktailController : Controller
    {
        string Url = "https://www.thecocktaildb.com/";
        [HttpGet]
        public ActionResult GetAll()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Busqueda(ML.Cocktail cocktail)
        {
            ML.Cocktail list = new ML.Cocktail();
            list.drinks = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url);
                var responseTask = client.GetAsync("api/json/v1/1/search.php?s=" + cocktail.strDrink);
                responseTask.Wait();

                var resultApi = responseTask.Result;
                if (resultApi.IsSuccessStatusCode)
                {
                    var readTask = resultApi.Content.ReadAsAsync<ML.Cocktail>();
                    if (readTask.Result.drinks == null)
                    {
                        @ViewBag.Message = "No se encontraron coincidencias";
                    }
                    else
                    {
                        ML.Cocktail resultList = new ML.Cocktail();
                        foreach (var result in readTask.Result.drinks)
                        {
                            resultList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(result.ToString());
                            resultList.Imagen = "https://www.thecocktaildb.com/images/ingredients/" + resultList.strIngredient1 + "-Small.png";
                            resultList.Imagen2 = "https://www.thecocktaildb.com/images/ingredients/" + resultList.strIngredient2 + "-Small.png";
                            resultList.Imagen3 = "https://www.thecocktaildb.com/images/ingredients/" + resultList.strIngredient3 + "-Small.png";
                            resultList.Imagen4 = "https://www.thecocktaildb.com/images/ingredients/" + resultList.strIngredient4 + "-Small.png";
                            resultList.Imagen5 = "https://www.thecocktaildb.com/images/ingredients/" + resultList.strIngredient5 + "-Small.png";

                            list.drinks.Add(resultList);
                        }
                    }
                }

                return View(list);
            }
        }        
    }
}