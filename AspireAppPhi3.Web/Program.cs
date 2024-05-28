using AspireAppPhi3.Web;
using AspireAppPhi3.Web.Components;
using AspirePhi3App.Web.Components.Chatbot;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        client.BaseAddress = new("https+http://apiservice");
    });

// Add kernel and Register the custom chat completion service
var customChatCompletionService = new CustomChatCompletionService()
{
    ModelName = "phi3",
    ModelUrl = "http://localhost:11434"
};
builder.Services.AddKernel();
builder.Services.AddKeyedSingleton<IChatCompletionService>("CustomChatCompletionService", customChatCompletionService);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
