using System;

namespace riode.AppCode.Infrastructure
{
    public class HistoryWatch
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
