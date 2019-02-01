using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WindowsFormAD
{
    public partial class Form1 : Form
    {
        private string Token;
        private IAuthenticationSettings authSettings;
        private AuthorizationAD authorizationAD;

        public Form1()
        {
            InitializeComponent();

            authSettings = new AuthenticationSettings();
            SetAuthenticationSettings();
            authorizationAD = new AuthorizationAD(authSettings);

            Task.Run(async () => { Token = await authorizationAD.GetToken(); }).Wait();
        }

       

        private void SetAuthenticationSettings()
        {
            this.authSettings.AADInstance = ConfigurationManager.AppSettings["AADInstance"];
            this.authSettings.ClientId = ConfigurationManager.AppSettings["ClientId"];
            this.authSettings.TenantId = ConfigurationManager.AppSettings["TenantId"];
            this.authSettings.ResourceIdForServer1 = ConfigurationManager.AppSettings["ResourceIdForServer1"];
            this.authSettings.ResourceIdForServer2 = ConfigurationManager.AppSettings["ResourceIdForServer2"];
            this.authSettings.RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
            var path = "http://localhost:60185/api/values/";
            //var prodPath = "http://basicwithdomainapp1.azurewebsites.net/api/values/";
            HttpResponseMessage response = null;
            Task.Run(async () => { response = await client.GetAsync(path); }).Wait();
            if (response.IsSuccessStatusCode)
            {
                string result = null;
                Task.Run(async () => { result = await response.Content.ReadAsStringAsync(); }).Wait();
                this.label2.Text = result;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
            var path = "http://localhost:60185/api/values/";
            //var prodPath = "http://basicwithdomainapp1.azurewebsites.net/api/values/";
            HttpResponseMessage response = null;
            Task.Run(async () => { response = await client.GetAsync(path); }).Wait();
            if (response.IsSuccessStatusCode)
            {
                string result = null;
                Task.Run(async () => { result = await response.Content.ReadAsStringAsync(); }).Wait();
                this.label2.Text = result;
            }
        }
    }
}
