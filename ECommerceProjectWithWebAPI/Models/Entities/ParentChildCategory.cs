using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProjectWithWebAPI.Models.Entities
{
    public class ParentChildCategory
    {
        public int Id { get; set; }

        [Required]
        public int ParentCategoryId { get; set; }

        [Required]
        public int ChildCategoryId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedTime { get; set; }
    }
}
