using System.Collections.Generic;

namespace Demo.GraphQLService.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public int NumberOfPages { get; set; }
        public List<Author> Authors { get; set; }

    }
}
