using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using UserInterfaceMinfin.Main.AddUserControlFull;

namespace UserInterfaceMinfin.Main.Mvvm
{
    public class DataContextWindowsMvvmAuto
    {
        public ICommand OpenForms { get; }

        public FullWindowInterfaceMethod FullWindow { get; }
        public DataContextWindowsMvvmAuto()
        {
            //PdfHelp help = new PdfHelp();
            var fullLogic = new AllLogicFull();
            FullWindow = fullLogic.FullWindowAdd();
            OpenForms = new DelegateCommand<object>(parameter => { FullWindow.IsCheked(parameter); });
            //OpenPdfHelp = new DelegateCommand(() => { help.OpenReport(ConfigFile.Help); });
            //User = $"Добро пожаловать {Environment.UserName}";
            //Web = ConfigFile.WebSite;
        }
    }
}
