using Demo.GraphQLService.Dtos;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Demo.GraphQLService.GraphQL
{
    public class LibraryMessageService
    {
        private readonly ISubject<BookAddedMessage> _messageStream = new ReplaySubject<BookAddedMessage>();

        public BookAddedMessage AddBookAddedMessage(BookDto book)
        {
            var message = new BookAddedMessage
            {
                Id = book.Id,
                Title = book.Title
            };
            _messageStream.OnNext(message);
            return message;
        }
        public IObservable<BookAddedMessage> GetMessages() => _messageStream.AsObservable();
    }
}