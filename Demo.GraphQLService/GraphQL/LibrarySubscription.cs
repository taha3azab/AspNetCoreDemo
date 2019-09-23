using Demo.GraphQLService.GraphQL.Types;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL
{
    public class LibrarySubscription : ObjectGraphType
    {
        public LibrarySubscription(LibraryMessageService messageService)
        {
            Name = "Subscription";
            AddField(new EventStreamFieldType
            {
                Name = "bookAdded",
                Type = typeof(BookAddedMessageType),
                Resolver = new FuncFieldResolver<BookAddedMessage>(c => c.Source as BookAddedMessage),
                Subscriber = new EventStreamResolver<BookAddedMessage>(c => messageService.GetMessages())
            });
        }
    }
}
