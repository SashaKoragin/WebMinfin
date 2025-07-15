using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;


namespace WebMinFin.WebMinFinAuthorization
{
    public class WebMinFinAuthorization : IDisposable
    {

        /// <summary>
        /// Признак открытия или закрытия сессии
        /// </summary>
        public bool IsSessionOpen { get; set; }

        private string WebSite = "https://gp.minfin.ru/";
        /// <summary>
        /// Получения данных из графа БД
        /// </summary>
        private string MinFinGraphQl = "https://gp.minfin.ru/graphql/";
        /// <summary>
        /// Ссылка для вкладки файла на сервер 
        /// </summary>
        private string AddFileToServer = "https://gp.minfin.ru/api-proxied/File";
        /// <summary>
        /// Параметры на отправку В МИНФИН
        /// </summary>
        private byte[] DatesBytes { get; set; }
        private X509Certificate2 Certificate { get; }
        /// <summary>
        /// Запрос на Сайт
        /// </summary>
        public HttpWebRequest Request { get; set; }
        /// <summary>
        /// Ответ с Сайта
        /// </summary>
        public HttpWebResponse Response { get; set; }

        /// <summary>
        /// Последняя Cookie сайта МИНФИН
        /// </summary>
        public string LastCookieMinFin { get; set; }
        /// <summary>
        /// Ответ по Ок
        /// </summary>
        private string DatesSite { get; set; }
        /// <summary>
        /// Куки сайта
        /// </summary>
        private List<string> CookieWeb { get; set; } = new List<string>();
        /// <summary>
        /// Заголовок сайта локация
        /// </summary>
        private List<string> HeaderLocation { get; set; } = new List<string>();
        /// <summary>
        /// Индекс Location
        /// </summary>
        private int IndexHeaderLocation { get; set; }

        /// <summary>
        /// Индекс Cookie
        /// </summary>
        private int IndexHeaderCookie { get; set; } = -1;


        public WebMinFinAuthorization(X509Certificate2 certificate)
        {
            Certificate = certificate;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificateReturn, chain, errors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls| SecurityProtocolType.Ssl3;
        }

