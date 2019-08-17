using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using WinForms = System.Windows.Forms;
using System.Windows.Forms;

namespace YanitskyiGLTest
{
    public partial class MainWindow : Window
    {
        public Models.Directory Directory { get; set; }
        public string SavePath1 { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void SelectDirectory_Click(object sender, RoutedEventArgs e)
        {            
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                DirectoryPath.Text = dialog.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(dialog.SelectedPath);
                Directory = GetChildren(dialog.SelectedPath);
            }
        }
        private Models.Directory GetChildren(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            Models.Directory directory = new Models.Directory
            {
                Name = info.Name,
                DataOfCreating = info.CreationTime.ToLongDateString()
            };
            List<Models.File> files = new List<Models.File>();
            foreach (var f in info.GetFiles())
            {
                files.Add(new Models.File
                {
                    Name = f.Name,
                    Size = f.Length,
                    Path = f.FullName
                });
            }

            directory.Files = files;
            if (info.GetDirectories().Count() < 1){}
            else
            {
                List<Models.Directory> directories = new List<Models.Directory>();
                foreach (var k in info.GetDirectories())
                {
                    directories.Add(GetChildren(k.FullName));
                }
                directory.Children = directories;
            }
            return directory;
        }
        private List<Models.File> GetFiles()
        {
            return new List<Models.File>();
        }
        private void SelectFilePath_Click(object sender, RoutedEventArgs e)
        {           
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                SavePath1 = dialog.SelectedPath + "\\Result.txt";
                SavePath.Text = SavePath1;
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {            
            if (SavePath == null)
            {
                WinForms.MessageBox.Show("Choose path for saving result file!");
            }
            else if (Directory == null)
            {
                WinForms.MessageBox.Show("Choose directory!");
            }
            else
            {
                string json = JsonConvert.SerializeObject(Directory);
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string jsonVlid =  JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                using (FileStream fs = new FileStream(SavePath1, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] jsonBuffer = Encoding.UTF8.GetBytes(jsonVlid);                   
                    fs.Write(jsonBuffer, 0, jsonBuffer.Length);
                }
                SavePath.Text = "[Path will appear here]";
                DirectoryPath.Text = "[Directory will apear here]";
                Directory = null;
                SavePath = null;
                WinForms.MessageBox.Show("Json file has been saved", "Success");
            }
        }
    }
}
