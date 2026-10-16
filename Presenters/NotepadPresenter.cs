using System;
using Notepad.Models;
using Notepad.Services;

namespace Notepad.Presenters
{
    public class NotepadPresenter
    {
        private readonly Document document;
        private readonly FileService fileService;

        public NotepadPresenter()
        {
            document = new Document();
            fileService = new FileService();
        }

        public string Content => document.Content;

        public string FileName => document.FileName;

        public bool IsModified => document.IsModified;

        public void UpdateContent(string content)
        {
            if (document.Content == content)
                return;

            document.Content = content;
            document.IsModified = true;
        }

        public void NewDocument()
        {
            document.Content = string.Empty;
            document.FileName = string.Empty;
            document.IsModified = false;
        }

        public void OpenFile(string filePath)
        {
            string content = fileService.ReadFile(filePath);

            document.Content = content;
            document.FileName = filePath;
            document.IsModified = false;
        }

        public void SaveFile(string filePath = null)
        {
            string path = string.IsNullOrWhiteSpace(filePath)
                ? document.FileName
                : filePath;

            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException(
                    "يجب تحديد مسار الملف للحفظ.");

            fileService.SaveFile(document, path);

            document.FileName = path;
            document.IsModified = false;
        }
    }
}
