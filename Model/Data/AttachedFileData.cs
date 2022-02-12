using Model.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace Model.Data
{
    [Serializable]
    public class AttachedFileData
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