        public void Authorization()
        {
            Request = (HttpWebRequest)WebRequest.Create(WebSite);
            Request.ProtocolVersion = HttpVersion.Version11;
            Request.Method = "GET";
            Request.AllowAutoRedirect = false;
            Request.Credentials = CredentialCache.DefaultCredentials;
            Request.ClientCertificates.Add(Certificate);
            //try
            //{
                Response = (HttpWebResponse) Request.GetResponse();
            //}
            //catch (WebException exceptionWeb)
            //{
                //Response = (HttpWebResponse)exceptionWeb.Response;
                if (Response.StatusCode == HttpStatusCode.OK || 
                    Response.StatusCode == HttpStatusCode.Redirect ||
                    Response.StatusCode == HttpStatusCode.Moved)
                {
                    //Response = (HttpWebResponse)exceptionWeb.Response;
                    var hostLocal1 = AnalysisParameter();                                    //Add https://poib.minfin.ru/identity/connect/authorize?client_id=gp.web&redirect_uri=http%3A%2F%2Fgp.minfin.ru%2Fidentity%2Fsignin-oidc&response_type=code&scope=openid%20profile%20roles%20role_ids%20usr_snils&nonce=638693515873171992.NzRjNzBmZTEtYzY3NC00YmI2LWEyODktY2I5MmIyYTVlNjkzNzM1OGU2YmMtMWU5Ny00MTBkLWFkNjItYzA1NDA2NTRlYmQ4&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&x-client-SKU=ID_NETSTANDARD2_0&x-client-ver=6.10.0.0
                    var hostLocal2 = RecursionParameters();                                  //Add https://poib.minfin.ru/identity/account/login?returnUrl=%2Fidentity%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dgp.web%26redirect_uri%3Dhttp%253A%252F%252Fgp.minfin.ru%252Fidentity%252Fsignin-oidc%26response_type%3Dcode%26scope%3Dopenid%2520profile%2520roles%2520role_ids%2520usr_snils%26nonce%3D638693515873171992.NzRjNzBmZTEtYzY3NC00YmI2LWEyODktY2I5MmIyYTVlNjkzNzM1OGU2YmMtMWU5Ny00MTBkLWFkNjItYzA1NDA2NTRlYmQ4%26state%3DCfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a%26x-client-SKU%3DID_NETSTANDARD2_0%26x-client-ver%3D6.10.0.0
                    AuthorizationParseMethod();                                              //Add https://poib.minfin.ru/identity/external/login?provider=TlsCertificate&returnUrl=/identity/connect/authorize/callback?client_id=gp.web&redirect_uri=http%3A%2F%2Fgp.minfin.ru%2Fidentity%2Fsignin-oidc&response_type=code&scope=openid%20profile%20roles%20role_ids%20usr_snils&nonce=638693515873171992.NzRjNzBmZTEtYzY3NC00YmI2LWEyODktY2I5MmIyYTVlNjkzNzM1OGU2YmMtMWU5Ny00MTBkLWFkNjItYzA1NDA2NTRlYmQ4&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&x-client-SKU=ID_NETSTANDARD2_0&x-client-ver=6.10.0.0
                    var hostLocal3 = RecursionParameters();                                  //Add https://cert.poib.minfin.ru/auth/authorize?redirect_uri=https%3A%2F%2Fpoib.minfin.ru%2Fidentity%2Fsignin-certificate&state=CfDJ8JAQMHR5dRVLk4A0l4ucs_ttmpivcsOybcUFkn4xYqagenlaCMEHccimsY57np_GYhm4Jv4SyeuYqkHjOSvR3qaMtYFRr5g8YMxlj7mQfHFRv2sJXafM6JcAsoT48wNqBXKGKxPpJjDVZNJZYDwUz9XWXnmLEWJ-qn6WR_ww3MCXGixBxPaTNrIx9kGa4Hs8exem83vsB5YqCCve2mWDRWbnqpvjMqVQ39sr-hwCP7IVmPuhVqTNq4a4B34aCZNnt_XSF6ewKxkQ64vSKw5uLxWNmuqnxkxCMQVTwp8xPEHoeMLU8XEgQ2N-ERyyS9WlUn_JtxWUbi43Fu8UQL9-c3zf6JUkxMKvJAFUMgVYA9qyQjgHJniXB56GpDIvobYC2cvT-2fUB1egeCtkx3rsZbM
                    var hostLocal4 = RecursionParameters();                                  //Add https://poib.minfin.ru/identity/signin-certificate?code=35cb4ea6dde9418f88488b4dab0612c7&state=CfDJ8JAQMHR5dRVLk4A0l4ucs_ttmpivcsOybcUFkn4xYqagenlaCMEHccimsY57np_GYhm4Jv4SyeuYqkHjOSvR3qaMtYFRr5g8YMxlj7mQfHFRv2sJXafM6JcAsoT48wNqBXKGKxPpJjDVZNJZYDwUz9XWXnmLEWJ-qn6WR_ww3MCXGixBxPaTNrIx9kGa4Hs8exem83vsB5YqCCve2mWDRWbnqpvjMqVQ39sr-hwCP7IVmPuhVqTNq4a4B34aCZNnt_XSF6ewKxkQ64vSKw5uLxWNmuqnxkxCMQVTwp8xPEHoeMLU8XEgQ2N-ERyyS9WlUn_JtxWUbi43Fu8UQL9-c3zf6JUkxMKvJAFUMgVYA9qyQjgHJniXB56GpDIvobYC2cvT-2fUB1egeCtkx3rsZbM
                    var hostLocal5 = RecursionParameters(true,-1,false,true);                  //Add https://poib.minfin.ru/identity/external/login/certificate?ReturnUrl=%2Fidentity%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dgp.web&redirect_uri=http%3A%2F%2Fgp.minfin.ru%2Fidentity%2Fsignin-oidc&response_type=code&scope=openid%20profile%20roles%20role_ids%20usr_snils&nonce=638693515873171992.NzRjNzBmZTEtYzY3NC00YmI2LWEyODktY2I5MmIyYTVlNjkzNzM1OGU2YmMtMWU5Ny00MTBkLWFkNjItYzA1NDA2NTRlYmQ4&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&x-client-SKU=ID_NETSTANDARD2_0&x-client-ver=6.10.0.0
                    var hostLocal6 = RecursionParameters(true);                  //Add https://poib.minfin.ru/identity/connect/authorize/callback?client_id=gp.web&redirect_uri=http%3A%2F%2Fgp.minfin.ru%2Fidentity%2Fsignin-oidc&response_type=code&scope=openid%20profile%20roles%20role_ids%20usr_snils&nonce=638693515873171992.NzRjNzBmZTEtYzY3NC00YmI2LWEyODktY2I5MmIyYTVlNjkzNzM1OGU2YmMtMWU5Ny00MTBkLWFkNjItYzA1NDA2NTRlYmQ4&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&x-client-SKU=ID_NETSTANDARD2_0&x-client-ver=6.10.0.0
                    var hostLocal7 = RecursionParameters();                                  //Add http://gp.minfin.ru/identity/signin-oidc?code=92766234B397285F76577FC3117B344F6E0FD32EC0A27A39B5F8447482664442&scope=openid%20profile%20roles%20role_ids%20usr_snils&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&session_state=oPgp67xFXegG3p1wRUG3DwB8IjdoW9n_oMx3UMgtl7I.0476AC0E4EE235CB1C22E9B8C39A3D0E
                    // var hostLocal8 = RecursionParameters();                                  //Add https://gp.minfin.ru/identity/signin-oidc?code=92766234B397285F76577FC3117B344F6E0FD32EC0A27A39B5F8447482664442&scope=openid%20profile%20roles%20role_ids%20usr_snils&state=CfDJ8HZ64PGCGsRPjG-4EDR7iFpM_u9GN2aYBWX3ZdJcxaTraXZ6ujyFvN9WffUUdSUPN9BxCyWbzuEaL8aobay3ybDLNuhRL-Oc3HL-O7ZGCgbxXaj0VnRHLAMae2SxtNpK5oFxKEdB1zeL_a3ZSVsZLB_LyPLV-HJ4jNj-aQlzCAwxKD6p8R6cCj38P7qySiQ_dZalGfmN9QrrBNF9LqN2DP9rUcHX2d28wppN-BCr9EkJ7pjKLbBnh_cIpg6SlU6jxG8WnM9LoZnXkfbs_NDwm5kYCpN4T2noXmripYGIF53a&session_state=oPgp67xFXegG3p1wRUG3DwB8IjdoW9n_oMx3UMgtl7I.0476AC0E4EE235CB1C22E9B8C39A3D0E
                    TestLastAuthorization();
                    //var hostLocal9 = RecursionParameters(false, 0);   //Add /
                    LastAuthorization();
            }
            //}
        }

