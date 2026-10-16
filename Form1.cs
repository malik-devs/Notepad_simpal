using System;
using System.IO;
using System.Windows.Forms;
using Notepad.Presenters;

namespace Notepad
{
    public partial class Form1 : Form
    {
        private readonly NotepadPresenter presenter =
            new NotepadPresenter();

        private bool isLoading;

        public Form1()
        {
            InitializeComponent();

            newToolStripMenuItem.ShortcutKeys =
                Keys.Control | Keys.N;

            openToolStripMenuItem.ShortcutKeys =
                Keys.Control | Keys.O;

            saveToolStripMenuItem.ShortcutKeys =
                Keys.Control | Keys.S;

            saveAsToolStripMenuItem.ShortcutKeys =
                Keys.Control | Keys.Shift | Keys.S;
        }

        private bool ConfirmSaveChanges()
        {
            if (!presenter.IsModified)
                return true;

            DialogResult result = MessageBox.Show(
                "يوجد تعديل على الملف لم تقم بحفظه، هل تريد حفظه؟",
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );

            switch (result)
            {
                case DialogResult.Yes:
                    return SaveDocument();

                case DialogResult.No:
                    return true;

                default:
                    return false;
            }
        }

        private bool SaveDocument(bool saveAs = false)
        {
            string path = presenter.FileName;

            if (saveAs || string.IsNullOrWhiteSpace(path))
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Text Files|*.txt|All Files|*.*";
                    dialog.DefaultExt = "txt";
                    dialog.AddExtension = true;
                    dialog.FileName = string.IsNullOrWhiteSpace(path)
                        ? "Untitled.txt"
                        : Path.GetFileName(path);

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return false;

                    path = dialog.FileName;
                }
            }

            try
            {
                presenter.SaveFile(path);
                return true;
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException ||
                ex is ArgumentException ||
                ex is NotSupportedException)
            {
                MessageBox.Show(
                    "تعذر حفظ الملف:\n" + ex.Message,
                    "خطأ في الحفظ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }

        private void newToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            if (!ConfirmSaveChanges())
                return;

            presenter.NewDocument();
            RefreshTextBox();
        }

        private void openToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            if (!ConfirmSaveChanges())
                return;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Text Files|*.txt|All Files|*.*";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    presenter.OpenFile(dialog.FileName);
                    RefreshTextBox();
                }
                catch (Exception ex) when (
                    ex is IOException ||
                    ex is UnauthorizedAccessException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException)
                {
                    MessageBox.Show(
                        "تعذر فتح الملف:\n" + ex.Message,
                        "خطأ في الفتح",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void saveToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void saveAsToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            SaveDocument(saveAs: true);
        }

        private void exitToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(
            object sender, FormClosingEventArgs e)
        {
            if (!ConfirmSaveChanges())
                e.Cancel = true;
        }

        private void textbox_TextChanged(
            object sender, EventArgs e)
        {
            if (isLoading)
                return;

            presenter.UpdateContent(textbox.Text);
        }

        private void RefreshTextBox()
        {
            isLoading = true;

            try
            {
                textbox.Text = presenter.Content;
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
