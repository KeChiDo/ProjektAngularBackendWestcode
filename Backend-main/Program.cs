
using Backend.Models;
using Backend.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Text.Json;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("database")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            AppDbContext context = app
                .Services
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();

                var FragenPath = Path.Combine(Directory.GetCurrentDirectory(), "www", "Fragenkatalog");

                if (Directory.Exists(FragenPath))
                {
                    foreach (var filePath  in Directory.GetFiles(FragenPath, "*.json"))
                    {
                        var fileinfo = new FileInfo(filePath);
                        var fileName = fileinfo.Name;
                        var lastModified = fileinfo.LastWriteTime;

                        var existingDatei = dbContext.Datei
                        .Include(d => d.Fragen)
                        .ThenInclude(f => f.Antworten)
                        .FirstOrDefault(d => d.Name == fileName);
                        if (existingDatei != null && existingDatei.DateTime >= lastModified)
                            continue;

                        if (existingDatei != null)
                        {
                            dbContext.Antworten.RemoveRange(existingDatei.Fragen.SelectMany(f => f.Antworten));
                            dbContext.Fragen.RemoveRange(existingDatei.Fragen);
                            dbContext.Datei.Remove(existingDatei);
                            dbContext.SaveChanges();
                        };

                        var datei = new Datei
                        {
                            Name = fileName,
                            DateTime = lastModified,
                            Fragen = new List<Fragen>()
                        };

                        var json = File.ReadAllText(filePath);
                        var jsonFragen = JsonSerializer.Deserialize<List<JsonFrage>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (jsonFragen != null)
                        {
                            foreach (var f in jsonFragen)
                            {
                                var frage = new Fragen
                                {
                                    Type = f.Type,
                                    QuestionText = f.QuestionText,
                                    hint = f.hint,
                                    Antworten = new List<Antworten>()
                                };

                                datei.Fragen.Add( frage );

                                foreach (var a in f.Answers)
                                {
                                    var antwort = new Antworten
                                    {
                                        AnswerText = a.AnswerText,
                                        IsCorrect = a.IsCorrect,
                                        Fragen = frage
                                    };

                                    frage.Antworten.Add( antwort );
                                }
                            }

                            dbContext.Datei.Add(datei);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }

            app.Run();
        }
    }
}
