using System;
using System.IO;
using Notepad.Models;

namespace Notepad.Services
{
	public class FileService
	{
        public string ReadFile(string path) => File.ReadAllText(path);
        public void SaveFile(Document document, string filePath)
        {
            File.WriteAllText(filePath, document.Content);
        }
    }
}