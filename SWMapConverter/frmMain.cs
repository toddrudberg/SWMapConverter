using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using static SWMapConverter.SwMapPoint;

namespace SWMapConverter
{
  public partial class frmMain : Form
  {
    List<SwMapPoint> swMapsPoints = new List<SwMapPoint>();
    List<SwMapPoint> otherSwMapPoints = new List<SwMapPoint>();
    List<SolidWorksPoint> swPoints = new List<SolidWorksPoint>();
    Settings settings = new Settings();
    public frmMain()
    {
      InitializeComponent();
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
      settings = Settings.Load();
      lblSWMapsFile.Text = settings.SwmapsInputPath; // Display the saved path if available
      if (!string.IsNullOrEmpty(settings.SwmapsInputPath))
      {
        swMapsPoints = SwMapPoint.LoadSwMapsFile(settings.SwmapsInputPath, lblSWMapsPointsCount, settings); // Load points if path is set
      }

      if( !string.IsNullOrEmpty(settings.OtherSWMapsInputPath))
      {
        lblOtherSWMapsFile.Text = settings.OtherSWMapsInputPath; // Display the saved path if available
        otherSwMapPoints = SwMapPoint.LoadSwMapsFile(settings.OtherSWMapsInputPath, lblOtherSWMapsPointsCount, settings); // Load points if path is set
      }

      if (!string.IsNullOrEmpty(settings.SolidworksPointPath))
      {
        lblSWPointsFile.Text = settings.SolidworksPointPath; // Display the saved path if available
        swPoints = SolidWorksPoint.LoadSWPointsFile(settings.SolidworksPointPath, lblSWPointsCount, settings); // Load points if path is set
      }

      lblInstructions.Text = "1. Load SWMaps CSV file to convert to SolidWorks points.\n" +
                              "   a) This file should remain your basis for the lat/lon conversion you will do later.\n" +
                              "   b) So don't change the point list.\n" +
                              "   c) center of mass of the points will be used to normalize the coordinates unless there is a point where in remarks the word \"origin\" exists and then the points will be normnalized to this point.\n" +
                              "   d) the filewill be converted for import to SolidWorks and saved to c:\\temp\\toSolidworks.csv\n" +
                             "2. Import into Solidworks using the macro Import2dPoints.swp.\n" +
                             "3. Manipulate the data in solidworks and output points using the macro.\n" +
                             "4. Load the SolidWorks points CSV file to convert to lat/lon format for Google Earth.\n" +
                             "5. The output file will be saved to c:\\temp\\toGoogleEarth.csv.\n" +
                             "6. Import in to Google Earth to visualize the points.\n" +
                             "7. Export as KML or KMZ for use in other applications - like SWMaps.\n" +
                             "    a) for SW Maps, move to Maps\\KML\n" +
                             "    b) in SW Maps, add layer."
                             ;
    }
    private void btnSWMapsToSW_Click(object sender, EventArgs e)
    {
      using OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "CSV files (*.csv)|*.csv";
      ofd.Title = "Select SWMaps Input File";
      ofd.InitialDirectory = Path.GetDirectoryName(settings.SwmapsInputPath); // Set initial directory to last used path
      ofd.FileName = Path.GetFileName(settings.SwmapsInputPath); // Set the file name to the last used file

      if (ofd.ShowDialog() == DialogResult.OK)
      {
        try
        {
          // Update and save settings
          settings.SwmapsInputPath = ofd.FileName;
          settings.Save();

          swMapsPoints = SwMapPoint.LoadSwMapsFile(settings.SwmapsInputPath, lblSWMapsPointsCount, settings); // your existing function
          lblSWMapsFile.Text = ofd.FileName;           // if you're showing it in a textbox

          List<SolidWorksPoint> toSolidWorks = SwMapPoint.GetSolidWorksPointList(swMapsPoints);

          string filePath = Path.Combine(@"c:\temp", "toSolidWorks.csv");

          using var writer = new StreamWriter(filePath);
          writer.WriteLine("Name,X,Y"); // header row

          for (int i = 0; i < toSolidWorks.Count; i++)
          {
            string name = $"P{i + 1}";
            string line = $"{name},{toSolidWorks[i].X:F3},{toSolidWorks[i].Y:F3}";
            writer.WriteLine(line);
          }

          MessageBox.Show($"Loaded {swMapsPoints.Count} points from SWMaps file. \nSent {toSolidWorks.Count} points to c:\\temp\\ToSolidworks.csv");

        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error loading file: {ex.Message}");
        }
      }
    }



