using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client.Models;
using Client.Helper;
using System.Net.Http;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        ProductAPI _api = new ProductAPI();

        public async Task<IActionResult> Index()
        {
            List<ProductData> products = new List<ProductData>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/product");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductData>>(results);
            }

            return View(products);
        }

        public async Task<IActionResult> Details (int Id)
        {
            var product = new ProductData();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/product/{Id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductData>(results);
            }

            return View(product);
        }

        public async Task<ActionResult> Edit (int Id)
        {
            var product = new ProductData();
            HttpClient client = _api.Initial();

            HttpResponseMessage res = await client.GetAsync($"api/product/{Id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductData>(results);
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit (int Id, ProductData product)
        {
            HttpClient client = _api.Initial();

            var postTask = client.PostAsJsonAsync<ProductData>($"api/product/{Id}", product);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductData product)
        {
            HttpClient client = _api.Initial();

            var postTask = client.PostAsJsonAsync<ProductData>("api/product", product);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = new ProductData();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.DeleteAsync($"api/product/{Id}");

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
