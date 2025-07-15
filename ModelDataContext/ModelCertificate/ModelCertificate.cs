using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ModelDataContext.ModelCertificate
{
    public class ModelCertificate : BindableBase, IDataErrorInfo
    {
        /// <summary>
        /// Коллекция сертификатов на пользовательской машине
        /// </summary>
        private ObservableCollection<X509Certificate2> _collectionCertificate = new ObservableCollection<X509Certificate2>();
        /// <summary>
        /// Выбранный сертификат
        /// </summary>
        private X509Certificate2 _selectCertificate;
        public ObservableCollection<X509Certificate2> CollectionCertificate
        {
            get { return _collectionCertificate; }
            set
            {
                _collectionCertificate = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Выбранный сертификат
        /// </summary>
        public X509Certificate2 SelectCertificate
        {
            get { return _selectCertificate; }
            set
            {
                _selectCertificate = value;
                RaisePropertyChanged();
            }
        }


        public string Error { get; set; }
        private bool IsValid { get; set; } = true;
        public string this[string columnName]
        {
            get { return ValidateErrs(columnName); }
        }
        /// <summary>
        /// Проверка Validation
        /// </summary>
        /// <returns></returns>
        public bool IsValidationCertificate()
        {
            IsValid = false;
            RaisePropertyChanged("SelectCertificate");
            return IsValid;
        }

        /// <summary>
        /// Проверка выбрана ли схема
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string ValidateErrs(string columnName)
        {
            Error = null;
            if (!IsValid)
                switch (columnName)
                {
                    case "SelectCertificate":
                        if (SelectCertificate != null)
                        { IsValid = true; break; }
                        { Error = "Не выбран сертификат для доступа!!!"; break; }
                }
            return Error;
        }
    }

    public class AddModelCertificate : ModelCertificate
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public AddModelCertificate()
        {
            AddAllCertificate();
        }

        private void AddAllCertificate()
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser); //Сертификаты текущего пользователя
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates;
            List<X509Certificate2> globalCollection = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false).Cast<X509Certificate2>().ToList();
            foreach (var x509Certificate2 in globalCollection)
            {
                CollectionCertificate.Add(x509Certificate2);
            }
        }
    }
}
