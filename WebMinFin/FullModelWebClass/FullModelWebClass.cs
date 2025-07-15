using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using ModelDataContext.ModelCertificate;
using ModelDataContext.ModelPeriod;
using ModelDataContext.ModelXlsx;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prism.Mvvm;
using WebMinFin.DataWebModel;
using WebMinFin.FullWebGlobalModelGraphQl;
using WebMinFin.ModelArrayGraphQl;
using WebMinFin.ModelDirectoryWeb;
using WebMinFin.ModelDirectoryWeb.DirectoryDepartments;
using WebMinFin.ModelDirectoryWeb.DirectoryNpakinds;
using WebMinFin.ModelEditAndAdd;
using File = System.IO.File;

public class IncludeNullPropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> _propertiesToInclude;

    public IncludeNullPropertiesResolver(IEnumerable<string> propertiesToInclude)
    {
        _propertiesToInclude = new HashSet<string>(propertiesToInclude);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (_propertiesToInclude.Contains(property.PropertyName))
        {
            property.NullValueHandling = NullValueHandling.Include;
        }
        else
        {
            property.NullValueHandling = NullValueHandling.Ignore;
        }

        return property;
    }
}

namespace WebMinFin.FullModelWebClass
{
    public class FullModelWebClass : BindableBase
    {
        private FullResponseWeb<GlobalModelWeb<List<AllProject>>> _allProjectModel;

        private FullResponseWeb<GlobalModelWeb<List<Versions>>> _allVersionData;

        private FullResponseWeb<GlobalModelWeb<List<Tasks>>> _allTask;
        /// <summary>
        /// Пользовательская модель раскладки на КТ
        /// </summary>
        private ObservableCollection<WebModelAndUserModel> _webModelAndUserModel;
        /// <summary>
        /// Модели редактирования и обновления данных
        /// </summary>
        private ModelEditWeb _editModel;
        /// <summary>
        /// Справочник Вида документа
        /// </summary>
        private DirectoryWeb<DirectoryNpakinds> _directoryNpakinds;
        /// <summary>
        /// Справочник организаций
        /// </summary>
        private DirectoryWeb<DirectoryDepartments> _directoryDepartments;

