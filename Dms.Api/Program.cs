using Dms.Api.Extensions;
using Dms.Api.Services.Navigation;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Services;
using Dms.Core.Infrastructure.Logging;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddTransient<IFilesStatisticService, FilesStatisticService>();
builder.Services.AddTransient<IDocumentHistoryService, DocumentHistoryService>();
builder.Services.AddSqlServerDbContext(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