        /// <summary>
        /// Тестовый метод на последние 2 запроса
        /// </summary>
        private void TestLastAuthorization()
        {
            Request = (HttpWebRequest)WebRequest.Create(HeaderLocation[IndexHeaderLocation]);
            Request.Proxy = new WebProxy("gp.minfin.ru");
            Request.ProtocolVersion = HttpVersion.Version11;
            Request.Method = "GET";
            Request.KeepAlive = true;
            Request.Host = "gp.minfin.ru";
            Request.AllowAutoRedirect = false;
            
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 YaBrowser/24.10.0.0 Safari/537.36";
            Request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            Request.Headers.Add("Accept-Language", "ru,en;q=0.9");
            Request.Headers.Add("Cache-Control", "no-cache");
            Request.Headers.Add("Pragma", "no-cache");
            Request.Headers.Add("Sec-Fetch-Dest", "document");
            Request.Headers.Add("Sec-Fetch-Mode", "navigate");
            Request.Headers.Add("Sec-Fetch-Site", "cross-site");
            Request.Headers.Add("Sec-Fetch-User", "?1");
            Request.Headers.Add("Upgrade-Insecure-Requests", "1");
            Request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"136\", \"YaBrowser\";v=\"25.6\", \"Not.A / Brand\";v=\"99\", \"Yowser\";v=\"2.5\"");
            Request.Headers.Add("sec-ch-ua-mobile", "?0");
            Request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            Request.Headers.Add("Upgrade-Insecure-Requests", "1");
            Request.Headers.Add("Cookie", CookieWeb[IndexHeaderCookie]);
            Request.Credentials = CredentialCache.DefaultNetworkCredentials;
            Request.ClientCertificates.Add(Certificate);
            Request.CookieContainer = new CookieContainer();
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode == HttpStatusCode.Redirect ||
                Response.StatusCode == HttpStatusCode.Moved ||
                Response.StatusCode == HttpStatusCode.Found ||
                Response.StatusCode == HttpStatusCode.OK)
            {
                var model = Response.Headers["Location"];
                Request = (HttpWebRequest)WebRequest.Create(model);
                Request.ProtocolVersion = HttpVersion.Version11;
                Request.Method = "GET";
                Request.KeepAlive = true;
                Request.Host = "gp.minfin.ru";
                Request.AllowAutoRedirect = false;

                Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
                Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 YaBrowser/24.10.0.0 Safari/537.36";
                Request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
                Request.Headers.Add("Accept-Language", "ru,en;q=0.9");
                Request.Headers.Add("Cache-Control", "no-cache");
                Request.Headers.Add("Pragma", "no-cache");
                Request.Headers.Add("Sec-Fetch-Dest", "document");
                Request.Headers.Add("Sec-Fetch-Mode", "navigate");
                Request.Headers.Add("Sec-Fetch-Site", "cross-site");
                Request.Headers.Add("Sec-Fetch-User", "?1");
                Request.Headers.Add("Upgrade-Insecure-Requests", "1");
                Request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"136\", \"YaBrowser\";v=\"25.6\", \"Not.A / Brand\";v=\"99\", \"Yowser\";v=\"2.5\"");
                Request.Headers.Add("sec-ch-ua-mobile", "?0");
                Request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                Request.Headers.Add("Upgrade-Insecure-Requests", "1");
                Request.Headers.Add("Cookie", CookieWeb[0]);
               // Request.Credentials = CredentialCache.;
                Request.ClientCertificates.Add(Certificate);
                Request.CookieContainer = new CookieContainer();
                try
                {
                    Response = (HttpWebResponse)Request.GetResponse();
                }
                catch (WebException exceptionWeb)
                {
                    Response = (HttpWebResponse)exceptionWeb.Response;
                    if (Response.StatusCode == HttpStatusCode.Redirect ||
                        Response.StatusCode == HttpStatusCode.Moved ||
                        Response.StatusCode == HttpStatusCode.Found)
                    {
                        using (var receiveStream = Response.GetResponseStream())
                        {
                            StreamReader readStream;
                            readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                                ? new StreamReader(receiveStream)
                                : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                            DatesSite = readStream.ReadToEnd();
                        }
                    }
                    //Ошибка
                    if (Response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        using (var receiveStream = Response.GetResponseStream())
                        {
                            StreamReader readStream;
                            readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                                ? new StreamReader(receiveStream)
                                : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                            DatesSite = readStream.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Последний запрос на авторизацию последняя Cookie проходная
        /// </summary>
        private void LastAuthorization()
        {
            Request = (HttpWebRequest)WebRequest.Create(WebSite);
            Request.ProtocolVersion = HttpVersion.Version11;
            Request.Method = "GET";
            Request.AllowAutoRedirect = false;
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 YaBrowser/24.10.0.0 Safari/537.36";
            Request.Headers.Add("Sec-Ch-Ua", "\"Chromium\";v=\"128\", \"Not; A=Brand\";v=\"24\", \"YaBrowser\";v=\"24.10\", \"Yowser\";v=\"2.5\"");
            Request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            Request.Headers.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            Request.Headers.Add("Upgrade-Insecure-Requests", "1");
            Request.Headers.Add("Cookie", CookieWeb[IndexHeaderCookie]);
            Request.Credentials = CredentialCache.DefaultNetworkCredentials;
            Request.ClientCertificates.Add(Certificate);
            Request.CookieContainer = new CookieContainer();
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode == HttpStatusCode.OK)
            {

                using (var receiveStream = Response.GetResponseStream())
                {
                    StreamReader readStream;
                    readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                        ? new StreamReader(receiveStream)
                        : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                    DatesSite = readStream.ReadToEnd();
                    LastCookieMinFin = CookieWeb[IndexHeaderCookie];
                    IsSessionOpen = true;
                }
            }
        }

        /// <summary>
        /// Первая ступень сайта парсим ссылку на странице
        /// </summary>
        private void AuthorizationParseMethod()
        {
            Request = (HttpWebRequest) WebRequest.Create(HeaderLocation[IndexHeaderLocation]);
            IndexHeaderLocation++;
            Request.ProtocolVersion = HttpVersion.Version11;
            Request.Method = "GET";
            Request.AllowAutoRedirect = true;
            Request.Credentials = CredentialCache.DefaultNetworkCredentials;
            Request.ClientCertificates.Add(Certificate);
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                string webPathLogin;
                using (var receiveStream = Response.GetResponseStream())
                {
                    StreamReader readStream;
                    readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                        ? new StreamReader(receiveStream)
                        : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                    DatesSite = readStream.ReadToEnd();
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(DatesSite);
                    webPathLogin = document.DocumentNode.SelectSingleNode(".//a[@class=\"btn btn--wide btn-primary provider--tlscertificate\"]").GetAttributeValue("href", "default").Replace("amp;", "");
                    var webResource = WebUtility.UrlDecode("https://poib.minfin.ru" + webPathLogin);
                    HeaderLocation.Add(webResource);
                }
            }
        }

        /// <summary>
        /// Запрос на пере направление на сайте МИНФИНА
        /// </summary>
        /// <param name="isCreateModel">Генерировать модель из параметров</param>
        /// <param name="indexCookie">Индекс Cookie</param>
        /// <param name="redirect">Перенаправление</param>
        private string RecursionParameters(bool isCreateModel = false, int indexCookie = -1, bool redirect = false, bool isCoder = false)
        {
            Request = (HttpWebRequest)WebRequest.Create(HeaderLocation[IndexHeaderLocation]);
            IndexHeaderLocation++;
            Request.ProtocolVersion = HttpVersion.Version11;
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 YaBrowser/24.12.0.0 Safari/537.36";
            Request.Method = "GET";
            Request.KeepAlive = true;
            Request.AllowAutoRedirect = redirect;
            Request.Credentials = CredentialCache.DefaultNetworkCredentials;
            Request.ClientCertificates.Add(Certificate);
            if (CookieWeb[IndexHeaderCookie] != null)
            {
                Request.Headers.Add("Cookie", CookieWeb[indexCookie >= 0? indexCookie : IndexHeaderCookie]);
            }
            try
            {
                Response = (HttpWebResponse) Request.GetResponse();
                if (Response.StatusCode == HttpStatusCode.Redirect ||
                    Response.StatusCode == HttpStatusCode.Moved ||
                    Response.StatusCode == HttpStatusCode.Found ||
                    Response.StatusCode == HttpStatusCode.OK)

                {
                    using (var receiveStream = Response.GetResponseStream())
                    {
                        StreamReader readStream;
                        readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                            ? new StreamReader(receiveStream)
                            : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                        DatesSite = readStream.ReadToEnd();
                    }
                }
            }
            catch (WebException exceptionWeb)
            {
                Response = (HttpWebResponse)exceptionWeb.Response;
                if (Response.StatusCode == HttpStatusCode.Redirect ||
                    Response.StatusCode == HttpStatusCode.Moved ||
                    Response.StatusCode == HttpStatusCode.Found)
                {
                    using (var receiveStream = Response.GetResponseStream())
                    {
                        StreamReader readStream;
                        readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                            ? new StreamReader(receiveStream)
                            : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                        DatesSite = readStream.ReadToEnd();
                    }
                }
                //Ошибка
                if (Response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    using (var receiveStream = Response.GetResponseStream())
                    {
                        StreamReader readStream;
                        readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                            ? new StreamReader(receiveStream)
                            : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                        DatesSite = readStream.ReadToEnd();
                    }
                }
            }
            return AnalysisParameter(isCreateModel, isCoder);
        }

        /// <summary>
        /// Анализ параметров для МИНФИН
        /// </summary>
        /// <param name="isCreateModel">Генерироавать модель из параметров</param>
        /// <param name="isCoder">Декодер</param>
        private string AnalysisParameter(bool isCreateModel = false, bool isCoder = false)
        {
            var locationHost = "";
            if (Response.Headers["Location"] != null)
            {
                if (isCreateModel)
                {

                    var x_client_ver = Regex.Match((HeaderLocation[0]), @"x-client-ver=(.+)").Value;
                    var x_client_verReplace = x_client_ver.Replace("x-client-ver=", "");

                    var x_client_SKU = Regex.Match((HeaderLocation[0]), @"x-client-SKU=(.+)&").Value.Replace(x_client_ver, "");
                    var x_client_SKUReplace = x_client_SKU.Replace("x-client-SKU=", "").Replace("&", "");

                    var state = Regex.Match((HeaderLocation[0]), @"state=(.+)&").Value.Replace(x_client_SKU, "").Replace(x_client_ver, "");
                    var stateReplace = state.Replace("state=", "").Replace("&", "");

                    var nonce = Regex.Match((HeaderLocation[0]), @"nonce=(.+)&").Value.Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
                    var nonceReplace = nonce.Replace("nonce=", "").Replace("&", "");

                    var scope = Regex.Match((HeaderLocation[0]), @"scope=(.+)&").Value.Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
                    var scopeReplace = scope.Replace("scope=", "").Replace("&", "");

                    var response_type = Regex.Match((HeaderLocation[0]), @"response_type=(.+)&").Value.Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
                    var response_typeReplace = response_type.Replace("response_type=", "").Replace("&", "");

                    var redirect_uri = Regex.Match((HeaderLocation[0]), @"redirect_uri=(.+)&").Value.Replace(response_type, "").Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, ""); ;
                    var redirect_uriReplace = redirect_uri.Replace("redirect_uri=", "").Replace("&", "");
                    if (isCoder)
                    {
                        response_type = Regex.Match((HeaderLocation[1]), @"response_type%3D(.+)%26nonce").Value.Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, "");
                        response_typeReplace = response_type.Replace("response_type%3D", "").Replace("%26nonce", "");
                        redirect_uri = Regex.Match((HeaderLocation[1]), @"redirect_uri%3D(.+)%26response").Value.Replace(response_type, "").Replace(scope, "").Replace(nonce, "").Replace(state, "").Replace(x_client_SKU, "").Replace(x_client_ver, ""); ;
                        redirect_uriReplace = redirect_uri.Replace("redirect_uri%3D", "").Replace("%26response", "");
                        locationHost = $"https://poib.minfin.ru{Response.Headers["Location"]}%26redirect_uri%3D{redirect_uriReplace}%26response_type%3D{response_typeReplace}%26nonce%3D{nonceReplace}%26state%3D{stateReplace}%26x-client-SKU%3D{x_client_SKUReplace}%26x-client-ver%3D{x_client_verReplace}";
                    }
                    else
                    {
                        locationHost = $"https://poib.minfin.ru{Response.Headers["Location"]}";
                    }
                    HeaderLocation.Add(locationHost);
                }
                else
                {
                    locationHost = Response.Headers["Location"];
                    HeaderLocation.Add(locationHost);
                }
            };
            if (Response.Headers["Set-Cookie"] != null)
            {
                CookieWeb.Add(Response.Headers["Set-Cookie"]);
                IndexHeaderCookie++;
            }

