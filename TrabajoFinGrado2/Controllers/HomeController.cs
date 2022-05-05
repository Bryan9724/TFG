using Connect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrabajoFinGrado2.Models;

namespace TrabajoFinGrado2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(String? message)
        {
            ConnectModel connectModel = new ConnectModel();
            String query = "SELECT * FROM dbo.[User]";
            using (connectModel.con = new SqlConnection())
            {
                using (connectModel.com = new SqlCommand(query, connectModel.con))
                {
                    connectModel.connectionString_noinit();
                    connectModel.con.Open();
                    connectModel.com.Connection = connectModel.con;
                    connectModel.com.CommandText = query;
                    connectModel.dr = connectModel.com.ExecuteReader();

                    if (connectModel.dr.Read())
                    {
                        ViewData["UserName"] = connectModel.dr["UserName"].ToString();
                        ViewData["password"] = connectModel.dr["password"].ToString();
                    }
                }
            }

            ViewData["where"] = "Login";
            if (message != null)
            {
                ViewData["ErrorLogin"] = message;
            }
            return View();
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
