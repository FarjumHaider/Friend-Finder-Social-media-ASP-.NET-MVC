using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mid_project.Models.Entities
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New Password Required")]
        public string New_Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        public string Con_Password { get; set; }
    }
}