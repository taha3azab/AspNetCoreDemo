using GraphQL.Types;

namespace Demo.GraphQLService.GraphQL.Types
{
    public class BookInputType : InputObjectGraphType
    {
        public BookInputType()
        {
            Name = "bookInput";
            Field<NonNullGraphType<StringGraphType>>("subject");
            Field<NonNullGraphType<StringGraphType>>("iSBN");
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<NonNullGraphType<StringGraphType>>("publisher");
            Field<StringGraphType>("Language");
            Field<IntGraphType>("NumberOfPages");

        }
    }
}
