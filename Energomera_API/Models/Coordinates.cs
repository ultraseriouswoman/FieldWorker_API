namespace Energomera_API.Models
{
    public class Coordinates
    {
        private double _lat;
        public double Lat 
        {
            get { return _lat; }
            set { _lat = value; } 
        }
        private double _lng;
        public double Lng 
        { 
            get { return _lng; }
            set { _lng = value; }
        }

        public Coordinates(double lat, double lng)
        {
            _lat = lat;
            _lng = lng;
        }
    }
}
