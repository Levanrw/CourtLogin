using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace PAB.LoginInCourt
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }
        bool isrun = false;
        int repeat = 0;
        List<CourtUser> CourtUsers = new List<CourtUser>();
        string UserName { get; set; }
        string Password { get; set; }
        int _CourtUserId { get; set; }
        private async void Form2_Load(object sender, EventArgs e)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ecd.court.ge");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //setup login data
                var username = "002342907215194";
                var password = "574298";
                var formContent = new FormUrlEncodedContent(new[]
                {
            // new KeyValuePair<string, string>("grant_type", "password"),
             new KeyValuePair<string, string>("UserName", username),
             new KeyValuePair<string, string>("Password", password),
         });
                //send request
                HttpResponseMessage responseMessage = await client.PostAsync("/User/Login", formContent);
                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                //var jObject = JObject.Parse(responseJson);
                //var token = jObject.GetValue("access_token").ToString();
            }

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://ecd.court.ge");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    //setup login data
              
            //    //send request
            //    HttpResponseMessage responseMessage = await client.PostAsync("/User/LogOut", null);
            //    //get access token from response body
            //    var responseJson = await responseMessage.Content.ReadAsStringAsync();
            //    //var jObject = JObject.Parse(responseJson);
            //    //var token = jObject.GetValue("access_token").ToString();
            //}





            // string id = @"/Cabinet/Document?caseFileId=5754319";



            using (var context = new Analytics_NewEntities())
            {
                CourtUsers = context.CourtUsers.Select(c => c).Where(w=>w.ReadyForLogin==true).ToList<CourtUser>();

            }
            //foreach (var item in CourtUsers)
            {

                //UserName = item.UserName;
                //Password = item.Password;
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Url = new Uri("http://ecd.court.ge/User/Login");
                webBrowser1.Dock = DockStyle.Fill;
                //this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);

            }
        }
        private async void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)

        {
            try
            {
               // foreach (var user in CourtUsers)
                {
                    //  

                    if (!isrun || repeat == 1)
                    {
                        if (CourtUsers.Count > 0)
                        {
                            UserName = CourtUsers[0].UserName;
                            Password = CourtUsers[0].Password;
                            _CourtUserId = CourtUsers[0].Id;
                            CourtUsers.RemoveAt(0);
                        }

                        //
                        webBrowser1.Document.GetElementById("UserName").InnerText = UserName;// "263925509203355";
                        webBrowser1.Document.GetElementById("Password").InnerText = Password;// "324859";


                        HtmlElement form = webBrowser1.Document.GetElementById("LoginForm");
                        if (form != null)
                        {
                            form.InvokeMember("submit");
                        }
                        repeat = 0;
                        isrun = true;
                    }
                    else
                    {
                        using (var db = new Analytics_NewEntities())
                        {
                            var result = db.CourtUsers.SingleOrDefault(b => b.Id == _CourtUserId);
                            if (result != null)
                            {
                                result.Logined = true;
                                result.LastLoginDate = DateTime.Now;
                                db.SaveChanges();
                            }
                        }
                        //  webBrowser1.Document.Body.Style = "zoom:30%;";
                        List<string> ll = new List<string>();
                        if (webBrowser1.DocumentText.Contains("მომხმარებელი ან პაროლი არასწორია"))
                        {
                            using (var db = new Analytics_NewEntities())
                            {
                                var result = db.CourtUsers.SingleOrDefault(b => b.Id == _CourtUserId);
                                if (result != null)
                                {
                                    result.Authorized = false;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            var rr = webBrowser1.DocumentText.ToString();
                            //var docc = webBrowser1.Document;
                          

                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(rr);

                            List<List<string>> tableActs = doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseDocs']")
                                        .Descendants("tr")
                                        .Skip(1)
                                        .Where(tr => tr.Elements("td").Count() > 1)
                                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                                        
                                        .ToList();
                           

                                foreach (var item in tableActs)
                            {
                                DateTime date;                              
                                if (item[0].Trim().Length > 0)
                                    date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture);
                                else date = Convert.ToDateTime("1900.01.01");
                                string file = item[1];
                                using (var db = new Analytics_NewEntities())
                                {
                                    var data = db.Set<CourtSiteInfo>();
                                    data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = file, TabName = "სასამართლო აქტები", CourtUserId = _CourtUserId, SysDate = DateTime.Now });
                                    db.SaveChanges();
                                }

                            }

                           
                           
                            var gidAttribute ="<root>"+doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseDocs']").InnerHtml+"</root>";
                            XmlDocument xd = new XmlDocument();
                            xd.LoadXml(gidAttribute);

                            XDocument doc6 = XDocument.Parse(xd.InnerXml);
                         
                            IEnumerable<string> links = doc6.Descendants("a")
                          .Select(element => element.Attribute("href").Value);
                            foreach (var item in links)
                            {
                                item.Select(a => a.ToString());
                             bool saveFile=  await SaveUserFile(UserName, "სასამართლო აქტები", item);
                            }
                            



                            List<List<string>> tableFile = doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseFiles']")
                                        .Descendants("tr")
                                        .Skip(1)
                                        .Where(tr => tr.Elements("td").Count() > 1)
                                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                                        .ToList();

                            foreach (var item in tableFile)
                            {
                                DateTime date;
                                if (item[0].Trim().Length > 0)
                                    date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture);
                                else date = Convert.ToDateTime("1900.01.01");
                              //   date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                    // System.Globalization.CultureInfo.InvariantCulture);
                                string file = item[1];

                                using (var db = new Analytics_NewEntities())
                                {
                                    var data = db.Set<CourtSiteInfo>();
                                    data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = file, TabName = "წარმოდგენილი ფაილები", CourtUserId = _CourtUserId, SysDate = DateTime.Now });
                                    db.SaveChanges();
                                }
                            }


                            var gidAttribute1 = "<root>" + doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseFiles']").InnerHtml + "</root>";
                            XmlDocument xd1 = new XmlDocument();
                            xd1.LoadXml(gidAttribute1);

                            XDocument doc61 = XDocument.Parse(xd1.InnerXml);

                            IEnumerable<string> links1 = doc61.Descendants("a")
                          .Select(element => element.Attribute("href").Value);
                            foreach (var item in links1)
                            {
                                item.Select(a => a.ToString());
                                bool saveFile = await SaveUserFile(UserName, "სასამართლო აქტები", item);
                            }


                            List<List<string>> tableHearings = doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseCourtSession']")
                                        .Descendants("tr")
                                        .Skip(1)
                                        .Where(tr => tr.Elements("td").Count() > 1)
                                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                                        .ToList();

                            foreach (var item in tableHearings)
                            {
                                DateTime date;
                                if (item[0].Trim().Length > 0)
                                    date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture);
                                else date = Convert.ToDateTime("1900.01.01");
                                string file = item[1];

                                using (var db = new Analytics_NewEntities())
                                {
                                    var data = db.Set<CourtSiteInfo>();
                                    data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = file, TabName = "სასამართლო სხდომები", CourtUserId = _CourtUserId, SysDate = DateTime.Now });
                                    db.SaveChanges();
                                }
                            }


                            var gidAttribute12 = "<root>" + doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseCourtSession']").InnerHtml + "</root>";
                            XmlDocument xd12 = new XmlDocument();
                            xd12.LoadXml(gidAttribute12);

                            XDocument doc612 = XDocument.Parse(xd1.InnerXml);

                            IEnumerable<string> links12 = doc612.Descendants("a")
                          .Select(element => element.Attribute("href").Value);
                            foreach (var item in links12)
                            {
                                item.Select(a => a.ToString());
                                bool saveFile = await SaveUserFile(UserName, "სასამართლო სხდომები", item);
                            }


                            using (var db = new Analytics_NewEntities())
                            {
                                var result = db.CourtUsers.SingleOrDefault(b => b.Id == _CourtUserId);
                                if (result != null)
                                {
                                    result.Authorized = true;
                                    db.SaveChanges();
                                }
                            }
                            if (CourtUsers.Count > 0)
                            {
                                webBrowser1.Url = new Uri("http://ecd.court.ge/User/Login");
                                repeat = 1;
                            }
                            if (CourtUsers.Count == 0)
                            {
                                this.Close();
                            }

                        }

                        //this.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                this.Close();
            }


        }
        public async Task<bool> SaveUserFile(string userName, string panelName, string postLink)
        {
            try
            {
                int start = postLink.IndexOf('=') + 1;
                string _id = postLink.Substring(start, postLink.Length + -start);
                var baseAddress = new Uri("http://ecd.court.ge");
                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
        new KeyValuePair<string, string>("caseFileId", _id),
    });
                    var test = $"http://ecd.court.ge{postLink}";
                    cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", "j1lz2qxrsaw4kpzqegnnehvs"));//j1lz2qxrsaw4kpzqegnnehvs
                    var result = await client.PostAsync($"http://ecd.court.ge{postLink}", content);
                  //  System.IO.Directory.CreateDirectory(Server.MapPath($"C:\\Users\\L.Shanava\\Downloads\\{userName}\\{panelName}));
                    var file = result.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();//ReadAsStringAsync().GetAwaiter().GetResult();                
                    string Filepath = $"C:\\Users\\L.Shanava\\Downloads\\CourtFiles\\{userName}\\{panelName}";
                    Directory.CreateDirectory(Filepath);
                 File.WriteAllBytes($"{Filepath}\\{_id.ToString()}.pdf", file);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
        }
}
