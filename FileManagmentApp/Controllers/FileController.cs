using FileManagmentApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagmentApp.Controllers
{


    public class FileController : Controller
    {
        private static List<FileModel> _files = new List<FileModel>();
        private static int _nextId = 1;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public FileController()
        {
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        public IActionResult Index()
        {
            return View(_files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_uploadFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileModel = new FileModel
                {
                    Id = _nextId++,
                    FileName = file.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.Now
                };

                _files.Add(fileModel);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Download(int id)
        {
            var file = _files.FirstOrDefault(f => f.Id == id);
            if (file != null)
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(file.FilePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, "application/octet-stream", file.FileName);
            }
            return NotFound();
        }

        public IActionResult Delete(int id)
        {
            var file = _files.FirstOrDefault(f => f.Id == id);
            if (file != null)
            {
                System.IO.File.Delete(file.FilePath);
                _files.Remove(file);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
