using Connect.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RPA.Models;
using User.Models; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TrabajoFinGrado2.Controllers
{
    public class RPAController : Controller
    {
        private readonly ILogger<RPAController> _logger;

        public RPAController(ILogger<RPAController> logger)
        {
            _logger = logger;
        }

        private String msjSucces = "<script>" +
                    "Swal.fire({ " +
                    "    type: 'success', " +
                    "    title: 'Good job', " +
                    "    text: 'Finish process' " +
                    "}).then((result) => { " +
                    "    location.reload(); " +
                    "}); " +
                    "</script>";
        private String msjError = "<script>" +
                    "Swal.fire({ " +
                    "    type: 'error', " +
                    "    title: 'We have a problem ', " +
                    "    text: '<msj>' " +
                    "}); " +
                    "</script>";

        [HttpGet]
        public IActionResult EditRPA(int idRPA)
        {
            ViewData["where"] = "RPA";
            ViewData["nameRole"] = HttpContext.Session.GetInt32("nameRole");
            ViewData["nameUser"] = HttpContext.Session.GetInt32("UserName");
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            String query = "dbo.getRPAOne";
            ConnectModel connec = new ConnectModel();
            RPAGet RPAGet = new RPAGet();
            using (connec.con = new SqlConnection())
            {
                using (connec.com = new SqlCommand(query, connec.con))
                {
                    connec.com.CommandType = CommandType.StoredProcedure;
                    connec.com.CommandTimeout = 120;
                    connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                    connec.com.Parameters["idRPA"].Value = idRPA;
                    connec.connectionString_noinit();
                    connec.con.Open();
                    connec.dr = connec.com.ExecuteReader();

                    if (connec.dr.Read())
                    {
                        String fechaStr = connec.dr["dateFirstExecution"].ToString();
                        DateTime dateTime = DateTime.Parse(fechaStr);
                        fechaStr = dateTime.ToString("yyyy-MM-dd");

                        String fechaStr2 = connec.dr["lastModify"].ToString();
                        dateTime = DateTime.Parse(fechaStr2);
                        fechaStr2 = dateTime.ToString("yyyy-MM-dd hh:mm:ss");


                        RPAGet = new RPAGet
                        {
                            nameRPA = connec.dr["nameRPA"].ToString(),
                            dateFirstExecution = fechaStr,
                            fileUrl = connec.dr["fileUrl"].ToString(),
                            UserModify = connec.dr["UserModify"].ToString(),
                            UserCreated = connec.dr["UserCreated"].ToString(),
                            lastModify = fechaStr2,
                            StatusRPA = Boolean.Parse(connec.dr["StatusRPA"].ToString()),
                            idRPA = int.Parse(connec.dr["idRPA"].ToString())
                        };

                    }
                    connec.con.Close();
                }
            }
            query = "dbo.getRPAOneDaysWeek";
            connec = new ConnectModel();
            List<RPADaysWeek> listRPADaysWeek = new List<RPADaysWeek>();
            using (connec.con = new SqlConnection())
            {
                using (connec.com = new SqlCommand(query, connec.con))
                {
                    connec.com.CommandType = CommandType.StoredProcedure;
                    connec.com.CommandTimeout = 120;
                    connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                    connec.com.Parameters["idRPA"].Value = idRPA;
                    connec.connectionString_noinit();
                    connec.con.Open();
                    connec.dr = connec.com.ExecuteReader();

                    while (connec.dr.Read())
                    {
                        listRPADaysWeek.Add(new RPADaysWeek
                        {
                            idRPA = int.Parse(connec.dr["idRPA"].ToString()),
                            DayWeek = int.Parse(connec.dr["DayWeek"].ToString()),
                            DayWeekName = connec.dr["DayWeekName"].ToString()
                        });

                    }
                    connec.con.Close();
                }
            }
            query = "dbo.getRPAOneHours";
            connec = new ConnectModel();
            List<RPAHours> listRPAHours = new List<RPAHours>();
            using (connec.con = new SqlConnection())
            {
                using (connec.com = new SqlCommand(query, connec.con))
                {
                    connec.com.CommandType = CommandType.StoredProcedure;
                    connec.com.CommandTimeout = 120;
                    connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                    connec.com.Parameters["idRPA"].Value = idRPA;
                    connec.connectionString_noinit();
                    connec.con.Open();
                    connec.dr = connec.com.ExecuteReader();

                    while (connec.dr.Read())
                    {
                        listRPAHours.Add(new RPAHours
                        {
                            idRPA = int.Parse(connec.dr["idRPA"].ToString()),
                            hourEx = connec.dr["hourEx"].ToString().Substring(0,5)
                        });

                    }
                    connec.con.Close();
                }
            }
            String[] WeekDays = { "Sunday" , "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
            EditRPA editRPA = new EditRPA();
            editRPA.rPAGet = RPAGet;
            editRPA.listRPADaysWeek = listRPADaysWeek;
            editRPA.listRpaHours = listRPAHours;
            editRPA.dayWeek = WeekDays;

            var model = editRPA;
            return View(model);
        }

        [HttpPost]
        public String ChangueAuthorized(int idRPA)
        {
            String query = "dbo.ChangueAuthorized";
            ConnectModel connec = new ConnectModel();
            try
            {
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                        connec.com.Parameters["idRPA"].Value = idRPA;
                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();
                    }
                }

                return msjSucces;
            }
            catch (Exception e)
            {
                return msjError.Replace("<msj>", e.Message);
            }
        }

        [HttpPost]
        public String ChangueStatus(int idRPA)
        {
            String query = "dbo.ChangueStatus";
            ConnectModel connec = new ConnectModel();
            try
            {
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                        connec.com.Parameters["idRPA"].Value = idRPA;
                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();
                    }
                }

                return msjSucces;
            }
            catch (Exception e)
            {
                return msjError.Replace("<msj>", e.Message);
            }
        }

        [HttpGet]
        public FileResult DownloadFile(String ruta, String nameRPA)
        {
            Uri uri = new Uri(ruta);
            WebClient wc = new WebClient();
            Stream downloadStream = wc.OpenRead(uri);
            MemoryStream ms = new MemoryStream();
            downloadStream.CopyTo(ms);

            return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, nameRPA + ".py");
        }

        [HttpPost]
        public IActionResult InsertRPA(String NameRPA, String dateFirstExecution, IFormFile FileRPA, 
            String DaysWeek, String Hours, int? idRPA)
        {
            if (idRPA == null)
            {
                idRPA = -1;
            }
            String ruta = "";
            NameRPA = NameRPA.Replace(" ", "_");
            ruta = ruta + "D:\\RPA\\" + NameRPA + "\\";
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            string filename = FileRPA.FileName;
            string filePath2 = Path.Combine(ruta, FileRPA.FileName);
            FileStream datos = new FileStream(filePath2, FileMode.Create);
            FileRPA.CopyTo(datos);
            datos.Close();


            String query = "dbo.InsertRPA";
            ConnectModel connec = new ConnectModel();
            try
            {
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.com.Parameters.AddWithValue("nameRPA", SqlDbType.VarChar);
                        connec.com.Parameters["nameRPA"].Value = NameRPA;
                        connec.com.Parameters.AddWithValue("DateFirstEx", SqlDbType.VarChar);
                        connec.com.Parameters["DateFirstEx"].Value = dateFirstExecution;
                        connec.com.Parameters.AddWithValue("URLRPA", SqlDbType.VarChar);
                        connec.com.Parameters["URLRPA"].Value = filePath2;
                        connec.com.Parameters.AddWithValue("DaysWeek", SqlDbType.VarChar);
                        connec.com.Parameters["DaysWeek"].Value = DaysWeek;
                        connec.com.Parameters.AddWithValue("Hours", SqlDbType.VarChar);
                        connec.com.Parameters["Hours"].Value = Hours;
                        connec.com.Parameters.AddWithValue("idUser", SqlDbType.Int);
                        connec.com.Parameters["idUser"].Value = 1;
                        connec.com.Parameters.AddWithValue("idRPA", SqlDbType.Int);
                        connec.com.Parameters["idRPA"].Value = idRPA;

                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }         
        }

        [HttpGet]
        public IActionResult Index(String userName, String password)
        {
            ViewData["where"] = "RPA";
            String query = "dbo.LoginUser";
            ConnectModel connec = new ConnectModel();
            bool answer = false;
            try
            {
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.com.Parameters.AddWithValue("userName", SqlDbType.VarChar);
                        connec.com.Parameters["userName"].Value = userName;
                        connec.com.Parameters.AddWithValue("password", SqlDbType.VarChar);
                        connec.com.Parameters["password"].Value = password;

                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();

                        if (connec.dr.Read())
                        {
                            answer = connec.dr["ans"].ToString() == "0" ? false : true;
                        }
                        connec.con.Close();
                    }
                }
            }
            catch (Exception e)
            {
                answer = false;
            }
           
            query = "dbo.getRPAs";

            if (answer)
            {
                HttpContext.Session.SetInt32("Login", 1);
                List<RPAGet> listRPAGet = new List<RPAGet>();
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();

                        while (connec.dr.Read())
                        {
                            String fechaStr = connec.dr["dateFirstExecution"].ToString();
                            DateTime dateTime = DateTime.Parse(fechaStr);
                            fechaStr = dateTime.ToString("yyyy-MM-dd");

                            String fechaStr2 = connec.dr["lastModify"].ToString();
                            dateTime = DateTime.Parse(fechaStr2);
                            fechaStr2 = dateTime.ToString("yyyy-MM-dd hh:mm:ss");


                            listRPAGet.Add(new RPAGet
                            {
                                nameRPA = connec.dr["nameRPA"].ToString(),
                                dateFirstExecution = fechaStr,
                                fileUrl = connec.dr["fileUrl"].ToString(),
                                UserModify = connec.dr["UserModify"].ToString(),
                                UserCreated = connec.dr["UserCreated"].ToString(),
                                lastModify = fechaStr2,
                                WeekDayName = connec.dr["WeekDayName"].ToString(),
                                HoursT = connec.dr["HoursT"].ToString(),
                                StatusRPA = Boolean.Parse(connec.dr["StatusRPA"].ToString()),
                                Authorized = Boolean.Parse(connec.dr["Authorized"].ToString()),
                                idRPA = int.Parse(connec.dr["idRPA"].ToString())
                            });
                            
                        }
                        connec.con.Close();
                    }
                }
                UserGet user = new UserGet();
                query = "dbo.getUserByUserPassword";
                using (connec.con = new SqlConnection())
                {
                    using (connec.com = new SqlCommand(query, connec.con))
                    {
                        connec.com.CommandType = CommandType.StoredProcedure;
                        connec.com.CommandTimeout = 120;
                        connec.com.Parameters.AddWithValue("userName", SqlDbType.VarChar);
                        connec.com.Parameters["userName"].Value = userName;
                        connec.com.Parameters.AddWithValue("password", SqlDbType.VarChar);
                        connec.com.Parameters["password"].Value = password;
                        connec.connectionString_noinit();
                        connec.con.Open();
                        connec.dr = connec.com.ExecuteReader();

                        if (connec.dr.Read())
                        {
                            user.idUser = int.Parse(connec.dr["idUser"].ToString());
                            user.UserName = connec.dr["UserName"].ToString();
                            user.Role = int.Parse(connec.dr["Role"].ToString());
                            user.nameRole = connec.dr["nameRole"].ToString();
                        }
                        connec.con.Close();
                    }
                }
                HttpContext.Session.SetInt32("Role", user.Role);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("nameRole", user.nameRole);

                ViewData["nameRole"] = user.nameRole;
                ViewData["nameUser"] = user.UserName;
                ViewData["Role"] = user.Role;

                IndexView indexView = new IndexView();
                indexView.listRPAGet = listRPAGet;
                indexView.user = user;
                var model = indexView;
                return View(model);
            }
            else
            {
                HttpContext.Session.SetInt32("Login", 0);
                return RedirectToAction("Index", "Home", new { message = "Error login"});
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("nameRole");
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Home");
        }


    }
}
