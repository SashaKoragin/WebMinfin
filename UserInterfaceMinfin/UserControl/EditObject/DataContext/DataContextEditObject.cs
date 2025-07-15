using System.Windows.Input;
using ModelDataContext.ModelCertificate;
using ModelDataContext.ModelPeriod;
using ModelDataContext.ModelXlsx;
using Prism.Commands;
using WebMinFin.FullModelWebClass;
//using WebMinFin.TestDataStart;

namespace UserInterfaceMinfin.UserControl.EditObject.DataContext
{
    public class DataContextEditObject
    {
        public ICommand StartWebSite { get; }

        public ICommand IsExpandModel { get; }

        public ICommand DeleteModel { get; }

        public ICommand EditModel { get; }
        /// <summary>
        /// Выгрузить справочник виды
        /// </summary>
        public ICommand DownLoadVid { get; }
        /// <summary>
        /// Выгрузить справочник организаций
        /// </summary>
        public ICommand DownLoadOrganization { get; }
        /// <summary>
        /// Полное редактирование данных на сайте МИНФИН
        /// </summary>
        public ICommand FullEditDataModelWeb { get; }
        public AddModelCertificate ModelCertificate { get; set; }
        /// <summary>
        /// Период для запроса
        /// </summary>
        public ModelPeriod ModelPeriod { get; set; }
        /// <summary>
        /// Выбор файла по средством контекстного меню
        /// </summary>
        public ICommand SelectFileTemplateUserXlsx { get; set; }
        /// <summary>
        /// Отправка на согласование модели КТ
        /// </summary>
        public ICommand ExecutionCheckpoint { get; set; }

        /// <summary>
        /// Отправка на согласование модели по контексту
        /// </summary>
        public ICommand AllExecutionCheckpoint { get; set; }
        public CollectionTemplateXlsx CollectionTemplateXlsx { get; set; }
        /// <summary>
        /// Общая логика работы с сайтом
        /// </summary>
        public ModelWebRequestAndResponse ModelWebRequestAndResponse { get; set; }

        //public TestStartWeb ModelWebRequestAndResponse { get; set; }

        public DataContextEditObject()
        {
            //Тестовый код
            //ModelWebRequestAndResponse = new TestStartWeb();
            //StartWebSite = new DelegateCommand(() => { ModelWebRequestAndResponse.StartModelWeb(ModelCertificate, CollectionTemplateXlsx); });
            //
            
            ModelPeriod = new ModelPeriod();
            ModelCertificate = new AddModelCertificate();
            CollectionTemplateXlsx = new CollectionTemplateXlsx();
            ////Рабочий код
            ModelWebRequestAndResponse = new ModelWebRequestAndResponse();
            StartWebSite = new DelegateCommand(() => { ModelWebRequestAndResponse.StartModelWeb(ModelCertificate, CollectionTemplateXlsx, ModelPeriod); });
            ////
            IsExpandModel = new DelegateCommand<object>((parameter => ModelWebRequestAndResponse.AllModel.ExpandModel(parameter)));
            ExecutionCheckpoint = new DelegateCommand<object>((parameter => ModelWebRequestAndResponse.ExecutionReg(parameter)));
            AllExecutionCheckpoint = new DelegateCommand<string>(parameter=> ModelWebRequestAndResponse.AllExecutionRegModel(parameter));

            //DeleteModel = new DelegateCommand<object>(parameter => ModelWebRequestAndResponse.DeleteModelCheck(parameter));
            //EditModel = new DelegateCommand<object>(parameter => ModelWebRequestAndResponse.EditModelCheck(parameter));
            //DownLoadVid = new DelegateCommand(() => ModelWebRequestAndResponse.DownloadDirectoryVid());
            //DownLoadOrganization = new DelegateCommand(() => ModelWebRequestAndResponse.DownloadDirectoryOrganization());

            FullEditDataModelWeb = new DelegateCommand((() => ModelWebRequestAndResponse.EditFullModelWeb()));
            SelectFileTemplateUserXlsx = new DelegateCommand((() => CollectionTemplateXlsx.SelectFile()));
        }
    }
}
