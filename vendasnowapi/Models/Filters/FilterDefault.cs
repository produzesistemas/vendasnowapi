
using System;

namespace Models.Filters
{
    public class FilterDefault
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int SizePage { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
