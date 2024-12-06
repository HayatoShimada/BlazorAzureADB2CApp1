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

// Azure AD B2C �F�؂ƃg�[�N���L���b�V�����\��
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// �O���[�o���ȔF�؃|���V�[��ǉ�
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// ���M���O�̐ݒ�
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddAzureWebAppDiagnostics();
});

// MudBlazor �T�[�r�X��ǉ�
builder.Services.AddMudServices();

// �f�[�^�x�[�X �R���e�L�X�g���\��
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Razor Pages �� Microsoft Identity UI ��ǉ�
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Interactive Server Components ��ǉ�
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// HTTP ���N�G�X�g�p�C�v���C���̍\��
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // createScopeForErrors �͕s�v�Ȃ̂ō폜
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// �F�؂ƔF��L����
app.UseAuthentication();
app.UseAuthorization();

// Razor Components �ƃ��[�g�̃}�b�s���O
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.MapRazorPages();

app.Run();
