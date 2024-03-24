using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PhotosessionInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbphotoSessionContext _context;

        public ChartController(DbphotoSessionContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var photosessions = _context.Photosessions.ToList();
            var months = Enumerable.Range(1, 12).Select(month => new DateTime(2024, month, 1)).ToList(); 
            var photosessionsPerMonth = new Dictionary<string, int>();

            foreach (var month in months)
            {
                var monthName = month.ToString("MMMM", CultureInfo.InvariantCulture);
                var photosessionsCount = photosessions.Count(p => p.DateTime.Month == month.Month);
                photosessionsPerMonth.Add(monthName, photosessionsCount);
            }

            var data = new List<object>();
            data.Add(new[] { "Month", "Number of photosessions" });
            foreach (var kvp in photosessionsPerMonth)
            {
                data.Add(new object[] { kvp.Key, kvp.Value });
            }

            return new JsonResult(data);
        }
        
        [HttpGet("JsonData2")]
           public JsonResult JsonData2()
           {
               var types = _context.PhotosessionTypes.Include(t => t.Photosessions).ToList();
               List<object> typePhotosession = new List<object>();
               typePhotosession.Add(new[] { "Types", "Number of photosessions" });
               int totalPhotosessions = types.Sum(t => t.Photosessions.Count());

               foreach (var t in types)
               {
                   double percentage = totalPhotosessions != 0 ? (double)t.Photosessions.Count() / totalPhotosessions * 100 : 0;
                   typePhotosession.Add(new object[] { t.TypeName, percentage });
               }

               return new JsonResult(typePhotosession);
           }
    }
}