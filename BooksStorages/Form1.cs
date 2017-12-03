using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage;

namespace BooksStorages
{
    public partial class Form1 : Form
    {
        Assembly BookCoreAssembly { get; set; }
        public Form1()
        {
            InitializeComponent();
            #region настройка главного окна
            this.Text = new StringBuilder(Application.ProductName + " (v " + Application.ProductVersion + ")").ToString();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = true;//значек отображения на панели задач
            this.MaximizeBox = true;//значек разворачивания на весь экран
            this.MinimizeBox = true;//значек сворачивания на панель задач
            this.MinimumSize = this.Size;
            
            #endregion

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var bookShop = new BooksShopCore.BooksShop();
            #region получение списка классов для работы с хранилищем данных

            var listAssembly = AppDomain.CurrentDomain.GetAssemblies();
            BookCoreAssembly = listAssembly.FirstOrDefault(p=>p.FullName.StartsWith("BooksShopCore"));
            var listTypes =
                BookCoreAssembly?.ExportedTypes.Where(
                    p => p.FullName != null && p.FullName.Contains("BooksShopCore.WorkWithUi.EntityUi"));

            if (listTypes != null)
            {
                this.comboBox1.Items.Clear();
                foreach (var item in listTypes)
                {
                    this.comboBox1.Items.Add(item.Name);
                }
                if (this.comboBox1.Items?.Count > 0)
                {
                    this.comboBox1.SelectedIndex = 0;
                }
            }
            #endregion


            

        }
        private void AddComboboxColumn()
        {
            DataGridViewComboBoxColumn ColComboBox = new DataGridViewComboBoxColumn();
            dataGridView1.Columns.Add(ColComboBox);
            ColComboBox.DataPropertyName = "Authors";
            ColComboBox.HeaderText = "Authors";
            ColComboBox.ValueType = typeof(string);
            ColComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            ColComboBox.DisplayIndex = 2;
            ColComboBox.Width = 150;
            //ColComboBox.DataSource = oleDs;
            ColComboBox.DisplayMember = "Name";
            ColComboBox.ValueMember = "Name";
            ColComboBox.Name = "Authors";
            ColComboBox.DataPropertyName = "Authors";
        }
        string GetExceptionStack(Exception ex)
        {
            string internalEx = String.Empty;
            if (ex.Data?.Count > 0)
            {
                internalEx += $"       Внутренняя ошибка в модуле {ex.Source}: {ex.Message}{Environment.NewLine}";
                foreach (DictionaryEntry except in ex.Data)
                {
                    internalEx += $"                \"{except.Value}\", модуль - {except.Key} {Environment.NewLine}";
                }
            }
            if (ex.InnerException != null)
            {
                internalEx += GetExceptionStack(ex.InnerException);
            }
            return internalEx;
        }
        void ShowException(Exception ex)
        {
            MessageBox.Show($"{GetExceptionStack(ex)}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void GetData(string typeUiName)
        {
            var nameType = "тип не найден";
            try
            {
                var listTypes =
                BookCoreAssembly?.ExportedTypes.Where(
                    p => p.FullName != null && p.FullName.Contains("BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage"));
                var workType = listTypes.FirstOrDefault(p => p.Name.Contains("WorkWith" + typeUiName.Replace("Ui", "")));
                nameType = workType.Name;
                var elemType = Activator.CreateInstance(workType);

                MethodInfo methodInfo = workType.GetMethod("ReadAll", new Type[] { });
                var data= methodInfo.Invoke(elemType, null);

                this.dataGridView1.DataSource = data;
                AddComboboxColumn();
            }
            catch (Exception ex)
            {
                ex.Data.Add("BooksStorages", $"Ошибка выполнения метода \"ReadAll\" для типа \"{nameType}\"");
                ShowException(ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetData(this.comboBox1.Text);
        }
    }
}
