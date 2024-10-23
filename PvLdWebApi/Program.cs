using System.Net;
using System.Net.Http.Headers;
using PvLdWebApi.Abstraction.Services;
using PvLdWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IFireAndForgetRunner, FireAndForgetRunner>();

services.AddHttpClient<ISlackClient, SlackClient>(client =>
{
    client.Timeout = TimeSpan.FromMilliseconds(10000);
    client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
    client.DefaultRequestHeaders
        .AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AutomaticDecompression = DecompressionMethods.GZip
});

services.AddHttpClient<ILdClient, LdClient>(client =>
{
    client.Timeout = TimeSpan.FromMilliseconds(10000);
    client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
    client.DefaultRequestHeaders
        .AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
    
        // add authentication header without schema
        
    // client.DefaultRequestHeaders.Add("Authorization", "api-09c5cacd-b800-470d-97ab-1b888dd8de98"); // clarizen 
    client.DefaultRequestHeaders.Add("Authorization", "api-9070a677-2520-454e-b2ae-0230b3d5a145"); // test
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AutomaticDecompression = DecompressionMethods.GZip
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
