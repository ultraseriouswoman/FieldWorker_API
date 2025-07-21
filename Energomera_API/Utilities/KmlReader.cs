using Aspose.Gis;
using Aspose.Gis.Geometries;
using Energomera_API.Models;
using Energomera_API.Utilities;

namespace Energomera_API
{
    public static class KmlReader
    {
        //Не работает? Проверьте указанные пути Paths.cs. Согласна, это не очень решение, но здесь пока мы двигаемся на неадекватном >:D
        public static IEnumerable<Fields> GetDataToList(string fieldPath, string centroidPath) 
        {
            List<Fields> fields = [];

            using (var fieldLayer = Drivers.Kml.OpenLayer(fieldPath))
            {
                foreach (var feature in fieldLayer)
                {
                    Fields newField = new()
                    {
                        Id = feature.GetValue<int>("fid"),
                        Name = feature.GetValue<string>("name"),
                        Size = feature.GetValue<double>("size"),
                    };

                    newField.Locations = GetLocations(newField.Id, fieldLayer, Drivers.Kml.OpenLayer(centroidPath));

                    fields.Add(newField);
                }
            }

            return fields;
        }

        public static Fields GetDataFromID(int id, string fieldPath, string centroidPath)
        {
            Fields field = new();

            using (var fieldLayer = Drivers.Kml.OpenLayer(fieldPath))
            {
                foreach (var feature in fieldLayer)
                {
                    if (feature.GetValue<int>("fid") == id)
                    {
                        field.Id = feature.GetValue<int>("fid");
                        field.Name = feature.GetValue<string>("name");
                        field.Size = feature.GetValue<double>("size");

                        field.Locations = GetLocations(field.Id, fieldLayer, Drivers.Kml.OpenLayer(centroidPath));
                    }  
                }
            }
            return field;
        }

        public static Locations GetLocations(int id, VectorLayer fieldLayer, VectorLayer centroidLayer)
        {
            Locations newLocations = new();

            newLocations.Center = new Coordinates(GetCentroid(id, centroidLayer).X, GetCentroid(id, centroidLayer).Y);
            newLocations.Polygon = [];

            foreach (var point in GetPolygon(id, fieldLayer).ExteriorRing)
            {
                newLocations.Polygon.Add(new Coordinates(point.X, point.Y));
            }

            return newLocations;
        }

        public static Point GetCentroid(int id, VectorLayer centroidLayer)
        {
            Point center = new();
            foreach (var feature in centroidLayer)
            {
                if (id == feature.GetValue<int>("fid"))
                {
                    var gottenPoint = feature.Geometry as Point ?? throw new NullReferenceException("Центроид не был найден");

                    center.X = gottenPoint.X;
                    center.Y = gottenPoint.Y;
                }
            }
            return center;
        }

        public static Polygon GetPolygon(int id, VectorLayer fieldLayer)
        {
            Polygon polygon = new();
            foreach (var feature in fieldLayer)
            {
                if (id == feature.GetValue<int>("fid"))
                {
                    var gottenPolygon = feature.Geometry as Polygon ?? throw new NullReferenceException("Полигон не был найден");

                    polygon = gottenPolygon;
                }
            }
            return polygon;
        }
        
        public static Fields GetFieldFromPolygon(Polygon foundedPolygon)
        {
            var layer = Drivers.Kml.OpenLayer(Paths.FieldPath);
            var foundedField = layer.FirstOrDefault(x => x.Geometry as Polygon == foundedPolygon);

            return new Fields()
            {
                Id = foundedField.GetValue<int>("fid"),
                Name = foundedField.GetValue<string>("name")
            };
        }

        public static IEnumerable<Polygon> GetAllPolygons(string path)
        {
            List<Polygon> polygons = [];
            using (var fieldLayer = Drivers.Kml.OpenLayer(path))
            {
                foreach (var feature in fieldLayer)
                {
                    var gottenPolygon = feature.Geometry as Polygon ?? throw new NullReferenceException("Полигон не был найден");

                    polygons.Add(gottenPolygon);
                }
            }
            return polygons;
        }
    }
}
