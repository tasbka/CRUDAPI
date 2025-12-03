using BusinessLogic.Users;
using BussinessLogic;
using BussinessLogic.Users;
using DataAccess;
using DataAccess.Category;
using DataAccess.Users;
using DataAccess.Notes;

using WebApi.Infrastructure;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDataAccess(connectionString);
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

//builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();
    ////+

////+
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();

app.Run();