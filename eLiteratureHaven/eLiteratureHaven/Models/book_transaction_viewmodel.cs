using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLiteratureHaven.Models
{
    public class book_transaction_viewmodel
    {
        public books books { get; set; }
        public transactions transactions { get; set; }
    }
}