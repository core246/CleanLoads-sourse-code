using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cleanloads
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Меняем GetFiles на GetFileSystemEntries, чтобы программа видела и папки тоже
            foreach (var file in Directory.GetFileSystemEntries(path))
            {
                string name = Path.GetFileName(file);
                // Пропускаем папки самой программы, чтобы они не улетели внутрь себя
                if (name == "Документы" || name == "Картинки" || name == "Приложения" || name == "Папки" || name == "Другое") continue;

                string ext = Path.GetExtension(file).ToLower();

                // НАША МЕХАНИКА: Если это папка -> имя "Папки", иначе -> ваша строка по расширениям
                string folder = Directory.Exists(file) ? "Папки" :
                                ((ext == ".doc" || ext == ".docx" || ext == ".pdf") ? "Документы" :
                                ((ext == ".jpg" || ext == ".png") ? "Картинки" :
                                ((ext == ".exe") ? "Приложения" :
                                ((ext == ".pptx" || ext == ".ppt") ? "Документы" :
                                ((ext == ".lnk") ? "Приложения" : "Другое")))));

                Directory.CreateDirectory(Path.Combine(path, folder));

                // Универсальный перенос с защитой от существующих файлов и папок
                string destinationPath = Path.Combine(path, folder, name);

                if (Directory.Exists(file))
                {
                    // Безопасное перемещение папки (с объединением дубликатов)
                    MoveDirectoryWithMerge(file, destinationPath);
                }
                else if (File.Exists(file))
                {
                    // Если целевой файл уже существует, удаляем его перед переносом
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Move(file, destinationPath);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Меняем GetFiles на GetFileSystemEntries, чтобы программа видела и папки тоже
            foreach (var file in Directory.GetFileSystemEntries(path))
            {
                string name = Path.GetFileName(file);
                // Пропускаем папки самой программы, чтобы они не улетели внутрь себя
                if (name == "Документы" || name == "Картинки" || name == "Приложения" || name == "Папки" || name == "Другое") continue;

                string ext = Path.GetExtension(file).ToLower();

                // НАША МЕХАНИКА: Если это папка -> имя "Папки", иначе -> ваша строка по расширениям
                string folder = Directory.Exists(file) ? "Папки" :
                                ((ext == ".doc" || ext == ".docx" || ext == ".pdf") ? "Документы" :
                                ((ext == ".jpg" || ext == ".png") ? "Картинки" :
                                ((ext == ".exe") ? "Приложения" :
                                ((ext == ".pptx" || ext == ".ppt") ? "Документы" :
                                ((ext == ".lnk") ? "Приложения" : "Другое")))));

                Directory.CreateDirectory(Path.Combine(path, folder));

                // Универсальный перенос с защитой от существующих файлов и папок
                string destinationPath = Path.Combine(path, folder, name);

                if (Directory.Exists(file))
                {
                    // Безопасное перемещение папки (с объединением дубликатов)
                    MoveDirectoryWithMerge(file, destinationPath);
                }
                else if (File.Exists(file))
                {
                    // Если целевой файл уже существует, удаляем его перед переносом
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Move(file, destinationPath);
                }
            }
        }

        /// <block>
        /// Вспомогательный метод для безопасного перемещения папок слиянием
        /// </block>
        private static void MoveDirectoryWithMerge(string sourceDir, string targetDir)
        {
            // Если целевая папка не существует, создаем её
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Переносим все файлы
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string targetFile = Path.Combine(targetDir, Path.GetFileName(file));

                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile); // Перезаписываем дубликаты файлов
                }
                File.Move(file, targetFile);
            }

            // Рекурсивно переносим все подпапки
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string targetSubDir = Path.Combine(targetDir, Path.GetFileName(subDir));
                MoveDirectoryWithMerge(subDir, targetSubDir);
            }

            // Удаляем исходную папку, когда она опустеет
            if (Directory.Exists(sourceDir))
            {
                Directory.Delete(sourceDir, true);
            }
        }
    }
}
