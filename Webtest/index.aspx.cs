using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Webtest
{
    public partial class index : System.Web.UI.Page
    {

        JavaScriptSerializer js = new JavaScriptSerializer();


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
                text = text.Replace(" ", "");
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
            result_display.Text = "";

            string mename1, mename2;
            string mdate1, mdate2;

            mename1 = M1.Value;
            mename2 = M2.Value;

            mdate1 = metricdate1.Value;
            mdate2 = metricdate2.Value;




            //convert to unix timestamp

            DateTime dt_mdate1 = Convert.ToDateTime(mdate1);
            DateTime dt_mdate2 = Convert.ToDateTime(mdate2);
            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0);




            TimeSpan timestamp1 = dt_mdate1 - unix;
            TimeSpan timestamp2 = dt_mdate2 - unix;
            double unixm1 = timestamp1.TotalSeconds;
            double unixm2 = timestamp2.TotalSeconds;

            //

            string req = "http://162.246.157.107:8888/call" + "?mdate1=" + unixm1.ToString() + "&mdate2=" + unixm2.ToString() + "&m1=" + mename1;

            if (function.SelectedIndex < 2)
            {
                req += "&m2=" + mename2;
            }


            req += "&func=" + function.SelectedIndex.ToString();


            Uri targetUri = new Uri(req);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);

            // expect json file
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            Stream newStream = response.GetResponseStream();

            StreamReader sr = new StreamReader(newStream);

            var result = sr.ReadToEnd();

            result =result.Replace("\"","");
            result =result.Replace(@"\","");
            if (function.SelectedIndex == 0)
            {



                responseobject_correlation u = (responseobject_correlation)js.Deserialize(result, typeof(responseobject_correlation));
                result_display.Text = u.r1;
            }
            else if (function.SelectedIndex == 1)
            { 
                responseobject_covariance u = (responseobject_covariance)js.Deserialize(result, typeof(responseobject_covariance));
                result_display.Text = u.r2;
            }
            else if (function.SelectedIndex == 2)
            {
                responseobject_deviation u = (responseobject_deviation)js.Deserialize(result, typeof(responseobject_deviation));  
                foreach(var i in u.r3)
                {
                    result_display.Text += i.ToString()+",";
                }
                
            }
            else
            {
                result_display.Text = result;
            }

                

            result_display.Visible = true;

            
            

            //create new dashboard from jsonfile
            /*Uri dashuri = new Uri("http://162.246.157.107:3000/api/dashboards/db");
            HttpWebRequest dash = (System.Net.HttpWebRequest)HttpWebRequest.Create(dashuri);
            dash.Method = "POST";
            dash.Headers.Add("Authorization", "Bearer eyJrIjoiaTAyWUhrNFRpRVdDZmR2UDdTRnhoamtOSzFEaWlSQjIiLCJuIjoiYWRkIiwiaWQiOjF9");
            dash.ContentType = "application/json; charset=utf-8";
            dash.Accept = "application/json; charset=utf-8";

            using (var streamWriter = new StreamWriter(dash.GetRequestStream()))
            {

               
                //json = json.Replace("\",", "\","   + "\"" +"\u002B");
                streamWriter.Write(result);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)dash.GetResponse();*/



            //get snapshot from dashboard


            //display on webpage

        }

        protected void checkdeviation(object sender, EventArgs e)
        {
            if(function.SelectedIndex==2)
            {
                M2.Disabled = true;
            }
            else
            {
                M2.Disabled = false;
            }
        }
    }
}