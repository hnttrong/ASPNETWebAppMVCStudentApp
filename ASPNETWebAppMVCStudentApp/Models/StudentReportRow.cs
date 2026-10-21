using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ASPNETWebAppMVCStudentApp.Models
{
    public class StudentReportRow
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Grade { get; set; }
    }
}
