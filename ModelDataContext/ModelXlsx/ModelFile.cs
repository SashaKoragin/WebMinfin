using System.Drawing;

namespace ModelDataContext.ModelXlsx
{
    public class ModelFile
    {
        private Icon _icon;
        private string _nameFile;
        private string _fullPath;
        private string _mimeFile;
        private byte[] _file;


        /// <summary>
        /// Иконка файла
        /// </summary>
        public Icon Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string NameFile
        {
            get { return _nameFile; }
            set { _nameFile = value; }
        }

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string FullPath
        {
            get { return _fullPath; }
            set { _fullPath = value; }
        }

        /// <summary>
        /// Mime файла
        /// </summary>
        public string MimeFile
        {
            get { return _mimeFile; }
            set { _mimeFile = value; }
        }

        /// <summary>
        /// Файл
        /// </summary>
        public byte[] File
        {
            get { return _file; }
            set { _file = value; }
        }

    }
}
