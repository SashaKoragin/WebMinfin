using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WebMinFin.FullWebGlobalModelGraphQl;
using WebMinFin.ModelArrayGraphQl;


namespace WebMinFin.DataWebModel
{
    public class PostModelWebMinFin : WebMinFinAuthorization.WebMinFinAuthorization
    {

        public PostModelWebMinFin(X509Certificate2 certificate) : base(certificate)
        {
            Authorization();
            if (IsSessionOpen)
            {
                //Соединение открыто
            }
            else
            {
                //Соединение закрыто
            }
        }


        public void ReturnModelProject()
        {
            var fullModel = "{\"operationName\":\"DepartamentData\",\"variables\":{\"pagination\":{\"limit\":100,\"offset\":0},\"filter\":{}},\"query\":\"query DepartamentData($pagination: PaginationInput, $filter: DepartmentalProjectListFilterInput) {\\n departmentalProject {\\n list(pagination: $pagination, filter: $filter) {\\n data {\\n id\\n name\\n code\\n started\\n finished\\n dictProjectStageName\\n dictProjectStateName\\n leadOrganizations\\n versionNumber\\n isBlockedToEdit\\n __typename\\n      }\\n pagination {\\n total\\n left\\n __typename\\n      }\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
            //var filterModelProject = "{\"operationName\":\"DepartamentData\",\"variables\":{\"pagination\":{\"limit\":10,\"offset\":0},\"filter\":{\"nameOrCode\":\"34\"}},\"query\":\"query DepartamentData($pagination: PaginationInput, $filter: DepartmentalProjectListFilterInput) {\\n departmentalProject {\\n list(pagination: $pagination, filter: $filter) {\\n data {\\n id\\n name\\n code\\n started\\n finished\\n dictProjectStageName\\n dictProjectStateName\\n leadOrganizations\\n versionNumber\\n isBlockedToEdit\\n __typename\\n      }\\n pagination {\\n total\\n left\\n __typename\\n      }\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
            var project = ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<AllProject>>>>(fullModel);
            var codeSelect = project.data.departmentalProject.list.data.FirstOrDefault(projectSel => projectSel.code == "39315");
            var filterCardProject = "{\"operationName\":\"DepartmentalProjectCardTabs\",\"variables\":{\"id\":"+ codeSelect.id + "},\"query\":\"query DepartmentalProjectCardTabs($id: Long!) {\\n departmentalProject {\\n card(id: $id) {\\n id\\n name\\n started\\n finished\\n projectCode\\n isBlockedToEdit\\n isBlockedToCreateEdition\\n dictProjectStateName\\n versions {\\n id\\n versionNumber\\n number\\n actualityPeriod\\n projectId\\n versionIsActual\\n editions {\\n id\\n changeDate\\n changeSetNumber\\n createAuthor\\n createDate\\n name\\n projectId\\n kind {\\n id\\n name\\n shortName\\n __typename\\n          }\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n          }\\n createType\\n __typename\\n        }\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n        }\\n archivedEditions {\\n id\\n changeDate\\n changeSetNumber\\n archiveAuthorName\\n archiveDate\\n name\\n projectId\\n kind {\\n id\\n name\\n shortName\\n __typename\\n          }\\n createType\\n __typename\\n        }\\n createType\\n __typename\\n      }\\n versionNumber\\n versionId\\n isVersion\\n changeSetKindId\\n isCanSyncResultsWithEsr\\n isPrimaryVersion\\n allowedTabs\\n budgetCycle {\\n id\\n name\\n startYear\\n __typename\\n      }\\n editYears\\n nationalDevelopmentYears\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}";
          //  var filterReplace = string.Format(filterCardProject, codeSelect.id);
            var projectCard = ReturnModelPostMinfin<FullResponseWeb<GlobalModelWeb<List<Versions>>>>(filterCardProject);

        }

        //public string ReturnProject()
        //{
        //    string model = null;
            
        //    Request = (HttpWebRequest)WebRequest.Create(MinFinGraphQl);
        //    Request.Method = "POST";
        //    Request.KeepAlive = true;
        //    Request.AllowAutoRedirect = true;
        //    Request.ContentLength = DatesBytes.Length;
        //    Request.ContentType = "application/json";
        //    Request.Credentials = CredentialCache.DefaultCredentials;
        //    Request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
        //    Request.UseDefaultCredentials = true;
        //    Request.ClientCertificates.Add(Certificate);
        //    Request.Headers.Add("Cookie", LastCookieMinFin);
        //    Request.Referer = "https://gp.minfin.ru/";
        //    AddHeadersRequest();
        //    using (var stream = Request.GetRequestStream())
        //    {
        //        stream.Write(DatesBytes, 0, DatesBytes.Length);
        //    }
        //    try
        //    {
        //        Response = (HttpWebResponse)Request.GetResponse();
        //        if (Response.StatusCode != HttpStatusCode.OK) return null;
        //        using (var receiveStream = Response.GetResponseStream())
        //        {
        //            StreamReader readStream;
        //            readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
        //                ? new StreamReader(receiveStream)
        //                : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
        //            model = readStream.ReadToEnd();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Console.WriteLine(e);
        //        //throw;
        //    }

        //    return model;
        //}


    }
}
