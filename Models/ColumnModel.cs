using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Price_lists_CRM.Models
{
    public class ColumnModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int PricelistId { get; set; }
        public virtual PricelistModel Pricelist { get; set; }

        [NotMapped]
        public List<SelectListItem> SelectTypeList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "text", Text = "Text"},
            new SelectListItem { Value = "number", Text = "Number"},
            new SelectListItem { Value = "date", Text = "Date"}
        };
    }
}
