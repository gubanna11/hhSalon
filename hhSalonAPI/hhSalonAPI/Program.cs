using hhSalon.Domain.Concrete;
using hhSalon.Services.Services.Implementations;
using hhSalon.Services.Services.Interfaces;
using hhSalonAPI.Domain.Concrete;
using hhSalonAPI.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddEntityFrameworkMySql().AddDbContext<AppDbContext>(options => {
	options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnectionString"), 
		new MySqlServerVersion(new Version(8, 0, 11)))
			.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IAttendancesService, AttendancesService>();
builder.Services.AddScoped<IUsersService, UsersService>();	
builder.Services.AddTransient<IWorkersService, WorkersService>();
builder.Services.AddScoped<IChatDataService, ChatDataService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddPolicy(name: "HHOrigins",
		policy =>
		{
			policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
			//policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
		}));


builder.Services.AddAuthentication(u =>
{
	u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey= true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
				.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
			ValidateAudience = false,
			ValidateIssuer = false,
			ClockSkew = TimeSpan.Zero
		};
	});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("HHOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hubs/chat");

AppDbInitializer.Seed(app);

app.Run();
