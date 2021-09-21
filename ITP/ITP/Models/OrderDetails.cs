using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITP.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Display(Name = "Order")]
        public int OrderId { get; set; }

        [Display(Name = "Product")]
        public int? ItemId { get; set; }

        //[Display(Name = "Customer Id")]
        //public int CustomerId { get; set; }

        public int Quntity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; } 


        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ItemId")]
        public ItemModel Item { get; set; }

        //[ForeignKey("CustomerId")]
        //public Customer Customer{ get; set; }

    }
}