            return locationHost;
        }
        /// <summary>
        /// Запрос на данные по фильтру
        /// </summary>
        /// <param name="bodyModel">Фильтр GraphQl</param>
        /// <param name="isAdd">Признак запроса на добавление месяца и шода</param>
        /// <param name="month">Генерация в заголовке месяца</param>
        /// <param name="year">Генерация в заголовке года</param>
        /// <returns></returns>
        public T ReturnModelPostMinfin<T>(string bodyModel, bool isAdd = false, int month=1, int year=2022)
        {
            DatesBytes = Encoding.UTF8.GetBytes(bodyModel);
            Request = (HttpWebRequest)WebRequest.Create(MinFinGraphQl);
            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.AllowAutoRedirect = true;
            Request.ContentLength = DatesBytes.Length;
            Request.ContentType = "application/json";
            Request.Credentials = CredentialCache.DefaultCredentials;
            Request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
            Request.UseDefaultCredentials = true;
            Request.ClientCertificates.Add(Certificate);
            Request.Headers.Add("Cookie", LastCookieMinFin);
            if (isAdd)
            {
                Request.Headers.Add("monitoringParams", "{\"month\":"+month+",\"year\":"+year+"}");
                Request.Headers.Add("departmentId", "69");
            }
            Request.Referer = "https://gp.minfin.ru/";
            AddHeadersRequest();
            using (var stream = Request.GetRequestStream())
            {
                stream.Write(DatesBytes, 0, DatesBytes.Length);
            }
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode != HttpStatusCode.OK) return default;
            using (var receiveStream = Response.GetResponseStream())
            {
                    StreamReader readStream;
                    readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                        ? new StreamReader(receiveStream)
                        : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                    DatesSite = readStream.ReadToEnd();
            }
            return typeof(T).FullName == "System.String"
                ? (T) Convert.ChangeType(DatesSite, typeof(T))
                : JsonConvert.DeserializeObject<T>(DatesSite, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "dd.MM.yyyy"
                });
        }

        /// <summary>
        /// Запрос на данные по фильтру
        /// </summary>
        /// <param name="bodyModel">Фильтр GraphQl</param>
        /// <returns></returns>
        public T ReturnModelPostMinfinSendDpCheckpointExecution<T>(string bodyModel)
        {
            DatesBytes = Encoding.UTF8.GetBytes(bodyModel);
            Request = (HttpWebRequest)WebRequest.Create("https://gp.minfin.ru/api-proxied/BpInteraction/SendDpCheckpointExecution");
            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.AllowAutoRedirect = true;
            Request.ContentLength = DatesBytes.Length;
            Request.ContentType = "application/json";
            Request.Credentials = CredentialCache.DefaultCredentials;
            Request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
            Request.UseDefaultCredentials = true;
            Request.ClientCertificates.Add(Certificate);
            Request.Headers.Add("Cookie", LastCookieMinFin);
            Request.Referer = "https://gp.minfin.ru/";
            AddHeadersRequest();
            using (var stream = Request.GetRequestStream())
            {
                stream.Write(DatesBytes, 0, DatesBytes.Length);
            }
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode != HttpStatusCode.OK) return default;
            using (var receiveStream = Response.GetResponseStream())
            {
                StreamReader readStream;
                readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                    ? new StreamReader(receiveStream)
                    : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                DatesSite = readStream.ReadToEnd();
            }
            return typeof(T).FullName == "System.String"
                ? (T)Convert.ChangeType(DatesSite, typeof(T))
                : JsonConvert.DeserializeObject<T>(DatesSite, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "dd.MM.yyyy"
                });
        }


        
        /// <summary>
        /// Вложить файл на сервер
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bodyModel"></param>
        /// <returns></returns>
        public T AddFileWebMultipart<T>(string bodyModel)
        {
            DatesBytes = Encoding.Default.GetBytes(bodyModel);
            Request = (HttpWebRequest)WebRequest.Create(AddFileToServer);
            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.AllowAutoRedirect = true;
            Request.ContentLength = DatesBytes.Length;
            Request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundarycFtVFV4TNVCBg4gD";
            Request.Credentials = CredentialCache.DefaultCredentials;
            Request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
            Request.UseDefaultCredentials = true;
            Request.ClientCertificates.Add(Certificate);
            Request.Headers.Add("Cookie", LastCookieMinFin);
            Request.Referer = "https://gp.minfin.ru/";
            AddHeadersRequest(true);
            using (var stream = Request.GetRequestStream())
            {
                stream.Write(DatesBytes, 0, DatesBytes.Length);
            }
            Response = (HttpWebResponse)Request.GetResponse();
            if (Response.StatusCode != HttpStatusCode.OK) return default;
            using (var receiveStream = Response.GetResponseStream())
            {
                StreamReader readStream;
                readStream = String.IsNullOrWhiteSpace(Response.CharacterSet)
                    ? new StreamReader(receiveStream)
                    : new StreamReader(receiveStream, Encoding.GetEncoding(Response.CharacterSet));
                DatesSite = readStream.ReadToEnd();
            }
            return typeof(T).FullName == "System.String"
                ? (T)Convert.ChangeType(DatesSite, typeof(T))
                : JsonConvert.DeserializeObject<T>(DatesSite, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "dd.MM.yyyy"
                });
        }


        /// <summary>
        /// Добавление заголовков
        /// </summary>
        private void AddHeadersRequest(bool isFile=false)
        {
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 YaBrowser/24.12.0.0 Safari/537.36";
            Request.Host = "gp.minfin.ru";
            Request.Accept = "*/*";
            Request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            Request.Headers.Add("Accept-Language", "ru,en;q=0.9");
            Request.Headers.Add("Cache-Control", "no-cache");
            Request.Headers.Add("Origin", "https://gp.minfin.ru");
            Request.Headers.Add("Pragma", "no-cache");
            Request.Headers.Add("Sec-Ch-Ua", "\"Chromium\";v=\"128\", \"Not; A = Brand\";v=\"24\", \"YaBrowser\";v=\"24.10\", \"Yowser\";v=\"2.5\"");
            Request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            Request.Headers.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            Request.Headers.Add("Sec-Fetch-Dest", "empty");
            Request.Headers.Add("Sec-Fetch-Mode", "cors");
            Request.Headers.Add("Sec-Fetch-Site", "same-origin");
            if (isFile)
            {
                Request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }
        }

        public void Dispose()
        {
            Certificate.Dispose();
            Response.Dispose();
            IsSessionOpen = false;
        }
    }
}
