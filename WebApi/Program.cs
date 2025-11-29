using BusinessLogic.Users;
using BussinessLogic;
using BussinessLogic.Users;
using DataAccess;
using DataAccess.Category;
using DataAccess.Users;
using DataAccess.Notes;

using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess();
builder.Services.AddBusinessLogic();
builder.Services.AddControllers(opts => opts.Filters.Add<ExceptionFilter>());
builder.Services.AddSwaggerGen();
    
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
builder.Services.AddScoped<IPostLikeService, PostLikeService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();