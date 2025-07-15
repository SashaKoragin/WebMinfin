using AttributeHelperModelXml;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Input;
using Prism.Commands;
using Microsoft.Win32;

namespace ModelDataContext.ModelXlsx
{
    public class ModelXlsx : BindableBase, IDataErrorInfo
    {

        public ICommand OpenFileDialogCommand { get; }

        public ICommand ClearAllFileCommand { get; }

        public ModelXlsx()
        {
            OpenFileDialogCommand = new DelegateCommand<ModelXlsx>(AddFile);
            ClearAllFileCommand = new DelegateCommand<ModelXlsx>(ClearAllFile);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterModel">Параметры модели</param>
        public void AddFile(ModelXlsx parameterModel)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Multiselect = true, Filter = "Типы файлов | *.docx; *.doc; *.pdf; *.odt" };
            if (openFileDialog.ShowDialog() == true)
            {
                var files = openFileDialog.FileNames.Select(name => new FileInfo(name)).ToArray();
                foreach (var fileInfo in files)
                {
                    if (parameterModel.Files is null)
                    {
                        parameterModel.Files = new ObservableCollection<ModelFile>();
                    }
                    if(!parameterModel.Files.Any(x => x.NameFile != null && x.NameFile == fileInfo.Name))
                       parameterModel.Files.Add(new ModelFile() { NameFile = fileInfo.Name, FullPath = fileInfo.FullName, Icon = Icon.ExtractAssociatedIcon(fileInfo.FullName), MimeFile = MimeMapping.GetMimeMapping(fileInfo.FullName), File = File.ReadAllBytes(fileInfo.FullName) });
                }
                parameterModel.IsValidation();
            }
        }
        /// <summary>
        /// Команда очистить файлы
        /// </summary>
        /// <param name="parameterMode">Параметры модели</param>
        public void ClearAllFile(ModelXlsx parameterMode)
        {
            parameterMode.Files?.Clear();
        }

        /// <summary>
        /// Справочник модель валидации
        /// </summary>
        private List<string> _modelValidation = new List<string>
        {
            "КТ",
            "КТ (прогнозные)",
            "КТ объекта результата",
            "КТ объекта результата (прогнозные)",
            "Объект результата",
            "Значение результата",
            "Результат"
        };

