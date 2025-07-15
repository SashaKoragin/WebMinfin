using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace ModelDataContext.ModelPeriod
{
    public class ModelPeriod : BindableBase
    {
        public ModelPeriod()
        {
            int currentYear = DateTime.Now.Year;
            _listYear = new List<int>();
            for (int year = 2022; year <= currentYear; year++)
            {
                _listYear.Add(year);
            }
            _dictionaryMonth = new List<DataMonth>()
            {
                new DataMonth() {IdMouth = 1, NameMouth = "Январь"},
                new DataMonth() {IdMouth = 2, NameMouth = "Февраль"},
                new DataMonth() {IdMouth = 3, NameMouth = "Март"},
                new DataMonth() {IdMouth = 4, NameMouth = "Апрель"},
                new DataMonth() {IdMouth = 5, NameMouth = "Май"},
                new DataMonth() {IdMouth = 6, NameMouth = "Июнь"},
                new DataMonth() {IdMouth = 7, NameMouth = "Июль"},
                new DataMonth() {IdMouth = 8, NameMouth = "Август"},
                new DataMonth() {IdMouth = 9, NameMouth = "Сентябрь"},
                new DataMonth() {IdMouth = 10, NameMouth = "Октябрь"},
                new DataMonth() {IdMouth = 11, NameMouth = "Ноябрь"},
                new DataMonth() {IdMouth = 12, NameMouth = "Декабрь"},
            };
            _selectYear = currentYear;
            _selectMonth = _dictionaryMonth.First(mouth => mouth.IdMouth == DateTime.Now.Month);
        }

        private List<int> _listYear;
        private List<DataMonth> _dictionaryMonth;
        private int _selectYear;
        private DataMonth _selectMonth;

        public int SelectYear
        {
            get { return _selectYear; }
            set
            {
                _selectYear = value;
                RaisePropertyChanged();
            }
        }

        public DataMonth SelectMonth
        {
            get { return _selectMonth; }
            set
            {
                _selectMonth = value;
                RaisePropertyChanged();
            }
        }

        public List<int> ListYear
        {
            get { return _listYear; }
            set
            {
                _listYear = value;
                RaisePropertyChanged();
            }
        }

        public List<DataMonth> DictionaryMonth
        {
            get { return _dictionaryMonth; }
            set
            {
                _dictionaryMonth = value;
                RaisePropertyChanged();
            }
        }
    }

    public class DataMonth
    {
        public int IdMouth { get; set; }
        public string NameMouth { get; set; }

    }
}
