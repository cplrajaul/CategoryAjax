using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryAjax.ViewModels
{
    public class UploadImageViewModel
    {
        [NotMapped]
        
        [Display(Name ="Image")]
        public IFormFile CategoryPicture { get; set; }
    }
}
