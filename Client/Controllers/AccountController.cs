using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JWT_API_NETCORE.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44385/api/")
        };


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginVM)
        {
           
            var myContent = JsonConvert.SerializeObject(loginVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("auth/login/", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var handler = new JwtSecurityTokenHandler();
                var datajson = handler.ReadJwtToken(data);

                // get token
                string token = "Bearer " + data;
                string role = datajson.Claims.First(claim => claim.Type == "Role").Value;
                string email = datajson.Claims.First(claim => claim.Type == "Email").Value;

                //set token
                HttpContext.Session.SetString("JWTToken", token);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", email);

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    return RedirectToAction("Index", "EmployeeDetail"); // go to employe details
                }



            }
            else 
            {

                return View();
            }

          
        }



        public IActionResult Logout()
        {
            // clear session
            HttpContext.Session.Remove("JWTToken");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");

            return RedirectToAction("Login", "Account"); // back to login

        }


        // error handling page
        public IActionResult notFound()
        {

            return View();
        }



    }
}