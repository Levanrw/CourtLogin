using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace PAB.LoginInCourt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // this.WindowState = FormWindowState.Minimized;
            // this.ShowInTaskbar = false;
        }
        public string CurrentUserName { get; set; }
        public int _start, _end;
        TimeSpan ? _exitTime;
        private  void Form1_Load(object sender, EventArgs e)
        {
            LBprogresstext.Visible = false;
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

        private void BtnSetting_Click(object sender, EventArgs e)
        {
            Settingcs Set = new Settingcs();
            Set.ShowDialog();
        }

        private async void BTNStart_Click(object sender, EventArgs e)

        {
            try
            {
                using (var context = new Analytics_NewEntities())
                {

                    var courtsetting = context.CourtSettings.Select(c => c).ToList<CourtSetting>().FirstOrDefault();

                    if (courtsetting != null)
                    {
                        _start =Convert.ToInt32(courtsetting.RandomStart);
                        _end =Convert.ToInt32(courtsetting.RandomEnd);
                        _exitTime = courtsetting.ProgramExitTime.Value;                       
                    }
                }
               

                    BtnSetting.Enabled = false;               
                int CheckedUsersCount = 0;
                int AuthorizedUsers = 0;
                int NotAuthorizedUsers = 0;
                int FileNameOrder = 0;
                string FilePath = ConfigurationManager.AppSettings["FilePath"];
                int step = 0;
                List<CourtUser> CourtUsers = new List<CourtUser>();
                using (var context = new Analytics_NewEntities())
                {
                    CourtUsers = context.CourtUsers.Select(c => c).Where(w => w.ReadyForLogin == true && w.Logined == false).ToList<CourtUser>();

                }
                LBalluser.Text = CourtUsers.Count.ToString();
                foreach (var user in CourtUsers)
                {
                    if (DateTime.Now.TimeOfDay > _exitTime)
                    {
                        MessageBox.Show($"პროგრამის მუშაობა დაშვებულია {_exitTime} - მდე");
                        await Task.Delay(TimeSpan.FromSeconds(4));
                        this.Close();
                    }
                    else
                    { 
                    LBprogresstext.Visible = true;
                    Random random = new Random();
                    int randomNumber = random.Next(_start, _end);
                    step++;
                    if (step > 1)
                    {
                        LBwait.Text = $"{randomNumber} წამი";
                        await Task.Delay(TimeSpan.FromSeconds(randomNumber));
                    }

                    CurrentUserName = user.UserName;
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri("https://ecd.court.ge");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var formContent = new FormUrlEncodedContent(new[]
                        {
                         new KeyValuePair<string, string>("UserName", user.UserName),
                         new KeyValuePair<string, string>("Password", user.Password),
                     });
                        //send request
                        HttpResponseMessage responseMessage = await client.PostAsync("/User/Login", formContent);
                        var responseJson = await responseMessage.Content.ReadAsStringAsync();
                        // var jObject = JObject.Parse(responseJson);
                        var r = responseMessage.RequestMessage;

                        using (var db = new Analytics_NewEntities())
                        {
                            var result = db.CourtUsers.SingleOrDefault(b => b.Id == user.Id);
                            if (result != null)
                            {
                                result.Logined = true;
                                result.LastLoginDate = DateTime.Now;
                                db.SaveChanges();
                            }
                            CheckedUsersCount++;
                            LBcheckusers.Text = CheckedUsersCount.ToString();
                        }

                        if (responseJson.Contains("მომხმარებელი ან პაროლი არასწორია"))
                        {
                            NotAuthorizedUsers++;
                            LBnotauthorizedusers.Text = NotAuthorizedUsers.ToString();
                            using (var db = new Analytics_NewEntities())
                            {
                                var result = db.CourtUsers.SingleOrDefault(b => b.Id == user.Id);
                                if (result != null)
                                {
                                    result.Authorized = false;
                                    result.LastLoginDate = DateTime.Now;
                                    db.SaveChanges();
                                }
                            }
                        }

                        else
                        {
                            AuthorizedUsers++;
                            LBautorizeduser.Text = AuthorizedUsers.ToString();



                            var _doc = responseJson.ToString();
                            //var docc = webBrowser1.Document;


                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(_doc);
                            if (doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseDocs']") == null || doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseFiles']") == null || doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseCourtSession']") == null)
                            {
                                using (var db = new Analytics_NewEntities())
                                {
                                    var result = db.CourtUsers.SingleOrDefault(b => b.Id == user.Id);
                                    if (result != null)
                                    {
                                        result.Logined = true;
                                        result.Authorized = true;
                                        result.Description = "Content is empty";
                                        result.LastLoginDate = DateTime.Now;
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                    //<div class="filterForm userCaseDetails">    class="sides_LIST"
                                    List<List<string>> Header = doc.DocumentNode.SelectSingleNode("//div[@class='filterForm userCaseDetails']")
                                           .Descendants("ul")
                                           .Skip(0)
                                           //.Where(tr => tr.Elements("li").Count() > 1)
                                           .Select(tr => tr.Elements("li").Select(td => td.InnerText.Trim()).ToList())

                                           .ToList();
                                    string HeaderText = "";

                                    foreach (var item in Header)
                                    {

                                        for (int i = 0; i < item.Count; i++)
                                        {
                                           HeaderText+= item[i].ToString().Replace("\r\n  "," : ") + "\r\n";
                                        }
                                    }
                                   


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
                                    {
                                        if (item[0].Trim().Length <= 11)
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                                        else
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy HH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else date = Convert.ToDateTime("1900.01.01");
                                    string _file = item[1];
                                    using (var db = new Analytics_NewEntities())
                                    {
                                        var data = db.Set<CourtSiteInfo>();
                                        data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = _file, TabName = "სასამართლო აქტები", CourtUserId = user.Id, SysDate = DateTime.Now ,Header= HeaderText });
                                        db.SaveChanges();
                                    }

                                }
                                List<string> files = new List<string>();
                                string _folderPath = $"{FilePath}\\{user.UserName}\\სასამართლო აქტები";
                                if (Directory.Exists(_folderPath))
                                {
                                    files.Clear();
                                    DirectoryInfo d = new DirectoryInfo(_folderPath);//Assuming Test is your Folder
                                    FileInfo[] Files = d.GetFiles(); //Getting Text files

                                    foreach (FileInfo file in Files)
                                    {
                                        files.Add(file.Name.Substring(0, file.Name.IndexOf('_')));
                                    }
                                }

                                var gidAttribute = "<root>" + doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseDocs']").InnerHtml + "</root>";
                                XmlDocument xd = new XmlDocument();
                                xd.LoadXml(gidAttribute);

                                XDocument doc6 = XDocument.Parse(xd.InnerXml);

                                IEnumerable<string> links = doc6.Descendants("a")
                              .Select(element => element.Attribute("href").Value);
                                FileNameOrder = links.Count();
                                foreach (var item in links)
                                {

                                    item.Select(a => a.ToString());
                                    string Filepath = $"{FilePath}\\{user.UserName}\\სასამართლო აქტები";
                                    Directory.CreateDirectory(Filepath);
                                    int start = item.IndexOf('=') + 1;
                                    string _id = item.Substring(start, item.Length + -start);
                                    if (!files.Contains(_id))
                                    {
                                        HttpResponseMessage responseMessage2 = await client.PostAsync(item, null);//"/Cabinet/Document?caseFileId=21277"
                                        var file = responseMessage2.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                                        File.WriteAllBytes($"{Filepath}\\{_id}_{FileNameOrder}.pdf", file);
                                    }
                                    FileNameOrder--;
                                    // bool saveFile = await SaveUserFile(UserName, "სასამართლო აქტები", item);
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
                                    {
                                        if (item[0].Trim().Length <= 11)
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                                        else
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy HH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else date = Convert.ToDateTime("1900.01.01");
                                    //   date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                    // System.Globalization.CultureInfo.InvariantCulture);
                                    string _file = item[1];

                                    using (var db = new Analytics_NewEntities())
                                    {
                                        var data = db.Set<CourtSiteInfo>();
                                        data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = _file, TabName = "წარმოდგენილი ფაილები", CourtUserId = user.Id, SysDate = DateTime.Now, Header = HeaderText });
                                        db.SaveChanges();
                                    }
                                }
                                string _folderPath1 = $"{FilePath}\\{user.UserName}\\წარმოდგენილი ფაილები";
                                if (Directory.Exists(_folderPath1))
                                {
                                    files.Clear();
                                    DirectoryInfo d = new DirectoryInfo(_folderPath1);//Assuming Test is your Folder
                                    FileInfo[] Files = d.GetFiles(); //Getting Text files

                                    foreach (FileInfo file in Files)
                                    {
                                        files.Add(file.Name.Substring(0, file.Name.IndexOf('_')));
                                    }
                                }

                                var gidAttribute1 = "<root>" + doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseFiles']").InnerHtml + "</root>";
                                XmlDocument xd1 = new XmlDocument();
                                xd1.LoadXml(gidAttribute1);

                                XDocument doc61 = XDocument.Parse(xd1.InnerXml);

                                IEnumerable<string> links1 = doc61.Descendants("a")
                              .Select(element => element.Attribute("href").Value);
                                FileNameOrder = links1.Count();
                                foreach (var item in links1)
                                {
                                    // FileNameOrder++;
                                    item.Select(a => a.ToString());
                                    string Filepath = $"{FilePath}\\{user.UserName}\\წარმოდგენილი ფაილები";
                                    Directory.CreateDirectory(Filepath);
                                    int start = item.IndexOf('=') + 1;
                                    string _id = item.Substring(start, item.Length + -start);
                                    if (!files.Contains(_id))
                                    {
                                        HttpResponseMessage responseMessage2 = await client.PostAsync(item, null);//"/Cabinet/Document?caseFileId=21277"
                                        var file = responseMessage2.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                                        File.WriteAllBytes($"{Filepath}\\{_id}_{FileNameOrder}.pdf", file);
                                    }
                                    FileNameOrder--;
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
                                    {
                                        if (item[0].Trim().Length <= 11)
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                                        else
                                            date = DateTime.ParseExact(item[0], "dd.MM.yyyy HH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else date = Convert.ToDateTime("1900.01.01");
                                    string _file = item[1];

                                    using (var db = new Analytics_NewEntities())
                                    {
                                        var data = db.Set<CourtSiteInfo>();
                                        data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = _file, TabName = "სასამართლო სხდომები", CourtUserId = user.Id, SysDate = DateTime.Now, Header = HeaderText });
                                        db.SaveChanges();
                                    }
                                }

                                string _folderPath2 = $"{FilePath}\\{user.UserName}\\სასამართლო სხდომები";
                                if (Directory.Exists(_folderPath2))
                                {
                                    files.Clear();
                                    DirectoryInfo d = new DirectoryInfo(_folderPath2);//Assuming Test is your Folder
                                    FileInfo[] Files = d.GetFiles(); //Getting Text files

                                    foreach (FileInfo file in Files)
                                    {
                                        files.Add(file.Name.Substring(0, file.Name.IndexOf('_')));
                                    }
                                }
                                var gidAttribute12 = "<root>" + doc.DocumentNode.SelectSingleNode("//table[@class='tableCommonStyle caseCourtSession']").InnerHtml + "</root>";
                                XmlDocument xd12 = new XmlDocument();
                                xd12.LoadXml(gidAttribute12);

                                XDocument doc612 = XDocument.Parse(xd12.InnerXml);

                                IEnumerable<string> links12 = doc612.Descendants("a")
                              .Select(element => element.Attribute("href").Value);
                                FileNameOrder = links12.Count();
                                foreach (var item in links12)
                                {
                                    //FileNameOrder++;
                                    item.Select(a => a.ToString());
                                    string Filepath = $"{FilePath}\\{user.UserName}\\სასამართლო სხდომები";
                                    Directory.CreateDirectory(Filepath);
                                    int start = item.IndexOf('=') + 1;
                                    string _id = item.Substring(start, item.Length + -start);
                                    if (!files.Contains(_id))
                                    {
                                        HttpResponseMessage responseMessage2 = await client.PostAsync(item, null);//"/Cabinet/Document?caseFileId=21277"
                                        var file = responseMessage2.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                                        File.WriteAllBytes($"{Filepath}\\{_id}_{FileNameOrder}.pdf", file);
                                    }
                                    FileNameOrder--;
                                }

                                using (var db = new Analytics_NewEntities())
                                {
                                    var result = db.CourtUsers.SingleOrDefault(b => b.Id == user.Id);
                                    if (result != null)
                                    {
                                        result.Authorized = true;
                                        result.LastLoginDate = DateTime.Now;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    } }
                }
                // this.Close();
                LBprogresstext.ForeColor = Color.Green;
                LBprogresstext.Text = "დასრულდა!";
                BtnSetting.Enabled = true;
            }
            catch (Exception ex)
            {
                LBprogresstext.ForeColor = Color.Red;
                LBprogresstext.Text = "დაფიქსირდა შეცდომა!";
                BtnSetting.Enabled = true;
                MessageBox.Show($"შეცდომა დაფიქსირდა იუზერზე : {CurrentUserName} . შეცდომის ტექსტი : {ex.Message}");
            }

        }
    }
}
