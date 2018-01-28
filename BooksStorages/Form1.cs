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
using BooksShopCore.WorkWithUi.WorkWithDataStorage;

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

        object SetBinding(object data)
        {
            if (data is List<BookUi>)
            {
                data = (data as List<BookUi>)?.Select(p => new
                {
                    Id = p.BookId,
                    Author = p.Authors[0].Name,
                    Name = p.ListName[0].Name,
                    Year = p.Year,
                    Count = p.Count,
                    Format = p.Format.FormatName,
                    Price = p.ListPrice[0].Price,
                    Currency = p.ListPrice[0].Currency.CurrencyCode
                }).ToList();
            }
            return data;
        }
        void GetData(string typeUiName)
        {
            var nameType = "тип не найден";
            try
            {
                var listTypes = BookCoreAssembly?.ExportedTypes.Where(
                    p => p.FullName != null && p.FullName.Contains("BooksShopCore.WorkWithUi.WorkWithDataStorage"));
                var workType = listTypes.FirstOrDefault(p => p.Name.Contains("WorkWith" + typeUiName.Replace("Ui", "")));
                nameType = workType.Name;
                var elemType = Activator.CreateInstance(workType);

                MethodInfo methodInfo = workType.GetMethod("ReadAll", new Type[] { });
                var data= methodInfo.Invoke(elemType, null);

                this.dataGridView1.DataSource = SetBinding(data);
                //AddComboboxColumn();
            }
            catch (Exception ex)
            {
                ex.Data.Add("BooksStorages", $"Ошибка выполнения метода \"ReadAll\" для типа \"{nameType}\"");
                ShowException(ex);
            }
        }

        async Task GetDataAsync(string typeUiName)
        {
            var nameType = "тип не найден";
            try
            {
                var listTypes = BookCoreAssembly?.ExportedTypes.Where(
                    p => p.FullName != null && p.FullName.Contains("BooksShopCore.WorkWithUi.WorkWithDataStorage"));
                var workType = listTypes.FirstOrDefault(p => p.Name.Contains("WorkWith" + typeUiName.Replace("Ui", "")));
                nameType = workType.Name;
                var elemType = Activator.CreateInstance(workType);

                MethodInfo methodInfo = workType.GetMethod("ReadAllAsync", new Type[] { });
                //var data = methodInfo.Invoke(elemType, null);

                var task = (Task)methodInfo.Invoke(elemType, null);
                await task;
                var resultProperty = task.GetType().GetProperty("Result");
                var data = resultProperty.GetValue(task);

                //dynamic awaitable=methodInfo.Invoke(elemType, null);
                //await awaitable;
                //var data=awaitable.GetAwaiter().GetResult();

                this.dataGridView1.DataSource = SetBinding(data);
                //AddComboboxColumn();
            }
            catch (Exception ex)
            {
                ex.Data.Add("BooksStorages", $"Ошибка выполнения метода \"ReadAll\" для типа \"{nameType}\"");
                ShowException(ex);
            }
        }

        void AddNewBook()
        {
            IWorkWithDataStorage<LanguageUi> Language = new WorkWithLanguageStorage();
            IWorkWithDataStorage<CountryUi> Country = new WorkWithCountryStorage();
            IWorkWithDataStorage<CurrencyUi> Currency = new WorkWithCurrencyStorage();
            IWorkWithDataStorage<AuthorUi> Author = new WorkWithAuthorStorage();

            var book = new BookUi();
            book.Authors = new List<AuthorUi>();
            book.Authors.Add(Author.Read("Стив Макконелл"));
            book.ListName = new List<BookNameUi>();
            book.ListName.Add(new BookNameUi() { Name = "Совершенный код", LanguageBookCode = Language.Read("rus") });
            book.ListName.Add(new BookNameUi() { Name = "Code Complete", LanguageBookCode = Language.Read("eng") });

            book.ListPrice = new List<PriceUi>();
            book.ListPrice.Add(new PriceUi() { Country = Country.Read("ru"), Currency = Currency.Read("rub"), Price = 120.23m });
            book.ListPrice.Add(new PriceUi() { Country = Country.Read("gb"), Currency = Currency.Read("gbp"), Price = 60.55m });
            book.Format = new FormatBookUi() { FormatName = "paper" };
            book.Year = new DateTime(2015, 09, 01);
            book.Count = 10;

            using (var myBook = new WorkWithBooksStorage())
            {
                myBook.Create(book);
            }
        }
        void AddNewBook_old()
        {
            IWorkWithDataStorage<LanguageUi> Language = new WorkWithLanguageStorage();
            IWorkWithDataStorage<CountryUi> Country = new WorkWithCountryStorage();
            IWorkWithDataStorage<CurrencyUi> Currency = new WorkWithCurrencyStorage();

            var book = new BookUi();
            book.Authors = new List<AuthorUi>();
            book.Authors.Add(new AuthorUi()
            {
                Name = "Стив Макконелл",
                Info = " американский программист, автор книг по разработке программного обеспечения.",
                Year = "1962"
            });
            book.ListName = new List<BookNameUi>();
            book.ListName.Add(new BookNameUi() { Name = "Совершенный код", LanguageBookCode = Language.Read("rus") });
            book.ListName.Add(new BookNameUi() { Name = "Code Complete", LanguageBookCode = Language.Read("eng") });
            book.ListPrice = new List<PriceUi>();
            book.ListPrice.Add(new PriceUi() { Country = Country.Read("ru"), Currency = Currency.Read("rub"), Price = 120.23m });
            book.ListPrice.Add(new PriceUi() { Country = Country.Read("gb"), Currency = Currency.Read("gbp"), Price = 60.55m });
            book.Format = new FormatBookUi() { FormatName = "paper" };
            book.Year = new DateTime(2015, 09, 01);
            book.Count = 10;

            using (var myBook = new WorkWithBooksStorage())
            {
                myBook.Create(book);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //GetData(this.comboBox1.Text);
            GetDataAsync(this.comboBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var index = this.dataGridView1.CurrentRow?.Index;
            if (index.HasValue && (index >= 0))
            {
                if (this.dataGridView1.SelectedRows?.Count > 0)
                {
                    switch (this.comboBox1.SelectedItem.ToString().ToLower())
                    {
                        case "bookui":
                            AddNewBook();
                            break;
                        default:
                            MessageBox.Show("Удаление данных из указанной таблицы не поддерживается", "Внимание - программная информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;


                    }

                }
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var index = this.dataGridView1.CurrentRow?.Index;
            if (index.HasValue && (index >= 0))
            {
                if (this.dataGridView1.SelectedRows?.Count>0)
                {
                    switch (this.comboBox1.SelectedItem.ToString().ToLower())
                    {
                        case "bookui":
                            List<int> listBookId = new List<int>();
                            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                            {
                                var bookId = (int)item.Cells["Id"].Value;
                                listBookId.Add(bookId);
                            }

                            if (listBookId.Count > 0)
                            {
                                using (var myBook = new WorkWithBooksStorage())
                                {
                                    foreach (var item in listBookId)
                                    {
                                        myBook.Delete(item);
                                    }
                                }
                            }
                            break;
                        default:
                            MessageBox.Show("Удаление данных из указанной таблицы не поддерживается", "Внимание - программная информация",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            break;
                            

                    }

                }
            }

        }
    }
}
