using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using IniParser;
using IniParser.Model;

namespace MyDownloader.Core.Common
{
    public class CookieManager
    {
        // Cookies are now randomized in IE - http://blogs.msdn.com/b/ieinternals/archive/2011/08/12/internet-explorer-9.0.2-update-changes-file-protocol-and-cookie-naming.aspx

        public class CookieStruct
        {
            //1 The Variable Name
            //2 The Value for the Variable
            //3 The Website of the Cookie’s Owner
            //4 Optional Flags
            //5 The Most Significant Integer for Expired Time, in FILETIME Format
            //6 The Least Significant Integer for Expired Time, in FILETIME Format
            //7 The Most Significant Integer for Creation Time, in FILETIME Format
            //8 The Least Significant Integer for Creation Time, in FILETIME Format
            //9 The Cookie Record Delimiter (a * character)
            public string VarName;
            public string VarValue;
            public string Domain;
            public int Flags;
            public DateTime ExpirationDate;
            public DateTime CreationDate;
        }
        public static CookieCollection GetCookieCollection(string url)
        {
            CookieCollection ret = new CookieCollection();
            Uri location = new Uri(url);

            if (Settings.Default.CookieSource == "Internet Explorer")
            {
                BinaryReader br = new BinaryReader(File.Open(Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "/Low/index.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                byte[] indexfile = br.ReadBytes((int)br.BaseStream.Length);
                br.Close();
                int loc = 0;
                int locfile = 0;
                int endlocfile = 0;
                byte[] domainfind = Encoding.Default.GetBytes(location.Host.Replace("www.", ""));
                loc = indexfile.indexOf(domainfind);
                byte[] bfile;
                if (loc != -1)
                {
                    locfile = Array.IndexOf(indexfile, (byte)0xDE, loc);
                    endlocfile = Array.IndexOf(indexfile, (byte)0x2E, locfile);
                    bfile = indexfile.sub(locfile + 1, endlocfile);
                    CookieStruct[] cs = ReadCookie(Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "/Low/" + Encoding.Default.GetString(bfile) + ".txt");
                    foreach (CookieStruct c in cs)
                    {
                        Cookie nc = new Cookie(c.VarName.Trim(), c.VarValue.Trim());
                        nc.Expires = c.ExpirationDate;
                        nc.Domain = c.Domain;
                        ret.Add(nc);
                    }
                }
            }
            else if (Settings.Default.CookieSource == "Chrome")
            {
                string chromepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default";
                //string cookie = chromepath + @"\Cookies";
                if (!File.Exists(chromepath + @"\Cookies"))
                {
                    throw new Exception("Could not find Chrome cookie file!");
                }
                if (File.Exists(chromepath + @"\Cookies.dndm"))
                {
                    File.Delete(chromepath + @"\Cookies.dndm");
                }

                // We have to copy the cookies SQLite file because Chrome locks it
                File.Copy(chromepath + @"\Cookies", chromepath + @"\Cookies.dndm", true);

                SQLiteDatabase db = new SQLiteDatabase(chromepath + @"\Cookies.dndm");
                
                DataTable cookietable = db.GetDataTable(String.Format("SELECT * FROM cookies where host_key = \"{0}\" or host_key = \"{1}\" or host_key = \"{2}\" or host_key = \"{3}\"", location.Host, location.Host, location.Host.Replace("www.", ""), "." + location.Host));
                if (cookietable.Rows.Count > 0)
                {
                    foreach(DataRow dr in cookietable.Rows)
                    {
                        Cookie nc = new Cookie(dr["name"].ToString().Trim(), dr["value"].ToString().Trim());
                        nc.Expires = DateTimeExtension.FromUnixTime((Double.Parse(dr["expires_utc"].ToString()) / 1000000) - 11644473600);
                        nc.Domain = dr["host_key"].ToString();
                        ret.Add(nc);
                    }
                }
                
                File.Delete(chromepath + @"\Cookies.dndm");

            }
            else if (Settings.Default.CookieSource == "Firefox")
            {
                //Get default folder
                string firefoxpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\..\Roaming\Mozilla\Firefox";
                firefoxpath = Path.GetFullPath(firefoxpath);
                string profilesini = firefoxpath + @"\profiles.ini";
                FileIniDataParser iniparser = new FileIniDataParser();
                IniData inifile = iniparser.ReadFile(profilesini);
                string profilefolder = inifile["Profile0"]["Path"];
                string userfolder = firefoxpath + @"\" + profilefolder;
                
                string ffbookmarks = userfolder + @"\cookies.sqlite";
                string tmpffbook = userfolder + @"\cookies.dndm";
                File.Copy(ffbookmarks, tmpffbook, true);

                SQLiteDatabase db = new SQLiteDatabase(Path.GetFullPath(tmpffbook));
                DataTable cookietable = db.GetDataTable(String.Format("SELECT * FROM moz_cookies where host = \"{0}\" or host = \"{1}\" or host = \"{2}\" or host = \"{3}\"", location.Host, location.Host, location.Host.Replace("www.", ""), "." + location.Host));
                if (cookietable.Rows.Count > 0)
                {
                    foreach (DataRow dr in cookietable.Rows)
                    {
                        Cookie nc = new Cookie(dr["name"].ToString(), dr["value"].ToString());
                        nc.Expires = DateTimeExtension.FromUnixTime(Double.Parse(dr["expiry"].ToString()));
                        nc.Domain = dr["host"].ToString();
                        ret.Add(nc);
                    }
                }
                File.Delete(tmpffbook);
            }

            // Old way of finding cookies - no longer works due to IE randomization of cookie file names
            /*FileInfo[] fis = SearchCookies(Domain);
            foreach (FileInfo fi in fis)
            {
                CookieStruct[] cs = ReadCookie(fi.FullName);
                foreach(CookieStruct c in cs)
                    if (baseUrl.EndsWith(c.Domain) && c.ExpirationDate > DateTime.Now)
                    {
                        Cookie nc = new Cookie(c.VarName, c.VarValue);
                        nc.Expires = c.ExpirationDate;
                        nc.Domain = c.Domain;
                        ret.Add(nc);
                    }
            }*/

            
            return ret;
        }

        private static FileInfo[] SearchCookies(string Domain) 
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fis = di.GetFiles(string.Format("*@{0}[*.txt", Domain));
            di = new DirectoryInfo(path + "//Low");
            if (di.Exists)
            {
                FileInfo[] fisLow = di.GetFiles(string.Format("*@{0}[*.txt", Domain));
                FileInfo[] ret = new FileInfo[fis.Length + fisLow.Length];
                fis.CopyTo(ret, 0);
                fisLow.CopyTo(ret, fis.Length);
                return ret;
            }
            else
                return fis;
        }
      
        private static CookieStruct[] ReadCookie(string fn)
        {
            StreamReader sr = new StreamReader(fn);
            string cookie = sr.ReadToEnd();
            sr.Close();
            string[] data=cookie.Split('\n');
            int cookieNum = data.Length / 9;
            CookieStruct[] ret = new CookieStruct[cookieNum];
            for (int i = 0; i < cookieNum; i++)
            {
                int bline = i * 9;
                ret[i] = new CookieStruct();
                ret[i].VarName = data[bline];
                ret[i].VarValue = data[bline+1];
                ret[i].Domain = data[bline + 2];
                ret[i].Flags = Convert.ToInt32(data[bline + 3]);
                long l = Convert.ToInt64(data[bline + 4]);
                l += Convert.ToInt64(data[bline + 5]) << 32;
                ret[i].ExpirationDate = DateTime.FromFileTime(l);
                l = Convert.ToInt64(data[bline + 6]);
                l += Convert.ToInt64(data[bline + 7]) << 32;
                ret[i].CreationDate = DateTime.FromFileTime(l);
            }
            return ret;
        }
    
        //[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //static extern bool InternetGetCookie(string lpszUrlName, string lpszCookieName, [Out] StringBuilder lpszCookieData, [MarshalAs(UnmanagedType.U4)] ref int lpdwSize);
        //[DllImport("wininet.dll", EntryPoint = "InternetSetCookie", ExactSpelling = false, CharSet = CharSet.Unicode, SetLastError = true)]
        //static extern bool InternetSetCookie(string url, string cookieName, string cookieData);
       
        //const int ERROR_NO_MORE_ITEMS = 259;
        //const int ERROR_INSUFFICIENT_BUFFER = 122;
        //const int ERROR_INVALID_PARAMETER = 87;
        //const int INTERNET_COOKIE_HTTPONLY = 0x00002000;


        //public static void SetCookie(string siteUrl, string cookieName, string cookieData)
        //{
        //    if (!InternetSetCookie(siteUrl, cookieName, cookieData))
        //    {
        //        throw new Exception("Exception setting cookie: Win32 Error code=" + Marshal.GetLastWin32Error());
        //    }
        //}



        /*public static CookieContainer GetUriCookieContainer(string uri)
        {
            CookieContainer cookies = null;

            // Determine the size of the cookie
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookieEx(uri, null, cookieData, ref datasize, INTERNET_COOKIE_HTTPONLY, null))
            {
                int er = Marshal.GetLastWin32Error();
                if (er == ERROR_NO_MORE_ITEMS)
                    return new CookieContainer();
                else
                    if (er == ERROR_INSUFFICIENT_BUFFER)
                    {
                        cookieData = new StringBuilder(datasize);
                        if (!InternetGetCookie(uri, null, cookieData, ref datasize))
                            return new CookieContainer();
                    }
                    else
                        if (er == ERROR_INVALID_PARAMETER)
                            throw new Exception("InternetGetCookie: Invalid Parameter submitted.");
                        else
                            throw new Exception(String.Format("InternetGetCookie: Error. (Error Code: {0})", er));

            }
            cookies = new CookieContainer();
            cookies.SetCookies(new Uri(uri), cookieData.ToString().Replace(';', ','));
            return cookies;
        }*/
    }
}
