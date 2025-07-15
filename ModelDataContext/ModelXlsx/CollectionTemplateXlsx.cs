using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AttributeHelperModelXml;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace ModelDataContext.ModelXlsx
{
    public class CollectionTemplateXlsx : BindableBase, IDataErrorInfo
    {

        public ICommand ClearAllModelCommand { get; }

        public CollectionTemplateXlsx()
        {
            ClearAllModelCommand = new DelegateCommand(ClearAllModel);
        }

        private ObservableCollection<ModelXlsx> _modelCollectionXlsx { get; set; } = new ObservableCollection<ModelXlsx>();
        private ModelXlsx _selectModel;


        /// <summary>
        /// Справочник наименовании документа
        /// </summary>
        public ObservableCollection<ModelXlsx> ModelCollectionXlsx
        {
            get { return _modelCollectionXlsx; }
            set
            {
                _modelCollectionXlsx = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Выбор модели
        /// </summary>
        public ModelXlsx SelectModel
        {
            get { return _selectModel; }
            set
            {
                _selectModel = value;
                RaisePropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get { return ValidateErrs(columnName); }
        }

        public string Error { get; set; }

        private bool IsValid { get; set; } = true;

        public bool IsValidation()
        {
            IsValid = false;
            RaisePropertyChanged("ModelCollectionXlsx");
            IsValid = String.IsNullOrEmpty(Error);
            return IsValid;
        }

        private string ValidateErrs(string columnName)
        {
            Error = null;
            if (!IsValid)
                switch (columnName)
                {
                    case "ModelCollectionXlsx":
                        if (ModelCollectionXlsx.Count <= 0)
                        { Error = "Не подгружен пользовательский шаблон!!!"; break; }

                        foreach (var modelXlsx in ModelCollectionXlsx)
                        {
                            if (!modelXlsx.IsValidation())
                            {
                                if (Error == null)
                                {
                                    {
                                        Error = modelXlsx.Error;
                                    }
                                }
                            }
                        }
                        break;
                }
            return Error;
        }

        /// <summary>
        /// Команда очистить файлы
        /// </summary>
        public void ClearAllModel()
        {
            ModelCollectionXlsx.Clear();
        }

        public void SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = true, Filter = "Типы файлов | *.xlsx" };
            if (openFileDialog.ShowDialog() == true)
            {
                ClearAllModel();
                var files = openFileDialog.FileNames.Select(name => new FileInfo(name)).ToArray();
                AddModelFile(files.Select(file => file.FullName).ToArray());
            }
        }

        /// <summary>
        /// Добавление файла в модель на ListView
        /// </summary>
        /// <param name="filesPath">Пути к файлам</param>
        public void AddModelFile(string[] filesPath)
        {
            foreach (var file in filesPath)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists && fileInfo.Extension == ".xlsx")
                {
                    try
                    {
                        XlsxToDataTable.XlsxToDataTable xlsxToDataTable = new XlsxToDataTable.XlsxToDataTable();
                        var dataTableXlsx = xlsxToDataTable.GetDateTableXslx(file, "Sheet1");
                        DataNamesMapper<ModelXlsx> mapper = new DataNamesMapper<ModelXlsx>();
                        var arrayModel = mapper.Map(dataTableXlsx).ToArray();
                        foreach (var modelXlsx in arrayModel)
                        {
                            ModelCollectionXlsx.Add(modelXlsx);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
