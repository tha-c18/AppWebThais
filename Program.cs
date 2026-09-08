using AppWebThais.Components;
using AppWebThais.Configs;
using AppWebThais.DAO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
 .AddInteractiveServerComponents();
// Configuração da Conexão com o Banco de Dados MySQL
builder.Services.AddScoped<Conexao>();
builder.Services.AddScoped<ProcessoDAO>();
var app = builder.Build();
 if (!app.Environment.IsDevelopment())
  {
    app.UseExceptionHandler("/Error", createScopeForErrors:
   true);
  }
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
 .AddInteractiveServerRenderMode();

app.Run();
