using Microsoft.EntityFrameworkCore;
using TodoApi.BookM.Models;
using TodoApi.BookM.Service;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 추가
builder.Services.AddDbContext<TodoContext>(opt => 
    opt.UseInMemoryDatabase("TodoList"));

// BookStoreDatabaseSettings 클래스에 appsetting.json 에서 작성한 BookStoreDatabase 데이터로 초기화?
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

// BookService를 싱글톤으로 DI에 등록. 스프링 빈 컨테이너 느낌인듯? 
builder.Services.AddSingleton<BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
