using Demo.GraphQLService.Dtos;
using Demo.GraphQLService.GraphQL.Types;
using Demo.GraphQLService.Services;
using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL
{
    public class LibraryMutation : ObjectGraphType
    {
        public LibraryMutation(ILibraryService libraryService, LibraryMessageService messageService)
        {
            FieldAsync<BookType>(
                name: "createBook",
                description: "",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BookInputType>> { Name = "book" }),
                resolve: async context =>
                {
                    var book = context.GetArgument<BookDto>("book");
                    var addedBook = await libraryService.AddBookAsync(book);
                    messageService.AddBookAddedMessage(addedBook);
                    return addedBook;
                });
        }
    }
}