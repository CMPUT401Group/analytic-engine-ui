using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Webtest
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string url = "162.246.157.107/metrics/index.json";
            Uri u = new Uri("http://162.246.157.107/metrics/index.json",UriKind.Absolute);
            var request = WebRequest.Create(u);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                text = text.Replace("\"", "");
                string[] t = text.Split(',');
                //Literal myLitCtrl = (Literal)FindControl("litDescription");
                string results = "";
                foreach(var i in t)
                {
                    results += "<option value =\""+i+"\"></option>";
                }
                metric1.Text = results;
                metric2.Text = results;

                
            }
        }
        protected virtual void submit_click(    EventArgs e)
        {
            string metricname1 = 
        }


    }
}