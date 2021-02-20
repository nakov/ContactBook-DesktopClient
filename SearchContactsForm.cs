using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ContactBook_DesktopClient
{
    public partial class SearchContactsForm : Form
    {
        private string apiBaseUrl;

        public SearchContactsForm()
        {
            InitializeComponent();
        }

        private void SearchContactsForm_Load(object sender, System.EventArgs e)
        {
            var formConnect = new FormConnect();
            if (formConnect.ShowDialog() == DialogResult.OK)
            {
                this.apiBaseUrl = formConnect.ApiUrl;
            }
            else
            {
                this.Close();
            }
        }

        private async void buttonSearch_Click(object sender, System.EventArgs e)
        {
            string keyword = this.textBoxSearch.Text;
            try
            {
                var restClient = new RestClient(this.apiBaseUrl) { Timeout = 3000 };
                var request = new RestRequest("/contacts/search/{keyword}", Method.GET);
                request.AddUrlSegment("keyword", keyword);
                var response = await restClient.ExecuteAsync(request);
                if (response.IsSuccessful & response.StatusCode == HttpStatusCode.OK)
                {
                    // Visualize the returned contacts
                    var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
                    ShowSuccessMsg($"Contacts found: {contacts.Count}");
                    this.dataGridViewContacts.DataSource = contacts;
                }
                else
                {
                    // Visualize an error message
                    if (string.IsNullOrWhiteSpace(response.ErrorMessage))
                        ShowErrorMsg($"HTTP error `{response.StatusCode}`.");
                    else
                        ShowErrorMsg($"HTTP error `{response.ErrorMessage}`.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
            }
        }

        private void ShowErrorMsg(string errMsg)
        {
            labelResult.Text = $"Error: {errMsg}";
            labelResult.BackColor = Color.Red;
            labelResult.ForeColor = Color.White;
        }

        private void ShowSuccessMsg(string msg)
        {
            labelResult.Text = msg;
            labelResult.BackColor = Color.Green;
            labelResult.ForeColor = Color.White;
        }
    }
}
