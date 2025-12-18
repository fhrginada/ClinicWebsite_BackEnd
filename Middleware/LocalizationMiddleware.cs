using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Clinical_project.Middleware
{
   
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
       
        private readonly List<string> _supportedCultures = new List<string> { "en-US", "ar-EG" };

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string cultureCode = "en-US"; // القيمة الافتراضية

            // 🆕 منطق قراءة رأس 'Accept-Language' [1], [21]
            if (context.Request.Headers.ContainsKey("Accept-Language"))
            {
                var acceptedLanguages = context.Request.Headers["Accept-Language"].ToString();

                // البحث عن أول لغة مدعومة
                var preferredCulture = acceptedLanguages
                    .Split(',')
                    .Select(c => c.Split(';').First().Trim())
                    .FirstOrDefault(c => _supportedCultures.Contains(c));

                if (preferredCulture != null)
                {
                    cultureCode = preferredCulture;
                }
            }

            // تعيين ثقافة الخيط الحالي
            CultureInfo.CurrentCulture = new CultureInfo(cultureCode);
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);

            await _next(context); // [21]
        }
    }
}