using Backend.Models.Database;

namespace Backend.Models
{
    public static class LoadData
    {
        public static async Task loading(IApplicationBuilder app)
        {
            var _ctx = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();


        }
    }
}
