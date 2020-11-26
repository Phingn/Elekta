using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.Errors;
using Elekta.Code.Challenge.Api.External.Api;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Models.External;
using Elekta.Code.Challenge.Api.Repository;
using Elekta.Code.Challenge.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elekta.Code.Challenge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataSourceContext")));

            services.AddControllers();

            var equipmentSetting = Configuration.GetSection("EquipmentSetting").Get<EquipmentSetting>();
            var emailSetting = Configuration.GetSection("EmailSetting").Get<EmailSetting>();

            services.AddSingleton(equipmentSetting);
            services.AddSingleton(emailSetting);

            services.AddScoped<IBooking, BookingService>();
            services.AddScoped<IBookingRepositoryService, BookingRepositoryService>();
            services.AddScoped<IPatientRespository, PatientRepositoryService>();
            services.AddScoped<IEquipmentApi, EquipmentApi>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
