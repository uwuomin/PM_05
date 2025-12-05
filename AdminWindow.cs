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
using System.Drawing.Text;

namespace ПМ._05
{
    public partial class AdminWindow : Form
    {
        private Font comicSansFont;
        private Color buttonColor = Color.FromArgb(146, 208, 80);
        private Color buttonTextColor = Color.Black;

        public string CurrentUserRole { get; set; }
        
        public class Product
        {
            public int Id { get; set; }
            public string Article { get; set; } 
            public string Name { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public string Supplier { get; set; } 
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public byte[] ImageData { get; set; }
            public string Category { get; set; }
            public string Unit { get; set; }
            public int MaxDiscount { get; set; } 
            public int CurrentDiscount { get; set; } 
        }

    
        public class ProductEditData
        {
            public int Id { get; set; }
            public string Article { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public string Supplier { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string Category { get; set; }
            public int MaxDiscount { get; set; }
            public int CurrentDiscount { get; set; }
            public byte[] ImageData { get; set; }
        }

        public class ProductEditForm : Form
        {
            private ProductEditData currentProduct;
            private bool isEditMode = false;
            private static bool isFormOpen = false;
            private Font comicSansFont;
            private Color buttonColor = Color.FromArgb(146, 208, 80);
            private Color buttonTextColor = Color.Black;

            private System.Windows.Forms.Label lblId;
            private TextBox txtId;
            private System.Windows.Forms.Label lblArticle;
            private TextBox txtArticle;
            private System.Windows.Forms.Label lblName;
            private TextBox txtName;
            private System.Windows.Forms.Label lblCategory;
            private ComboBox cmbCategory;
            private System.Windows.Forms.Label lblQuantity;
            private NumericUpDown numQuantity;
            private System.Windows.Forms.Label lblManufacturer;
            private TextBox txtManufacturer;
            private System.Windows.Forms.Label lblSupplier;
            private TextBox txtSupplier;
            private System.Windows.Forms.Label lblPrice;
            private NumericUpDown numPrice;
            private System.Windows.Forms.Label lblMaxDiscount;
            private NumericUpDown numMaxDiscount;
            private System.Windows.Forms.Label lblCurrentDiscount;
            private NumericUpDown numCurrentDiscount;
            private System.Windows.Forms.Label lblDescription;
            private TextBox txtDescription;
            private PictureBox pictureBox;
            private Button btnLoadImage;
            private Button btnClearImage;
            private Button btnSave;
            private Button btnCancel;

            public ProductEditData EditedProduct { get; private set; }

            public ProductEditForm()
            {
                InitializeFont();
                InitializeComponent();
                currentProduct = new ProductEditData();
                isEditMode = false;
                SetupForm();
                ApplyStyles();
            }

            public ProductEditForm(ProductEditData product)
            {
                if (isFormOpen)
                {
                    throw new InvalidOperationException("Форма редактирования уже открыта!");
                }

                InitializeFont();
                InitializeComponent();
                currentProduct = product;
                isEditMode = true;
                SetupForm();
                LoadProductData();
                ApplyStyles();
            }

            private void InitializeFont()
            {
                try
                {
                    if (FontFamily.Families.Any(f => f.Name.Equals("Comic Sans MS", StringComparison.OrdinalIgnoreCase)))
                    {
                        comicSansFont = new Font("Comic Sans MS", 9f, FontStyle.Regular);
                    }
                    else
                    {
                    
                        comicSansFont = new Font(this.Font.FontFamily, 9f, FontStyle.Regular);
                    }
                }
                catch
                {
                    comicSansFont = this.Font;
                }
            }

            private void InitializeComponent()
            {
                this.Text = isEditMode ? "Редактирование товара" : "Добавление товара";
                this.Size = new Size(600, 750);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

              
                Panel mainPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true 
                };

                Panel contentPanel = new Panel
                {
                    Width = 550, 
                    Padding = new Padding(20)
                };

                // ID товара
                lblId = new System.Windows.Forms.Label
                {
                    Text = "ID товара:",
                    Location = new Point(20, 20),
                    Width = 150,
                    Font = comicSansFont,
                    Visible = isEditMode
                };

                txtId = new TextBox
                {
                    Location = new Point(180, 17),
                    Width = 200,
                    ReadOnly = true,
                    Font = comicSansFont,
                    Visible = isEditMode
                };

                // Артикул
                lblArticle = new System.Windows.Forms.Label
                {
                    Text = "Артикул: *",
                    Location = new Point(20, 50),
                    Width = 150,
                    Font = comicSansFont
                };

                txtArticle = new TextBox
                {
                    Location = new Point(180, 47),
                    Width = 200,
                    Font = comicSansFont
                };

                // Наименование
                lblName = new System.Windows.Forms.Label
                {
                    Text = "Наименование: *",
                    Location = new Point(20, 80),
                    Width = 150,
                    Font = comicSansFont
                };

                txtName = new TextBox
                {
                    Location = new Point(180, 77),
                    Width = 300,
                    Font = comicSansFont
                };

                // Категория
                lblCategory = new System.Windows.Forms.Label
                {
                    Text = "Категория: *",
                    Location = new Point(20, 110),
                    Width = 150,
                    Font = comicSansFont
                };

                cmbCategory = new ComboBox
                {
                    Location = new Point(180, 107),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = comicSansFont
                };

                // Количество
                lblQuantity = new System.Windows.Forms.Label
                {
                    Text = "Количество на складе:",
                    Location = new Point(20, 140),
                    Width = 150,
                    Font = comicSansFont
                };

                numQuantity = new NumericUpDown
                {
                    Location = new Point(180, 137),
                    Width = 100,
                    Minimum = 0,
                    Maximum = 10000,
                    Font = comicSansFont
                };

                // Производитель
                lblManufacturer = new System.Windows.Forms.Label
                {
                    Text = "Производитель: *",
                    Location = new Point(20, 170),
                    Width = 150,
                    Font = comicSansFont
                };

                txtManufacturer = new TextBox
                {
                    Location = new Point(180, 167),
                    Width = 200,
                    Font = comicSansFont
                };

                // Поставщик
                lblSupplier = new System.Windows.Forms.Label
                {
                    Text = "Поставщик: *",
                    Location = new Point(20, 200),
                    Width = 150,
                    Font = comicSansFont
                };

                txtSupplier = new TextBox
                {
                    Location = new Point(180, 197),
                    Width = 200,
                    Font = comicSansFont
                };

                // Цена
                lblPrice = new System.Windows.Forms.Label
                {
                    Text = "Цена: *",
                    Location = new Point(20, 230),
                    Width = 150,
                    Font = comicSansFont
                };

                numPrice = new NumericUpDown
                {
                    Location = new Point(180, 227),
                    Width = 100,
                    Minimum = 0,
                    Maximum = 1000000,
                    DecimalPlaces = 2,
                    Font = comicSansFont
                };

                System.Windows.Forms.Label lblCurrency = new System.Windows.Forms.Label
                {
                    Text = "руб.",
                    Location = new Point(290, 230),
                    Width = 40,
                    Font = comicSansFont
                };

                // Максимальная скидка
                lblMaxDiscount = new System.Windows.Forms.Label
                {
                    Text = "Макс. скидка (%):",
                    Location = new Point(20, 260),
                    Width = 150,
                    Font = comicSansFont
                };

                numMaxDiscount = new NumericUpDown
                {
                    Location = new Point(180, 257),
                    Width = 100,
                    Minimum = 0,
                    Maximum = 100,
                    Font = comicSansFont
                };

                // Текущая скидка
                lblCurrentDiscount = new System.Windows.Forms.Label
                {
                    Text = "Текущая скидка (%):",
                    Location = new Point(20, 290),
                    Width = 150,
                    Font = comicSansFont
                };

                numCurrentDiscount = new NumericUpDown
                {
                    Location = new Point(180, 287),
                    Width = 100,
                    Minimum = 0,
                    Maximum = 100,
                    Font = comicSansFont
                };

                // Изображение
                System.Windows.Forms.Label lblImage = new System.Windows.Forms.Label
                {
                    Text = "Изображение:",
                    Location = new Point(20, 320),
                    Width = 150,
                    Font = comicSansFont
                };

                pictureBox = new PictureBox
                {
                    Location = new Point(180, 320),
                    Size = new Size(150, 150),
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                btnLoadImage = new Button
                {
                    Text = "Загрузить",
                    Location = new Point(340, 320),
                    Size = new Size(80, 30),
                    Font = comicSansFont,
                    BackColor = buttonColor,
                    ForeColor = buttonTextColor,
                    FlatStyle = FlatStyle.Flat
                };
                btnLoadImage.Click += BtnLoadImage_Click;

                btnClearImage = new Button
                {
                    Text = "Очистить",
                    Location = new Point(340, 355),
                    Size = new Size(80, 30),
                    Font = comicSansFont,
                    BackColor = buttonColor,
                    ForeColor = buttonTextColor,
                    FlatStyle = FlatStyle.Flat
                };
                btnClearImage.Click += BtnClearImage_Click;

                // Описание
                lblDescription = new System.Windows.Forms.Label
                {
                    Text = "Описание:",
                    Location = new Point(20, 480),
                    Width = 150,
                    Font = comicSansFont
                };

                txtDescription = new TextBox
                {
                    Location = new Point(180, 480),
                    Size = new Size(300, 100),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = comicSansFont
                };

                // Кнопки
                btnSave = new Button
                {
                    Text = "Сохранить",
                    Location = new Point(180, 600),
                    Size = new Size(100, 35),
                    BackColor = buttonColor,
                    ForeColor = buttonTextColor,
                    Font = comicSansFont,
                    FlatStyle = FlatStyle.Flat
                };
                btnSave.Click += BtnSave_Click;

                btnCancel = new Button
                {
                    Text = "Отмена",
                    Location = new Point(290, 600),
                    Size = new Size(100, 35),
                    BackColor = buttonColor,
                    ForeColor = buttonTextColor,
                    Font = comicSansFont,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancel.Click += BtnCancel_Click;

                // Добавляем элементы на панель контента
                contentPanel.Controls.AddRange(new Control[]
                {
                    lblId, txtId,
                    lblArticle, txtArticle,
                    lblName, txtName,
                    lblCategory, cmbCategory,
                    lblQuantity, numQuantity,
                    lblManufacturer, txtManufacturer,
                    lblSupplier, txtSupplier,
                    lblPrice, numPrice, lblCurrency,
                    lblMaxDiscount, numMaxDiscount,
                    lblCurrentDiscount, numCurrentDiscount,
                    lblImage, pictureBox, btnLoadImage, btnClearImage,
                    lblDescription, txtDescription,
                    btnSave, btnCancel
                });

                
                mainPanel.Controls.Add(contentPanel);
                this.Controls.Add(mainPanel);
            }

            private void ApplyStyles()
            {
                
                ApplyFontToControls(this.Controls);
            }

            private void ApplyFontToControls(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    control.Font = comicSansFont;

                    
                    if (control.HasChildren)
                    {
                        ApplyFontToControls(control.Controls);
                    }
                }
            }

            private void SetupForm()
            {
                isFormOpen = true;
                this.Font = comicSansFont;

                
                cmbCategory.Items.AddRange(new string[]
                {
                    "Спортивный инвентарь",
                    "Одежда",
                    "Прочее"
                });

               
                if (!isEditMode)
                {
                    cmbCategory.SelectedIndex = 0;
                    numCurrentDiscount.Value = 0;
                    numMaxDiscount.Value = 0;
                }
            }

            private void LoadProductData()
            {
                txtId.Text = currentProduct.Id.ToString();
                txtArticle.Text = currentProduct.Article;
                txtName.Text = currentProduct.Name;

                if (!string.IsNullOrEmpty(currentProduct.Category))
                {
                    cmbCategory.SelectedItem = currentProduct.Category;
                }
                else
                {
                    cmbCategory.SelectedIndex = 0;
                }

                numQuantity.Value = currentProduct.StockQuantity;
                txtManufacturer.Text = currentProduct.Manufacturer;
                txtSupplier.Text = currentProduct.Supplier;
                numPrice.Value = currentProduct.Price;
                numMaxDiscount.Value = currentProduct.MaxDiscount;
                numCurrentDiscount.Value = currentProduct.CurrentDiscount;
                txtDescription.Text = currentProduct.Description;

                // Загрузка изображения
                if (currentProduct.ImageData != null && currentProduct.ImageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(currentProduct.ImageData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
            }

            private void BtnLoadImage_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    openFileDialog.Title = "Выберите изображение товара";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                        }
                    }
                }
            }

