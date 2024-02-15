namespace HomeWork6.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public Book Book { get; set; }
        public Order Order { get; set; }

        public override string ToString()
        {
            return String.Format("Book - {0}, Quantity- {1}.", Book, Quantity);
        }

    }

}