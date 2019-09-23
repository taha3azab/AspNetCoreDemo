using Demo.GraphQLService.Dtos;
using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL.Types
{
    public class BookType : ObjectGraphType<BookDto>
    {
        public BookType()
        {
            Field(b => b.Id);
            Field(b => b.ISBN);
            Field(b => b.Title);
            Field(b => b.Subject);
            Field(b => b.Publisher);
            Field(b => b.Language);
            Field(b => b.NumberOfPages);
        }

    }
}
