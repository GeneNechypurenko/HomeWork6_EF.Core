using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Comment { get; set; }
        public byte Stars { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public override string ToString()
        {
            return String.Format("UserName - {0}, UserEmail - {1}, Comment - {2}, Stars - {3}.",
                UserName, UserEmail, Comment, Stars);
        }
    }
}
