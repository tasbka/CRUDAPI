using BusinessLogic.Users;
using BussinessLogic;
using BussinessLogic.Comments;
using BussinessLogic.Stats;
using BussinessLogic.Users;
using BussinessLogic.Users.DTOs;
using DataAccess;
using DataAccess.Category;
using DataAccess.Comments;
using DataAccess.Users;
using DataAccess.Notes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using WebApi.Infrastructure;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "TalkHub.Auth";
        options.LoginPath = "/api/Users/login";
        options.LogoutPath = "/api/Users/logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/api/Users/accessdenied";
        
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Для HTTP
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Укажите оба протокола
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Для cookies
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString != null) builder.Services.AddDataAccess(connectionString);
builder.Services.AddBusinessLogic();
builder.Services.AddControllers(opts => opts.Filters.Add<ExceptionFilter>());
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
builder.Services.AddScoped<IPostLikeService, PostLikeService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<IStatsService, StatsService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
    
app.MapControllers();

app.Run();