using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }

        public string? Publisher { get; set; }
        public decimal Price { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public Promotion? Promotion { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public override string ToString()
        {
            return String.Format("Title - {0}\nDescription - {1}\nCategory - {2}\nPublishedOn - {3}\nPublisher - {4}\nPrice - {5}",
                Title, Description, null, PublishedOn.ToShortDateString(),
                Publisher, Price);
        }

    }
}
