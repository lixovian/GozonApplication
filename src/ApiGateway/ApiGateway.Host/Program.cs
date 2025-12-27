using ApiGateway.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddPolicy("frontend", p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddOpenApi("api");
builder.Services.AddSwaggerGen();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();
app.UseCors("frontend");

app.MapMethods("{**path}", new[] { "OPTIONS" }, () => Results.Ok());

app.MapReverseProxy()
    .RequireCors("frontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/orders/openapi/api.json", "Orders API");
        options.SwaggerEndpoint("/payments/openapi/api.json", "Payments API");
    });
}

app.UseHttpsRedirection();

app.MapGatewayEndpoints();

app.MapReverseProxy();

app.Run();