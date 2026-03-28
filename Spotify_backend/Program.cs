using Spotify_backend.Services;
using Spotify_backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<StateGenerate>();
builder.Services.AddScoped<ISpotifyAuthService, SpotifyAuthService>();
builder.Services.AddSingleton<SpotifyPlayerManager>();
builder.Services.AddScoped<SpotifyGetInfo>();
builder.Services.AddScoped<SpotifyPlaylistService>();
builder.Services.AddScoped<MediaPlayer>();
builder.Services.AddSingleton<Dictionary<string, Lobby>>();
builder.Services.AddSingleton<LobbyManager>();

builder.Services.AddHttpClient<SpotifyGetInfo>();
builder.Services.AddHttpClient<SpotifyAuthService>();
builder.Services.AddHttpClient<SpotifyPlaylistService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.UseSession();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<LobbyHub>("/hubs/Lobby");

app.Run();