        /// <summary>
        /// Справочник видов документа
        /// </summary>
        private List<DirectoryVidDocuments> _arrayViewDocument = new List<DirectoryVidDocuments>
        {
            new DirectoryVidDocuments() { Id = 0, Name = "", PpNumber = 0},
            new DirectoryVidDocuments() { Id = 8, Name = "Письмо служебное", PpNumber = 8},
            new DirectoryVidDocuments() { Id = 18018024004, Name = "Протокол", PpNumber = 71},
            new DirectoryVidDocuments() { Id = 18018024014, Name = "Приказ", PpNumber = 81},
            new DirectoryVidDocuments() { Id = 16, Name = "План-график закупок", PpNumber = 16},
            new DirectoryVidDocuments() { Id = 17, Name = "Сведения о государственном контракте", PpNumber = 17},
            new DirectoryVidDocuments() { Id = 27, Name = "Акт приёма-передачи", PpNumber = 26},
            new DirectoryVidDocuments() { Id = 48, Name = "Платежное поручение (платежный документ)", PpNumber = 47},
            new DirectoryVidDocuments() { Id = 33, Name = "Документ о приёмке товаров, выполненной работы, оказанной услуги, в том числе в электронной форме", PpNumber = 32},
        };
        /// <summary>
        /// Справочник наименовании документа
        /// </summary>
        private List<DirectoryOrganization> _arrayTheReceivingBody = new List<DirectoryOrganization>
        {
            new DirectoryOrganization() {Id = 0, Name = "", ShortName = "", Inn = "", Kpp = ""},
            new DirectoryOrganization() {Id = 37213769920, Name = "ФЕДЕРАЛЬНАЯ НАЛОГОВАЯ СЛУЖБА", ShortName = "ФНС РОССИИ", Inn = "7707329152", Kpp = "770701001"},
            new DirectoryOrganization() {Id = 38295177296, Name = "МИНИСТЕРСТВО ФИНАНСОВ РОССИЙСКОЙ ФЕДЕРАЦИИ", ShortName = "МИНФИН РОССИИ", Inn = "7710168360", Kpp = "771001001"},
            new DirectoryOrganization() {Id = 38745130448, Name = "МИНИСТЕРСТВО ЮСТИЦИИ РОССИЙСКОЙ ФЕДЕРАЦИИ", ShortName = "МИНЮСТ РОССИИ", Inn = "7707211418", Kpp = "770601001"},
            new DirectoryOrganization() {Id = 37281252114, Name = "ПРАВИТЕЛЬСТВО РОССИЙСКОЙ ФЕДЕРАЦИИ", ShortName = "ПРАВИТЕЛЬСТВО РОССИЙСКОЙ ФЕДЕРАЦИИ"},
            new DirectoryOrganization() {Id = 8308707141, Name = "ПРЕЗИДЕНТ РОССИЙСКОЙ ФЕДЕРАЦИИ", ShortName = "ПРЕЗИДЕНТ РОССИЙСКОЙ ФЕДЕРАЦИИ"},
            new DirectoryOrganization() {Id = 40216203050, Name = "ФЕДЕРАЛЬНОЕ КАЗЕННОЕ УЧРЕЖДЕНИЕ \"НАЛОГ-СЕРВИС\" ФЕДЕРАЛЬНОЙ НАЛОГОВОЙ СЛУЖБЫ (Г. МОСКВА)", ShortName = "ФКУ \"НАЛОГ-СЕРВИС\" ФНС РОССИИ"},
        };


        private string _idTemplate;
        private string _content;
        private string _nameResult;       //Справочник ?
        private string _factParameter;    //Пока оставим строкой
        private string _forecastParameter; //Пока оставим строкой
        private DateTime? _dateExecution;
        private string _numberDocument;
        private DateTime? _dateDocument;
        private string _viewDocument;     //Справочник надо забрать
        private string _theReceivingBody; //Справочник надо забрать
        private DirectoryVidDocuments _viewVidDocument; //Справочник вид документа
        private DirectoryOrganization _theReceivingOrganization; //Справочник организаций выбор
        private string _name;
        private string _link;
        private ObservableCollection<ModelFile> _files;       //Файлы пока вопрос

