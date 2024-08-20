using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App32_MasterDetail.Models
{
    public class SalesMaster
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string InvoiceNo { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [StringLength(150)]
        public string? Customer { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public double VAT { get; set; }
        public double NetAmount { get; set; }
        public int InvoiceStatus { get; set; }
        public List<SalesDetail>? SalesDetail { get; set; }
    }
}
