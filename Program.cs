using Microsoft.EntityFrameworkCore;
using lmsApi.Data;
using LMS.MiddleWare;
using lmsApi.jwt;
using Microsoft.AspNetCore.Mvc;
using lmsApi.Helpers;
using lmsApi.Helper.Jwt;

var builder = WebApplication.CreateBuilder(args);

JWTSetting.Initialize(builder.Configuration);


builder.Services.AddOpenApi();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>();


builder.Services.AddControllers();
builder
    .Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context
                .ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var errorResponse = new
            {
                Title = "Validation Failed",
                Status = 400,
                Errors = errors,
            };

            return new BadRequestObjectResult(errorResponse);
        };
    });
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlerMiddleWare>();
app.UseHttpsRedirection();


app.MapControllers();


app.Run();
