using Microsoft.AspNetCore.Http;
using riode.AppCode.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProjectWithWebAPI.Models.Entities
{
    public class Products : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<ProductCategoryItem> CategoryItems { get; set; }

        [NotMapped]
        public ImageItem[] Files { get; set; }
    }
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public string ImagePath { get; set; }
        public bool IsMain { get; set; }
    }
    public class ProductCategoryItem : HistoryWatch
    {
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
    public class ImageItem
    {
        public int? Id { get; set; }
        public bool IsMain { get; set; }
        public string TempPath { get; set; }
        public IFormFile File { get; set; }
    }
}
