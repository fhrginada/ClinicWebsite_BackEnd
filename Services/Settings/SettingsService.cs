using Clinical_project.Models.Entities; 
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PatientApi.Data;
using Clinical_project.Services.Settings;
using PatientApi.Models.Entities;

namespace Clinical_project.Services.Settings
{
   
    public class SettingsService
    {
        private readonly AppDbContext _context;

        public SettingsService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<SystemSettings> GetSettingsAsync()
        {

            var settings = await _context.SystemSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                
                settings = new SystemSettings
                {
                    Id = 1, 
                    PlatformName = "Clinical Project System",
                    TimeZoneId = "Egypt Standard Time",
                    DefaultConsultationDurationMinutes = 30,
                    SupportEmail = "support@clinical.com"
                };
                _context.SystemSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings;
        }


        public async Task<SystemSettings> UpdateSettingsAsync(UpdateSettingsRequest request)
        {
            
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
    
                settings = new SystemSettings { Id = 1 };
                _context.SystemSettings.Add(settings);
            }


            settings.PlatformName = request.PlatformName;
            settings.TimeZoneId = request.TimeZoneId;
            settings.DefaultConsultationDurationMinutes = request.DefaultConsultationDurationMinutes;
            settings.SupportEmail = request.SupportEmail;

            _context.SystemSettings.Update(settings);
            await _context.SaveChangesAsync();

            return settings;
        }
    }
}
