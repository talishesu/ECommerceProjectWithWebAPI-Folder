using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProjectWithWebAPI.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        public int UserId { get; set; }
        public virtual User User { get; set; }


        public int TrackActionId { get; set; }
        public virtual TrackAction TrackAction { get; set; }


        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedTime { get; set; }
    }
}
