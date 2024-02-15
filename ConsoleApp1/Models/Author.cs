using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6.Models
{
    public class Author
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return String.Format("Name - {0}", Name);
        }

    }
}
