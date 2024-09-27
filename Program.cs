var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RestrictionService.Services.FirestoreService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; 
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}

app.UseAuthorization();
app.MapControllers(); 

app.Run();
