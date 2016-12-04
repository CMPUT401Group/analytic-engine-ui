using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webtest
{
    public class responseobject_correlation
    {
        public string r1;
    }
    public class responseobject_covariance
    {
        public string r2;
    }
    public class responseobject_deviation
    {
        public List<int> r3;

    }
    public class RenderRes1
    {
        public string target { get; set; }
        public List<List<double>> datapoints { get; set; }
    }

    public class responseobject_datapackage
    {
        public List<RenderRes1> renderRes1 { get; set; }



        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



        public graphobject setupgraph()
        {


            List<List<double>> datapoints = renderRes1[0].datapoints;

            List<double> datavalue = datapoints.Select(x => x[0]).ToList();
            List<double> timestamps = datapoints.Select(x => x[1]).ToList();
            List<DateTime> converted_timstamp = timestamps.ConvertAll<DateTime>(new Converter<double, DateTime>(UnixTimeStampToDateTime));
            return new graphobject(converted_timstamp, datavalue);

        }
    }
    public class graphobject
    {
        public graphobject(List<DateTime> input1, List<double> input2)
        {
            // This is the no parameter constructor method.
            // First 
            x = input1;
            y = input2;
            
        }

        public List<DateTime> x { get; set; }
        public List<double> y { get; set; }

    }


   


}