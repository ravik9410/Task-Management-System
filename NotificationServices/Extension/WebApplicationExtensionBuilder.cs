using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace NotificationServices.Extensions
{
    public static class WebApplicationExtensionBuilder
    {
        public static WebApplicationBuilder AddJwtAuthenticationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddScoped<ITaskCreation, CreateTaskServices>();
            //builder.Services.AddScoped<IGetUserDetailsServices, UserDetailsServices>();
            string Issuer = builder.Configuration.GetValue<string>("JwtOptions:Issuer");
            string Secret = builder.Configuration.GetValue<string>("JwtOptions:Secret");
            string Audience = builder.Configuration.GetValue<string>("JwtOptions:Audience");
            var key = Encoding.UTF8.GetBytes(Secret);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                };
            });

            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return builder;
        }
    }
}
