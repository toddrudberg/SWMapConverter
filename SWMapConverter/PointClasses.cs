using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWMapConverter
{
  public class SwMapPoint
  {
    public string ID;
    public string Remarks;
    public string Time;
    public string Geometry;
    public string Latitude;
    public string Longitude;
    public string X;
    public string Y;
    public string Elevation;
    public string OrthoHeight;
    public string InstrumentHt;
    public string FixID;
    public string Speed;
    public string Bearing;
    public string HorizontalAccuracy;
    public string VerticalAccuracy;
    public string PDOP;
    public string HDOP;
    public string VDOP;
    public string SatellitesInView;
    public string SatellitesInUse;

    public static List<SolidWorksPoint> GetSolidWorksPointList(List<SwMapPoint> swmapspoints, List<SwMapPoint> otherswmappoints = null)
    {
      List<SolidWorksPoint> result = new List<SolidWorksPoint>();
      List<double> Xs = SwMapPoint.GetXs(swmapspoints);
      List<double> Ys = SwMapPoint.GetYs(swmapspoints);

      double xmean = Xs.Average();
      double ymean = Ys.Average();

      var originPoint = swmapspoints
    .FirstOrDefault(p => string.Equals(p.Remarks, "origin", StringComparison.OrdinalIgnoreCase));

      if (originPoint != null) 
      {
        xmean = double.TryParse(originPoint.X, out double x) ? x : xmean;
        ymean = double.TryParse(originPoint.Y, out double y) ? y : ymean;
      }


      if (otherswmappoints == null)
      {
        List<double> XsForSolidworks = Xs.Select(x => x - xmean).ToList();
        List<double> YsForSolidworks = Ys.Select(y => y - ymean).ToList();

        //if no other file is provided, just convert the swmapspoints
        for (int i = 0; i < swmapspoints.Count; i++)
        {
          result.Add(new SolidWorksPoint
          {
            X = XsForSolidworks[i].ToString(),
            Y = YsForSolidworks[i].ToString(),
            Z = "0" // Assuming Z is not used, set to zero or any default value
          });
        }
      }
      //if other file is provided, convert those points instead, but use swmapspoints to get the origin
      else
      {
        List<double> otherXs = SwMapPoint.GetXs(otherswmappoints);
        List<double> otherYs = SwMapPoint.GetYs(otherswmappoints);
        for (int i = 0; i < otherswmappoints.Count; i++)
        {
          double adjustedX = otherXs[i] - xmean;
          double adjustedY = otherYs[i] - ymean;
          result.Add(new SolidWorksPoint
          {
            X = adjustedX.ToString(),
            Y = adjustedY.ToString(),
            Z = "0" // Assuming Z is not used, set to zero or any default value
          });
        }
      }

      return result;
    }

    public static (double Slope, double Intercept, double residuals, double stdev, double Average) FitLine(List<double> xs, List<double> ys)
    {
      if (xs.Count != ys.Count || xs.Count == 0)
        throw new ArgumentException("Lists must be of equal length and non-empty.");

      double sumX = xs.Sum();
      double sumY = ys.Sum();
      double sumXY = xs.Zip(ys, (x, y) => x * y).Sum();
      double sumXX = xs.Sum(x => x * x);
      int n = xs.Count;

      double slope = (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
      double intercept = (sumY - slope * sumX) / n;

      //let's get the residuals
      double rss = ys.Zip(xs, (y, x) => y - (slope * x + intercept)).Select(r => r * r).Sum();
      double stdev = Math.Sqrt(rss / (n - 2));
      //calculate average error
      double avgError = ys.Zip(xs, (y, x) => Math.Abs(y - (slope * x + intercept))).Average();

      return (slope, intercept, stdev, rss, avgError);
    }


    private static List<double> GetLongitudes(List<SwMapPoint> swmapspoints)
    {
      List<double> result = new List<double>();
      foreach (var point in swmapspoints)
      {
        if (double.TryParse(point.Longitude, out double longitude))
        {
          result.Add(longitude);
        }
      }
      return result;
    }

    private static List<double> GetLatitudes(List<SwMapPoint> swmapspoints)
    {
      List<double> result = new List<double>();
      foreach (var point in swmapspoints)
      {
        if (double.TryParse(point.Latitude, out double latitude))
        {
          result.Add(latitude);
        }
      }
      return result;
    }

    private static List<double> GetXs(List<SwMapPoint> swmapspoints)
    {
      List<double> result = new List<double>();
      foreach (var point in swmapspoints)
      {
        if (double.TryParse(point.X, out double x))
        {
          result.Add(x);
        }
      }
      return result;
    }

    private static List<double> GetYs(List<SwMapPoint> swmapspoints)
    {
      List<double> result = new List<double>();
      foreach (var point in swmapspoints)
      {
        if (double.TryParse(point.Y, out double y))
        {
          result.Add(y);
        }
      }
      return result;
    }


    public static List<SwMapPoint> LoadSwMapsFile(string filePath, Label lbl, Settings settings)
    {
      if(!File.Exists(filePath))
      {
        lbl.Text = "File not found.";
        return new List<SwMapPoint>();
      }
      var lines = File.ReadAllLines(filePath).Skip(1); // skip header
      var points = new List<SwMapPoint>();

      foreach (var line in lines)
      {
        var parts = line.Split(',');

        if (parts.Length >= 21)
        {
          points.Add(new SwMapPoint
          {
            ID = parts[0],
            Remarks = parts[1],
            Time = parts[2],
            Geometry = parts[3],
            Latitude = parts[4],
            Longitude = parts[5],
            X = parts[6],
            Y = parts[7],
            Elevation = parts[8],
            OrthoHeight = parts[9],
            InstrumentHt = parts[10],
            FixID = parts[11],
            Speed = parts[12],
            Bearing = parts[13],
            HorizontalAccuracy = parts[14],
            VerticalAccuracy = parts[15],
            PDOP = parts[16],
            HDOP = parts[17],
            VDOP = parts[18],
            SatellitesInView = parts[19],
            SatellitesInUse = parts[20]
          });
        }
      }
      lbl.Text = $"Loaded {points.Count} SWMaps Points";
      settings.Save();
      return points;
    }

    internal static List<GoogleEarthPoint> ConvertXYZToLatLon(List<SwMapPoint> swmapspoints, List<SolidWorksPoint> swdata)
    {
      double lat, lon;
      List<double> xs = GetXs(swmapspoints);
      List<double> ys = GetYs(swmapspoints);
      List<double> lats = GetLatitudes(swmapspoints);
      List<double> lons = GetLongitudes(swmapspoints);
      var Xline = FitLine(xs, lons);
      var Yline = FitLine(ys, lats);

      MessageBox.Show($"Longitude Fit: Stdev={Xline.stdev * 1000:F3}, RSS={Xline.residuals * 1000:F3}, A3s={(Xline.Average + 3 * Xline.stdev)*1000:F3}mm");
      MessageBox.Show($"Latitude Fit: Stdev={Yline.stdev * 1000:F3}, RSS={Yline.residuals * 1000:F3}, A3s={(Yline.Average + 3 * Yline.stdev)*1000:F3}mm");

      
      double xmean = xs.Average();
      double ymean = ys.Average();

      var originPoint = swmapspoints
      .FirstOrDefault(p => string.Equals(p.Remarks, "origin", StringComparison.OrdinalIgnoreCase));

      if (originPoint != null)
      {
        xmean = double.TryParse(originPoint.X, out double x) ? x : xmean;
        ymean = double.TryParse(originPoint.Y, out double y) ? y : ymean;
      }

      List<GoogleEarthPoint> result = new List<GoogleEarthPoint>();

      for (int i = 0; i < swdata.Count; i++)
      {
        double x = swdata[i].GetXAsDouble() + xmean;
        double y = swdata[i].GetYAsDouble() + ymean;
        double z = swdata[i].GetZAsDouble();
        // Convert X and Y to Latitude and Longitude using the fitted lines
        lon = Xline.Slope * x + Xline.Intercept;
        lat = Yline.Slope * y + Yline.Intercept;

        result.Add(new GoogleEarthPoint
        {
          Name = $"P{i + 1}",
          Latitude = lat.ToString("F6"),
          Longitude = lon.ToString("F6"),
          Elevation = z.ToString("F6")
        });
      }
      return result;
    }

    public class GoogleEarthPoint
    {
      public string Name;
      public string Latitude;
      public string Longitude;
      public string Elevation;

      public static string GetHeader()
      {
        return "Name,Latitude,Longitude,Altitude";
      }

      public string GetCSVLine()
      {
        return $"{Name},{Latitude},{Longitude},{Elevation}";
      }
      public static void SaveToCSV(string filePath, List<GoogleEarthPoint> points)
      {
        using (var writer = new StreamWriter(filePath))
        {
          if (!File.Exists(filePath))
          {
            using (var fs = File.Create(filePath)) { } // Create the file if it doesn't exist
          }
          writer.WriteLine(GoogleEarthPoint.GetHeader());
          foreach (var point in points)
          {
            writer.WriteLine(point.GetCSVLine());
          }
        }
      }

      public static void SaveToGPX(string filePath, List<GoogleEarthPoint> points)
      {
        using (var writer = new StreamWriter(filePath))
        {
          writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
          writer.WriteLine("<gpx version=\"1.1\" creator=\"SWMapConverter\" xmlns=\"http://www.topografix.com/GPX/1/1\">");
          foreach (var point in points)
          {
            writer.WriteLine($"  <wpt lat=\"{point.Latitude}\" lon=\"{point.Longitude}\">");
            writer.WriteLine($"    <name>{point.Name}</name>");
            writer.WriteLine($"    <ele>{point.Elevation}</ele>");
            writer.WriteLine("  </wpt>");
          }
          writer.WriteLine("</gpx>");
        }
      }
    }
    public class SolidWorksPoint
    {
      public string X;
      public string Y;
      public string Z;

      public double GetXAsDouble()
      {
        return double.TryParse(X, out double x) ? x : 0.0;
      }
      public double GetYAsDouble()
      {
        return double.TryParse(Y, out double y) ? y : 0.0;
      }
      public double GetZAsDouble()
      {
        return double.TryParse(Z, out double z) ? z : 0.0;
      }

      public static List<GoogleEarthPoint> GetGoogleEarthCSV(List<SwMapPoint> swmapsdata, List<SolidWorksPoint> solidworksdata)
      {
        List<GoogleEarthPoint> result = new List<GoogleEarthPoint>();

        result = SwMapPoint.ConvertXYZToLatLon(swmapsdata, solidworksdata);


        return result;
      }
      public static List<SolidWorksPoint> LoadSWPointsFile(string fileName, Label lbl, Settings settings)
      {
        if (!File.Exists(fileName))
        {
          lbl.Text = "File not found.";
          return new List<SolidWorksPoint>();
        }
        var lines = File.ReadAllLines(fileName).Skip(1); // skip header
        var points = new List<SolidWorksPoint>();

        foreach (var line in lines)
        {
          var parts = line.Split(',');

          if (parts.Length >= 3)
          {
            points.Add(new SolidWorksPoint
            {
              X = parts[0],
              Y = parts[1],
              Z = parts[2]
            });
          }
        }
        lbl.Text = $"Loaded {points.Count} SolidWorks Points";
        settings.Save();
        return points;
      }
    }
  }
}
