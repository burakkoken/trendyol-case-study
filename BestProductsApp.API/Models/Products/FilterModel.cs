using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestProductsApp.API.Models.Products
{
    public class FilterModel
    {
        public object Draw { get; set; }
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 10;
        public object Data { get; set; }
        public int RecordFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}
