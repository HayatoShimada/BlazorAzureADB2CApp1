using BlazorAzureADB2CApp1.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using BlazorAzureADB2CApp1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;


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

builder.Services.AddScoped<UserStateService>();


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

// 認証サービスを登録
builder.Services.AddAuthentication()
    .AddCookie("LINE_Cookie") // LINE専用のCookieスキーム
    .AddOAuth("LINE", options =>
    {
        options.ClientId = "2006662452";
        options.ClientSecret = "411631a9659ad46f3d28421323fd4bb9";
        options.CallbackPath = new PathString("/signin-line"); // LINEログイン用のコールバック

        options.AuthorizationEndpoint = "https://access.line.me/oauth2/v2.1/authorize";
        options.TokenEndpoint = "https://api.line.me/oauth2/v2.1/token";
        options.UserInformationEndpoint = "https://api.line.me/v2/profile";

        options.Scope.Add("profile"); // ユーザーID取得のためのスコープ
        options.SaveTokens = true;

        options.SignInScheme = "LINE_Cookie"; // LINE用のCookieスキームを指定

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");

        options.Events = new OAuthEvents
        {
            OnCreatingTicket = async context =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);
                var response = await context.Backchannel.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var user = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
                context.RunClaimActions(user);
            }
        };
    });




// Razor Pages と Microsoft Identity UI の登録
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Interactive Server Components の登録
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// サービスの追加
builder.Services.AddControllers();
builder.Services.AddRazorComponents();
builder.Services.AddServerSideBlazor();

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
