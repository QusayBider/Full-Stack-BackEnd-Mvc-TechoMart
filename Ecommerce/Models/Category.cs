using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Ecommerce.Models
{

    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(35)]
        public string Name { get; set; }

        [AllowNull]
        [ValidateNever]
        public string Image { get; set; }

        [ValidateNever]
        [AllowNull]
        public List<Product> products { get; set; }
    }
}
