using BlazorAzureADB2CApp1.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using BlazorAzureADB2CApp1.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Azure AD B2C 認証とトークンキャッシュ設定
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// グローバル認証ポリシーの設定
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// ログ設定
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddAzureWebAppDiagnostics();
});

// テーマサービスの登録
builder.Services.AddScoped<ThemeService>();

// MudBlazor サービス追加
builder.Services.AddMudServices();

// Azure SQL Database への接続
// 環境変数を優先し、無い場合は appsettings から取得
string? connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string for the database is not configured.");
}

// DbContext 登録
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(connectionString));

// Razor Pages と Microsoft Identity UI の登録
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Interactive Server Components の登録
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 開発環境用の例外ページ設定
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 認証と認可を有効化
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Razor Components とルートのマッピング
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.MapRazorPages();

app.Run();
