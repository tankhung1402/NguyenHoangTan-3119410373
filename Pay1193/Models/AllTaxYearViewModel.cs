using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Service;
using Pay1193.Service.Implementation;

namespace Pay1193.Models
{
    public class AllTaxYearViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> list { get; set; } 
    }
}