            private void BtnClearImage_Click(object sender, EventArgs e)
            {
                pictureBox.Image = null;
            }

            private void BtnSave_Click(object sender, EventArgs e)
            {
                if (!ValidateData())
                    return;

                SaveProductData();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            private bool ValidateData()
            {
          
                if (string.IsNullOrWhiteSpace(txtArticle.Text))
                {
                    MessageBox.Show("Введите артикул товара!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    txtArticle.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите наименование товара!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    txtName.Focus();
                    return false;
                }

                if (cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Выберите категорию товара!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    cmbCategory.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtManufacturer.Text))
                {
                    MessageBox.Show("Введите производителя товара!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    txtManufacturer.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtSupplier.Text))
                {
                    MessageBox.Show("Введите поставщика товара!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    txtSupplier.Focus();
                    return false;
                }

                if (numPrice.Value <= 0)
                {
                    MessageBox.Show("Цена должна быть больше 0!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    numPrice.Focus();
                    return false;
                }

                if (numCurrentDiscount.Value > numMaxDiscount.Value)
                {
                    MessageBox.Show("Текущая скидка не может быть больше максимальной!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    numCurrentDiscount.Focus();
                    return false;
                }

                return true;
            }

            private void SaveProductData()
            {
                EditedProduct = new ProductEditData
                {
                    Id = isEditMode ? currentProduct.Id : 0,
                    Article = txtArticle.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Category = cmbCategory.SelectedItem.ToString(),
                    StockQuantity = (int)numQuantity.Value,
                    Manufacturer = txtManufacturer.Text.Trim(),
                    Supplier = txtSupplier.Text.Trim(),
                    Price = numPrice.Value,
                    MaxDiscount = (int)numMaxDiscount.Value,
                    CurrentDiscount = (int)numCurrentDiscount.Value,
                    Description = txtDescription.Text.Trim()
                };

                if (pictureBox.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        EditedProduct.ImageData = ms.ToArray();
                    }
                }
                else
                {
                    EditedProduct.ImageData = isEditMode ? currentProduct.ImageData : null;
                }
            }

            private void BtnCancel_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            protected override void OnFormClosed(FormClosedEventArgs e)
            {
                base.OnFormClosed(e);
                isFormOpen = false;
            }
        }

        private List<Product> products = new List<Product>();
        private DataGridView dataGridView;
        private Button btnAddProduct;
        private Button btnEditProduct;
        private Button btnDeleteProduct;
        private Button btnRefresh;
        private Button btnExit;
        private TextBox txtSearch;
        private Button btnSearch;
        private Panel dataGridPanel;

        public string CurrentUserName { get; set; }

        public AdminWindow()
        {
            InitializeFont();
            InitializeComponent();
            InitializeAdminControls();
            LoadExcelData(); 
            RefreshDataGrid();
            ApplyStyles();
        }

        private void InitializeFont()
        {
            try
            {
                if (FontFamily.Families.Any(f => f.Name.Equals("Comic Sans MS", StringComparison.OrdinalIgnoreCase)))
                {
                    comicSansFont = new Font("Comic Sans MS", 9f, FontStyle.Regular);
                }
                else
                {
                    
                    comicSansFont = new Font(this.Font.FontFamily, 9f, FontStyle.Regular);
                }
            }
            catch
            {
                comicSansFont = this.Font;
            }
        }

        private void InitializeAdminControls()
        {
            
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Font = comicSansFont
            };

          
            Panel controlPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Font = comicSansFont
            };

            
            btnAddProduct = new Button
            {
                Text = "Добавить товар",
                Location = new Point(10, 10),
                Size = new Size(120, 35),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                FlatStyle = FlatStyle.Flat
            };
            btnAddProduct.Click += BtnAddProduct_Click;

            
            btnEditProduct = new Button
            {
                Text = "Редактировать",
                Location = new Point(140, 10),
                Size = new Size(120, 35),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                Enabled = false,
                FlatStyle = FlatStyle.Flat
            };
            btnEditProduct.Click += BtnEditProduct_Click;

           
            btnDeleteProduct = new Button
            {
                Text = "Удалить",
                Location = new Point(270, 10),
                Size = new Size(120, 35),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                Enabled = false,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteProduct.Click += BtnDeleteProduct_Click;

          
            btnRefresh = new Button
            {
                Text = "Обновить",
                Location = new Point(400, 10),
                Size = new Size(120, 35),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += BtnRefresh_Click;

          
            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label
            {
                Text = "Поиск:",
                Location = new Point(10, 55),
                Width = 50,
                Font = comicSansFont
            };

            txtSearch = new TextBox
            {
                Location = new Point(70, 52),
                Width = 300,
                Font = comicSansFont
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnSearch = new Button
            {
                Text = "Найти",
                Location = new Point(380, 50),
                Size = new Size(80, 25),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;

        
            btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(650, 10),
                Size = new Size(100, 35),
                BackColor = buttonColor,
                ForeColor = buttonTextColor,
                Font = comicSansFont,
                FlatStyle = FlatStyle.Flat
            };
            btnExit.Click += ButtonExit_Click;

         
            controlPanel.Controls.AddRange(new Control[]
            {
                btnAddProduct, btnEditProduct, btnDeleteProduct, btnRefresh,
                lblSearch, txtSearch, btnSearch, btnExit
            });

          
            dataGridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };

           
            dataGridView = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                RowHeadersVisible = false,
                Font = comicSansFont,
                DefaultCellStyle = new DataGridViewCellStyle { Font = comicSansFont },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = comicSansFont,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                RowTemplate = new DataGridViewRow { DefaultCellStyle = new DataGridViewCellStyle { Font = comicSansFont } },
                ScrollBars = ScrollBars.None 
            };

            
            dataGridView.Width = 1500; 

            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;

           
            dataGridPanel.Controls.Add(dataGridView);

           
            mainPanel.Controls.Add(dataGridPanel);
            mainPanel.Controls.Add(controlPanel);

          
            this.Controls.Add(mainPanel);

            
            this.Size = new Size(1200, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadExcelData()
        {
          
            products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Article = "А112Т4",
                    Name = "Боксерская груша",
                    Unit = "шт.",
                    Price = 778,
                    MaxDiscount = 30,
                    Manufacturer = "X-Match",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 6,
                    Description = "Боксерская груша X-Match черная"
                },
                new Product
                {
                    Id = 2,
                    Article = "G598Y6",
                    Name = "Спортивный мат",
                    Unit = "шт.",
                    Price = 2390,
                    MaxDiscount = 15,
                    Manufacturer = "Perfetto Sport",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 2,
                    StockQuantity = 16,
                    Description = "Спортивный мат 100x100x10 см Perfetto Sport № 3 бежевый"
                },
                new Product
                {
                    Id = 3,
                    Article = "F746E6",
                    Name = "Шведская стенка",
                    Unit = "шт.",
                    Price = 9900,
                    MaxDiscount = 10,
                    Manufacturer = "ROMANA Next",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 5,
                    Description = "Шведская стенка ROMANA Next, pastel"
                },
                new Product
                {
                    Id = 4,
                    Article = "D830R5",
                    Name = "Тренажер прыжков",
                    Unit = "шт.",
                    Price = 1120,
                    MaxDiscount = 15,
                    Manufacturer = "Moby Kids",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 8,
                    Description = "Тренажер для прыжков Moby Kids Moby-Jumper со счетчиком"
                },
                new Product
                {
                    Id = 5,
                    Article = "B538G6",
                    Name = "Спортивный костюм",
                    Unit = "шт.",
                    Price = 839,
                    MaxDiscount = 5,
                    Manufacturer = "playToday",
                    Supplier = "Спортмастер",
                    Category = "Одежда",
                    CurrentDiscount = 3,
                    StockQuantity = 17,
                    Description = "Спортивный костюм playToday (футболка + шорты)"
                },
                new Product
                {
                    Id = 6,
                    Article = "D648N7",
                    Name = "Набор для хоккея",
                    Unit = "шт.",
                    Price = 350,
                    MaxDiscount = 10,
                    Manufacturer = "Совтехстром",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 7,
                    Description = "Набор для хоккея Совтехстром"
                },
                new Product
                {
                    Id = 7,
                    Article = "F735B6",
                    Name = "Игровой набор",
                    Unit = "шт.",
                    Price = 320,
                    MaxDiscount = 15,
                    Manufacturer = "Совтехстром",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 2,
                    StockQuantity = 9,
                    Description = "Игровой набор Совтехстром Кегли и шары"
                },
                new Product
                {
                    Id = 8,
                    Article = "F937G4",
                    Name = "Игровой набор",
                    Unit = "шт.",
                    Price = 480,
                    MaxDiscount = 10,
                    Manufacturer = "Abtoys",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 12,
                    Description = "Набор Abtoys Бадминтон и теннис"
                },
                new Product
                {
                    Id = 9,
                    Article = "E324U7",
                    Name = "Велотренажер",
                    Unit = "шт.",
                    Price = 6480,
                    MaxDiscount = 25,
                    Manufacturer = "DFC",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 5,
                    Description = "Велотренажер двойной DFC B804 dual bike"
                },
                new Product
                {
                    Id = 10,
                    Article = "G403T5",
                    Name = "Тюбинг",
                    Unit = "шт.",
                    Price = 1450,
                    MaxDiscount = 15,
                    Manufacturer = "Nordway",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 13,
                    Description = "Тюбинг Nordway, 73 см"
                },
                new Product
                {
                    Id = 11,
                    Article = "N483G5",
                    Name = "Клюшка",
                    Unit = "шт.",
                    Price = 1299,
                    MaxDiscount = 10,
                    Manufacturer = "Nordway",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 4,
                    Description = "Клюшка Nordway NDW300 (2019/2020) SR лев. 19 150см"
                },
                new Product
                {
                    Id = 12,
                    Article = "D038G6",
                    Name = "Лыжный комплект",
                    Unit = "шт.",
                    Price = 3000,
                    MaxDiscount = 30,
                    Manufacturer = "Nordway",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 23,
                    Description = "Лыжный комплект беговые NORDWAY XC Classic, 45-45-45мм, 160см"
                },
                new Product
                {
                    Id = 13,
                    Article = "G480F5",
                    Name = "Ролики",
                    Unit = "шт.",
                    Price = 1600,
                    MaxDiscount = 15,
                    Manufacturer = "Ridex",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 7,
                    Description = "Коньки роликовые Ridex Cricket жен. ABEC 3 кол.:72мм р.:39-42 синий"
                },
                new Product
                {
                    Id = 14,
                    Article = "C324S5",
                    Name = "Шлем",
                    Unit = "шт.",
                    Price = 4000,
                    MaxDiscount = 10,
                    Manufacturer = "Salomon",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 16,
                    Description = "Шлем г.л./сноуб. Salomon Grom р.:KS черный (L40836800)"
                },
                new Product
                {
                    Id = 15,
                    Article = "V312R4",
                    Name = "Мяч",
                    Unit = "шт.",
                    Price = 4150,
                    MaxDiscount = 20,
                    Manufacturer = "Mikasa",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 2,
                    StockQuantity = 5,
                    Description = "Мяч волейбольный MIKASA VT370W, для зала, 5-й размер, желтый/синий"
                },
                new Product
                {
                    Id = 16,
                    Article = "J4DF5E",
                    Name = "Насос",
                    Unit = "шт.",
                    Price = 300,
                    MaxDiscount = 5,
                    Manufacturer = "Molten",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 12,
                    Description = "Насос Molten HP-18-B для мячей мультиколор"
                },
                new Product
                {
                    Id = 17,
                    Article = "G522B5",
                    Name = "Ласты",
                    Unit = "шт.",
                    Price = 1980,
                    MaxDiscount = 15,
                    Manufacturer = "Colton",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 6,
                    Description = "Ласты Colton CF-02 для плавания р.:33-34 серый/голубой"
                },
                new Product
                {
                    Id = 18,
                    Article = "K432G6",
                    Name = "Шапочка для плавания",
                    Unit = "шт.",
                    Price = 440,
                    MaxDiscount = 25,
                    Manufacturer = "Atemi",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 17,
                    Description = "Шапочка для плавания Atemi PU 140 ткань с покрытием желтый"
                },
                new Product
                {
                    Id = 19,
                    Article = "J532D4",
                    Name = "Перчатки для карате",
                    Unit = "шт.",
                    Price = 1050,
                    MaxDiscount = 15,
                    Manufacturer = "Green Hill",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 5,
                    Description = "Перчатки для каратэ Green Hill KMС-6083 L красный"
                },
                new Product
                {
                    Id = 20,
                    Article = "G873H4",
                    Name = "Велосипед",
                    Unit = "шт.",
                    Price = 14930,
                    MaxDiscount = 5,
                    Manufacturer = "SKIF",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 6,
                    Description = "Велосипед SKIF 29 Disc (2021), горный (взрослый), рама: 17\", колеса: 29\", темно-серый"
                },
                new Product
                {
                    Id = 21,
                    Article = "V423D4",
                    Name = "Штанга",
                    Unit = "шт.",
                    Price = 5600,
                    MaxDiscount = 10,
                    Manufacturer = "Starfit",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 8,
                    Description = "Штанга Starfit BB-401 30кг пласт. черный"
                },
                new Product
                {
                    Id = 22,
                    Article = "K937A5",
                    Name = "Гиря",
                    Unit = "шт.",
                    Price = 890,
                    MaxDiscount = 5,
                    Manufacturer = "Starfit",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 10,
                    Description = "Гиря Starfit ГМБ4 мягкое 4кг синий/оранжевый"
                },
                new Product
                {
                    Id = 23,
                    Article = "F047J7",
                    Name = "Коврик",
                    Unit = "шт.",
                    Price = 720,
                    MaxDiscount = 15,
                    Manufacturer = "Bradex",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 11,
                    Description = "Коврик Bradex для мягкой йоги дл.:1730мм ш.:610мм т.:3мм серый"
                },
                new Product
                {
                    Id = 24,
                    Article = "S374B5",
                    Name = "Ролик для йоги",
                    Unit = "шт.",
                    Price = 700,
                    MaxDiscount = 10,
                    Manufacturer = "Bradex",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 12,
                    Description = "Ролик для йоги Bradex Туба d=14см ш.:33см оранжевый"
                },
                new Product
                {
                    Id = 25,
                    Article = "F687G5",
                    Name = "Защита голени",
                    Unit = "шт.",
                    Price = 1900,
                    MaxDiscount = 15,
                    Manufacturer = "Green Hill",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 6,
                    Description = "Защита голени GREEN HILL Panther, L, синий/черный"
                },
                new Product
                {
                    Id = 26,
                    Article = "N892G6",
                    Name = "Очки для плавания",
                    Unit = "шт.",
                    Price = 500,
                    MaxDiscount = 5,
                    Manufacturer = "Atemi",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 5,
                    StockQuantity = 14,
                    Description = "Очки для плавания Atemi N8401 синий"
                },
                new Product
                {
                    Id = 27,
                    Article = "D893W4",
                    Name = "Мяч",
                    Unit = "шт.",
                    Price = 900,
                    MaxDiscount = 5,
                    Manufacturer = "Demix",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 2,
                    StockQuantity = 5,
                    Description = "Мяч футбольный DEMIX 1STLS1JWWW, универсальный, 4-й размер, белый/зеленый"
                },
                new Product
                {
                    Id = 28,
                    Article = "N836R5",
                    Name = "Коньки",
                    Unit = "шт.",
                    Price = 2000,
                    MaxDiscount = 10,
                    Manufacturer = "Atemi",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 16,
                    Description = "Коньки ATEMI AKSK01DXS, раздвижные, прогувочные, унисекс, 27-30, черный/зеленый"
                },
                new Product
                {
                    Id = 29,
                    Article = "D927K3",
                    Name = "Перчатки",
                    Unit = "шт.",
                    Price = 660,
                    MaxDiscount = 15,
                    Manufacturer = "Starfit",
                    Supplier = "Декатлон",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 4,
                    StockQuantity = 3,
                    Description = "Перчатки Starfit SU-125 атлетические S черный"
                },
                new Product
                {
                    Id = 30,
                    Article = "V392H7",
                    Name = "Степ-платформа",
                    Unit = "шт.",
                    Price = 4790,
                    MaxDiscount = 10,
                    Manufacturer = "Starfit",
                    Supplier = "Спортмастер",
                    Category = "Спортивный инвентарь",
                    CurrentDiscount = 3,
                    StockQuantity = 15,
                    Description = "Степ-платформа Starfit SP-204 серый/черный"
                }
            };
        }

        private void ApplyStyles()
        {
           
            this.Font = comicSansFont;

            ApplyFontToControls(this.Controls);

          
            StyleButtons(this.Controls);
        }

        private void ApplyFontToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                control.Font = comicSansFont;

                if (control.HasChildren)
                {
                    ApplyFontToControls(control.Controls);
                }
            }
        }

        private void StyleButtons(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Button button)
                {
                    button.BackColor = buttonColor;
                    button.ForeColor = buttonTextColor;
                    button.Font = comicSansFont;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.DarkGray;
                }

                
                if (control.HasChildren)
                {
                    StyleButtons(control.Controls);
                }
            }
        }

