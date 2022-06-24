using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Domain.Settings;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly IMailjetClient _client;
        public MailService(IMailjetClient client)
        {
            _client = client;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                    string mail = mailRequest.ToEmail;
                    MailjetRequest request = new MailjetRequest { Resource = SendV31.Resource }
                    .Property(Send.Messages, new JArray {
                    new JObject
                    {
                        {
                           "From",new JObject
                           {
                              {"Email","sq010dotnet@gmail.com"},
                              {"Name", "Aqua Water"}
                           }
                        },
                        {
                            "To", new JArray
                            {
                               new JObject
                               {
                                  {"Email", mail },
                               }
                            }
                        },
                      {"Subject", mailRequest.Subject},
                      { "HtmlPart",  $@"{mailRequest.Body}" },
                      {"CustomId", "AppGettingStartedTest"}
                    }
                    });
                    MailjetResponse response = await _client.PostAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailTemplate(string templateName)
        {
            var baseDir = Directory.GetCurrentDirectory();
            string folderName = "/HtmlTemplate/";
            var path = Path.Combine(baseDir + folderName, templateName);
            return File.ReadAllText(path);
        }
    }
}
