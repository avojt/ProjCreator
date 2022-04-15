using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ProjCreator
{

    public partial class MainForm : Form
    {
        private bool fileOpened = false;
        string[] arrayRecentDocuments = new string[5];
        string RecentFile;
        string DefaultText;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.Dock = DockStyle.Fill;
            toolStrip1.Dock = DockStyle.Fill;
            textBoxProject.Dock = DockStyle.Fill;
            textBoxRecent.Dock = DockStyle.Fill;
            LoadXMLFile();
            textBoxProject.Text = DefaultText;

        }
        private void LoadXMLFile()
        {
            XmlDocument mxDoc;
            string xmlPath;

            xmlPath = Application.StartupPath + @"\ProjCreator.exe.config";  // "\Config.xml"  ' AppPath & "Config.xml"  'cAppObject.AppPath & "Config.xml";
            mxDoc = new XmlDocument();
            mxDoc.Load(xmlPath);

            ReadXMLFile(mxDoc, 0);
        }

        private void ReadXMLFile(XmlNode xNode, int intLevel)
        {
            //XmlNode xNodeLoop;

            if (xNode.HasChildNodes)
            {
                foreach (XmlNode xNodeLoop in xNode.ChildNodes)
                    ReadXMLFile(xNodeLoop, intLevel + 1);

                switch(xNode.Name)
                {
                     case "DefaultString":
                        DefaultText = xNode.InnerText;
                        break;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripNew_Click(object sender, EventArgs e)
        {
            textBoxProject.Text = "";
            textBoxProject.Select();
            LoadXMLFile();
            textBoxProject.Text = DefaultText;
        }

        private void ToolStripOpen_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                openFileDialog.InitialDirectory = folderBrowserDialog.SelectedPath;
                openFileDialog.FileName = null;
            }

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    Stream StreamLoad = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(StreamLoad))
                    {
                        
                        textBoxProject.Text = reader.ReadToEnd();
                    }
                    StreamLoad.Close();
                    //fileOpened = true;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                    //fileOpened = false;
                }
                Invalidate();
            }

            // Cancel button was pressed.
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            RecentFile = openFileDialog.FileName;
            RecentDocuments();
            textBoxProject.Focus();
        }

        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            FileStream StreamSave = null;

            if (result == DialogResult.OK)
            {
                try
                {
                    StreamSave = new FileStream(fileName, FileMode.OpenOrCreate);
                    using (StreamWriter writer = new StreamWriter(StreamSave))
                    {
                        writer.Write(textBoxProject.Text);
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                    fileOpened = false;
                }
                finally
                {
                    RecentFile = fileName;
                    if (StreamSave != null)
                        StreamSave.Dispose();
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            RecentDocuments();
        }

        private void ToolStripExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RecentDocuments()
        {
            bool upisano = false;

            textBoxRecent.Text = "";
            
            for (int i = 0; i < 5; i++)
            {
                if ((arrayRecentDocuments[i] == null) || (arrayRecentDocuments[i] == ""))
                {
                    arrayRecentDocuments[i] = RecentFile;
                    upisano = true;
                    break;
                }
            }
            if (!upisano)
            {
                for (int i = 0; i < arrayRecentDocuments.Length - 1; i++)
                    arrayRecentDocuments[i] = arrayRecentDocuments[i + 1];
                arrayRecentDocuments[4] = RecentFile;
            }
            textBoxRecent.Text = "";
            for (int i = 0; i < arrayRecentDocuments.Length; i++)
            {
                if (!(arrayRecentDocuments[i] == null) && !(arrayRecentDocuments[i] == ""))
                {
                    textBoxRecent.AppendText(i+1 + ". " + arrayRecentDocuments[i] + "\n" );
                }
            }
        }
    }
}
