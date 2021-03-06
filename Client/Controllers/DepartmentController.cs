﻿using System;
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

    public class DepartmentController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44385/api/")
        };

        public IActionResult Index()
        {
            
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin") {
                return View(LoadDepartment());
            }

            return RedirectToAction("notFound", "Account");
           

        }


        public JsonResult LoadDepartment()
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            DepartmentJson departmentVM = null;
            
            var responTask = client.GetAsync("department");
            responTask.Wait();
            var result = responTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error, try after some time");
            }

            return Json(departmentVM);

        }

        public JsonResult InsertOrUpdate(DepartmentVM departmentVM)
        {
            //add jwt token 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            var myContent = JsonConvert.SerializeObject(departmentVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (departmentVM.Id == 0)
            {
                var result = client.PostAsync("department", byteContent).Result;
                //return Json(result);
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("department/" + departmentVM.Id, byteContent).Result;
                return Json(result);
            }

        }

        public JsonResult GetById(int Id)
        {
            //add jwt token 
           client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            DepartmentVM departmentVM = null;
           
            var responseTask = client.GetAsync("department/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(departmentVM);
        }


        public JsonResult Delete(int Id)
        {
            //add jwt token 
           client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            var result = client.DeleteAsync("department/" + Id).Result;
            return Json(result);
        }



    }
}