        private Visibility _visibilityPlus = Visibility.Visible;
        private Visibility _visibilityMinus = Visibility.Collapsed;
        /// <summary>
        /// Все проекты модели
        /// </summary>
        public FullResponseWeb<GlobalModelWeb<List<AllProject>>> AllProjectModel
        {
            get { return _allProjectModel; }
            set
            {
                _allProjectModel = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Все версии объектов интереса
        /// </summary>
        public FullResponseWeb<GlobalModelWeb<List<Versions>>> AllVersionData
        {
            get { return _allVersionData; }
            set
            {
                _allVersionData = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Все внутренности задания объектов
        /// </summary>
        public FullResponseWeb<GlobalModelWeb<List<Tasks>>> AllTask
        {
            get { return _allTask; }
            set
            {
                _allTask = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Пользовательская модель раскладки на КТ
        /// </summary>
        public ObservableCollection<WebModelAndUserModel> WebModelAndUserModel
        {
            get { return _webModelAndUserModel; }
            set
            {
                _webModelAndUserModel = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Модель редактирования данных
        /// </summary>
        public ModelEditWeb EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged();
            }
        }

        public DirectoryWeb<DirectoryNpakinds> DirectoryNpakinds
        {
            get { return _directoryNpakinds; }
            set
            {
                _directoryNpakinds = value;
                RaisePropertyChanged();
            }
        }
        public DirectoryWeb<DirectoryDepartments> DirectoryDepartments
        {
            get { return _directoryDepartments; }
            set
            {
                _directoryDepartments = value;
                RaisePropertyChanged();
            }
        }
        public Visibility VisibilityPlus
        {
            get { return _visibilityPlus; }
            set
            {
                _visibilityPlus = value;
                RaisePropertyChanged();
            }
        }

        public Visibility VisibilityMinus
        {
            get { return _visibilityMinus; }
            set
            {
                _visibilityMinus = value;
                RaisePropertyChanged();
            }
        }


        public void ExpandModel(object parameter)
        {
            var id = (int) parameter;
        }

    }

    public class ModelWebRequestAndResponse : FullModelWebClass
    {
        /// <summary>
        /// Модель периода
        /// </summary>
        public ModelPeriod ModelPeriod { get; set; }

        /// <summary>
        /// Пользовательский шаблон для отправки данных
        /// </summary>
        public CollectionTemplateXlsx UserModelTemplateXlsx { get; set; }
        /// <summary>
        /// Класс взаимодействия с сайтом
        /// </summary>
        public PostModelWebMinFin PostModelWebMinFin { get; set; }
        /// <summary>
        /// Все модели с сайта МИНФИН
        /// </summary>
        public FullModelWebClass AllModel { get; set; } = new FullModelWebClass();
        /// <summary>
        /// Общий фильтр
        /// </summary>
        public string FilterModelProject = "{\"operationName\":\"DepartamentData\",\"variables\":{\"pagination\":{\"limit\":10,\"offset\":0},\"filter\":{\"nameOrCode\":\"39315\"}},\"query\":\"query DepartamentData($pagination: PaginationInput, $filter: DepartmentalProjectListFilterInput) {\\n departmentalProject {\\n list(pagination: $pagination, filter: $filter) {\\n data {\\n id\\n name\\n code\\n started\\n finished\\n dictProjectStageName\\n dictProjectStateName\\n leadOrganizations\\n versionNumber\\n isBlockedToEdit\\n __typename\\n      }\\n pagination {\\n total\\n left\\n __typename\\n      }\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Фильтр карточек версий проекта
        /// </summary>
        public string FilterCardProject = "{\"operationName\":\"DepartmentalProjectCardTabs\",\"variables\":{\"id\":{id}},\"query\":\"query DepartmentalProjectCardTabs($id: Long!) {\\n departmentalProject {\\n card(id: $id) {\\n id\\n name\\n started\\n finished\\n projectCode\\n isBlockedToEdit\\n isBlockedToCreateEdition\\n dictProjectStateName\\n versions {\\n id\\n versionNumber\\n number\\n actualityPeriod\\n projectId\\n versionIsActual\\n editions {\\n id\\n changeDate\\n changeSetNumber\\n createAuthor\\n createDate\\n name\\n projectId\\n kind {\\n id\\n name\\n shortName\\n __typename\\n          }\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n          }\\n createType\\n __typename\\n        }\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n        }\\n archivedEditions {\\n id\\n changeDate\\n changeSetNumber\\n archiveAuthorName\\n archiveDate\\n name\\n projectId\\n kind {\\n id\\n name\\n shortName\\n __typename\\n          }\\n createType\\n __typename\\n        }\\n createType\\n __typename\\n      }\\n versionNumber\\n versionId\\n isVersion\\n changeSetKindId\\n isCanSyncResultsWithEsr\\n isPrimaryVersion\\n allowedTabs\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n      }\\n editYears\\n nationalDevelopmentYears\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Фильтр значений объектов
        /// </summary>
        public string FilterTaskObject = "{\"operationName\":\"ResultAchievementsList\",\"variables\":{\"filter\":{\"projectId\":{id},\"projectKind\":\"DEPARTMENTAL_PROJECT\",\"year\":{year},\"month\":{mouth},\"period\":\"CURRENT_REPORT_PERIOD\",\"currentPeriod\":\"ALL\"}},\"query\":\"query ResultAchievementsList($filter: ExecutionMonitoringFilterInput) {\\n  executionMonitoring {\\n    tree(filter: $filter) {\\n      lastReportForPeriodInfo {\\n        id\\n        dictProjectStateId\\n        dictProjectStageId\\n        approvalDate\\n        __typename\\n      }\\n      tasks {\\n        id\\n        name\\n        results {\\n          id\\n          name\\n          responsibleExecutor\\n          isMonetary\\n          bpOkeiName\\n          versionMetaId\\n          dataSource\\n          executionsCount\\n          unpResultTypeCode\\n          execution {\\n            ...executionResult\\n            __typename\\n          }\\n          autocreateValue\\n          values {\\n            id\\n            name\\n            resultName\\n            resultId\\n            value\\n            responsibleExecutor\\n            date\\n            versionMetaId\\n            plannedDate\\n            autocreateValue\\n            canEditExecution\\n            canDeleteExecution\\n            canAddExecution\\n            resultValueYear\\n            hasCharacteristics\\n            needFinSourceWarning\\n            indicatorDynamics\\n            monthlyValues {\\n              month\\n              planValue\\n              __typename\\n            }\\n            tooltipInfo {\\n              addExecutionButton\\n              __typename\\n            }\\n            executionsCount\\n            execution {\\n              ...exec\\n              date\\n              __typename\\n            }\\n            checkpoints {\\n              ...checkpoint\\n              unpKtTypeId\\n              autocreateValue\\n              __typename\\n            }\\n            objects {\\n              id\\n              name\\n              value\\n              responsibleExecutor\\n              date\\n              versionMetaId\\n              bpOkeiName\\n              dataSource\\n              canEditExecution\\n              canDeleteExecution\\n              canAddExecution\\n              tooltipInfo {\\n                addExecutionButton\\n                __typename\\n              }\\n              executionsCount\\n              execution {\\n                ...executionObject\\n                date\\n                __typename\\n              }\\n              checkpoints {\\n                ...checkpoint\\n                __typename\\n              }\\n              autocreateValue\\n              __typename\\n            }\\n            __typename\\n          }\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\\nfragment exec on BaseExecutionOut {\\n  id\\n  delay\\n  status\\n  number\\n  achievementStatus\\n  statusType\\n  documentType\\n  type\\n  period\\n  dictProjectStageName\\n  dictProjectStateName\\n  parentId\\n  isAutoCreate\\n  manualEdit\\n  prove\\n  serviceExpired\\n  serviceForecast\\n  isHaveDocuments\\n  dictProjectStateId\\n  dictProjectStageId\\n  canSendForApproval\\n  __typename\\n}\\n\\nfragment executionObject on ObjectExecutionOut {\\n  id\\n  delay\\n  status\\n  number\\n  achievementStatus\\n  statusType\\n  documentType\\n  type\\n  period\\n  dictProjectStageName\\n  dictProjectStateName\\n  dictProjectStageId\\n  dictProjectStateId\\n  parentId\\n  isAutoCreate\\n  manualEdit\\n  prove\\n  serviceExpired\\n  serviceForecast\\n  isApprovedByResult\\n  isHaveDocuments\\n  canSendForApproval\\n  __typename\\n}\\n\\nfragment executionCheckpoint on CheckpointExecutionOut {\\n  id\\n  delay\\n  status\\n  number\\n  achievementStatus\\n  statusType\\n  documentType\\n  type\\n  period\\n  dictProjectStageName\\n  dictProjectStateName\\n  dictProjectStageId\\n  dictProjectStateId\\n  parentId\\n  isAutoCreate\\n  manualEdit\\n  prove\\n  serviceExpired\\n  serviceForecast\\n  isApprovedByResult\\n  isApprovedByObject\\n  incorrectConfirmingDocument\\n  incorrectExecutionDate\\n  incorrectOther\\n  commentDisagreement\\n  isHaveDocuments\\n  otpNumber\\n  bpUrl\\n  reasonOfEdit\\n  canSendForApproval\\n  __typename\\n}\\n\\nfragment executionResult on ResultExecutionOut {\\n  id\\n  delay\\n  status\\n  number\\n  achievementStatus\\n  statusType\\n  documentType\\n  type\\n  period\\n  dictProjectStageName\\n  dictProjectStateName\\n  parentId\\n  isAutoCreate\\n  manualEdit\\n  prove\\n  serviceExpired\\n  serviceForecast\\n  isHaveDocuments\\n  dictProjectStateId\\n  dictProjectStageId\\n  approvalDate\\n  canSendForApproval\\n  __typename\\n}\\n\\nfragment checkpoint on ExecutionMonitoringCheckpointOut {\\n  id\\n  name\\n  date\\n  value\\n  responsibleExecutor\\n  ktTypeDisplayName\\n  bpOkeiName\\n  dataSource\\n  type\\n  versionMetaId\\n  plannedDate\\n  unpKtTypeId\\n  autocreateValue\\n  canEditExecution\\n  canDeleteExecution\\n  canAddExecution\\n  tooltipInfo {\\n    addExecutionButton\\n    __typename\\n  }\\n  executionsCount\\n  execution {\\n    ...executionCheckpoint\\n    date\\n    __typename\\n  }\\n  __typename\\n}\\n\"}";
        /// <summary>
        /// Фильтр редактирования модели КТ и КТ объектов
        /// </summary>
        public string FilterEditModelCheckpoint = "{\"operationName\":\"CheckpointExecution\",\"variables\":{\"executionId\":{executionId},\"checkpointId\":{checkpointId},\"unpKtTypeId\":{unpKtTypeId},\"projectKind\":\"DEPARTMENTAL_PROJECT\"},\"query\":\"query CheckpointExecution($executionId: Long!, $checkpointId: Long, $unpKtTypeId: Long, $projectKind: ProjectItemKind!) {\\n  executions {\\n    checkpointExec(\\n      executionId: $executionId\\n      checkpointId: $checkpointId\\n      unpKtTypeId: $unpKtTypeId\\n      projectKind: $projectKind\\n    ) {\\n      id\\n      planValue\\n      factValue\\n      endDate\\n      executionDate\\n      delay\\n      type\\n      achievementStatus\\n      status\\n      number\\n      statusType\\n      documentType\\n      comment\\n      additionalInformation\\n      dictProjectStageName\\n      dictProjectStateName\\n      reasonOfEdit\\n      documents {\\n        id\\n        bpFileId\\n        orderNumber\\n        name\\n        number\\n        bpFileId\\n        authorDepartmentId\\n        isDSP\\n        npaKind {\\n          id\\n          name\\n          __typename\\n        }\\n        date\\n        link\\n        authorDepartment {\\n          id\\n          name\\n          __typename\\n        }\\n        authorDepartmentName\\n        files {\\n          id\\n          name\\n          url\\n          size\\n          __typename\\n        }\\n        __typename\\n      }\\n      bpAgreementsInformation {\\n        totalAgreementSum\\n        totalLboSum\\n        reportCountPlan\\n        providedReportCount\\n        unprovidedReportCount\\n        approvedGz\\n        subjectCount\\n        agreementConclusionCount\\n        agreementRejectedCount\\n        agreementNotConclusionCount\\n        reportMaxDate\\n        reasonDocuments {\\n          additionalAgreements {\\n            recepientInn\\n            recepientKpp\\n            npaSigningDate\\n            npaName\\n            npaNumber\\n            acceptedAgency\\n            npaState\\n            subjectName\\n            reportName\\n            hasReport\\n            reportPeriodType\\n            reportApprovalDate\\n            foivApprovalDate\\n            foivOrgCode\\n            foivName\\n            bpEntityId\\n            bpMainReportId\\n            bpTabReportId\\n            fileData {\\n              rejectionDate\\n              mbtDocumentId\\n              fileInfos {\\n                fileInfoId\\n                fileName\\n                key\\n                __typename\\n              }\\n              externalId\\n              __typename\\n            }\\n            bpEntityId\\n            subsidyAmount\\n            subAgreementAmount\\n            uniqueRegistryNumber\\n            inn\\n            kpp\\n            expensesName\\n            subAgreementConclusionDate\\n            agreementNumber\\n            recepientName\\n            bpFileId\\n            conclusionDate\\n            subjectOktmo\\n            kbkCode\\n            directionCostName\\n            mbtAmount\\n            npaKind\\n            registryNumber\\n            departmentCode\\n            departmentName\\n            approvalDate\\n            gzExecutionNumber\\n            rejectionMbtDate\\n            ikzFromPpg\\n            objectName\\n            finAmount1\\n            finAmount2\\n            finAmount3\\n            positionPgDate\\n            customerName\\n            isSmallProcurement\\n            contractRegistryNumber\\n            contractSignDate\\n            contractPrice\\n            stateContract\\n            eisContractUrl\\n            ikzFromGk\\n            documentNumber\\n            documentSignName\\n            documentType\\n            documentDraftDate\\n            commitmentCost\\n            factPayment\\n            registryNumberPositionPpgFromEis\\n            isCurrentBudgetCycle\\n            __typename\\n          }\\n          mainAgreement {\\n            recepientInn\\n            recepientKpp\\n            npaSigningDate\\n            npaName\\n            npaNumber\\n            acceptedAgency\\n            npaState\\n            subjectName\\n            reportName\\n            hasReport\\n            reportPeriodType\\n            reportApprovalDate\\n            foivApprovalDate\\n            foivOrgCode\\n            foivName\\n            bpEntityId\\n            bpMainReportId\\n            bpTabReportId\\n            fileData {\\n              rejectionDate\\n              mbtDocumentId\\n              fileInfos {\\n                fileInfoId\\n                fileName\\n                key\\n                __typename\\n              }\\n              externalId\\n              __typename\\n            }\\n            bpEntityId\\n            subsidyAmount\\n            subAgreementAmount\\n            uniqueRegistryNumber\\n            inn\\n            kpp\\n            expensesName\\n            subAgreementConclusionDate\\n            agreementNumber\\n            recepientName\\n            bpFileId\\n            conclusionDate\\n            subjectOktmo\\n            kbkCode\\n            directionCostName\\n            mbtAmount\\n            npaKind\\n            registryNumber\\n            departmentCode\\n            departmentName\\n            approvalDate\\n            gzExecutionNumber\\n            rejectionMbtDate\\n            ikzFromPpg\\n            objectName\\n            finAmount1\\n            finAmount2\\n            finAmount3\\n            positionPgDate\\n            customerName\\n            isSmallProcurement\\n            contractRegistryNumber\\n            contractSignDate\\n            contractPrice\\n            stateContract\\n            eisContractUrl\\n            ikzFromGk\\n            documentNumber\\n            documentSignName\\n            documentType\\n            documentDraftDate\\n            commitmentCost\\n            factPayment\\n            registryNumberPositionPpgFromEis\\n            isCurrentBudgetCycle\\n            __typename\\n          }\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Фильтр редактирования модели Значений результатов
        /// </summary>
        public string FilterEditModelValue = "{\"operationName\":\"ResultValueExecution\",\"variables\":{\"executionId\":{executionId},\"projectKind\":\"DEPARTMENTAL_PROJECT\"},\"query\":\"query ResultValueExecution($executionId: Long!, $projectKind: ProjectItemKind!) {\\n executions {\\n resultValueExec(executionId: $executionId, projectKind: $projectKind) {\\n id\\n planValue\\n factValue\\n forecastValue\\n endDate\\n executionDate\\n delay\\n type\\n achievementStatus\\n status\\n number\\n statusType\\n documentType\\n comment\\n additionalInformation\\n dictProjectStageName\\n dictProjectStateName\\n dynamics\\n warningMessage\\n dynamics\\n documents {\\n id\\n orderNumber\\n name\\n number\\n bpFileId\\n authorDepartmentId\\n isDSP\\n npaKind {\\n id\\n name\\n __typename\\n        }\\n date\\n link\\n authorDepartment {\\n id\\n name\\n __typename\\n        }\\n authorDepartmentName\\n files {\\n id\\n name\\n url\\n size\\n __typename\\n        }\\n __typename\\n      }\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Фильтр редактирования модели Объектов результата
        /// </summary>
        public string FilterEditModelObject = "{\"operationName\":\"ObjectExecution\",\"variables\":{\"executionId\":{executionId},\"projectKind\":\"DEPARTMENTAL_PROJECT\"},\"query\":\"query ObjectExecution($executionId: Long!, $projectKind: ProjectItemKind!) {\\n  executions {\\n    workPlanObjectExec(executionId: $executionId, projectKind: $projectKind) {\\n      id\\n      planValue\\n      factValue\\n      forecastValue\\n      endDate\\n      executionDate\\n      delay\\n      type\\n      achievementStatus\\n      status\\n      number\\n      statusType\\n      documentType\\n      comment\\n      additionalInformation\\n      dictProjectStageName\\n      dictProjectStateName\\n      documents {\\n        id\\n        orderNumber\\n        name\\n        number\\n        bpFileId\\n        authorDepartmentId\\n        isDSP\\n        npaKind {\\n          id\\n          name\\n          __typename\\n        }\\n        date\\n        link\\n        authorDepartment {\\n          id\\n          name\\n          __typename\\n        }\\n        authorDepartmentName\\n        files {\\n          id\\n          name\\n          url\\n          size\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";

        /// <summary>
        /// Выгрузка справочника вид
        /// </summary>
        public string DirectoryVid = "{\"operationName\":\"NpakindAutocomplete\",\"variables\":{\"pagination\":{\"limit\":200,\"offset\":0}},\"query\":\"query NpakindAutocomplete($pagination: PaginationInput, $filter: NpakindListFilterInput) {\\n  npakinds {\\n    list(pagination: $pagination, filter: $filter) {\\n      data {\\n        id\\n        name\\n        ppNumber\\n        __typename\\n      }\\n      pagination {\\n        total\\n        left\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Фильтр справочника Организаций
        /// </summary>
        public string DirectoryOrganization = "{\"operationName\":\"DepartmentsListAutocomplete\",\"variables\":{\"pagination\":{\"limit\":{limit},\"offset\":{offset}}},\"query\":\"query DepartmentsListAutocomplete($filter: DepartmentFilterInput, $pagination: PaginationInput) {\\n  departments {\\n    list(filter: $filter, pagination: $pagination) {\\n      data {\\n        id\\n        name\\n        shortName\\n        inn\\n        kpp\\n        deleted\\n        dateEnd\\n        __typename\\n      }\\n      pagination {\\n        left\\n        total\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";

        public string FileFormRequest = "------WebKitFormBoundarycFtVFV4TNVCBg4gD\r\n" + 
                                    "Content-Disposition: form-data; name=\"File\"; filename=\"{NameFileReport}\"\r\n" +
                                    "Content-Type: {TypeFileReport}\r\n\r\n" +
                                    "{FileReport}\r\n" +
                                    "------WebKitFormBoundarycFtVFV4TNVCBg4gD--";
        /// <summary>
        /// Обновление или редактирования модели Чекпоинтов
        /// </summary>
        public string UpdateAndAddModelCheckpoint = "{\"operationName\":\"CreateOrUpdateCheckpointExecution\",\"variables\":{Request},\"query\":\"mutation CreateOrUpdateCheckpointExecution($request: CheckpointExecutionCreateOrUpdateInInput) {\\n executions {\\n createOrUpdateCheckpointExec(request: $request)\\n __typename\\n  }\\n}\\n\"}";

        /// <summary>
        /// Обновление или редактирования модели Значение результата
        /// </summary>
        public string UpdateAndAddModelValue = "{\"operationName\":\"CreateOrUpdateResultValueExecution\",\"variables\":{Request},\"query\":\"mutation CreateOrUpdateResultValueExecution($request: ResultValueExecutionCreateOrUpdateInInput) {\\n    executions {\\n    createOrUpdateResultValueExec(request: $request)\\n    __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Обновление или редактирование объектов результата
        /// </summary>
        public string UpdateAndModelObject = "{\"operationName\":\"CreateOrUpdateObjectExecution\",\"variables\":{Request},\"query\":\"mutation CreateOrUpdateObjectExecution($request: WorkPlanObjectExecutionCreateOrUpdateInInput) {\\n    executions {\\n    createOrUpdateWorkPlanObjectExec(request: $request)\\n    __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// Запрос на удаление контрольной точки
        /// </summary>
        public string DeleteCheckpoint = "{\"operationName\":\"ExecutionDelete\",\"variables\":{RequestDelete},\"query\":\"mutation ExecutionDelete($request: ExecutionDeleteInInput) {\\n executions {\\n delete(request: $request)\\n __typename\\n  }\\n}\\n\"}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelCertificate">Модель сертификата</param>
        /// <param name="modelTemplateXlsx">Модель Excel</param>
        /// <param name="modelPeriod">Модель периодов</param>
        public void StartModelWeb(AddModelCertificate modelCertificate, CollectionTemplateXlsx modelTemplateXlsx, ModelPeriod modelPeriod)
        {
            ModelPeriod = modelPeriod;
            if (modelCertificate.IsValidationCertificate() & modelTemplateXlsx.IsValidation())
            {
                UserModelTemplateXlsx = modelTemplateXlsx;
                PostModelWebMinFin = new PostModelWebMinFin(modelCertificate.SelectCertificate);
                AllModel.AllProjectModel = PostModelWebMinFin.ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<AllProject>>>>(FilterModelProject);
                var codeSelect = AllModel.AllProjectModel.data.departmentalProject.list.data.FirstOrDefault(projectSel => projectSel.code == "39315");
                FilterCardProject = FilterCardProject.Replace("{id}", codeSelect.id.ToString());
                AllModel.AllVersionData = PostModelWebMinFin.ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<Versions>>>>(FilterCardProject);
                FilterTaskObject = FilterTaskObject.Replace("{id}", codeSelect.id.ToString())
                                                   .Replace("{year}", modelPeriod.SelectYear.ToString())
                                                   .Replace("{mouth}", modelPeriod.SelectMonth.IdMouth.ToString());
                AllModel.AllTask = PostModelWebMinFin.ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<Tasks>>>>(FilterTaskObject);
                WebModelTreeToTable(modelTemplateXlsx);
            }
        }

        /// <summary>
        /// Редактирование всей модели по шаблону пользователей
        /// Редактирование моделей работает не чего не ломаем
        /// </summary>
        public void EditFullModelWeb()
        {
            try
            {
                foreach (var modelUpdate in AllModel.WebModelAndUserModel)
                {
                    if (modelUpdate.IsValidation())
                    {
                        if (modelUpdate.Name == "КТ")
                        {
                            if (modelUpdate.Checkpoints.execution != null)
                            {
                                EditModelCheckpoints(modelUpdate.Checkpoints, modelUpdate.ModelXlsx); //Редактирование
                                modelUpdate.ResultResponseEdit = $"КТ под УН {modelUpdate.Checkpoints.id} успешно отредактировано!";
                            }
                            else
                            {
                                AddModelCheckpoint(modelUpdate.Checkpoints, modelUpdate.ModelXlsx);
                            }
                        }
                        if (modelUpdate.Name == "Значения результата")
                        {
                            if (modelUpdate.values.execution != null)
                            {
                                EditModelValue(modelUpdate.values, modelUpdate.ModelXlsx);
                                modelUpdate.ResultResponseEdit = $"Значения результата под УН {modelUpdate.values.id} успешно отредактировано!";
                            }
                            else
                            {
                                AddModelValue(modelUpdate.values, modelUpdate.ModelXlsx);
                            }
                        }
                        if (modelUpdate.Name == "Объект результата")
                        {
                            if (modelUpdate.Objects.execution != null)
                            {
                                EditModelObject(modelUpdate.Objects, modelUpdate.ModelXlsx);
                                modelUpdate.ResultResponseEdit = $"Объект результата под УН {modelUpdate.Objects.id} успешно отредактировано!";
                            }
                            else
                            {
                                AddModelObject(modelUpdate.Objects, modelUpdate.ModelXlsx);
                            }
                        }
                        if (modelUpdate.Name == "КТ Объекта результата")
                        {
                            if (modelUpdate.Checkpoints.execution != null)
                            {
                                EditModelCheckpoints(modelUpdate.Checkpoints, modelUpdate.ModelXlsx); //Редактирование
                                 modelUpdate.ResultResponseEdit = $"КТ Объекта результата под УН {modelUpdate.Checkpoints.id} успешно отредактировано!";
                            }
                            else
                            {
                                AddModelCheckpoint(modelUpdate.Checkpoints, modelUpdate.ModelXlsx);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Редактирование Объектов результата 
        /// </summary>
        /// <param name="parameter">Параметр Объектов результата</param>
        /// <param name="modelXlsx">Пользовательская модель редактирования Объектов результата</param>
        public void EditModelObject(object parameter, ModelXlsx modelXlsx)
        {
            var objects = (objects)parameter;

            var parameterObject = FilterEditModelObject.Replace("{executionId}", objects.execution.id.ToString());
            var dataEditObject = PostModelWebMinFin.ReturnModelPostMinfin<ModelEditWeb>(parameterObject);

            var listFile = UpdateToServerModelCheckPoint(modelXlsx);
            var model = GenerateModelAddObject(modelXlsx, dataEditObject, listFile, objects.id);
            var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                DateFormatString = "MM.dd.yyyy",
                ContractResolver = new IncludeNullPropertiesResolver(new[] {"authorDepartmentId",  "bpFileId", "parentId", "additionalInformation" })
            });
            modelUpdate = UpdateAndModelObject.Replace("{Request}", modelUpdate);
            //AddExecutionValue(objects.id, modelXlsx, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
            var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
        }
        /// <summary>
        /// Редактирование Значения результата
        /// </summary>
        /// <param name="parameter">Параметр Значения результата</param>
        /// <param name="modelXlsx">Пользовательская модель редактирования Значения результата</param>
        public void EditModelValue(object parameter, ModelXlsx modelXlsx)
        {
            var values = (values)parameter;
            
            var parameterValue = FilterEditModelValue.Replace("{executionId}", values.execution.id.ToString());
            var dataEditValue = PostModelWebMinFin.ReturnModelPostMinfin<ModelEditWeb>(parameterValue);

            var listFile = UpdateToServerModelCheckPoint(modelXlsx);

            var model = GenerateModelAddValue(modelXlsx, dataEditValue, listFile, values.id);
            var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                DateFormatString = "MM.dd.yyyy",
                ContractResolver = new IncludeNullPropertiesResolver(new[] { "comment", "additionalInformation" })
            });
            modelUpdate = UpdateAndAddModelValue.Replace("{Request}", modelUpdate);
            //AddExecutionValue(values.id, modelXlsx, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
            var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
        }

        /// <summary>
        /// Редактирование (КТ, КТ Объекта результата) Модели одинаковые
        /// </summary>
        /// <param name="parameter">Параметр (КТ, КТ Объекта результата)</param>
        /// <param name="modelXlsx">Пользовательская модель редактирования (КТ, КТ Объекта результата)</param>
        public void EditModelCheckpoints(object parameter, ModelXlsx modelXlsx)
        {
            var checkpoint = (checkpoints)parameter;

            var parameterCheckpoint = FilterEditModelCheckpoint
                .Replace("{executionId}", checkpoint.execution.id.ToString())
                .Replace("{checkpointId}", checkpoint.id.ToString())
                .Replace("{unpKtTypeId}", checkpoint.unpKtTypeId.ToString());
            var dataEditCheckpoints = PostModelWebMinFin.ReturnModelPostMinfin<ModelEditWeb>(parameterCheckpoint);

            var listFile = UpdateToServerModelCheckPoint(modelXlsx);

            var model = GenerateModelAddCheckpoint(modelXlsx, dataEditCheckpoints, listFile, checkpoint.id);
            var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                DateFormatString = "MM.dd.yyyy",
                ContractResolver = new IncludeNullPropertiesResolver(new[] { "bpFileId", "parentId", "additionalInformation" })
            });

            modelUpdate = UpdateAndAddModelCheckpoint.Replace("{Request}", modelUpdate);
            //AddExecutionValue(checkpoint.id, modelXlsx, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
            var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
        }

        /// <summary>
        /// Модель редактирования (КТ, КТ Объекта результата) 
        /// </summary>
        /// <param name="modelUser">Пользовательская модель для редактирования</param>
        /// <param name="parameter">Параметры редактирования</param>
        /// <param name="idFiles">Лист файлов</param>
        /// <param name="checkpointId">Ун КТ(для каждой разная)</param>
        /// <returns></returns>
        public ModelEditAndAdd.variables GenerateModelAddCheckpoint(ModelXlsx modelUser, ModelEditWeb parameter, List<int> idFiles, int checkpointId = 0)
        {
            var variables = new variables();
            variables.request = new request();
            variables.request.factValue = Convert.ToInt32(modelUser.FactParameter);
            variables.request.executionDate = modelUser.DateExecution; 
            variables.request.delay = parameter.data.executions.checkpointExec.delay;
            variables.request.type = parameter.data.executions.checkpointExec.type;
            variables.request.achievementStatus = parameter.data.executions.checkpointExec.achievementStatus;
            variables.request.status = parameter.data.executions.checkpointExec.status;
            variables.request.statusType = parameter.data.executions.checkpointExec.statusType;
            variables.request.documentType = parameter.data.executions.checkpointExec.documentType;
            variables.request.month = ModelPeriod.SelectMonth.IdMouth; //Месяц
            variables.request.year = ModelPeriod.SelectYear; //Год
            variables.request.comment = parameter.data.executions.checkpointExec.comment;
            variables.request.additionalInformation = parameter.data.executions.checkpointExec.additionalInformation;
            variables.request.id = parameter.data.executions.checkpointExec.id;
            variables.request.parentId = null;      //Не знаю
            variables.request.isAutoCreate = false; //Не знаю
            variables.request.manualEdit = true;    //Не знаю
            variables.request.prove = false;        //Не знаю
            variables.request.serviceExpired = false;         //Не знаю
            variables.request.serviceForecast = false;         //Не знаю
            variables.request.checkpointId = checkpointId;
            variables.request.projectKind = "DEPARTMENTAL_PROJECT"; //Не знаю
            if (parameter.data.executions.checkpointExec.documents.Count > 0)
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        id = parameter.data.executions.checkpointExec.documents[0].id,
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        isDSP = false,
                        fileIdList = idFiles
                    }
                };
            }
            else
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        isDSP = false,
                        fileIdList = idFiles
                    }
                };
            }
            return variables;
        }


