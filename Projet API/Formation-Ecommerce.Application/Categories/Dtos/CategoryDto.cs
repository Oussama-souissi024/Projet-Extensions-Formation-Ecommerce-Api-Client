using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formation_Ecommerce_11_2025.Application.Categories.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Category name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string Description { get; set; }
    }
}
