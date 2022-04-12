using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mid_project.Models.Entities
{
    public class AdminModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "Name must be greater than 2")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        public string Image { get; set; }
        public int User_id { get; set; }

        [Required(ErrorMessage = "Image Required")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}