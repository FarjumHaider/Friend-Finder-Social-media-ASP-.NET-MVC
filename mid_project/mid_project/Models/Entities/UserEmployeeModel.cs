using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mid_project.Models.Entities
{
    public class UserEmployeeModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public string Status { get; set; }
        public int User_id { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}