using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuprematyDemo.Application.Abstractions;
using SuprematyDemo.Application.Auth;
using SuprematyDemo.Application.Products;
using SuprematyDemo.Api.Options;
using SuprematyDemo.Domain.Entities;
using SuprematyDemo.Infrastructure.Auth;
using SuprematyDemo.Infrastructure.Persistence;
using SuprematyDemo.Infrastructure.Repositories;

var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); builder.Services.AddProblemDetails(); builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{c.SwaggerDoc("v1",new OpenApiInfo{Title="SuprematyDemo API",Version="v1"});c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme{Description="JWT Authorization header. Enter: Bearer {token}",Name="Authorization",In=ParameterLocation.Header,Type=SecuritySchemeType.Http,Scheme="bearer",BearerFormat="JWT"});c.AddSecurityRequirement(new OpenApiSecurityRequirement{{new OpenApiSecurityScheme{Reference=new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="Bearer"}},Array.Empty<string>()}});});
builder.Services.AddDbContext<AppDbContext>(o=>o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IProductRepository,ProductRepository>(); builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IAuthService,AuthService>(); builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
builder.Services.AddCors(o=>o.AddPolicy("ui",p=>p.WithOrigins(builder.Configuration["FrontendOrigin"]??"http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));
var jwt=builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>()??throw new InvalidOperationException("JWT config missing");
builder.Services.AddSingleton(new AuthTokenOptions(jwt.Issuer,jwt.Audience,jwt.SigningKey,builder.Configuration.GetValue("Jwt:ExpiryMinutes",60)));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o=>o.TokenValidationParameters=new(){ValidateIssuer=true,ValidIssuer=jwt.Issuer,ValidateAudience=true,ValidAudience=jwt.Audience,ValidateIssuerSigningKey=true,IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),ValidateLifetime=true,ClockSkew=TimeSpan.FromSeconds(30)});
builder.Services.AddAuthorization();
var app=builder.Build(); app.UseExceptionHandler(); if(app.Environment.IsDevelopment()){app.UseSwagger();app.UseSwaggerUI();}
app.UseCors("ui"); app.UseAuthentication(); app.UseAuthorization();
app.MapGet("/health",()=>Results.Ok(new{status="OK",service="SuprematyDemo",utc=DateTimeOffset.UtcNow})).AllowAnonymous(); app.MapControllers();
using(var scope=app.Services.CreateScope()){await DatabaseSeeder.SeedAsync(scope.ServiceProvider.GetRequiredService<AppDbContext>(),scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>());}
app.Run(); public partial class Program {}
