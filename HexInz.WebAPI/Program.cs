using HexInz.Runner;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
HexInzApp.Build(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
HexInzApp.Init(app.Services, builder.Configuration, builder.Environment);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();