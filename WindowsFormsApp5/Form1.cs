using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private List<PointLatLng> _points;
// WHEN USING THE APPLICATION, PUT IN THE TEXT BOX THE LONG AND LAT AND PRESS ADD, YOU CAN VERIFY THE PLACE
// BY PRESSING THE LOAD MAP.  THEN CHANGE THE TEST BOXES TO THE NEXT POINT (LONG AND LAT) THEN PRESS ADD AGAIN
// YOU NOW HAVE TO POINTS IN THE LIST _Points.  THEN PRESS THE GET BUTTON WHICH WILL DRAW THE ROUTE AND CALULATE 
// THE DISTANCE IN kms


        public Form1()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = "INSERT YOUR GOOGLE API KEY HERE";// YOU NEED TO CREATE A GOOGLE API FOR MAPS, THIS SHOULD BE FREE!
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.CacheLocation = @"cache";
            map.DragButton = MouseButtons.Right;
            map.MapProvider = GMapProviders.GoogleMap;
            map.ShowCenter = true;
            map.MinZoom = 3;
            map.MaxZoom = 100;
                       
        }

        private void btnLoadIntoMap_Click(object sender, EventArgs e)
        {
            
            double lat = Convert.ToDouble(txtLat.Text);
            double Longt = Convert.ToDouble(txtLong.Text);
            map.Position = new PointLatLng(lat, Longt);

                      
        }
        
        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            _points.Add(new PointLatLng(Convert.ToDouble(txtLat.Text), Convert.ToDouble(txtLong.Text)));
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            _points.Clear();
        }

        private void btnGetRoute_Click(object sender, EventArgs e)
        {
            var route = GoogleMapProvider.Instance.GetRoute(_points[0], _points[1], false, false, 10);
            var r = new GMapRoute(route.Points, "Own Route")
            {
                Stroke = new Pen(Color.Red, 5)
            };
            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
            MessageBox.Show(getDistanceFromLatLonInKm(_points[0].Lat, _points[0].Lng, _points[1].Lat, _points[1].Lng));
        }

        // STANDARD ALGORITHM FOR CALULATING DISTANCE FROM ONE POINT TO ANOTHER
        public static string getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; 
            var dLat = deg2rad(lat2 - lat1);  
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; 
            string Dis;
            Dis = d.ToString();
            return Dis;
        }

        public static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
