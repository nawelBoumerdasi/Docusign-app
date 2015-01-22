using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocuSignApp
{
    public partial class ContactInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                {
                    string name = Request.Form["Name"];
                    string email = Request.Form["Email"];
                    bool local = Request.Form["Radio1"] =="RadioLocal" ? true : false;
                    if (local)
                        {
                            Send_Envelope(name,email);
                        }
                    else
                        {
                            Send_Template(name,email);
                        }
                }
        }

        protected DocuSignAPI.APIServiceSoapClient Get_Proxy()
            {
                DocuSignAPI.APIServiceSoapClient apiClient = new DocuSignAPI.APIServiceSoapClient(
                    "APIServiceSoap",ConfigurationManager.AppSettings["APIUrl"]);
                apiClient.ClientCredentials.UserName.UserName="[" + ConfigurationManager.AppSettings["IntegratorsKey"]
                    +"]" + ConfigurationManager.AppSettings["APIUserEmail"];
                apiClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["Password"];
            return apiClient;

            }
        protected void Send_Envelope(string name,string email) 
            {
                // Create the recipient
                DocuSignAPI.Recipient recipient = new DocuSignAPI.Recipient();
                recipient.UserName = name;
                recipient.Email = email;
                recipient.ID = "1";
                recipient.Type = DocuSignAPI.RecipientTypeCode.Signer;
                recipient.RoutingOrder = 1;

                // Create the envelope content
                DocuSignAPI.Envelope envelope = new DocuSignAPI.Envelope();
                envelope.Subject = "This envelope is from the Cookbook";
                envelope.EmailBlurb = "Check out the Webminar";
                envelope.AccountId = ConfigurationManager.AppSettings["APIAccountId"];
                envelope.Recipients = new DocuSignAPI.Recipient[] { recipient };

                // Attach the document(s)
                envelope.Documents = new DocuSignAPI.Document[1];
                DocuSignAPI.Document document = new DocuSignAPI.Document();
                document.ID = "1";
                document.Name = "TestResource.pdf";
                document.PDFBytes = Resource1.TestResource;
                envelope.Documents[0] = document;

                // Create tabs
                DocuSignAPI.Tab tab1 = new DocuSignAPI.Tab();
                tab1.RecipientID = "1";
                tab1.PageNumber = "1";
                tab1.DocumentID = "1";
                tab1.Type = DocuSignAPI.TabTypeCode.SignHere;
                tab1.XPosition = "50";
                tab1.YPosition = "200";
                DocuSignAPI.Tab tab2 = new DocuSignAPI.Tab();
                tab2.RecipientID = "1";
                tab2.PageNumber = "1";
                tab2.DocumentID = "1";
                tab2.Type = DocuSignAPI.TabTypeCode.DateSigned;
                tab2.XPosition = "110";
                tab2.YPosition = "225";
                envelope.Tabs = new DocuSignAPI.Tab[] { tab1, tab2 };

                // Create the envelope and send it
                DocuSignAPI.APIServiceSoapClient proxy = Get_Proxy();
                DocuSignAPI.EnvelopeStatus status = proxy.CreateAndSendEnvelope(envelope);

                // An envelope ID indicates that it succeeded
                StatusLabel.Text = "The envelope is " + status.Status.ToString() + ".";
            }
        protected void Send_Template(string name, string email)
        {
            DocuSignAPI.EnvelopeInformation envelopeInfo = new DocuSignAPI.EnvelopeInformation();
            envelopeInfo.Subject = "This!envelope!is!from!the!Cookbook";
            envelopeInfo.EmailBlurb = "Check!out!the!SI!and!Consultant!Webinar!";
            envelopeInfo.AccountId = ConfigurationManager.AppSettings["APIAccountId"];

            DocuSignAPI.Recipient recipient = new DocuSignAPI.Recipient();
            recipient.UserName = name;
            recipient.Email = email;
            recipient.ID = "1";
            recipient.Type = DocuSignAPI.RecipientTypeCode.Signer;
            DocuSignAPI.TemplateReferenceRoleAssignment roleAssignment = new DocuSignAPI.TemplateReferenceRoleAssignment();
            roleAssignment.RecipientID = recipient.ID;
            roleAssignment.RoleName = "Taxpayer";
            
            DocuSignAPI.TemplateReference reference = new DocuSignAPI.TemplateReference();
            reference.TemplateLocation = DocuSignAPI.TemplateLocationCode.Server;
            reference.Template = "A8A70CB8-469C-4BA2-AC8A-6507C598991F";
            reference.RoleAssignments = new DocuSignAPI.TemplateReferenceRoleAssignment[] {roleAssignment};

            DocuSignAPI.APIServiceSoapClient proxy=Get_Proxy();
            DocuSignAPI.EnvelopeStatus status = proxy.CreateEnvelopeFromTemplates(  
                new DocuSignAPI.TemplateReference[]{reference},
                new DocuSignAPI.Recipient[]{recipient},
                envelopeInfo, true);

            StatusLabel.Text = "The!envelope!is!" + status.Status.ToString() + ".";
        }
    }
}