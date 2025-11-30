
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL; // PostgreSQL sürücüsü için gerekli
using YuvaCep.Persistence; // DbContext sýnýfý için

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL'in eski tarih formatýný kabul etmesini saðlayan ayar
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// --- VERÝTABANI BAÐLANTISI ---
builder.Services.AddDbContext<YuvaCep.Persistence.Contexts.YuvaCepDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controller ve Swagger Servisleri
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2.1. JWT (GÝZLÝ ANAHTAR VE DOÐRULAMA) ---

// JWT Secret Key'i appsettings.json'dan okur
var jwtSecret = builder.Configuration.GetSection("JwtSettings:Secret").Value ?? throw new Exception("JWT Secret key not found.");
var key = Encoding.ASCII.GetBytes(jwtSecret);

// Kimlik Doðrulama (Authentication) servisi
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Geliþtirme ortamý için izin verir
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Token'ý imzalayan gizli anahtarý doðrula
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        // Issuer ve Audience kontrolü
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Yetkilendirme (Authorization) servisi
builder.Services.AddAuthorization();

// --- 2.2. DB Context ve PostgreSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//BUNU YORUM SATIRINDAN ÇIKAR YuvaCepDbContext SINIFI ELENDÝKTEN SONRA

/*builder.Services.AddDbContext<YuvaCepDbContext>(options =>
{
    // Npgsql (PostgreSQL) sürücüsünü kullanarak baðlan
    options.UseNpgsql(connectionString);
});*/
// 3. MIDDLEWARE PIPELINE (Use App)
var app = builder.Build();

// Development ortamýnda Swagger'ý etkinleþtir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
//Console.ReadKey();