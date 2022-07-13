using System;
using static HousewareWebAPI.Helpers.Common.GlobalVariable;

namespace HousewareWebAPI.Data.Entities
{
    public class Discount
    {
        public Guid DiscountId { get; set; }
        public DiscountRange Range { get; set; }
        public DiscountType Type { get; set; }
        public int Value { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
