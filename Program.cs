using BlazorAzureADB2CApp1.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using BlazorAzureADB2CApp1.Models;

var builder = WebApplication.CreateBuilder(args);

// Azure AD B2C 認証とトークンキャッシュを構成
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// グローバルな認証ポリシーを追加
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// ロギングの設定
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddAzureWebAppDiagnostics();
});

// MudBlazor サービスを追加
builder.Services.AddMudServices();

// データベース コンテキストを構成
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Razor Pages と Microsoft Identity UI を追加
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Interactive Server Components を追加
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// HTTP リクエストパイプラインの構成
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // createScopeForErrors は不要なので削除
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 認証と認可を有効化
app.UseAuthentication();
app.UseAuthorization();

// Razor Components とルートのマッピング
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.MapRazorPages();

app.Run();
