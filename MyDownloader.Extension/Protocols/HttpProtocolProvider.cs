using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using MyDownloader.Core;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using MyDownloader.Core.Common;

namespace MyDownloader.Extension.Protocols
{
    public class HttpProtocolProvider : BaseProtocolProvider, IProtocolProvider
    {
        private Downloader downloader;

        static HttpProtocolProvider()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(certificateCallBack);
        }

        static bool certificateCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void FillCredentials(HttpWebRequest request, ResourceLocation rl)
        {
            if (rl.Authenticate)
            {
                string login = rl.Login;
                string domain = string.Empty;

                int slashIndex = login.IndexOf('\\');

                if (slashIndex >= 0)
                {
                    domain = login.Substring(0, slashIndex );
                    login = login.Substring(slashIndex + 1);
                }

                NetworkCredential myCred = new NetworkCredential(login, rl.Password);
                myCred.Domain = domain;

                request.Credentials = myCred;
            }
        }

        #region IProtocolProvider Members

        public virtual void Initialize(Downloader d)
        {
            downloader = d;
        }

        private static CookieContainer GetUriCookieContainer(string uri)
        {
            CookieContainer cookies=new CookieContainer();
            foreach (Cookie c in CookieManager.GetCookieCollection(uri))
                cookies.SetCookies(new Uri(uri), c.ToString());
            return cookies;

        }

        private HttpWebRequest GetRequest(ResourceLocation rl)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetRequest(rl);
            request.CookieContainer = GetUriCookieContainer(rl.URL);
            return request;

        }
        public virtual Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
        {
            HttpWebRequest request = this.GetRequest(rl);

            FillCredentials(request, rl);

            if (initialPosition != 0)
            {
                if (endPosition == 0)
                {
                    request.AddRange((int)initialPosition);
                }
                else
                {
                    request.AddRange((int)initialPosition, (int)endPosition);
                }
            }

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        public virtual RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream)
        {
        restart:
            HttpWebRequest request = this.GetRequest(rl);

            FillCredentials(request, rl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            if (response.ResponseUri.AbsoluteUri != rl.URL)
            {
                rl.URL = response.ResponseUri.AbsoluteUri;
                goto restart;
            }
            RemoteFileInfo result = new RemoteFileInfo();
            string filename=response.ResponseUri.LocalPath;
            result.FileName = filename.Substring(filename.LastIndexOf("/") + 1);
            if ((result.FileName == "") && response.ContentType == "text/html")
                result.FileName = "temp.html";
            result.MimeType = response.ContentType;
            result.LastModified = response.LastModified;
            result.FileSize = response.ContentLength;
            result.AcceptRanges = String.Compare(response.Headers["Accept-Ranges"], "bytes", true) == 0;

            stream = response.GetResponseStream();

            return result;
        }

        #endregion
    }
}
