using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1,1000000)]
        public decimal Price { get; set; }
        [Range(1,5)]
        [AllowNull]
        [ValidateNever]
        public double Rate  { get; set; }
        [AllowNull]
        [ValidateNever]
        public int Quantity { get; set; }
        public double Discount  { get; set; }
        [AllowNull]
        [ValidateNever]
        public string Image { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

    }
}