        private void RefreshDataGrid()
        {
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();

            dataGridView.Columns.Add("Id", "ID");
            dataGridView.Columns.Add("Article", "Артикул");
            dataGridView.Columns.Add("Name", "Наименование");
            dataGridView.Columns.Add("Unit", "Ед. изм.");
            dataGridView.Columns.Add("Price", "Стоимость");
            dataGridView.Columns.Add("MaxDiscount", "Макс. скидка (%)");
            dataGridView.Columns.Add("Manufacturer", "Производитель");
            dataGridView.Columns.Add("Supplier", "Поставщик");
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("CurrentDiscount", "Текущ. скидка (%)");
            dataGridView.Columns.Add("StockQuantity", "Кол-во на складе");
            dataGridView.Columns.Add("Description", "Описание");

          
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            foreach (var product in products)
            {
                dataGridView.Rows.Add(
                    product.Id,
                    product.Article,
                    product.Name,
                    product.Unit,
                    $"{product.Price:N0} руб.",
                    product.MaxDiscount,
                    product.Manufacturer,
                    product.Supplier,
                    product.Category,
                    product.CurrentDiscount,
                    product.StockQuantity,
                    product.Description
                );
            }

            UpdateButtonStates();

            UpdateDataGridWidth();
        }

        private void UpdateDataGridWidth()
        {
            int totalWidth = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                totalWidth += column.Width;
            }

            
            dataGridView.Width = totalWidth + 50;
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = dataGridView.SelectedRows.Count > 0;
            btnEditProduct.Enabled = hasSelection;
            btnDeleteProduct.Enabled = hasSelection;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedProduct();
            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            using (ProductEditForm editForm = new ProductEditForm())
            {
                if (editForm.ShowDialog() == DialogResult.OK && editForm.EditedProduct != null)
                {
                  
                    int newId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;

                    
                    Product newProduct = new Product
                    {
                        Id = newId,
                        Article = editForm.EditedProduct.Article,
                        Name = editForm.EditedProduct.Name,
                        Category = editForm.EditedProduct.Category,
                        StockQuantity = editForm.EditedProduct.StockQuantity,
                        Unit = "шт.", 
                        Manufacturer = editForm.EditedProduct.Manufacturer,
                        Supplier = editForm.EditedProduct.Supplier,
                        Price = editForm.EditedProduct.Price,
                        MaxDiscount = editForm.EditedProduct.MaxDiscount,
                        CurrentDiscount = editForm.EditedProduct.CurrentDiscount,
                        Description = editForm.EditedProduct.Description,
                        ImageData = editForm.EditedProduct.ImageData
                    };

                    products.Add(newProduct);
                    RefreshDataGrid();

                    MessageBox.Show("Товар успешно добавлен!",
                                  "Успех",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEditProduct_Click(object sender, EventArgs e)
        {
            EditSelectedProduct();
        }

        private void EditSelectedProduct()
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int selectedId = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
            Product selectedProduct = products.FirstOrDefault(p => p.Id == selectedId);

            if (selectedProduct != null)
            {
                // Создаем данные для редактирования
                ProductEditData editData = new ProductEditData
                {
                    Id = selectedProduct.Id,
                    Article = selectedProduct.Article,
                    Name = selectedProduct.Name,
                    Category = selectedProduct.Category,
                    StockQuantity = selectedProduct.StockQuantity,
                    Manufacturer = selectedProduct.Manufacturer,
                    Supplier = selectedProduct.Supplier,
                    Price = selectedProduct.Price,
                    MaxDiscount = selectedProduct.MaxDiscount,
                    CurrentDiscount = selectedProduct.CurrentDiscount,
                    Description = selectedProduct.Description,
                    ImageData = selectedProduct.ImageData
                };

                using (ProductEditForm editForm = new ProductEditForm(editData))
                {
                    if (editForm.ShowDialog() == DialogResult.OK && editForm.EditedProduct != null)
                    {
                        // Обновляем товар
                        selectedProduct.Article = editForm.EditedProduct.Article;
                        selectedProduct.Name = editForm.EditedProduct.Name;
                        selectedProduct.Category = editForm.EditedProduct.Category;
                        selectedProduct.StockQuantity = editForm.EditedProduct.StockQuantity;
                        selectedProduct.Unit = "шт.";
                        selectedProduct.Manufacturer = editForm.EditedProduct.Manufacturer;
                        selectedProduct.Supplier = editForm.EditedProduct.Supplier;
                        selectedProduct.Price = editForm.EditedProduct.Price;
                        selectedProduct.MaxDiscount = editForm.EditedProduct.MaxDiscount;
                        selectedProduct.CurrentDiscount = editForm.EditedProduct.CurrentDiscount;
                        selectedProduct.Description = editForm.EditedProduct.Description;
                        selectedProduct.ImageData = editForm.EditedProduct.ImageData;

                        RefreshDataGrid();

                        MessageBox.Show("Товар успешно обновлен!",
                                      "Успех",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int selectedId = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
            Product selectedProduct = products.FirstOrDefault(p => p.Id == selectedId);

            if (selectedProduct != null)
            {
                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар '{selectedProduct.Name}' (арт. {selectedProduct.Article})?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    products.Remove(selectedProduct);
                    RefreshDataGrid();

                    MessageBox.Show("Товар успешно удален!",
                                  "Успех",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearch();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplySearch();
        }

        private void ApplySearch()
        {
            string searchText = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                RefreshDataGrid();
                return;
            }

            var filteredProducts = products.Where(p =>
                p.Name.ToLower().Contains(searchText) ||
                p.Article.ToLower().Contains(searchText) ||
                p.Category.ToLower().Contains(searchText) ||
                p.Manufacturer.ToLower().Contains(searchText) ||
                p.Supplier.ToLower().Contains(searchText) ||
                p.Description.ToLower().Contains(searchText) ||
                p.Id.ToString().Contains(searchText)
            ).ToList();

            dataGridView.Rows.Clear();
            foreach (var product in filteredProducts)
            {
                dataGridView.Rows.Add(
                    product.Id,
                    product.Article,
                    product.Name,
                    product.Unit,
                    $"{product.Price:N0} руб.",
                    product.MaxDiscount,
                    product.Manufacturer,
                    product.Supplier,
                    product.Category,
                    product.CurrentDiscount,
                    product.StockQuantity,
                    product.Description
                );
            }
        }

       
        private void AdminWindow_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentUserName) && label1 != null)
            {
                label1.Text = CurrentUserName;
                label1.Font = comicSansFont;
            }
        }

        private void CurrentUserName_Click(object sender, EventArgs e)
        {
            
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }
    }
}