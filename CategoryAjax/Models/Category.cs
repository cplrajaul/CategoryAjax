using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryAjax.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }


        
        [DisplayName("Category Name")]
        [MaxLength(12)]
        [Required(ErrorMessage = "Please enter your Category Name ")]
        public string CategoryName { get; set; }



        [Required(ErrorMessage ="Enter Description")]
        [DisplayName("Description")]
        [MaxLength(200)]
        public string Description { get; set; }


        [DisplayName("Image")]
        //[Required(ErrorMessage ="Enter Image")]
        public string ProfilePicture { get; set; }



       [Display(Name ="Created By")]
        public string CreatedBy { get; set; }



        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }


        [DisplayName("Created Time")]
        [DataType(DataType.Time)]
        public DateTime CreatedTime { get; set; }


        [DisplayName("Modified Time")]
        [DataType(DataType.Time)]
        public DateTime ModifiedTime { get; set; }

    }
}
