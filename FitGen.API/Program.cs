using FitGen.Aplicacion.UseCases;
using FitGen.Dominio.Interfaces;
using FitGen.Infrastructure.Email;
using FitGen.Infrastructure.OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IOpenAIService>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    var apiKey = builder.Configuration["Groq:ApiKey"];
    return new OpenAIService(httpClient, apiKey);
});

builder.Services.AddScoped<IEmailService>(sp =>
{
    var gmailUser = builder.Configuration["Gmail:Usuario"];
    var gmailPassword = builder.Configuration["Gmail:Contrasena"];
    return new GmailService(gmailUser, gmailPassword);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GenerarRutinaUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
