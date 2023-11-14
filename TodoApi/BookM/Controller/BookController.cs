using Microsoft.AspNetCore.Mvc;
using TodoApi.BookM.Models;
using TodoApi.BookM.Service;

namespace TodoApi.BookM.Controller;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly BookService bookService;

    public BookController(BookService bookService) =>
        this.bookService = bookService;

    [HttpGet]
    public async Task<List<Book>> Get() =>
        await bookService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await bookService.GetAsync(id);

        if (book is null) return NotFound();

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Book newBook)
    {
        await bookService.CreateAsync(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book updatedBook)
    {
        var book = await bookService.GetAsync(id);

        if (book is null) return NotFound();

        updatedBook.Id = book.Id;

        await bookService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await bookService.GetAsync(id);

        if (book is null) return NotFound();

        await bookService.RemoveAsync(id);

        return NoContent();
    }
}