        /// <summary>
        /// Модель редактирования (Значения результата) 
        /// </summary>
        /// <param name="modelUser">Пользовательская модель для редактирования</param>
        /// <param name="parameter">Параметры редактирования</param>
        /// <param name="idFiles">Лист файлов</param>
        /// <param name="checkpointId">Ун КТ(для каждой разная)</param>
        /// <returns></returns>
        public ModelEditAndAdd.variables GenerateModelAddValue(ModelXlsx modelUser, ModelEditWeb parameter, List<int> idFiles, int checkpointId)
        {
            var variables = new variables();
            variables.request = new request();
            variables.request.factValue = Convert.ToInt32(modelUser.FactParameter);
            variables.request.forecastValue = Convert.ToInt32(modelUser.ForecastParameter);
            variables.request.executionDate = modelUser.DateExecution;
            variables.request.delay = parameter.data.executions.resultValueExec.delay;
            variables.request.type = parameter.data.executions.resultValueExec.type;
            variables.request.achievementStatus = parameter.data.executions.resultValueExec.achievementStatus;
            variables.request.status = parameter.data.executions.resultValueExec.status;
            variables.request.statusType = parameter.data.executions.resultValueExec.statusType;
            variables.request.documentType = parameter.data.executions.resultValueExec.documentType;
            variables.request.month = ModelPeriod.SelectMonth.IdMouth; //Месяц
            variables.request.year = ModelPeriod.SelectYear; //Год
            variables.request.comment = parameter.data.executions.resultValueExec.comment;
            variables.request.additionalInformation = parameter.data.executions.resultValueExec.additionalInformation;
            variables.request.id = parameter.data.executions.resultValueExec.id;
            variables.request.parentId = null;      //Не знаю
            variables.request.isAutoCreate = false; //Не знаю
            variables.request.manualEdit = true;    //Не знаю
            variables.request.prove = false;        //Не знаю
            variables.request.serviceExpired = false;         //Не знаю
            variables.request.serviceForecast = false;         //Не знаю
            variables.request.projectTaskResultValueId = checkpointId;
            variables.request.projectKind = "DEPARTMENTAL_PROJECT"; //Не знаю
            if (parameter.data.executions.resultValueExec.documents.Count > 0)
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        id = parameter.data.executions.resultValueExec.documents[0].id,
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        bpFileId = null,
                        isDSP = false,
                        fileIdList = idFiles
                    }
                };
            }
            else
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        isDSP = false,
                        fileIdList = idFiles,
                        bpFileId = null,
                    }
                };
            }
            return variables;
        }

        /// <summary>
        /// Модель редактирования (Объект результата) 
        /// </summary>
        /// <param name="modelUser">Пользовательская модель для редактирования</param>
        /// <param name="parameter">Параметры редактирования</param>
        /// <param name="idFiles">Лист файлов</param>
        /// <param name="checkpointId">Ун КТ(для каждой разная)</param>
        /// <returns></returns>
        public ModelEditAndAdd.variables GenerateModelAddObject(ModelXlsx modelUser, ModelEditWeb parameter, List<int> idFiles, int checkpointId)
        {
            var variables = new variables();
            variables.request = new request();
            variables.request.factValue = Convert.ToInt32(modelUser.FactParameter);
            variables.request.forecastValue = Convert.ToInt32(modelUser.ForecastParameter);
            variables.request.executionDate = modelUser.DateExecution;
            variables.request.delay = parameter.data.executions.workPlanObjectExec.delay;
            variables.request.type = parameter.data.executions.workPlanObjectExec.type;
            variables.request.achievementStatus = parameter.data.executions.workPlanObjectExec.achievementStatus;
            variables.request.status = parameter.data.executions.workPlanObjectExec.status;
            variables.request.statusType = parameter.data.executions.workPlanObjectExec.statusType;
            variables.request.documentType = parameter.data.executions.workPlanObjectExec.documentType;
            variables.request.month = ModelPeriod.SelectMonth.IdMouth; //Месяц
            variables.request.year = ModelPeriod.SelectYear; //Год
            variables.request.comment = parameter.data.executions.workPlanObjectExec.comment;
            variables.request.additionalInformation = parameter.data.executions.workPlanObjectExec.additionalInformation;
            variables.request.id = parameter.data.executions.workPlanObjectExec.id;
            variables.request.parentId = null;      //Не знаю
            variables.request.isAutoCreate = false; //Не знаю
            variables.request.manualEdit = true;    //Не знаю
            variables.request.prove = false;        //Не знаю
            variables.request.serviceExpired = false;         //Не знаю
            variables.request.serviceForecast = false;         //Не знаю
            variables.request.workPlanObjectId = checkpointId;
            variables.request.projectKind = "DEPARTMENTAL_PROJECT"; //Не знаю
            if (parameter.data.executions.workPlanObjectExec.documents.Count > 0)
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        id = parameter.data.executions.workPlanObjectExec.documents[0].id,
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = null,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        bpFileId = null,
                        isDSP = false,
                        fileIdList = idFiles
                    }
                };
            }
            else
            {
                variables.request.documents = new List<documents>()
                {
                    new documents()
                    {
                        name = modelUser.Name,
                        number = modelUser.NumberDocument,
                        date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                        npaKindId = modelUser.ViewVidDocument.Id,
                        link = modelUser.Link,
                        authorDepartmentId = null,
                        authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                        isDSP = false,
                        fileIdList = idFiles,
                        bpFileId = null,
                    }
                };
            }
            return variables;
        }

        /// <summary>
        /// Отправка на Согласование
        /// </summary>
        /// <param name="executionObject">Объект Execution</param>
        public void ExecutionReg(object executionObject)
        {
            if (executionObject != null)
            {
                var executionId = (int)executionObject;
                var executionReg = "{\"executionIds\":[" + executionId + "],\"skipWarningControls\":false}";
                var returnModel = PostModelWebMinFin.ReturnModelPostMinfinSendDpCheckpointExecution<string>(executionReg);
                MessageBox.Show(returnModel);
            }
            else
            {
                MessageBox.Show("Данные для согласования отсутствуют!");
            }
        }

        /// <summary>
        /// Массовая отправка на согласование 
        /// </summary>
        /// <param name="modelCase">Наименование модели на согласование</param>
        public void AllExecutionRegModel(string modelCase)
        {
            if (AllModel.WebModelAndUserModel == null) return;
            if (AllModel.WebModelAndUserModel.Count <= 0) return;
            foreach (var resultExecution in AllModel.WebModelAndUserModel.Where(x => x.Name == modelCase))
            {
                ExecutionReg(resultExecution.IdExecution);
            }
        }



























        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="parameter"></param>
        public void DeleteModelCheck(object parameter)
        {
            var checkpoint = (checkpoints)parameter;
            var modelDelete = GenerateModelDeleteCheckpoint(checkpoint);
            var modelJson = JsonConvert.SerializeObject(modelDelete, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DateFormatString = "dd.MM.yyyy" });
            var modelDeleteCheckpoint = DeleteCheckpoint.Replace("{RequestDelete}", modelJson);
            var response = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelDeleteCheckpoint);
            AllModel.AllTask = PostModelWebMinFin.ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<Tasks>>>>(FilterTaskObject);

        }


        /// <summary>
        /// Добавление Объект результата
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="modelXlsx"></param>
        public void AddModelObject(object parameter, ModelXlsx modelXlsx)
        {
            //var objects = (objects)parameter;
            //var list = UpdateToServerModelCheckPoint(modelXlsx);
            //var model = GenerateModelAddObject(modelXlsx, objects, list);
            //var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DateFormatString = "MM.dd.yyyy" });
            //modelUpdate = UpdateAndAddModelValue.Replace("{Request}", modelUpdate);
            //var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate);
        }
        /// <summary>
        /// Добавление Значения результата
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="modelXlsx"></param>
        public void AddModelValue(object parameter, ModelXlsx modelXlsx)
        {
            //var values = (values)parameter;
            //var list = UpdateToServerModelCheckPoint(modelXlsx);

            //var model = GenerateModelAddValue(modelXlsx, values, list);
            //var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            //{
            //    DateFormatString = "MM.dd.yyyy",
            //    ContractResolver = new IncludeNullPropertiesResolver(new[] { "id", "forecastValue", "comment" })
            //});
            //modelUpdate = UpdateAndAddModelValue.Replace("{Request}", modelUpdate);
            //AddExecutionValue(values.id, modelXlsx, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
            //var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate, true, ModelPeriod.SelectMonth.IdMouth, ModelPeriod.SelectYear);
        }

        /// <summary>
        /// Добавление КТ и КТ Объекта результата
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="modelXlsx"></param>
        public void AddModelCheckpoint(object parameter, ModelXlsx modelXlsx)
        {
            var checkpoint = (checkpoints)parameter;
            var list = UpdateToServerModelCheckPoint(modelXlsx);
            //var model = GenerateModelAddCheckpoint(modelXlsx, checkpoint, list);
            //var modelUpdate = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, DateFormatString = "MM.dd.yyyy" });
            //modelUpdate = UpdateAndAddModelCheckpoint.Replace("{Request}", modelUpdate);
            //var returnModel = PostModelWebMinFin.ReturnModelPostMinfin<string>(modelUpdate);
        }



        public void AddExecutionValue(int projectTaskResultValueId, ModelXlsx modelUser, bool isAdd = false, int month = 1, int year = 2022)
        {
            var execution = "{\"operationName\":\"ExecutionItemPreview\",\"variables\":{\"request\":{\"projectKind\":\"DEPARTMENTAL_PROJECT\",\"projectId\":72,\"year\":" + year + ",\"month\":" + month + ",\"endDate\":\"" + modelUser.DateExecution?.ToString("MM.dd.yyyy") + "\",\"executionDate\":\"" + modelUser.DateExecution?.ToString("MM.dd.yyyy") + "\",\"resultValueId\":" + projectTaskResultValueId + ",\"forecastValue\":" + Convert.ToInt32(modelUser.ForecastParameter) + ",\"factValue\":" + Convert.ToInt32(modelUser.FactParameter) + ",\"planValueEndPeriod\":1,\"isNeedToBeFilled\":false}},\"query\":\"query ExecutionItemPreview($request: ExecutionItemPreviewInInput) {\\n executions {\\n itemPreview(request: $request) {\\n delay\\n type\\n achievementStatus\\n status\\n statusType\\n factValue\\n forecastValue\\n comment\\n executionDate\\n planValueEndPeriod\\n dynamics\\n warningMessage\\n forecastPeriodPlanValues {\\n factValue\\n month\\n planValue\\n __typename\\n      }\\n documents {\\n id\\n orderNumber\\n name\\n number\\n bpFileId\\n authorDepartmentId\\n isDSP\\n npaKind {\\n id\\n name\\n __typename\\n        }\\n copiedDocumentId\\n date\\n link\\n authorDepartment {\\n id\\n name\\n __typename\\n        }\\n files {\\n id\\n name\\n url\\n size\\n __typename\\n        }\\n authorDepartmentName\\n __typename\\n      }\\n additionalInformation\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
            var validate = "{\"operationName\":\"ValidateDataBeforeAddExecutions\",\"variables\":{\"request\":{\"entityIds\":[" + projectTaskResultValueId + "],\"executionEntityType\":\"RESULT_VALUES\",\"projectItemKind\":\"COMPLEX_MEASURE\"}},\"query\":\"query ValidateDataBeforeAddExecutions($request: PreSaveValidationQueryInInput) {\\n executions {\\n newExecutionPreSaveValidate(request: $request) {\\n descriptionValidation\\n hasWarnings\\n isSuccess\\n processInformationModels {\\n descriptionValidation\\n errorMessages\\n __typename\\n      }\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
            var executionValue = PostModelWebMinFin.ReturnModelPostMinfin<string>(execution, isAdd, month, year);
            var validateValue = PostModelWebMinFin.ReturnModelPostMinfin<string>(validate, isAdd, month, year);
        }


        /// <summary>
        /// Метод генерации модели редактирования и добавления
        /// </summary>
        /// <param name="modelUser"></param>
        /// <param name="modelEdit"></param>
        /// <param name="checkpointId"></param>
        /// <param name="idFiles"></param>
        /// <returns></returns>
        public ModelEditAndAdd.variables GenerateModelAddСheckpoint(ModelXlsx modelUser, ModelEditAndAdd.data modelEdit, int checkpointId, List<int> idFiles)
        {
            var variables = new variables();
            variables.request = new request();
            variables.request.factValue = modelEdit.executions.checkpointExec.factValue;
            variables.request.executionDate = modelEdit.executions.checkpointExec.executionDate;
            variables.request.delay = modelEdit.executions.checkpointExec.delay;
            variables.request.type = modelEdit.executions.checkpointExec.type;
            variables.request.achievementStatus = modelEdit.executions.checkpointExec.achievementStatus;
            variables.request.status = modelEdit.executions.checkpointExec.status;
            variables.request.statusType = modelEdit.executions.checkpointExec.statusType;
            variables.request.month = ModelPeriod.SelectMonth.IdMouth; //Месяц
            variables.request.year = ModelPeriod.SelectYear; //Год
            variables.request.documentType = modelEdit.executions.checkpointExec.documentType;
            variables.request.comment = modelEdit.executions.checkpointExec.comment;
            variables.request.id = modelEdit.executions.checkpointExec.id;
            variables.request.additionalInformation = null;
            variables.request.parentId = null;      //Не знаю
            variables.request.isAutoCreate = false; //Не знаю
            variables.request.manualEdit = true;    //Не знаю
            variables.request.prove = false;        //Не знаю
            variables.request.serviceExpired = false;         //Не знаю
            variables.request.serviceForecast = false;         //Не знаю
            variables.request.checkpointId = checkpointId;
            variables.request.projectKind = "DEPARTMENTAL_PROJECT"; //Не знаю
            variables.request.documents = new List<documents>()
            {
                new documents()
                {
                    //id = modelEdit.executions.checkpointExec.documents[0].id,
                    name = modelUser.Name,
                    number = modelUser.NumberDocument,
                    date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                    npaKindId = modelUser.ViewVidDocument.Id,
                    link = modelUser.Link,
                    authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                    authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                    isDSP = false,
                    fileIdList = idFiles
                }
            };

            return variables;
        }




        /// <summary>
        /// Обновление контрольной точки для модели
        /// </summary>
        /// <param name="modelUser"></param>
        public List<int> UpdateToServerModelCheckPoint(ModelXlsx modelUser)
        {
            List<int> modeFiles = new List<int>();
            if (modelUser.Files != null)
            {
                foreach (var file in modelUser.Files)
                {
                    var modelParameter = FileFormRequest.Replace("{NameFileReport}", file.NameFile)
                        .Replace("{TypeFileReport}", file.MimeFile)
                        .Replace("{FileReport}", Encoding.Default.GetString(file.File,0,file.File.Length));
                    var modelResponseFile =  PostModelWebMinFin.AddFileWebMultipart<ModelEditAndAdd.File>(modelParameter);
                    if (modelResponseFile.id != null)
                    {
                        var id = Convert.ToInt32(modelResponseFile.id);
                        modeFiles.Add(id);
                    }
                    
                }
            }
            return modeFiles;
        }




        /// <summary>
        /// Метод генерации модели редактирования и добавления
        /// </summary>
        /// <param name="modelUser"></param>
        /// <param name="modelEdit"></param>
        /// <param name="checkpointId"></param>
        /// <param name="idFiles"></param>
        /// <returns></returns>
        public ModelEditAndAdd.variables GenerateModelUpdateAndAdd(ModelXlsx modelUser, ModelEditAndAdd.data modelEdit, int checkpointId, List<int> idFiles)
        {
            var variables = new variables();
            variables.request = new request();
            variables.request.factValue = modelEdit.executions.checkpointExec.factValue;
            variables.request.executionDate = modelEdit.executions.checkpointExec.executionDate;
            variables.request.delay = modelEdit.executions.checkpointExec.delay;
            variables.request.type = modelEdit.executions.checkpointExec.type;
            variables.request.achievementStatus = modelEdit.executions.checkpointExec.achievementStatus;
            variables.request.status = modelEdit.executions.checkpointExec.status;
            variables.request.statusType = modelEdit.executions.checkpointExec.statusType;
            variables.request.month = ModelPeriod.SelectMonth.IdMouth; //Месяц
            variables.request.year = ModelPeriod.SelectYear; //Год
            variables.request.documentType = modelEdit.executions.checkpointExec.documentType;
            variables.request.comment = modelEdit.executions.checkpointExec.comment;
            variables.request.id = modelEdit.executions.checkpointExec.id;
            variables.request.additionalInformation = null;
            variables.request.parentId = null;      //Не знаю
            variables.request.isAutoCreate = false; //Не знаю
            variables.request.manualEdit = true;    //Не знаю
            variables.request.prove = false;        //Не знаю
            variables.request.serviceExpired = false;         //Не знаю
            variables.request.serviceForecast = false;         //Не знаю
            variables.request.checkpointId = checkpointId;
            variables.request.projectKind = "DEPARTMENTAL_PROJECT"; //Не знаю
            variables.request.documents = new List<documents>()
            {
                new documents()
                {
                    //id = modelEdit.executions.checkpointExec.documents[0].id,
                    name = modelUser.Name,
                    number = modelUser.NumberDocument,
                    date = modelUser.DateDocument.Value.ToString("yyyy-MM-dd"),
                    npaKindId = modelUser.ViewVidDocument.Id,
                    link = modelUser.Link,
                    authorDepartmentId = modelUser.TheReceivingOrganization.Id,
                    authorDepartmentName = modelUser.TheReceivingOrganization.Name,
                    isDSP = false,
                    fileIdList = idFiles
                }
            };
            
            return variables;
        }


        public ModelDelete.DeleteCheckpoint.variables GenerateModelDeleteCheckpoint(checkpoints idCheckpoint)
        {
            var variables = new ModelDelete.DeleteCheckpoint.variables();
            variables.request = new ModelDelete.DeleteCheckpoint.request();
            variables.request.entityType = "CHECKPOINT";
            variables.request.executionIds = new List<int>() {idCheckpoint.id};
            return variables;
        }




        /// <summary>
        /// Конвертация Tree модели сайта в табличный вид
        /// </summary>
        /// <param name="modelTemplateXlsx">Пользовательская модель данных</param>
        private void WebModelTreeToTable(CollectionTemplateXlsx modelTemplateXlsx)
        {
            AllModel.WebModelAndUserModel = new ObservableCollection<WebModelAndUserModel>();
            foreach (var objectResult in AllModel.AllTask.data.executionMonitoring.tree.tasks[0].results)
            {
                foreach (var values in objectResult.values)
                {
                    //Значение результата
                    WebModelAndUserModel newValueCheckpointsValue = new WebModelAndUserModel
                    {
                        Id = values.id,
                        Name = "Значения результата",
                        values = values,
                        NameWebModel = values.name,
                        ModelCollectionXlsx = modelTemplateXlsx.ModelCollectionXlsx.Where(x => x.NameResult == "Значение результата").ToList(),
                        IdExecution = values?.execution?.id
                    };
                    AllModel.WebModelAndUserModel.Add(newValueCheckpointsValue);
                    foreach (var checkpointValue in values.checkpoints)
                    {
                        //КТ
                        WebModelAndUserModel newValueCheckpoints = new WebModelAndUserModel
                        {
                            Id = checkpointValue.id,
                            Name = "КТ",
                            Checkpoints = checkpointValue,
                            NameWebModel = checkpointValue.name,
                            ModelCollectionXlsx = modelTemplateXlsx.ModelCollectionXlsx.Where(x => x.NameResult == "КТ").ToList(),
                            IdExecution = checkpointValue?.execution?.id
                        };
                        AllModel.WebModelAndUserModel.Add(newValueCheckpoints);
                    }
                    foreach (var objectModel in values.objects)
                    {
                        //Объект результата
                        WebModelAndUserModel newValueCheckpointsObject = new WebModelAndUserModel
                        {
                            Id = objectModel.id,
                            Name = "Объект результата",
                            Objects = objectModel,
                            NameWebModel = objectModel.name,
                            ModelCollectionXlsx = modelTemplateXlsx.ModelCollectionXlsx.Where(x => x.NameResult == "Объект результата").ToList(),
                            IdExecution = objectModel?.execution?.id
                        };
                        AllModel.WebModelAndUserModel.Add(newValueCheckpointsObject);
                        foreach (var checkpointObject in objectModel.checkpoints)
                        {
                            //КТ Объекта результата
                            WebModelAndUserModel newValueCheckpoints = new WebModelAndUserModel
                            {
                                Id = checkpointObject.id,
                                Name = "КТ Объекта результата",
                                Checkpoints = checkpointObject,
                                NameWebModel = checkpointObject.name,
                                ModelCollectionXlsx = modelTemplateXlsx.ModelCollectionXlsx.Where(x => x.NameResult == "КТ объекта результата").ToList(),
                                IdExecution = checkpointObject?.execution?.id
                            };
                            AllModel.WebModelAndUserModel.Add(newValueCheckpoints);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Класс выгрузки справочников с сайта минфина
    /// </summary>
    public class DownloadDirectory : ModelWebRequestAndResponse
    {
        /// <summary>
        /// Выгрузить справочник Вид
        /// </summary>
        public void DownloadDirectoryVid()
        {
            if (PostModelWebMinFin != null)
            {
                AllModel.DirectoryNpakinds = PostModelWebMinFin.ReturnModelPostMinfin<DirectoryWeb<DirectoryNpakinds>>(DirectoryVid);
                XmlSerializer writer = new XmlSerializer(typeof(DirectoryWeb<DirectoryNpakinds>));
                FileStream file = File.Create("C:\\Test\\DirectoryVid.xml");
                writer.Serialize(file, AllModel.DirectoryNpakinds);
                file.Close();
                file.Dispose();
            }
        }

        /// <summary>
        /// Выгрузить справочник Вид
        /// </summary>
        public void DownloadDirectoryOrganization()
        {
            int limitMax = 500000;
            var offset = 0;
            if (PostModelWebMinFin != null)
            {
                while (limitMax >= offset)
                {
                    var filterOrganization = DirectoryOrganization.Replace("{limit}", 100000.ToString()).Replace("{offset}", offset.ToString());
                    var model = PostModelWebMinFin.ReturnModelPostMinfin<DirectoryWeb<DirectoryDepartments>>(filterOrganization);
                    if (AllModel.DirectoryDepartments != null)
                    {
                        AllModel.DirectoryDepartments.data.departments.list.data.AddRange(model.data.departments.list.data);
                    }
                    else
                    {
                        AllModel.DirectoryDepartments = model;
                    }
                    offset += 100000;
                }
                XmlSerializer writer = new XmlSerializer(typeof(DirectoryWeb<DirectoryDepartments>));
                FileStream file = File.Create("C:\\Test\\DirectoryOrganization.xml");
                writer.Serialize(file, AllModel.DirectoryDepartments);
                file.Close();
                file.Dispose();
            }
        }
    }
}
