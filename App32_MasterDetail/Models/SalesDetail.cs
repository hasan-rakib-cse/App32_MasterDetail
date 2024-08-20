using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App32_MasterDetail.Models
{
    public class SalesDetail
    {
        public int Id { get; set; }

        [Required, Display(Name="Sales Master")]
        public int SalesMasterId { get; set; }

        [Required, Display(Name = "Product")]
        public int ProductId { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        [NotMapped]
        public double SubTotal { get; set; }

        public SalesMaster? SalesMaster { get; set; }
        public Product? Product { get; set; }
    }
}
