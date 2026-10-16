using System;

namespace Notepad.Models
{
    public class Document
    {

        public string Content { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool IsModified { get; set; } = false;

    }
}

