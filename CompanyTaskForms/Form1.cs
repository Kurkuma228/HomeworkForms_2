using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using static CompanyTaskForms.Departments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompanyTaskForms
{
    public partial class Form1 : Form
    {
        private GlobalCompany company;

        public Form1()
        {
            InitializeComponent();

            buttonLoadXml.Click += buttonLoadXml_Click;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.AfterSelect += treeView1_AfterSelect_1;
        }

        private void buttonLoadXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var serializer = new XmlSerializer(typeof(GlobalCompany));
                using (var reader = new StreamReader(ofd.FileName))
                {
                    company = (GlobalCompany)serializer.Deserialize(reader);

                    UpdateTreeView();
                    ShowFirstData();
                }
            }
        }

        private void buttonLoadJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(ofd.FileName);
                        JObject rootObject = JObject.Parse(json);

                        treeView1.Nodes.Clear();
                        TreeNode rootNode = BuildTreeNode("root", rootObject);
                        treeView1.Nodes.Add(rootNode);

                        treeView1.ExpandAll();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки JSON: " + ex.Message);
                    }
                }
            }
        }
        private TreeNode BuildTreeNode(string key, JToken token)
        {
            var node = new TreeNode(key);

            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty property in token.Children<JProperty>())
                    {
                        node.Nodes.Add(BuildTreeNode(property.Name, property.Value));
                    }
                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (var item in token.Children())
                    {
                        node.Nodes.Add(BuildTreeNode($"[{index++}]", item));
                    }
                    break;

                default:
                    node.Text = $"{key}: {token}";
                    break;
            }

            return node;
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            if (selectedNode.Nodes.Count == 0 && selectedNode.Text.Contains(":"))
            {
                dataGridView1.DataSource = null;
                return;
            }

            DataTable table = new DataTable();
            table.Columns.Add("Key");
            table.Columns.Add("Value");

            foreach (TreeNode child in selectedNode.Nodes)
            {
                if (child.Text.Contains(":"))
                {
                    string[] parts = child.Text.Split(new[] { ':' }, 2);
                    table.Rows.Add(parts[0].Trim(), parts[1].Trim());
                }
                else
                {
                    table.Rows.Add(child.Text, "");
                }
            }

            dataGridView1.DataSource = table;
        }

        private void UpdateTreeView()
        {
            treeView1.Nodes.Clear();

            TreeNode root = new TreeNode("Компания");

            if (company.Employee != null)
            {
                TreeNode employeeNode = new TreeNode("Сотрудник");
                employeeNode.Nodes.Add(company.Employee.Name);
                root.Nodes.Add(employeeNode);
            }

            if (company.Office != null)
            {
                TreeNode officeNode = new TreeNode("Офис");
                officeNode.Nodes.Add(company.Office.Location.City);
                root.Nodes.Add(officeNode);
            }

            if (company.Project != null)
            {
                TreeNode projectNode = new TreeNode("Проект");
                projectNode.Nodes.Add(company.Project.Name);
                root.Nodes.Add(projectNode);
            }

            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
        }

        private void ShowFirstData()
        {
            if (treeView1.Nodes.Count > 0 &&
                treeView1.Nodes[0].Nodes.Count > 0 &&
                treeView1.Nodes[0].Nodes[0].Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0].Nodes[0];
                ShowDataInDataGridView();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowDataInDataGridView();
        }

        private void ShowDataInDataGridView()
        {
            if (company == null) return;

            dataGridView1.DataSource = null; 
            dataGridView1.Columns.Clear();  
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("Property", "Свойство");
            dataGridView1.Columns.Add("Value", "Значение");

            string selectedText = treeView1.SelectedNode.Text;

            if (selectedText == company.Employee?.Name)
            {
                AddRow("Имя", company.Employee.Name);
                AddRow("Национальность", company.Employee.Nationality);
                AddRow("Должность", company.Employee.Position);
                AddRow("Отдел", company.Employee.Department);
                AddRow("Дата найма", company.Employee.EmploymentDate);
            }
            else if (selectedText == company.Office?.Location?.City)
            {
                AddRow("Страна", company.Office.Location.Country);
                AddRow("Город", company.Office.Location.City);
                AddRow("Улица", company.Office.Address.Street);
                AddRow("Почтовый индекс", company.Office.Address.PostalCode);
                AddRow("Количество сотрудников", company.Office.EmployeesCount.ToString());
                AddRow("Менеджер", company.Office.Manager);
            }
            else if (selectedText == company.Project?.Name)
            {
                AddRow("Название", company.Project.Name);
                AddRow("Описание", company.Project.Description);
                AddRow("Дата начала", company.Project.StartDate);
                AddRow("Дата завершения", company.Project.EndDate);
                AddRow("Бюджет", $"{company.Project.BudgetUSD:C0}");
            }
        }

        private void AddRow(string property, object value)
        {
            dataGridView1.Rows.Add(property, value);
        }
    }
}
