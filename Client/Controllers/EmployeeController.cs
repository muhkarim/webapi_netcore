using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JWT_API_NETCORE.Models;
using JWT_API_NETCORE.ViewModels;
using Microsoft.AspNetCore.Http;
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
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                return View(LoadEmployee());
            }

            return RedirectToAction("notFound", "Account");



            //return View(LoadEmployee());
        }

       

        public JsonResult LoadEmployee()
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

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

        //public JsonResult InsertOrUpdate(EmployeeVM employee)
        //{
        //    var myContent = JsonConvert.SerializeObject(employee);
        //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //    var byteContent = new ByteArrayContent(buffer);
        //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    if (employee.Email == null)
        //    {
        //        var result = client.PostAsync("employee", byteContent).Result;
        //        return Json(result);
        //    }
        //    else
        //    {
        //        var result = client.PutAsync("employee/" + employee.Email, byteContent).Result;
        //        return Json(result);
        //    }
        //}

        public JsonResult Insert(RegisterViewModel employee)
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           
            var result = client.PostAsync("auth/register", byteContent).Result;
            return Json(result);
           
        }

        public JsonResult GetById(string Email)
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee/" + Email); 
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

        public JsonResult Update(RegisterViewModel employee)
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PutAsync("Employee/" + employee.Email, byteContent).Result;
            return Json(result);
        }




        public JsonResult Delete(string Email)
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            var result = client.DeleteAsync("employee/" + Email).Result;
            return Json(result);
        }





    }
}