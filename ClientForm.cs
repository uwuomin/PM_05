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

namespace ПМ._05
{
    public partial class ClientForm : Form
    {
        public string CurrentUserRole { get; set; }
        
        public class Product
        {
            public int Id { get; set; }
            public string Article { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public byte[] ImageData { get; set; }
            public bool IsSelected { get; set; } 
            public int QuantityInCart { get; set; } 
            public string Category { get; set; }
        }

        
        public class Order
        {
            public int OrderNumber { get; set; }
            public string OrderItems { get; set; } 
            public DateTime OrderDate { get; set; }
            public DateTime DeliveryDate { get; set; }
            public string PickupPoint { get; set; }
            public string CustomerName { get; set; }
            public string ReceiptCode { get; set; }
            public string Status { get; set; }
        }

      
        public class ShoppingBasketForm : Form
        {
            private List<Product> selectedProducts;
            private DataGridView dataGridView;
            private Label lblTotal;
            private Button btnConfirm;
            private TextBox txtCustomerName;
            private ComboBox cmbPickupPoint;
            private DateTimePicker dtpDeliveryDate;
            private Button btnAddToOrders;
            private Random random = new Random();

            public ShoppingBasketForm(List<Product> products, string currentUserName)
            {
                selectedProducts = products.Where(p => p.IsSelected && p.QuantityInCart > 0).ToList();
                InitializeForm(currentUserName);
                LoadProducts();
            }

            private void InitializeForm(string currentUserName)
            {
                this.Text = "Корзина покупок";
                this.Size = new Size(800, 600);
                this.StartPosition = FormStartPosition.CenterParent;

                
                Panel mainPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10)
                };

                
                Panel infoPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 120,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10)
                };

                
                Label lblCustomer = new Label
                {
                    Text = "ФИО клиента:",
                    Location = new Point(10, 10),
                    Width = 100
                };

                txtCustomerName = new TextBox
                {
                    Location = new Point(120, 8),
                    Width = 200,
                    Text = currentUserName
                };

               
                Label lblPickupPoint = new Label
                {
                    Text = "Пункт выдачи:",
                    Location = new Point(10, 40),
                    Width = 100
                };

                cmbPickupPoint = new ComboBox
                {
                    Location = new Point(120, 38),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbPickupPoint.Items.AddRange(new string[] { "18", "25", "31", "42", "53" });
                cmbPickupPoint.SelectedIndex = 0;

                
                Label lblDeliveryDate = new Label
                {
                    Text = "Дата доставки:",
                    Location = new Point(10, 70),
                    Width = 100
                };

                dtpDeliveryDate = new DateTimePicker
                {
                    Location = new Point(120, 68),
                    Width = 200,
                    Value = DateTime.Now.AddDays(3),
                    Format = DateTimePickerFormat.Short
                };

                btnAddToOrders = new Button
                {
                    Text = "Добавить в заказы",
                    Location = new Point(350, 65),
                    Width = 150,
                    Height = 25,
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White
                };
                btnAddToOrders.Click += BtnAddToOrders_Click;

                infoPanel.Controls.Add(lblCustomer);
                infoPanel.Controls.Add(txtCustomerName);
                infoPanel.Controls.Add(lblPickupPoint);
                infoPanel.Controls.Add(cmbPickupPoint);
                infoPanel.Controls.Add(lblDeliveryDate);
                infoPanel.Controls.Add(dtpDeliveryDate);
                infoPanel.Controls.Add(btnAddToOrders);

                
                dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    Margin = new Padding(0, 10, 0, 0)
                };


