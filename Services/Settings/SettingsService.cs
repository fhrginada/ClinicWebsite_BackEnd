using Clinical_project.Data;
using Clinical_project.Models.Entities; 
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clinical_project.Services.Settings
{
    // 🆕 خدمة إعدادات النظام (System Settings Service)
    public class SettingsService
    {
        private readonly ApplicationDbContext _context;

        public SettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. استرداد إعدادات النظام (يجب أن يوجد إعداد واحد فقط بالـ ID=1)
        public async Task<SystemSettings> GetSettingsAsync()
        {
            // محاولة الحصول على الإعدادات، أو إنشاء إعدادات افتراضية إذا كانت غير موجودة
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                // إذا لم يتم العثور على إعدادات، يتم إنشاء سجل جديد بالقيم الافتراضية
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

        // 2. تحديث إعدادات النظام (يجب أن يتم هذا الإجراء بواسطة المسؤول فقط)
        public async Task<SystemSettings> UpdateSettingsAsync(UpdateSettingsRequest request)
        {
            // نبحث عن سجل الإعدادات (نستخدم FirstOrDefaultAsync بدلاً من FindAsync
            // لضمان التعامل مع حالة عدم وجود سجل)
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                // إذا كان السجل غير موجود لسبب ما، نحاول إنشاء سجل جديد قبل التحديث
                settings = new SystemSettings { Id = 1 };
                _context.SystemSettings.Add(settings);
            }

            // تطبيق التحديثات من نموذج الطلب
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
