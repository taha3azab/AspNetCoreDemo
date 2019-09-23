using GraphQL;
using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL
{
    public class LibrarySchema : Schema
    {
        public LibrarySchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<LibraryQuery>();
            Mutation = resolver.Resolve<LibraryMutation>();
            Subscription = resolver.Resolve<LibrarySubscription>();
        }
    }
}
