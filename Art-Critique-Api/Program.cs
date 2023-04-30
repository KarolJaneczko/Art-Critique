using Art_Critique_Api.Entities;
using Art_Critique_Api.Services;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

#region Creating a builder with database context
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkMySQL().AddDbContext<ArtCritiqueDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString)) {
        throw new Exception("Cant get a connection string from appsettings");
    }
    options.UseMySQL(connectionString);
});
#endregion

#region Adding services
builder.Services.AddScoped<IUser, UserService>();
#endregion

#region Rest of the invokes
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion
//Scaffold-DbContext "server=localhost;port=3306;user=root;password=Niewiem123;database=art-critique-db" MySql.EntityFrameworkCore -OutputDir Entities -f