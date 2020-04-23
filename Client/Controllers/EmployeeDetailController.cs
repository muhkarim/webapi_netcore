using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JWT_API_NETCORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class EmployeeDetailController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44385/api/")
        };

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Employee")
            {
                return View(LoadData());
            }
            else
            {
                return RedirectToAction("notFound", "Account");
            }

        }

        public JsonResult LoadData()
        {

            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            var email = HttpContext.Session.GetString("Email");

            var responseTask = client.GetAsync("employee/" + email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>();
                readTask.Wait();
                return Json(readTask.Result[0]);
            }
            else
            {
                return Json(result);
            }
        }


        public JsonResult Update(Employee model)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("employee/" + model.Email, byteContent).Result;
            return Json(result);
        }


    }
}