
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Persistence.Contexts;
using YuvaCep.Application.Services;


var builder = WebApplication.CreateBuilder(args);

// PostgreSQL'in eski tarih formatını kabul etmesini saðlayan ayar
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// --- VERİTABANI BAĞLANTISI ---
builder.Services.AddDbContext<YuvaCep.Persistence.Contexts.YuvaCepDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controller ve Swagger Servisleri
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IMonthlyPlanService, MonthlyPlanService>();
builder.Services.AddScoped<IPushNotificationService, MockPushNotificationService>();


// 1. JSON'dan doğru anahtarı ("Jwt:Key") okuyoruz
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Value ?? throw new Exception("JWT Key bulunamadı! appsettings.json dosyasını kontrol et.");
var key = Encoding.UTF8.GetBytes(jwtKey); // AuthService ile uyumlu olması için UTF8 yapıyoruz

// 2. JWT Servisini Ekliyoruz
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        // JSON'daki Issuer ve Audience ile eşleşmeli
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Yetkilendirme (Authorization) servisi
builder.Services.AddAuthorization();

// --- 2.2. DB Context ve PostgreSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//BUNU YORUM SATIRINDAN ÇIKAR YuvaCepDbContext SINIFI ELENDÝKTEN SONRA

builder.Services.AddDbContext<YuvaCepDbContext>(options =>
{
    // Npgsql (PostgreSQL) sürücüsünü kullanarak baðlan
    options.UseNpgsql(connectionString);
});
// 3. MIDDLEWARE PIPELINE (Use App)
var app = builder.Build();

// --- RESİM DOSYALARINI DIŞARI AÇ ---
app.UseStaticFiles();
// Bu komut "wwwroot" klasöründeki dosyaların tarayıcıdan açılmasını sağlar.

// Development ortamýnda Swagger'ý etkinleþtir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
