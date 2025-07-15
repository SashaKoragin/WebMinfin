using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebMinFin.WebMinFinAuthorization
{
    public class ParametersAuthorizationMinFin 
    {
      //  public string edirect_uri


        /// <summary>
        /// Класс для генерации параметров
        /// </summary>
        /// <param name="webLink">Ссылка с параметрами авторизации сайта МИНФИН</param>
        public ParametersAuthorizationMinFin(string webLink)
        {
            var x_client_ver = Regex.Match((webLink), @"x-client-ver=(.+)").Value;
            var x_client_verReplace = x_client_ver.Replace("x-client-ver=", "");

            var x_client_SKU = Regex.Match((webLink), @"x-client-SKU=(.+)&").Value.Replace(x_client_ver, "");
            var x_client_SKUReplace = x_client_SKU.Replace("x-client-SKU=", "").Replace("&", "");

            var state = Regex.Match((webLink), @"state=(.+)&").Value.Replace(x_client_SKU, "").Replace(x_client_ver, "");
            var stateReplace = state.Replace("state=", "").Replace("&", "");

            var nonce = Regex.Match((webLink), @"nonce=(.+)&").Value.Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
            var nonceReplace = nonce.Replace("nonce=", "").Replace("&", "");

            var scope = Regex.Match((webLink), @"scope=(.+)&").Value.Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
            var scopeReplace = scope.Replace("scope=", "").Replace("&", "");

            var response_type = Regex.Match((webLink), @"response_type=(.+)&").Value.Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
            var response_typeReplace = response_type.Replace("response_type=", "").Replace("&", "");

            var redirect_uri = Regex.Match((webLink), @"redirect_uri=(.+)&").Value.Replace(response_type, "").Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, ""); ;
            var redirect_uriReplace = redirect_uri.Replace("redirect_uri=", "").Replace("&", "");
        }

    }
}
