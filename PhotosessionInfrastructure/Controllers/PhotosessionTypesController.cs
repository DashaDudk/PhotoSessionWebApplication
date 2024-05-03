using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PhotosessionDomain.Model;
using System.IO;


namespace PhotosessionInfrastructure.Controllers
{
    public class PhotosessionTypesController : Controller
    {
        private readonly DbphotoSessionContext _context;

        public PhotosessionTypesController(DbphotoSessionContext context)
        {
            _context = context;
        }

        // GET: PhotosessionTypes
        public async Task<IActionResult> Index()
        {

            return View(await _context.PhotosessionTypes.ToListAsync());
        }

        // GET: PhotosessionTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionType == null)
            {
                return NotFound();
            }

            //return View(photosessionType);
            return RedirectToAction("Index", "Photosessions", new { id = photosessionType.Id, name = photosessionType.TypeName, aaa = 1 });
        }

        // GET: PhotosessionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhotosessionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeName,Id")] PhotosessionType photosessionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(photosessionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photosessionType);
        }

        // GET: PhotosessionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes.FindAsync(id);
            if (photosessionType == null)
            {
                return NotFound();
            }
            return View(photosessionType);
        }

        // POST: PhotosessionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeName,Id")] PhotosessionType photosessionType)
        {
            if (id != photosessionType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photosessionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosessionTypeExists(photosessionType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photosessionType);
        }

        // GET: PhotosessionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionType == null)
            {
                return NotFound();
            }

            return View(photosessionType);
        }

        // POST: PhotosessionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photosessionType = await _context.PhotosessionTypes.FindAsync(id);
            if (photosessionType != null)
            {
                _context.PhotosessionTypes.Remove(photosessionType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosessionTypeExists(int id)
        {
            return _context.PhotosessionTypes.Any(e => e.Id == id);
        }


        [HttpPost]
        [ActionName("Export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var photoSessions = await _context.Photosessions
            .Include(p => p.PhotosessionType)
        .Include(p => p.PhotosessionLocation)
        .Include(p => p.PhotosessionStatus)
        .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PhotoSessions");
                worksheet.Cells["A1"].LoadFromCollection(photoSessions.Select(p => new
                {
                    p.Id,
                    p.Price,
                    DateTime = p.DateTime.ToString("dd/MM/yyyy HH:mm"),
                    p.Description,
                    Location = p.PhotosessionLocation.CityName,
                    Status = p.PhotosessionStatus.StatusName,
                    Type = p.PhotosessionType.TypeName
                }), true);
                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhotoSessions.xlsx");
            }
        }



        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {

            if (fileExcel == null || fileExcel.Length <= 0)
            {
                return RedirectToAction("Index");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await fileExcel.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {                      
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string price = worksheet.Cells[row, 2].Value?.ToString();
                            string dateTimeString = worksheet.Cells[row, 3].Value?.ToString();
                            string description = worksheet.Cells[row, 4].Value?.ToString();
                            string locationName = worksheet.Cells[row, 5].Value?.ToString();
                            string statusName = worksheet.Cells[row, 6].Value?.ToString();
                            string typeName = worksheet.Cells[row, 7].Value?.ToString();

                            var photosessionType = await _context.PhotosessionTypes.FirstOrDefaultAsync(t => t.TypeName == typeName);
                            if (photosessionType == null)
                            {
                                photosessionType = new PhotosessionType { TypeName = typeName };
                                _context.PhotosessionTypes.Add(photosessionType);
                            }

                            var photosessionLocation = await _context.PhotosessionLocations.FirstOrDefaultAsync(l => l.CityName == locationName);
                            if (photosessionLocation == null)
                            {
                                photosessionLocation = new PhotosessionLocation { CityName = locationName };
                                _context.PhotosessionLocations.Add(photosessionLocation);
                            }

                            var photosessionStatus = await _context.PhotosessionStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
                            if (photosessionStatus == null)
                            {
                                photosessionStatus = new PhotosessionStatus { StatusName = statusName };
                                _context.PhotosessionStatuses.Add(photosessionStatus);
                            }


                            var existingPhotosession = await _context.Photosessions.FirstOrDefaultAsync(p =>
                                p.Price.Equals(decimal.Parse(price)) &&
                                p.Description == description &&
                                p.DateTime == DateTime.Parse(dateTimeString) &&
                                p.PhotosessionLocationId == photosessionLocation.Id &&
                                p.PhotosessionStatusId == photosessionStatus.Id &&
                                p.PhotosessionTypeId == photosessionType.Id
                            );

                            if (existingPhotosession == null)
                            {
                                var photosession = new Photosession
                                {
                                    Price = (double)decimal.Parse(price),
                                    DateTime = DateTime.Parse(dateTimeString),
                                    Description = description,
                                    PhotosessionLocationId = photosessionLocation.Id,
                                    PhotosessionStatusId = photosessionStatus.Id,
                                    PhotosessionTypeId = photosessionType.Id
                                };

                                _context.Photosessions.Add(photosession);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ActionName("ExportToDocx")]
        public async Task<IActionResult> ExportToDocx()
        {
            var photoSessions = await _context.Photosessions
                .Include(p => p.PhotosessionType)
                .Include(p => p.PhotosessionLocation)
                .Include(p => p.PhotosessionStatus)
                .OrderBy(p => p.Id)
                .ToListAsync();

            MemoryStream stream = new MemoryStream();
            using (WordprocessingDocument doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Додавання таблиці до документу
                Table table = new Table();
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Color = "000000" },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Color = "000000" },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Color = "000000" },
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Color = "000000" }
                    ),
                    new TableStyle() { Val = "TableGrid" },
                    new TableWidth() { Type = TableWidthUnitValues.Auto }
                );
                table.AppendChild(tableProperties);

                // Додавання заголовків стовпців
                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateTableCellWithColorAndFont("ID", "#00FFFF", "Times New Roman"), 
                    CreateTableCellWithColorAndFont("Price", "#00FFFF", "Times New Roman"),
                    CreateTableCellWithColorAndFont("DateTime", "#00FFFF", "Times New Roman"), 
                    CreateTableCellWithColorAndFont("Description", "#00FFFF", "Times New Roman"), 
                    CreateTableCellWithColorAndFont("Location", "#00FFFF", "Times New Roman"), 
                    CreateTableCellWithColorAndFont("Status", "#00FFFF", "Times New Roman"), 
                    CreateTableCellWithColorAndFont("Type", "#00FFFF", "Times New Roman") 
                );
                table.Append(headerRow);

                // Додавання даних про фотосесії до таблиці
                foreach (var session in photoSessions)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        new TableCell(new Paragraph(new Run(new Text(session.Id.ToString())))),
                        new TableCell(new Paragraph(new Run(new Text(session.Price.ToString())))),
                        new TableCell(new Paragraph(new Run(new Text(session.DateTime.ToString())))),
                        new TableCell(new Paragraph(new Run(new Text(session.Description ?? "")))),
                        new TableCell(new Paragraph(new Run(new Text(session.PhotosessionLocation.CityName)))),
                        new TableCell(new Paragraph(new Run(new Text(session.PhotosessionStatus.StatusName)))),
                        new TableCell(new Paragraph(new Run(new Text(session.PhotosessionType.TypeName))))
                    );
                    table.Append(dataRow);
                }

                body.Append(table);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "PhotoSessions.docx");
        }

        private TableCell CreateTableCellWithColorAndFont(string text, string color, string fontName)
        {
            TableCell cell = new TableCell(new Paragraph(new Run(new Text(text))));
            TableCellProperties properties = new TableCellProperties();
            Shading shading = new Shading() { Val = ShadingPatternValues.Clear, Fill = color }; // Встановлення кольору фону
            properties.Append(shading);

            // Встановлення параметрів тексту (шрифт, розмір, жирний)
            RunProperties runProperties = new RunProperties();
            RunFonts runFonts = new RunFonts() { Ascii = fontName, HighAnsi = fontName };
            runProperties.Append(runFonts);
            cell.Append(properties);
            cell.Append(runProperties);

            return cell;
        }
    }
}