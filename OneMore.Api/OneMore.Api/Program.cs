using OneMore.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.AddDatabase();

builder.AddCors();

builder.AddSwagger();

builder.AddDependencies();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseCors("SignalRCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.MapHub<SessionHub>("/game");

app.Run();