using System.Xml;
using System;
using System.Configuration;

namespace k190144_Q3
{
    public partial class Form1 : Form
    {
        private string path;
        private List<MarketSummaryMainBoard> summary; 
        
        public Form1()
        {
            InitializeComponent();
            path = ConfigurationManager.AppSettings["path"];
            summary = new List<MarketSummaryMainBoard>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListAllCategories();
        }

        private void ListAllCategories()
        {
            DirectoryInfo di = new(path);
            DirectoryInfo[] diArr = di.GetDirectories();
            foreach (DirectoryInfo dri in diArr)
                comboBox1.Items.Add(dri.Name);

            foreach (DirectoryInfo dri in diArr)
            {
                parseXml(dri.ToString());
            }

            addDataToGridView();
        }

        private void parseXml(string dri)
        {
            var directory = new DirectoryInfo(dri);
            var fileName = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            XmlTextReader readFile = new XmlTextReader(fileName.ToString());
            string script = "", price = "";

            while (readFile.Read())
            {
                if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Script")
                {
                    script = readFile.ReadElementString();
                }
                if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Price")
                {
                    price = readFile.ReadElementString();
                    summary.Add(new MarketSummaryMainBoard() { Script = script, CurrentPrice = price });
                }
            }

        }
        private void addDataToGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            for (int i = 0; i < summary.Count; i++)
            {
                dataGridView1.Rows.Add(summary[i].Script, summary[i].CurrentPrice);
            }

            summary.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            parseXml(path + "\\" + comboBox1.Text);
            addDataToGridView();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ListAllCategories();
        }
    }
}