using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JWT_API_NETCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{

    public class EmployeeController : Controller
    {

        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44385/api/")
        };


        public IActionResult Index()
        {
            return View(LoadEmployee());
        }

        public JsonResult LoadEmployee()
        {
            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee"); 
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) 
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>(); 
                readTask.Wait();
                employee = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employee);
        }

        public JsonResult InsertOrUpdate(EmployeeVM employee)
        {
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (employee.Id == 0)
            {
                var result = client.PostAsync("employee", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("employee/" + employee.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee/" + Id); 
            responseTask.Wait(); 
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) 
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>(); 
                readTask.Wait();
                employee = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employee);
        }




        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("employee/" + Id).Result;
            return Json(result);
        }


    }
}