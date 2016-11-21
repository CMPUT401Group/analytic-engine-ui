using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        protected virtual void submit_Click(object sender, EventArgs e)
        {
            NameValueCollection nvc = Request.Form;
            
            string m1, m2;
            string mdate1, mdate2;
            if (!string.IsNullOrEmpty(nvc["metric1"]))
            {
                mdate1 = nvc["metric1"];
            }
            else { return; }

            if (!string.IsNullOrEmpty(nvc["metric2"]))
            {
                mdate2 = nvc["metric2"];
            }
            else { return; }
            if (!string.IsNullOrEmpty(nvc["mname1"]))
            {
                m1 = nvc["mname1"];
            }
            else { return; }
            if (!string.IsNullOrEmpty(nvc["mname2"]))
            {
                m2 = nvc["mname2"];
            }
            else { return; }
            



            //convert to unix timestamp

            DateTime dt_mdate1 = Convert.ToDateTime(mdate1);
            DateTime dt_mdate2 = Convert.ToDateTime(mdate2);
            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0);

            
             

            TimeSpan timestamp1 = dt_mdate1-unix;
            TimeSpan timestamp2 = dt_mdate2-unix;
            double unixm1 = timestamp1.TotalSeconds;
            double unixm2 = timestamp2.TotalSeconds;

            //

            string req = "http://162.246.157.107/call" + "?mdate1=" + unixm1.ToString() + "?mdate2=" + unixm2.ToString()+"?m1="+m1+"?m2="+m2+"?func="+ function.SelectedIndex.ToString();

            Uri targetUri = new Uri(req);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);
            // expect json file
            var response = request.GetResponse() as HttpWebResponse;

        }


    }
}