var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RestrictionService.Services.FirestoreService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{

}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}

app.UseAuthorization();
app.MapControllers(); 

app.Run();
