using Serilog;
using TechnicalAssesment.Services.MovieService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMovieService, MovieService>();


var logPath = builder.Configuration.GetSection("Logs").Value;
Serilog.Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
.WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
.CreateLogger();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
