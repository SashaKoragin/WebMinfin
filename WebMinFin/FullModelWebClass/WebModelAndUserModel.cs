using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelDataContext.ModelXlsx;
using Prism.Mvvm;
using WebMinFin.ModelArrayGraphQl;

namespace WebMinFin.FullModelWebClass
{
    public class WebModelAndUserModel : BindableBase, IDataErrorInfo
    {
        private int _id;
        private string _name;
        private string _nameWebModel;
        private string _resultResponseEdit;
        private List<ModelXlsx> _modelCollectionXlsx;
        private ModelXlsx _modelXlsx;
        private values _values;
        private objects _objects;
        private checkpoints _checkpoints;
        private int? _idExecution;
        /// <summary>
        /// Ун контрольной точки
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Наименование контрольной точки
        /// </summary>
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
        /// Наименование моделей для Web сайта
        /// </summary>
        public string NameWebModel
        {
            get { return _nameWebModel; }
            set
            {
                _nameWebModel = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Результат обработки модели данных 
        /// </summary>
        public string ResultResponseEdit
        {
            get { return _resultResponseEdit; }
            set
            {
                _resultResponseEdit = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Контрольные точки для добавления
        /// </summary>
        public checkpoints Checkpoints
        {
            get { return _checkpoints; }
            set
            {
                _checkpoints = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Объект результата
        /// </summary>
        public objects Objects
        {
            get
            {
                return _objects;
            }
            set
            {
                _objects = value;
            }
        }
        /// <summary>
        /// Значения результата
        /// </summary>
        public values values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
        /// <summary>
        /// Ун объекта согласования
        /// </summary>
        public int? IdExecution
        {
            get { return _idExecution; }
            set
            {
                _idExecution = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Пользовательский справочник наименовании документа
        /// </summary>
        public List<ModelXlsx> ModelCollectionXlsx
        {
            get { return _modelCollectionXlsx; }
            set
            {
                _modelCollectionXlsx = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Пользовательская модель накладки данных
        /// </summary>
        public ModelXlsx ModelXlsx
        {
            get { return _modelXlsx; }
            set
            {
                _modelXlsx = value;
                RaisePropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get { return ValidateErrs(columnName); }
        }

        public string Error { get; set; }

        private bool IsValid { get; set; } = true;
        /// <summary>
        /// Проверка валидации наложения данных
        /// </summary>
        /// <returns></returns>
        public bool IsValidation()
        {
            IsValid = false;
            RaisePropertyChanged("ModelXlsx");
            if (!String.IsNullOrEmpty(Error))
            {
                return IsValid;
            }
            return IsValid;
        }

        private string ValidateErrs(string columnName)
        {
            Error = null;
            if (!IsValid)
                switch (columnName)
                {
                    case "ModelXlsx":
                        if (ModelXlsx == null)
                        {
                            Error = "Нет сопоставления шаблону с сайта!";
                            ResultResponseEdit = "Редактирование не отправлено!";
                        }
                        else
                        {
                            ResultResponseEdit = null;
                            IsValid = true;
                        }
                        break;
                }
            return Error;
        }
    }
}
