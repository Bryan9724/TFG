using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Models;

namespace RPA.Models
{
    public class RPAGet
    {
        public int idRPA { get; set; }
        public String nameRPA { get; set; }
        public String dateFirstExecution { get; set; }
        public String fileUrl { get; set; }
        public String UserModify { get; set; }
        public String UserCreated { get; set; }
        public String lastModify { get; set; }
        public String WeekDayName { get; set; }
        public String HoursT { get; set; }
        public bool StatusRPA { get; set; }
        public bool Authorized { get; set; }
    }
    public class RPADaysWeek
    {
        public int idRPA { get; set; }
        public int DayWeek { get; set; }
        public String DayWeekName { get; set; }
    }

    public class RPAHours
    {
        public int idRPA { get; set; }
        public String hourEx { get; set; }
    }

    public class EditRPA
    {
        public List<RPAHours> listRpaHours { get; set; }
        public List<RPADaysWeek> listRPADaysWeek { get; set; }
        public RPAGet rPAGet { get; set; }
        public String[] dayWeek { get; set; }
    }

    public class IndexView
    {
        public UserGet user { get; set; }
        public List<RPAGet> listRPAGet { get; set; }
    }
}