    private void lblSWMapsFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string filePath = settings.SwmapsInputPath;

      if (File.Exists(filePath))
      {
        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
      }
      else if (Directory.Exists(filePath))
      {
        System.Diagnostics.Process.Start("explorer.exe", $"\"{filePath}\"");
      }
      else
      {
        MessageBox.Show("File or folder not found.");
      }
    }

    private void btnConvertSWPointsToLatLon_Click(object sender, EventArgs e)
    {
      using OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "CSV files (*.csv)|*.csv";
      ofd.Title = "Select SolidWorks Input File (csv)";
      ofd.InitialDirectory = Path.GetDirectoryName(settings.SolidworksPointPath); // Set initial directory to last used path
      ofd.FileName = Path.GetFileName(settings.SolidworksPointPath); // Set the file name to the last used file

      if (ofd.ShowDialog() == DialogResult.OK)
      {
        try
        {

          // Update and save settings
          settings.SolidworksPointPath = ofd.FileName;
          settings.Save();

          swPoints = SolidWorksPoint.LoadSWPointsFile(ofd.FileName, lblSWPointsCount, settings); // your existing function
          lblSWPointsFile.Text = ofd.FileName;           // if you're showing it in a textbox

          List<GoogleEarthPoint> googleEarth = SolidWorksPoint.GetGoogleEarthCSV(swMapsPoints, swPoints);

          string filePath = Path.Combine(@"c:\temp", "toGoogleEarth.csv");
          GoogleEarthPoint.SaveToCSV(filePath, googleEarth);
          string filePathgpx = Path.Combine(@"c:\temp", "toGoogleEarth.gpx");
          GoogleEarthPoint.SaveToGPX(filePathgpx, googleEarth);


          MessageBox.Show($"Loaded {swPoints.Count} points from SWMaps file.\nData Written to C:\\temp\\toGoogleEarth.csv");
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error loading file: {ex.Message}");
        }
      }
    }


    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string filePath = settings.SolidworksPointPath;

      if (File.Exists(filePath))
      {
        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
      }
      else if (Directory.Exists(filePath))
      {
        System.Diagnostics.Process.Start("explorer.exe", $"\"{filePath}\"");
      }
      else
      {
        MessageBox.Show("File or folder not found.");
      }
    }

    private void btnOtherSWMapsToSW_Click(object sender, EventArgs e)
    {
      using OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "CSV files (*.csv)|*.csv";
      ofd.Title = "Select Other SWMaps Input File";
      ofd.InitialDirectory = Path.GetDirectoryName(settings.OtherSWMapsInputPath); // Set initial directory to last used path
      ofd.FileName = Path.GetFileName(settings.OtherSWMapsInputPath); // Set the file name to the last used file

      if (ofd.ShowDialog() == DialogResult.OK)
      {
        try
        {

          // Update and save settings
          settings.OtherSWMapsInputPath = ofd.FileName;
          settings.Save();

          otherSwMapPoints = SwMapPoint.LoadSwMapsFile(settings.OtherSWMapsInputPath, lblOtherSWMapsPointsCount, settings); 
          lblOtherSWMapsFile.Text = ofd.FileName;           // if you're showing it in a textbox



          List<SolidWorksPoint> toSolidWorks = SwMapPoint.GetSolidWorksPointList(swMapsPoints, otherSwMapPoints);

          string filePath = Path.Combine(@"c:\temp", "toSolidWorks.csv");

          using var writer = new StreamWriter(filePath);
          writer.WriteLine("Name,X,Y"); // header row

          for (int i = 0; i < toSolidWorks.Count; i++)
          {
            string name = $"P{i + 1}";
            string line = $"{name},{toSolidWorks[i].X:F3},{toSolidWorks[i].Y:F3}";
            writer.WriteLine(line);
          }

          MessageBox.Show($"Loaded {otherSwMapPoints.Count} points from SWMaps file. \nSent {toSolidWorks.Count} points to c:\\temp\\ToSolidworks.csv");

        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error loading file: {ex.Message}");
        }
      }
    }
  }

  public class Settings
  {
    public string SwmapsInputPath { get; set; }
    public string SolidworksPointPath { get; set; }
    public string OtherSWMapsInputPath { get; set; }

    private static string SettingsFile => "settings.json";

    public static Settings Load()
    {
      if (File.Exists(SettingsFile))
      {
        var json = File.ReadAllText(SettingsFile);
        return JsonSerializer.Deserialize<Settings>(json);
      }
      return new Settings();
    }

    public void Save()
    {
      var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(SettingsFile, json);
    }
  }
}