                dataGridView.Columns.Add("Name", "Наименование");
                dataGridView.Columns.Add("Manufacturer", "Производитель");
                dataGridView.Columns.Add("Price", "Цена");
                dataGridView.Columns.Add("Quantity", "Количество");
                dataGridView.Columns.Add("Total", "Сумма");

                
                Panel bottomPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 80,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10)
                };

                lblTotal = new Label
                {
                    Text = "Итого: 0 руб.",
                    Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                    ForeColor = Color.Green,
                    Dock = DockStyle.Left,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                btnConfirm = new Button
                {
                    Text = "Оформить заказ",
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                    BackColor = Color.Green,
                    ForeColor = Color.White,
                    Size = new Size(150, 40),
                    Dock = DockStyle.Right
                };
                btnConfirm.Click += BtnConfirm_Click;

                bottomPanel.Controls.Add(lblTotal);
                bottomPanel.Controls.Add(btnConfirm);

                mainPanel.Controls.Add(infoPanel);
                mainPanel.Controls.Add(dataGridView);
                mainPanel.Controls.Add(bottomPanel);

                this.Controls.Add(mainPanel);
            }

            private void LoadProducts()
            {
                dataGridView.Rows.Clear();
                decimal total = 0;

                foreach (var product in selectedProducts)
                {
                    decimal itemTotal = product.Price * product.QuantityInCart;
                    dataGridView.Rows.Add(
                        product.Name,
                        product.Manufacturer,
                        $"{product.Price:C}",
                        product.QuantityInCart,
                        $"{itemTotal:C}"
                    );
                    total += itemTotal;
                }

                lblTotal.Text = $"Итого: {total:C}";
            }

            private void BtnAddToOrders_Click(object sender, EventArgs e)
            {
                
                string orderItems = string.Join(", ", selectedProducts.Select(p =>
                    $"{p.Article}, {p.QuantityInCart}"));

                
                Order newOrder = new Order
                {
                    OrderNumber = random.Next(1, 1000),
                    OrderItems = orderItems,
                    OrderDate = DateTime.Now,
                    DeliveryDate = dtpDeliveryDate.Value,
                    PickupPoint = cmbPickupPoint.SelectedItem.ToString(),
                    CustomerName = txtCustomerName.Text,
                    ReceiptCode = random.Next(100, 1000).ToString(),
                    Status = "Новый"
                };

                
                OrdersDisplayForm ordersForm = new OrdersDisplayForm(new List<Order> { newOrder });
                ordersForm.ShowDialog();
            }

            private void BtnConfirm_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
                {
                    MessageBox.Show("Введите ФИО клиента!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

               
                string orderItems = string.Join(", ", selectedProducts.Select(p =>
                    $"{p.Article}, {p.QuantityInCart}"));

                MessageBox.Show($"Заказ оформлен успешно!\n\n" +
                              $"Состав заказа: {orderItems}\n" +
                              $"Пункт выдачи: {cmbPickupPoint.SelectedItem}\n" +
                              $"Дата доставки: {dtpDeliveryDate.Value:dd.MM.yyyy}\n" +
                              $"Код для получения: {random.Next(100, 1000)}",
                              "Заказ оформлен",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public class OrdersDisplayForm : Form
        {
            public OrdersDisplayForm(List<Order> orders)
            {
                InitializeForm(orders);
            }

            private void InitializeForm(List<Order> orders)
            {
                this.Text = "Список заказов";
                this.Size = new Size(900, 400);
                this.StartPosition = FormStartPosition.CenterParent;

                DataGridView dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    ReadOnly = true
                };

                
                dataGridView.Columns.Add("OrderNumber", "Номер заказа");
                dataGridView.Columns.Add("OrderItems", "Состав заказа");
                dataGridView.Columns.Add("OrderDate", "Дата заказа");
                dataGridView.Columns.Add("DeliveryDate", "Дата доставки");
                dataGridView.Columns.Add("PickupPoint", "Пункт выдачи");
                dataGridView.Columns.Add("CustomerName", "ФИО клиента");
                dataGridView.Columns.Add("ReceiptCode", "Код для получения");
                dataGridView.Columns.Add("Status", "Статус заказа");

                
                foreach (var order in orders)
                {
                    dataGridView.Rows.Add(
                        order.OrderNumber,
                        order.OrderItems,
                        order.OrderDate.ToString("dd.MM.yyyy"),
                        order.DeliveryDate.ToString("dd.MM.yyyy"),
                        order.PickupPoint,
                        order.CustomerName,
                        order.ReceiptCode,
                        order.Status
                    );
                }

                
                Button btnClose = new Button
                {
                    Text = "Закрыть",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    BackColor = Color.Gray,
                    ForeColor = Color.White
                };
                btnClose.Click += (s, e) => this.Close();

                this.Controls.Add(dataGridView);
                this.Controls.Add(btnClose);
            }
        }

        
        private List<Product> allProducts = new List<Product>();
       
        private List<Product> filteredProducts = new List<Product>();
        
        private string placeholderImagePath = Path.Combine(Application.StartupPath, "Resources", "picture.png");
        
        private string productsImagesPath = @"C:\Users\PC\Desktop\Практика ПМ 03-ПМ05\Задание на практику\Вариант 2\Сессия 1\Товар_import";
        
        private bool sortAscending = true;
        private bool isSorted = false;
        
        private Dictionary<int, CheckBox> productCheckBoxes = new Dictionary<int, CheckBox>();
        
        private Dictionary<int, NumericUpDown> productQuantityControls = new Dictionary<int, NumericUpDown>();

        public string CurrentUserName { get; set; }

        public ClientForm()
        {
            InitializeComponent();
            InitializeSampleData(); 
            UpdateButtonText(); 
        }

   
        private byte[] LoadProductImage(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return null;

            string imagePath = Path.Combine(productsImagesPath, imageFileName);

            if (File.Exists(imagePath))
            {
                try
                {
                    return File.ReadAllBytes(imagePath);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        
        private void InitializeSampleData()
        {
            allProducts = new List<Product>
            {
                new Product { Id = 1, Article = "А112Т4", Name = "Боксерская груша", Description = "Боксерская груша X-Match черная", Manufacturer = "X-Match", Price = 778, StockQuantity = 6, ImageData = LoadProductImage("А112Т4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 2, Article = "G598Y6", Name = "Спортивный мат", Description = "Спортивный мат 100x100x10 см Perfetto Sport № 3 бежевый", Manufacturer = "Perfetto Sport", Price = 2390, StockQuantity = 16, ImageData = LoadProductImage("G598Y6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 3, Article = "F746E6", Name = "Шведская стенка", Description = "Шведская стенка ROMANA Next, pastel", Manufacturer = "ROMANA Next", Price = 9900, StockQuantity = 5, ImageData = LoadProductImage("F746E6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 4, Article = "D830R5", Name = "Тренажер прыжков", Description = "Тренажер для прыжков Moby Kids Moby-Jumper со счетчиком", Manufacturer = "Moby Kids", Price = 1120, StockQuantity = 8, ImageData = LoadProductImage("D830R5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 5, Article = "B538G6", Name = "Спортивный костюм", Description = "Спортивный костюм playToday (футболка + шорты)", Manufacturer = "playToday", Price = 839, StockQuantity = 17, ImageData = LoadProductImage("B538G6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Одежда" },
                new Product { Id = 6, Article = "D648N7", Name = "Набор для хоккея", Description = "Набор для хоккея Совтехстром", Manufacturer = "Совтехстром", Price = 350, StockQuantity = 7, ImageData = LoadProductImage("D648N7.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 7, Article = "F735B6", Name = "Игровой набор", Description = "Игровой набор Совтехстром Кегли и шары", Manufacturer = "Совтехстром", Price = 320, StockQuantity = 9, ImageData = LoadProductImage("F735B6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 8, Article = "F937G4", Name = "Игровой набор", Description = "Набор Abtoys Бадминтон и теннис", Manufacturer = "Abtoys", Price = 480, StockQuantity = 12, ImageData = LoadProductImage("F937G4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 9, Article = "E324U7", Name = "Велотренажер", Description = "Велотренажер двойной DFC B804 dual bike", Manufacturer = "DFC", Price = 6480, StockQuantity = 5, ImageData = LoadProductImage("E324U7.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 10, Article = "G403T5", Name = "Тюбинг", Description = "Тюбинг Nordway, 73 см", Manufacturer = "Nordway", Price = 1450, StockQuantity = 13, ImageData = LoadProductImage("G403T5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 11, Article = "N483G5", Name = "Клюшка", Description = "Клюшка Nordway NDW300 (2019/2020) SR лев. 19 150см", Manufacturer = "Nordway", Price = 1299, StockQuantity = 4, ImageData = LoadProductImage("N483G5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 12, Article = "D038G6", Name = "Лыжный комплект", Description = "Лыжный комплект беговые NORDWAY XC Classic, 45-45-45мм, 160см", Manufacturer = "Nordway", Price = 3000, StockQuantity = 23, ImageData = LoadProductImage("D038G6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 13, Article = "G480F5", Name = "Ролики", Description = "Коньки роликовые Ridex Cricket жен. ABEC 3 кол.:72мм р.:39-42 синий", Manufacturer = "Ridex", Price = 1600, StockQuantity = 7, ImageData = LoadProductImage("G480F5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 14, Article = "C324S5", Name = "Шлем", Description = "Шлем г.л./сноуб. Salomon Grom р.:KS черный (L40836800)", Manufacturer = "Salomon", Price = 4000, StockQuantity = 16, ImageData = LoadProductImage("C324S5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 15, Article = "V312R4", Name = "Мяч", Description = "Мяч волейбольный MIKASA VT370W, для зала, 5-й размер, желтый/синий", Manufacturer = "Mikasa", Price = 4150, StockQuantity = 5, ImageData = LoadProductImage("V312R4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 16, Article = "J4DF5E", Name = "Насос", Description = "Насос Molten HP-18-B для мячей мультиколор", Manufacturer = "Molten", Price = 300, StockQuantity = 12, ImageData = LoadProductImage("J4DF5E.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 17, Article = "G522B5", Name = "Ласты", Description = "Ласты Colton CF-02 для плавания р.:33-34 серый/голубой", Manufacturer = "Colton", Price = 1980, StockQuantity = 6, ImageData = LoadProductImage("G522B5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 18, Article = "K432G6", Name = "Шапочка для плавания", Description = "Шапочка для плавания Atemi PU 140 ткань с покрытием желтый", Manufacturer = "Atemi", Price = 440, StockQuantity = 17, ImageData = LoadProductImage("K432G6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 19, Article = "J532D4", Name = "Перчатки для карате", Description = "Перчатки для каратэ Green Hill KMС-6083 L красный", Manufacturer = "Green Hill", Price = 1050, StockQuantity = 5, ImageData = LoadProductImage("J532D4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 20, Article = "G873H4", Name = "Велосипед", Description = "Велосипед SKIF 29 Disc (2021), горный (взрослый), рама: 17\", колеса: 29\", темно-серый", Manufacturer = "SKIF", Price = 14930, StockQuantity = 6, ImageData = LoadProductImage("G873H4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 21, Article = "V423D4", Name = "Штанга", Description = "Штанга Starfit BB-401 30кг пласт. черный", Manufacturer = "Starfit", Price = 5600, StockQuantity = 8, ImageData = LoadProductImage("V423D4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 22, Article = "K937A5", Name = "Гиря", Description = "Гиря Starfit ГМБ4 мягкое 4кг синий/оранжевый", Manufacturer = "Starfit", Price = 890, StockQuantity = 10, ImageData = LoadProductImage("K937A5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 23, Article = "F047J7", Name = "Коврик", Description = "Коврик Bradex для мягкой йоги дл.:1730мм ш.:610мм т.:3мм серый", Manufacturer = "Bradex", Price = 720, StockQuantity = 11, ImageData = LoadProductImage("F047J7.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 24, Article = "S374B5", Name = "Ролик для йоги", Description = "Ролик для йоги Bradex Туба d=14см ш.:33см оранжевый", Manufacturer = "Bradex", Price = 700, StockQuantity = 12, ImageData = LoadProductImage("S374B5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 25, Article = "F687G5", Name = "Защита голени", Description = "Защита голени GREEN HILL Panther, L, синий/черный", Manufacturer = "Green Hill", Price = 1900, StockQuantity = 6, ImageData = LoadProductImage("F687G5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 26, Article = "N892G6", Name = "Очки для плавания", Description = "Очки для плавания Atemi N8401 синий", Manufacturer = "Atemi", Price = 500, StockQuantity = 14, ImageData = LoadProductImage("N892G6.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 27, Article = "D893W4", Name = "Мяч", Description = "Мяч футбольный DEMIX 1STLS1JWWW, универсальный, 4-й размер, белый/зеленый", Manufacturer = "Demix", Price = 900, StockQuantity = 5, ImageData = LoadProductImage("D893W4.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 28, Article = "N836R5", Name = "Коньки", Description = "Коньки ATEMI AKSK01DXS, раздвижные, прогулочные, унисекс, 27-30, черный/зеленый", Manufacturer = "Atemi", Price = 2000, StockQuantity = 16, ImageData = LoadProductImage("N836R5.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 29, Article = "D927K3", Name = "Перчатки", Description = "Перчатки Starfit SU-125 атлетические S черный", Manufacturer = "Starfit", Price = 660, StockQuantity = 3, ImageData = LoadProductImage("D927K3.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" },
                new Product { Id = 30, Article = "V392H7", Name = "Степ-платформа", Description = "Степ-платформа Starfit SP-204 серый/черный", Manufacturer = "Starfit", Price = 4790, StockQuantity = 15, ImageData = LoadProductImage("V392H7.jpg"), IsSelected = false, QuantityInCart = 0, Category = "Спортивный инвентарь" }
            };

            
            UpdateManufacturerFilter();
           
            ApplyFilters();
        }

        
        private void UpdateButtonText()
        {
            int selectedCount = allProducts.Count(p => p.IsSelected && p.QuantityInCart > 0);
            if (selectedCount > 0)
            {
                btnAddProduct.Text = $"В корзину ({selectedCount})";
                btnAddProduct.BackColor = Color.Orange;
                btnAddProduct.ForeColor = Color.White;
            }
            else
            {
                btnAddProduct.Text = "В корзину";
                btnAddProduct.BackColor = SystemColors.Control;
                btnAddProduct.ForeColor = SystemColors.ControlText;
            }
        }

       
        private void UpdateManufacturerFilter()
        {
            cmbManufacturer.Items.Clear();
            cmbManufacturer.Items.Add("Все производители");

            var manufacturers = allProducts
                .Select(p => p.Manufacturer)
                .Distinct()
                .OrderBy(m => m);

            foreach (var manufacturer in manufacturers)
            {
                cmbManufacturer.Items.Add(manufacturer);
            }

            cmbManufacturer.SelectedIndex = 0;
        }

        
        private void ApplyFilters()
        {
            
            filteredProducts = allProducts.ToList();

            
            string searchText = txtSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.ToLower().Contains(searchText) ||
                    p.Description.ToLower().Contains(searchText) ||
                    p.Manufacturer.ToLower().Contains(searchText) ||
                    p.Price.ToString().Contains(searchText) ||
                    p.StockQuantity.ToString().Contains(searchText) ||
                    p.Article.ToLower().Contains(searchText) ||
                    p.Category.ToLower().Contains(searchText)
                ).ToList();
            }

            
            if (cmbManufacturer.SelectedIndex > 0)
            {
                string selectedManufacturer = cmbManufacturer.SelectedItem.ToString();
                filteredProducts = filteredProducts
                    .Where(p => p.Manufacturer == selectedManufacturer)
                    .ToList();
            }

            
            if (isSorted)
            {
                filteredProducts = sortAscending
                    ? filteredProducts.OrderBy(p => p.Price).ToList()
                    : filteredProducts.OrderByDescending(p => p.Price).ToList();
            }

            
            UpdateCounter();

           
            DisplayProducts();
        }

       
        private void UpdateCounter()
        {
            lblCount.Text = $"{filteredProducts.Count} из {allProducts.Count}";
        }

        
        private void DisplayProducts()
        {
            flowLayoutProducts.Controls.Clear();
            productCheckBoxes.Clear();
            productQuantityControls.Clear();

            foreach (var product in filteredProducts)
            {
                
                Panel productPanel = CreateProductPanel(product);
                flowLayoutProducts.Controls.Add(productPanel);
            }
        }

       
        private Panel CreateProductPanel(Product product)
        {
            Panel panel = new Panel
            {
                Width = 300,
                Height = 450,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = product.StockQuantity > 0 ? Color.White : Color.LightGray
            };

            
            NumericUpDown quantityUpDown = new NumericUpDown
            {
                Location = new Point(100, 8),
                Width = 60,
                Minimum = 0,
                Maximum = Math.Max(product.StockQuantity, 1),
                Value = product.QuantityInCart,
                Enabled = product.IsSelected && product.StockQuantity > 0
            };

           
            CheckBox checkBox = new CheckBox
            {
                Text = "Выбрать",
                Location = new Point(10, 10),
                Width = 80,
                Checked = product.IsSelected,
                Enabled = product.StockQuantity > 0 
            };

            
            checkBox.CheckedChanged += (s, e) =>
            {
                product.IsSelected = checkBox.Checked;
                quantityUpDown.Enabled = checkBox.Checked;
                UpdateButtonText();
            };

            quantityUpDown.ValueChanged += (s, e) =>
            {
                product.QuantityInCart = (int)quantityUpDown.Value;
                if (quantityUpDown.Value > 0)
                {
                    product.IsSelected = true;
                    checkBox.Checked = true;
                }
                UpdateButtonText();
            };

          
            PictureBox pictureBox = new PictureBox
            {
                Width = 200,
                Height = 150,
                Location = new Point(50, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            
            if (product.ImageData != null && product.ImageData.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(product.ImageData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    
                    if (File.Exists(placeholderImagePath))
                    {
                        pictureBox.Image = Image.FromFile(placeholderImagePath);
                    }
                    else
                    {
                        
                        pictureBox.BackColor = Color.Black;
                    }
                }
            }
            else
            {
                
                if (File.Exists(placeholderImagePath))
                {
                    pictureBox.Image = Image.FromFile(placeholderImagePath);
                }
                else
                {
                    
                    pictureBox.BackColor = Color.Black;
                }
            }

            
            Label lblName = new Label
            {
                Text = $"{product.Article} - {product.Name}",
                Location = new Point(10, 200),
                Width = 280,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            
            TextBox txtDescription = new TextBox
            {
                Text = product.Description,
                Location = new Point(10, 230),
                Width = 280,
                Height = 60,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = panel.BackColor,
                ScrollBars = ScrollBars.Vertical
            };

            // Производитель
            Label lblManufacturer = new Label
            {
                Text = $"Производитель: {product.Manufacturer}",
                Location = new Point(10, 300),
                Width = 280,
                Font = new Font("Microsoft Sans Serif", 9)
            };

            // Категория
            Label lblCategory = new Label
            {
                Text = $"Категория: {product.Category}",
                Location = new Point(10, 315),
                Width = 280,
                Font = new Font("Microsoft Sans Serif", 9)
            };

            // Цена
            Label lblPrice = new Label
            {
                Text = $"Цена: {product.Price:C}",
                Location = new Point(10, 340),
                Width = 140,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // Наличие на складе
            Label lblStock = new Label
            {
                Text = product.StockQuantity > 0 ?
                      $"В наличии: {product.StockQuantity} шт." :
                      "Нет в наличии",
                Location = new Point(150, 340),
                Width = 140,
                Font = new Font("Microsoft Sans Serif", 9),
                ForeColor = product.StockQuantity > 0 ? Color.Black : Color.Red
            };

           
            productCheckBoxes[product.Id] = checkBox;
            productQuantityControls[product.Id] = quantityUpDown;

          
            panel.Controls.Add(checkBox);
            panel.Controls.Add(quantityUpDown);
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblName);
            panel.Controls.Add(txtDescription);
            panel.Controls.Add(lblManufacturer);
            panel.Controls.Add(lblCategory);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(lblStock);

            return panel;
        }

        private void CurrentUserName_Click(object sender, EventArgs e)
        {

        }

        private void UserRole_Click(object sender, EventArgs e)
        {

        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentUserName) && label1 != null)
            {
                label1.Text = $"{CurrentUserName}";
            }
        }

        private void panelFilters_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutProducts_Paint(object sender, PaintEventArgs e)
        {

        }

       
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        
        private void cmbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

       
        private void btnSort_Click(object sender, EventArgs e)
        {
            if (!isSorted)
            {
                // Первое нажатие - сортировка по возрастанию
                isSorted = true;
                sortAscending = true;
                btnSort.Text = "Цена ↓";
            }
            else if (sortAscending)
            {
                // Второе нажатие - сортировка по убыванию
                sortAscending = false;
                btnSort.Text = "Цена ↑";
            }
            else
            {
                // Третье нажатие - сброс сортировки
                isSorted = false;
                btnSort.Text = "Сортировать";
            }

            ApplyFilters();
        }

        
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            var selectedProducts = allProducts.Where(p => p.IsSelected && p.QuantityInCart > 0).ToList();

            if (selectedProducts.Count == 0)
            {
                MessageBox.Show("Выберите товары для добавления в корзину!",
                              "Корзина пуста",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            foreach (var product in selectedProducts)
            {
                if (product.QuantityInCart > product.StockQuantity)
                {
                    MessageBox.Show($"Товар '{product.Name}': запрошенное количество ({product.QuantityInCart}) превышает наличие на складе ({product.StockQuantity})!",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                    return;
                }
            }

       
            using (ShoppingBasketForm basketForm = new ShoppingBasketForm(allProducts, CurrentUserName))
            {
                if (basketForm.ShowDialog() == DialogResult.OK)
                {
                    
                    foreach (var product in selectedProducts)
                    {
                        product.StockQuantity -= product.QuantityInCart;
                        product.IsSelected = false;
                        product.QuantityInCart = 0;
                    }

                    
                    ApplyFilters();
                    UpdateButtonText();

                    MessageBox.Show("Заказ успешно оформлен! Количество на складе обновлено.",
                                  "Успех",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
        }

        private void lblCount_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}