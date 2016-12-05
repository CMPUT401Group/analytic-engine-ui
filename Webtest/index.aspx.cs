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
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Webtest
{
    public partial class index : System.Web.UI.Page
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        /// <summary>
        /// grabs the latest list of metric names from matt's server for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //string url = "162.246.157.107/metrics/index.json";
            Uri u = new Uri("http://162.246.157.107/metrics/index.json",UriKind.Absolute);
            var request = WebRequest.Create(u);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            pointschart.Enabled = false;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                text = text.Replace("[", "");
                text = text.Replace("]", "");
                text = text.Replace("\"", "");
                text = text.Replace(" ", "");
                string[] t = text.Split(',');
                //Literal myLitCtrl = (Literal)FindControl("litDescription");
                StringBuilder result = new StringBuilder();
                foreach(var i in t)
                {
                    result.Append("<option value =\"" + i + "\"></option>");
                }
                metric1.Text = result.ToString();
                metric2.Text = metric1.Text;

                
            }
        }
        /// <summary>
        /// submits the data user filled in the form displayed, creates a get request to matt's server, reads response, and displays data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


            if(mename1 == "" || mdate1=="" || mdate2=="")
            {
                ErrorTrap("Please fill in data before continuing.");
                return;
            }

            //convert to unix timestamp

            DateTime dt_mdate1 = Convert.ToDateTime(mdate1);
            DateTime dt_mdate2 = Convert.ToDateTime(mdate2);
            if(dt_mdate1 == dt_mdate2 || dt_mdate2<dt_mdate1)
            {
                ErrorTrap("Please make sure from and between are not exactly the same. Or 'To' is earlier than 'From'");
            }

            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0);




            TimeSpan timestamp1 = dt_mdate1 - unix;
            TimeSpan timestamp2 = dt_mdate2 - unix;
            double unixm1 = timestamp1.TotalSeconds;
            double unixm2 = timestamp2.TotalSeconds;

            //generate get request 

            string req = "http://162.246.157.107:8888/call" + "?mdate1=" + unixm1.ToString() + "&mdate2=" + unixm2.ToString() + "&m1=" + mename1;

            if (function.SelectedIndex < 2)
            {
                req += "&m2=" + mename2;
            }
            else
            {
                req += "&func=";
                displaygraph(req);
               

                return;
            }

            string result = makerequest(req);



            if (function.SelectedIndex == 0)
            {
                responseobject_correlation u = (responseobject_correlation)js.Deserialize(result, typeof(responseobject_correlation));
                result_display.Text = "Correlation: "+u.r1;
            }
            else if (function.SelectedIndex == 1)
            { 
                responseobject_covariance u = (responseobject_covariance)js.Deserialize(result, typeof(responseobject_covariance));
                result_display.Text = "Covariance: "+u.r2;
            }
            else
            {
                result_display.Text = result;
            }

            result_display.Visible = true;

        }
        /// <summary>
        /// disables the second metric name input if user selects deviation radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        void displaygraph(string req)
        {
            string data = makerequest(req + "3");

            data = data.Replace("target:", "target:\"");
            data = data.Replace(",datapoints", "\",datapoints");
            string exclusion = makerequest(req + "2");

            responseobject_datapackage u = (responseobject_datapackage)js.Deserialize(data, typeof(responseobject_datapackage));
            responseobject_deviation o = (responseobject_deviation)js.Deserialize(exclusion, typeof(responseobject_deviation));

            graphobject origin = u.setupgraph();

            u.renderRes1[0].datapoints.RemoveAll(x => o.r3.Contains(Convert.ToInt32(x[1])));

            graphobject graphobj = u.setupgraph();








            Series datapoints = new Series("default");
            datapoints.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Point;
            ChartArea Point_Area = new ChartArea("PointArea");
            Axis yaxis = new Axis(Point_Area, AxisName.Y);
            Axis xaxis = new Axis(Point_Area, AxisName.X);
           
            datapoints.Points.DataBindXY(graphobj.x, graphobj.y);
            datapoints.XValueType = ChartValueType.Time;

            pointschart.Series.Add(datapoints);
            pointschart.ChartAreas.Add(Point_Area);
            pointschart.Enabled = true;




            Series datapointsorigin = new Series("default");
            datapointsorigin.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Point;
            ChartArea Point_Areaorigin = new ChartArea("PointArea");
            Axis yaxisorigin = new Axis(Point_Areaorigin, AxisName.Y);
            Axis xaxisorigin = new Axis(Point_Areaorigin, AxisName.X);

            datapointsorigin.Points.DataBindXY(origin.x, origin.y);
            datapointsorigin.XValueType = ChartValueType.Time;

            pointorigin.Series.Add(datapointsorigin);
            pointorigin.ChartAreas.Add(Point_Areaorigin);
            pointorigin.Enabled = true;



        }
        string makerequest(string req)
        {
            Uri targetUri = new Uri(req);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);

            // expect json file
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream newStream = response.GetResponseStream();

            StreamReader sr = new StreamReader(newStream);

            var result = sr.ReadToEnd();

            result = result.Replace("\"", "");
            result = result.Replace(@"\", "");
            return result;
        }
        public void ErrorTrap(string str)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert" + UniqueID,
               "alert('" + str + "');", true);
        }



    }
}