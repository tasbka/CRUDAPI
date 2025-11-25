using BusinessLogic.Users;
using BussinessLogic;
using BussinessLogic.Users;
using DataAccess;
using DataAccess.Users;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess();
builder.Services.AddBusinessLogic();
builder.Services.AddControllers(opts => opts.Filters.Add<ExceptionFilter>());
builder.Services.AddSwaggerGen();
    
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();