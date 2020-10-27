using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pab.LoginCourt.TaskScheduler
{
    class Program
    {
       //GIT
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }//
        static async Task MainAsync()


        {
            string CurrentUserName;
            int _start=0, _end=0;
            TimeSpan? _exitTime= TimeSpan.FromMinutes(1);
            try
            {
                
                using (var context = new Analytics_NewEntities())
                {

                    var courtsetting = context.CourtSettings.Select(c => c).ToList<CourtSetting>().FirstOrDefault();

                    if (courtsetting != null)
                    {
                        _start = Convert.ToInt32(courtsetting.RandomStart);
                        _end = Convert.ToInt32(courtsetting.RandomEnd);
                        _exitTime = courtsetting.ProgramExitTime.Value;
                    }
                }


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
                foreach (var user in CourtUsers)
                {
                   
                    if (DateTime.Now.TimeOfDay > _exitTime)
                    {
                        Console.WriteLine($"Program work is allowed {_exitTime} ");
                        await Task.Delay(TimeSpan.FromSeconds(4));
                        return;
                    }
                    else
                    {
                        Console.WriteLine(user.UserName);
                        Random random = new Random();
                        int randomNumber = random.Next(_start, _end);
                        step++;
                        if (step > 1)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(randomNumber));
                        }

                        CurrentUserName = user.UserName;
                        using (var client = new HttpClient())
                        {

                            client.BaseAddress = new Uri("http://ecd.court.ge");
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
                            }

                            if (responseJson.Contains("მომხმარებელი ან პაროლი არასწორია"))
                            {
                                NotAuthorizedUsers++;
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
                                            HeaderText += item[i].ToString().Replace("\r\n  ", " : ") + "\r\n";
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
                                            data.Add(new CourtSiteInfo { CourtDate = Convert.ToDateTime(date), FileName = _file, TabName = "სასამართლო აქტები", CourtUserId = user.Id, SysDate = DateTime.Now, Header = HeaderText });
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

                        }
                    }
                }
                Console.WriteLine("Successfully completed!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

        }
    }
}
