using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL.Types
{
    public class BookAddedMessageType : ObjectGraphType<BookAddedMessage>
    {
        public BookAddedMessageType()
        {
            Field(m => m.Id);
            Field(m => m.Title);
        }
    }
}