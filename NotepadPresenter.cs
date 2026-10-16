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

        public string Content
        {
            get => document.Content;
            set
            {
                document.Content = value;
                document.IsModified = true;
            }
        }

        public string FileName => document.FileName;

        public bool IsModified => document.IsModified;

        public void NewDocument()
        {
            document.Content = string.Empty;
            document.FileName = string.Empty;
            document.IsModified = false;
        }

        public void OpenFile(string filePath)
        {
            document.Content = fileService.ReadFile(filePath);
            document.FileName = filePath;
            document.IsModified = false;
        }

        public void SaveFile(string filePath = null)
        {
            string path = string.IsNullOrEmpty(filePath)
                ? document.FileName
                : filePath;

            if (string.IsNullOrEmpty(path))
                return;

            fileService.SaveFile(document, path);

            document.FileName = path;
            document.IsModified = false;
        }
    }
}
