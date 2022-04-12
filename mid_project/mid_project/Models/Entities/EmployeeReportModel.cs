using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mid_project.Models.Entities
{
    public class EmployeeReportModel
    {
        [Required(ErrorMessage = "From Date Date Required")]
        public System.DateTime fromDate { get; set; }

        [Required(ErrorMessage = "To Date Date Required")]
        public System.DateTime toDate { get; set; }
    }
}