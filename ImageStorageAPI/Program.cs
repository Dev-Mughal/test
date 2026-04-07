using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ImageStorageAPI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve images from wwwroot
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
//app.UseAntiforgery();

app.MapPost("/api/images/upload/{imageType:int}", async ([FromForm] IFormFile file, [FromRoute] int imageType, [FromServices] IImageStorageService storageService) =>
{
    if (!Enum.IsDefined(typeof(ImageTypeEnum), imageType))
        return Results.BadRequest("Invalid image type.");

    var res = await storageService.UploadImageAsync(file, (ImageTypeEnum)imageType).ConfigureAwait(false);
    if (string.IsNullOrEmpty(res))
    {
        return Results.BadRequest("Image upload failed.");
    }
    return Results.Ok(res);
}).DisableAntiforgery();

// Backward-compatible route
app.MapPost("/UploadImages/{imageType:int}", async ([FromForm] IFormFile file, [FromRoute] int imageType, [FromServices] IImageStorageService storageService) =>
{
    if (!Enum.IsDefined(typeof(ImageTypeEnum), imageType))
        return Results.BadRequest("Invalid image type.");

    var res = await storageService.UploadImageAsync(file, (ImageTypeEnum)imageType).ConfigureAwait(false);
    if (string.IsNullOrEmpty(res))
    {
        return Results.BadRequest("Image upload failed.");
    }
    return Results.Ok(res);
}).DisableAntiforgery();
app.Run();
