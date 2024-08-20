using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App32_MasterDetail.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }
        
        [Required, StringLength(500)]
        public string Description { get; set; }

        [Required, StringLength(20)]
        public string Unit { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        [NotMapped]
        public double SubTotal { get; set; }
        public List<SalesDetail>? SalesDetail { get; set; }
    }
}
