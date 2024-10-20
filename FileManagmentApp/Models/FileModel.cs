using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileManagmentApp.Models
{
    public class FileModel
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
