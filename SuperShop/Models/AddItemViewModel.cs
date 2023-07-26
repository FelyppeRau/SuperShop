using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SuperShop.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Product.")] // O valor '1' não permite que inclua um producto com "(Select a Product...)" que está no código 'ProductRepository' linha 33
        public int ProductId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The Qauntity must be a positive number.")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
