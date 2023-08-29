using Api;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    //option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    In = ParameterLocation.Header,
    //    Description = "Please enter a valid token",
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.Http,
    //    BearerFormat = "JWT",
    //    Scheme = "Bearer"
    //});
    //option.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type=ReferenceType.SecurityScheme,
    //                Id="Bearer"
    //            }
    //        },
    //        new string[]{}
    //    }
    //});
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(c => {
    return new CosmosClient(connectionString);
});
builder.Services.AddSingleton(c => {
    return new CosmosDbClient(new CosmosClient(connectionString));
});

//builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
//{
//    options.Authority = "https://localhost:5001";
//    options.TokenValidationParameters.ValidateAudience = false;
//    options.IncludeErrorDetails = true;
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ApiScope", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("scope", "api1");
//    });
//});

builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("default");
app.UseAuthentication();
//app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
//app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
