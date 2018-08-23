using DataWhisperer.Common;
using DataWhisperer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataWhisperer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var baseGoJiraUrl = @"http://localhost:57883/";
            RequestModel model = new RequestModel()
            {
                RequestTypeId = 1,
                AsUserName = "Test Subject" + "at " + DateTime.Now,
                AdUserStartDate = "2018-09-1T13:34:00.000",
                JiraAccountType = "Confluence",
                CreatedDate = "2018-08-1T13:34:00.000",
                CreatedBy = "uscorp/glendonr"
            };

            var res = CreateJiraRequests(baseGoJiraUrl, model);
            //var res = FetchData();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<OperationResult> CreateJiraRequests(string baseGoJiraUrl, RequestModel model)
        {
            var orResponse = new OperationResult { Status = OperationStatus.Error };

            using (var client = getADSvcClient(baseGoJiraUrl))
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(@"http://localhost:57883/jira/request", content);
                orResponse.Status = response.IsSuccessStatusCode ? OperationStatus.Success : OperationStatus.Error;
                orResponse.Message = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    orResponse.Status = OperationStatus.Duplicate;
                    orResponse.Message = "UPS has successfully processed your request. However, an Active Directory account was unable to be created for this employee. A support ticket has been created to investigate the failure.";
                }
            }
            Console.WriteLine(orResponse);
            return orResponse;
        }

        public async Task<OperationResult> FetchData()
        {
            var orResponse = new OperationResult { Status = OperationStatus.Error };

            using (var client = getADSvcClient(@"http://localhost:57883/jira/employee?adusername=glendonr"))
            {
                var response = await client.GetAsync(@"http://localhost:57883/jira/employee?adusername=glendonr");
                orResponse.Status = response.IsSuccessStatusCode ? OperationStatus.Success : OperationStatus.Error;
                orResponse.Message = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    orResponse.Status = OperationStatus.Duplicate;
                    orResponse.Message = "UPS has successfully processed your request. However, an Active Directory account was unable to be created for this employee. A support ticket has been created to investigate the failure.";
                }
            }

            return orResponse;
        }

        private HttpClient getADSvcClient(string baseADSvcUrl)
        {

            var adSvcClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            adSvcClient.BaseAddress = new Uri(baseADSvcUrl);
            adSvcClient.DefaultRequestHeaders.Accept.Clear();
            adSvcClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            return adSvcClient;
        }
        
    }
}