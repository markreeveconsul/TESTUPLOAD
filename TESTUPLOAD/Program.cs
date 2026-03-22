var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(); // enable session

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // must enable session

app.MapRazorPages();
app.Run();