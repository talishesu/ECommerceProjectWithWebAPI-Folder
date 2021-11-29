﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProjectWithWebAPI.Models.Entities
{
    public class Size
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedTime { get; set; }
    }
}
