using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace eLiteratureHaven.Models
{
    public class transactions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id
        {
            get; set;
        }
        
        [Display(Name = "Creation date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime create_date { get; set; }
        
        [Display(Name = "Payment date")]
        public DateTime? payment_date { get; set; }
        
        [Display(Name = "Due date")]
        public DateTime? due_date { get; set; }

        [Display(Name = "Status")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string transaction_status { get; set; }

        [Display(Name = "Book ID")]
        public int book_id { get; set; }

        [Display(Name = "User ID")]
        public int user_id { get; set; }
    }
}