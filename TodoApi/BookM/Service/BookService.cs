using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.BookM.Models;

namespace TodoApi.BookM.Service;

public class BookService
{
    private readonly IMongoCollection<Book> _booksCollection;

    // IOptions <- 생성자 주입 시켜주는 클래스?
    public BookService(IOptions<BookStoreDatabaseSettings> settings)
    {
        MongoClient mongoClient = new MongoClient(settings.Value.ConnectionString);

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(settings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updateBook) =>
        await _booksCollection.ReplaceOneAsync(book => book.Id == id, updateBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(id);
}
