using Demo.GraphQLService.GraphQL.Types;
using Demo.GraphQLService.Services;
using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL
{
    public class LibraryQuery : ObjectGraphType
    {
        public LibraryQuery(ILibraryService libraryService)
        {
            FieldAsync<ListGraphType<BookType>>(
                name: "books",
                resolve: async context => (await libraryService.GetBooksAsync())?.Items);
        }
    }
}
