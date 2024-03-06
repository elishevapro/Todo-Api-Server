using TodoApi;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});
builder.Services.AddDbContext<ToDoDbContext>(
    options=>options.UseMySql("server=localhost;user=root;password=ELI7eli7@;database=tododb",new MySqlServerVersion(new Version(8, 0, 36)))
    );
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
var app = builder.Build();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
// if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.UseRouting();
app.UseCors(builder => 
{
    builder.AllowAnyOrigin()
        // .WithOrigins()
        .AllowAnyHeader()
        .AllowAnyMethod();
});
app.MapGet("/", () => "Hello World!");
app.MapGet("/items", ([FromServices] ToDoDbContext db)=> db.Items);
app.MapPost("/items", async (ToDoDbContext db, HttpContext context)=>{
    var item = await context.Request.ReadFromJsonAsync<Item>();
    item.IsComplete=false;
    if(item !=null){
        db.Items.Add(item);
        db.SaveChanges();
    }
});
app.MapPut("/items/{id}",(int id, bool IsComplete, ToDoDbContext db)=>{
    var item = db.Items.Find(id);
    if(item!=null){
        item.IsComplete=IsComplete;
    }
    db.SaveChanges();
});
app.MapDelete("/items/{id}",(int id, ToDoDbContext db)=>{
    var item = db.Items.Find(id);
    if(item!=null)
        db.Items.Remove(item);
    db.SaveChanges();
});

app.Run();
