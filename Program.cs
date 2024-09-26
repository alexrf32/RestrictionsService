var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para escuchar en el puerto 8080
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Escucha solo en el puerto 8080
    serverOptions.ListenAnyIP(8080);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<RestrictionService.Services.FirestoreService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Comentado para evitar conflictos en producción

app.UseAuthorization();
app.MapControllers();
app.Run();
