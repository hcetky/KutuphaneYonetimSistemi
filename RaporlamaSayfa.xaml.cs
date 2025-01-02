using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KKU_Kutuphane
{
    public partial class RaporlamaSayfa : Page
    {
        private const string connectionString = "Data Source=database.db;Version=3;";



        public RaporlamaSayfa()
        {
            InitializeComponent();
            Loaded += RaporlamaSayfa_Loaded;
        }

        private void RaporlamaSayfa_Loaded(object sender, RoutedEventArgs e)
        {
            SetPlaceholderText();
            AraButton_Click(null, null);
        }

        private void LoadAllBooks()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("SELECT transactions.kullaniciId, transactions.kitapId, users.adSoyad AS KullaniciAdi, books.kitapAdi AS KitapAdi, transactions.teslimTarihi AS TeslimTarihi FROM transactions INNER JOIN users ON transactions.kullaniciId = users.id INNER JOIN books ON transactions.kitapId = books.id WHERE teslimTarihi IS NOT NULL", conn);
                var adapter = new SQLiteDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                raporDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


        private void AraButton_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = kullaniciAramaTextBox.Text.Trim();
            string kitapAdi = kitapAramaTextBox.Text.Trim();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT transactions.kullaniciId, transactions.kitapId, transactions.verilenTarih, users.adSoyad AS KullaniciAdi, books.kitapAdi AS KitapAdi, transactions.teslimTarihi AS TeslimTarihi FROM transactions INNER JOIN users ON transactions.kullaniciId = users.id INNER JOIN books ON transactions.kitapId = books.id WHERE teslimTarihi IS NOT NULL";

                bool isFiltering = false;

                if (!string.IsNullOrEmpty(kullaniciAdi) && kullaniciAdi != "Kullanıcı Adı Ara...")
                {
                    query += " AND users.adSoyad LIKE @kullaniciAdi";
                    isFiltering = true;
                }

                if (!string.IsNullOrEmpty(kitapAdi) && kitapAdi != "Kitap Adı Ara...")
                {
                    query += " AND books.kitapAdi LIKE @kitapAdi";
                    isFiltering = true;
                }

                var command = new SQLiteCommand(query, conn);
                if (isFiltering)
                {
                    if (!string.IsNullOrEmpty(kullaniciAdi) && kullaniciAdi != "Kullanıcı Adı Ara...")
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", "%" + kullaniciAdi + "%");
                    }

                    if (!string.IsNullOrEmpty(kitapAdi) && kitapAdi != "Kitap Adı Ara...")
                    {
                        command.Parameters.AddWithValue("@kitapAdi", "%" + kitapAdi + "%");
                    }
                }

                var adapter = new SQLiteDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                raporDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


        private void TemizleButton_Click(object sender, RoutedEventArgs e)
        {
            kullaniciAramaTextBox.Clear();
            kitapAramaTextBox.Clear();
            SetPlaceholderText(); 
            AraButton_Click(null, null);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == kullaniciAramaTextBox)
                {
                    textBox.Text = "Kullanıcı Adı Ara...";
                }
                else if (textBox == kitapAramaTextBox)
                {
                    textBox.Text = "Kitap Adı Ara...";
                }
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void SetPlaceholderText()
        {
            if (string.IsNullOrWhiteSpace(kullaniciAramaTextBox.Text))
            {
                kullaniciAramaTextBox.Text = "Kullanıcı Adı Ara...";
                kullaniciAramaTextBox.Foreground = Brushes.Gray;
            }

            if (string.IsNullOrWhiteSpace(kitapAramaTextBox.Text))
            {
                kitapAramaTextBox.Text = "Kitap Adı Ara...";
                kitapAramaTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}