        /// <summary>
        /// Справочник видов документа
        /// </summary>
        public List<DirectoryVidDocuments> ArrayViewDocument
        {
            get { return _arrayViewDocument; }
            set
            {
                _arrayViewDocument = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Справочник наименовании документа
        /// </summary>
        public List<DirectoryOrganization> ArrayTheReceivingBody
        {
            get { return _arrayTheReceivingBody; }
            set
            {
                _arrayTheReceivingBody = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Ун шаблона
        /// </summary>
        [DataNames("№ п/п")]
        public string IdTemplate
        {
            get { return _idTemplate; }
            set
            {
                _idTemplate = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Составляющие результата ВП ВПЦТ в ЭБ
        /// </summary>
        [DataNames("Составляющие результата ВП ВПЦТ в ЭБ")]
        public string NameResult
        {
            get { return _nameResult; }
            set
            {
                _nameResult = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Содержание
        /// </summary>
        [DataNames("Содержание")]
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
            }
            
        }
        /// <summary>
        /// Фактическое значение на конец отчетного периода
        /// </summary>
        [DataNames("Фактическое значение на конец отчетного периода")]
        public string FactParameter
        {
            get { return _factParameter; }
            set
            {
                _factParameter = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Прогнозное значение на конец текущего года
        /// </summary>
        [DataNames("Прогнозное значение на конец текущего года")]
        public string ForecastParameter
        {
            get { return _forecastParameter; }
            set
            {
                _forecastParameter = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Дата исполнения
        /// </summary>
        [DataNames("Дата исполнения")]
        public DateTime? DateExecution
        {
            get { return _dateExecution; }
            set
            {
                _dateExecution = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Номер документа
        /// </summary>
        [DataNames("Номер документа")]
        public string NumberDocument
        {
            get { return _numberDocument; }
            set
            {
                _numberDocument = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Дата документа
        /// </summary>
        [DataNames("Дата документа")]
        public DateTime? DateDocument
        {
            get { return _dateDocument; }
            set
            {
                _dateDocument = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Вид
        /// </summary>
        [DataNames("Вид")]
        public string ViewDocument
        {
            get { return _viewDocument; }
            set
            {
                _viewDocument = value;
                ViewVidDocument = ArrayViewDocument.FirstOrDefault(x => x.Name.ToLower() == _viewDocument.ToLower());
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Вид документа
        /// </summary>
        public DirectoryVidDocuments ViewVidDocument
        {
            get { return _viewVidDocument; }
            set
            {
                _viewVidDocument = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Принявший орган (Справочник)
        /// </summary>
        [DataNames("Принявший орган")]
        public string TheReceivingBody
        {
            get { return _theReceivingBody; }
            set
            {
                _theReceivingBody = value;
                TheReceivingOrganization = ArrayTheReceivingBody.FirstOrDefault(x => x.ShortName.ToLower() == _theReceivingBody.ToLower() || x.Name.ToLower() == _theReceivingBody.ToLower());
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Принявший орган (Справочник)
        /// </summary>
        public DirectoryOrganization TheReceivingOrganization
        {
            get { return _theReceivingOrganization; }
            set
            {
                _theReceivingOrganization = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        [DataNames("Наименование")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Ссылка
        /// </summary>
        [DataNames("Ссылка")]
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Файлы
        /// </summary>
        [DataNames("Файл")]
        public ObservableCollection<ModelFile> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                RaisePropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get { return ValidateErrs(columnName); }
        }

        public  string Error { get; set; }

        private bool IsValid { get; set; } = true;

        public bool IsValidation()
        {
            IsValid = false;
            RaisePropertyChanged($"IdTemplate");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"NameResult");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"FactParameter");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"ForecastParameter");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"DateExecution");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"NumberDocument");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"DateDocument");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"ViewVidDocument");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"TheReceivingOrganization");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"Name");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"Link");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            RaisePropertyChanged($"Files");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            return IsValid;
        }

        private string ValidateErrs(string columnName)
        {
            Error = null;
            if(!IsValid)
                switch (columnName)
                {
                    case "IdTemplate":
                        if (IdTemplate != null)
                        {
                            if (!Regex.IsMatch(IdTemplate, @"^\d+$"))
                            { Error = "Строка не является числом!!!"; }
                        }
                        else
                        { Error = "Ун шаблона не может быть равно NULL!!!"; }
                        break;
                    case "NameResult":
                        if (string.IsNullOrWhiteSpace(NameResult))
                        { Error = "Составляющие результата  не может быть равно NULL!!!"; }

                        if (_modelValidation.All(model => model != NameResult))
                        {
                            Error = "Наименование результата не соответствует справочнику ТЗ!";
                        }
                        break;
                    case "FactParameter":
                        if (NameResult != "Результат")
                        {
                            switch (FactParameter)
                            {
                                case "1":
                                    break;
                                case "0":
                                    break;
                                default:
                                {
                                    Error = "Фактический параметр должен быть равен 0 или 1";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(FactParameter))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "ForecastParameter":  //Надо разбираться
                        if (NameResult != "Результат")
                        {
                            if ( NameResult == "КТ" | NameResult == "КТ объекта результата" | NameResult == "КТ (прогнозные)" | NameResult == "КТ объекта результата (прогнозные)")
                            {
                                if (!string.IsNullOrWhiteSpace(ForecastParameter))
                                { Error = "У этой модели не может быть параметра"; }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ForecastParameter))
                                { break; }
                                { Error = "Прогнозное значение не может быть равно NULL!!!"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ForecastParameter))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "DateExecution": //Тоже нужны комментарии по дате
                        if (NameResult != "Результат")
                        {
                            if (DateExecution == null)
                            { Error = "Дата исполнения не может быть равно NULL!!!"; }
                        }
                        else
                        {
                            if (DateExecution != null)
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "NumberDocument":
                        if (NameResult != "Результат")
                        {
                            if (NameResult != "КТ (прогнозные)" || NameResult != "КТ объекта результата (прогнозные)")
                            {
                                if (!string.IsNullOrWhiteSpace(NumberDocument))
                                { break; }
                                { Error = "Номер документа не может быть равно NULL!!!"; }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(NumberDocument))
                                { Error = "У этой модели не может быть параметра"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(NumberDocument))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "DateDocument":
                        if (NameResult != "Результат")
                        {
                            if (NameResult != "КТ (прогнозные)" || NameResult != "КТ объекта результата (прогнозные)")
                            {
                                if (DateDocument == null)
                                { Error = "Дата документа не может быть равно NULL!!!"; }
                            }
                            else
                            {
                                if (DateDocument != null)
                                { Error = "У этой модели не может быть параметра"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(DateDocument?.ToString()))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "ViewVidDocument":
                        if (NameResult != "Результат")
                        { 
                            if ( NameResult != "КТ (прогнозные)" || NameResult != "КТ объекта результата (прогнозные)")
                            {
                                if (!string.IsNullOrWhiteSpace(ViewVidDocument?.Name))
                                { break; }
                                { Error = " Вид документа не может быть равно NULL!!!"; }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(ViewVidDocument?.Name))
                                { Error = "У этой модели не может быть параметра"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ViewVidDocument?.Name))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "TheReceivingOrganization":
                        if (NameResult != "Результат")
                        {
                            if (!string.IsNullOrWhiteSpace(TheReceivingOrganization?.ShortName))
                            { break; }
                            { Error = "Принявший орган не может быть равно NULL!!!"; }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(TheReceivingOrganization?.ShortName))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "Name":
                        if (NameResult != "Результат")
                        {
                            if (NameResult != "КТ (прогнозные)" || NameResult != "КТ объекта результата (прогнозные)")
                            {
                                if (!string.IsNullOrWhiteSpace(Name))
                                { break; }
                                { Error = "Наименование не может быть равно NULL!!!"; }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(Name))
                                { Error = "У этой модели не может быть параметра"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Name))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "Link":
                        if (NameResult != "Результат")
                        {
                            if ( NameResult != "КТ (прогнозные)" || NameResult != "КТ объекта результата (прогнозные)")
                            {
                               if (!string.IsNullOrWhiteSpace(Link))
                               { break; }
                               { Error = "Ссылка не может быть равно NULL!!!"; }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(Link))
                                { Error = "У этой модели не может быть параметра"; }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Link))
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                    case "Files":
                        if (NameResult != "Результат")
                        {
                            if (Link != null)
                            {
                                break;
                            }
                            if (Files?.Count > 0)
                            {
                                break;
                            }
                            if (Link == null && Files?.Count > 0)
                            {
                                 Error = "Файл или ссылка должны быть определены!!!"; 
                            }
                        }
                        else
                        {
                            if (Files?.Count > 0)
                            { Error = "У этой модели не может быть параметра"; }
                        }
                        break;
                }
            return Error;

        }
    }

    public class DirectoryVidDocuments
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int PpNumber { get; set; }


    }
    public class DirectoryOrganization
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Inn { get; set; }

        public string Kpp { get; set; }
    }
}
