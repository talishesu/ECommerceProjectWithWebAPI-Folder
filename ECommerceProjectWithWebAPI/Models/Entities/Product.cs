using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProjectWithWebAPI.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }


        [Required]
        public Brand Brand { get; set; }


        public List<Color> Colors { get; set; }


        public List<Size> Sizes { get; set; }
        public List<Category> Categories { get; set; }


        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedTime { get; set; }
    }
}
