using System.Text.Json;
using RestSharp;
using RestSharp.Serializers.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var restClient = GetRestClient();

builder.Services.AddSingleton(restClient);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Rest ayarları

RestClient GetRestClient()
{
    var restClientOptions = new RestClientOptions
    {
        FailOnDeserializationError = true
    };
    return new RestClient(restClientOptions).UseSystemTextJson(new JsonSerializerOptions
    {
        IncludeFields = true
    });
}
