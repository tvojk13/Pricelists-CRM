using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Price_lists_CRM.Models
{
    public class PricelistModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
    }
}
