using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eLiteratureHaven.Models
{
    public class books
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id
        {
            get; set;
        }
   
        [Display(Name = "Book Title")]
        [Required(ErrorMessage = " • Book title is required.")]
        public string title { get; set; }

        [Display(Name = "Author")]
        public string author { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Display(Name = "Published Year")]
        public int publication_year { get; set; }

        [Display(Name = "Category")]
        public string category { get; set; }

        [Display(Name = "Genre")]
        public string genre { get; set; }

        [Display(Name = "Image Path")]
        public string image_path { get; set; }

        [Display(Name = "PDF Path")]
        public string pdf_path { get; set; }

    }
}