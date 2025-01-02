using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace KKU_Kutuphane
{
    public partial class kitapSayfa : Page
    {
        private const string connectionString = "Data Source=database.db;Version=3;";
        private int selectedKitapId = -1; // Düzenleme için seçili kitap ID'si
        private string kapakResmiPath = string.Empty;

        public kitapSayfa()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("SELECT * FROM books", conn);
                var adapter = new SQLiteDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                kitapDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Ekle_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("INSERT INTO books (kitapAdi, yazar, yayinYili, tur, isbn, adet) VALUES (@kitapAdi, @yazar, @yayinYili, @tur, @isbn, @adet)", conn);
                command.Parameters.AddWithValue("@kitapAdi", kitapAdiTextBox.Text);
                command.Parameters.AddWithValue("@yazar", yazarTextBox.Text);
                command.Parameters.AddWithValue("@yayinYili", yayinYiliTextBox.Text);
                command.Parameters.AddWithValue("@tur", turTextBox.Text);
                command.Parameters.AddWithValue("@isbn", isbnTextBox.Text);
                command.Parameters.AddWithValue("@adet", Convert.ToInt32(adetTextBox.Text));
                command.ExecuteNonQuery();

                // Eklenen kaydın ID'sini al
                long kitapId = conn.LastInsertRowId;

                // Kapak resmini kaydet ve veritabanına yolu ekle
                if (!string.IsNullOrEmpty(kapakResmiPath) && File.Exists(kapakResmiPath))
                {
                    try
                    {
                        string yeniDosyaYolu = Path.Combine("kapak", $"{kitapId}.jpg");
                        Directory.CreateDirectory("kapak"); // Kapak klasörünü oluştur (eğer yoksa)
                        File.Copy(kapakResmiPath, yeniDosyaYolu, true);

                        var updateCommand = new SQLiteCommand("UPDATE books SET kapakResmi = @kapakResmi WHERE id = @id", conn);
                        updateCommand.Parameters.AddWithValue("@kapakResmi", yeniDosyaYolu);
                        updateCommand.Parameters.AddWithValue("@id", kitapId);
                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Kapak resmi başarıyla kaydedildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kapak resmi kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            LoadBooks();
            MessageBox.Show("Kitap başarıyla eklendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Guncelle_Click(object sender, RoutedEventArgs e)
        {
            if (selectedKitapId > -1)
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    var command = new SQLiteCommand("UPDATE books SET kitapAdi=@kitapAdi, yazar=@yazar, yayinYili=@yayinYili, tur=@tur, isbn=@isbn, adet=@adet WHERE id=@id", conn);
                    command.Parameters.AddWithValue("@kitapAdi", kitapAdiTextBox.Text);
                    command.Parameters.AddWithValue("@yazar", yazarTextBox.Text);
                    command.Parameters.AddWithValue("@yayinYili", yayinYiliTextBox.Text);
                    command.Parameters.AddWithValue("@tur", turTextBox.Text);
                    command.Parameters.AddWithValue("@isbn", isbnTextBox.Text);
                    command.Parameters.AddWithValue("@adet", Convert.ToInt32(adetTextBox.Text));
                    command.Parameters.AddWithValue("@id", selectedKitapId);
                    command.ExecuteNonQuery();

                    // Kapak resmi güncellemesi
                    if (!string.IsNullOrEmpty(kapakResmiPath) && File.Exists(kapakResmiPath))
                    {
                        try
                        {
                            string yeniDosyaYolu = Path.Combine("kapak", $"{selectedKitapId}.jpg");
                            Directory.CreateDirectory("kapak");
                            File.Copy(kapakResmiPath, yeniDosyaYolu, true);

                            var updateCommand = new SQLiteCommand("UPDATE books SET kapakResmi = @kapakResmi WHERE id = @id", conn);
                            updateCommand.Parameters.AddWithValue("@kapakResmi", yeniDosyaYolu);
                            updateCommand.Parameters.AddWithValue("@id", selectedKitapId);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show("Kapak resmi başarıyla güncellendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Kapak resmi kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    MessageBox.Show("Kitap başarıyla güncellendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    selectedKitapId = -1;
                }
                LoadBooks();
            }
        }


        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            if (kitapDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int kitapId = Convert.ToInt32(selectedRow["id"]);
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    var command = new SQLiteCommand("DELETE FROM books WHERE id=@id", conn);
                    command.Parameters.AddWithValue("@id", kitapId);
                    command.ExecuteNonQuery();
                }
                LoadBooks();
                MessageBox.Show("Kitap başarıyla silindi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Duzenle_Click(object sender, RoutedEventArgs e)
        {
            if (kitapDataGrid.SelectedItem is DataRowView selectedRow)
            {
                selectedKitapId = Convert.ToInt32(selectedRow["id"]);
                kitapAdiTextBox.Text = selectedRow["kitapAdi"].ToString();
                yazarTextBox.Text = selectedRow["yazar"].ToString();
                yayinYiliTextBox.Text = selectedRow["yayinYili"].ToString();
                turTextBox.Text = selectedRow["tur"].ToString();
                isbnTextBox.Text = selectedRow["isbn"].ToString();
                adetTextBox.Text = selectedRow["adet"].ToString();

                // Kapak resmini yükle
                string kapakResmiYolu = selectedRow["kapakResmi"]?.ToString();
                if (!string.IsNullOrEmpty(kapakResmiYolu) && File.Exists(kapakResmiYolu))
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(kapakResmiYolu, UriKind.RelativeOrAbsolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Dosya referansını serbest bırak
                        bitmap.EndInit();
                        kapakResmiImage.Source = bitmap;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kapak resmi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    kapakResmiImage.Source = null; // Kapak resmi yoksa boş bırak
                }

                // Kitap Ekle/Düzenle sekmesine geçiş yap
                TabControl.SelectedIndex = 1;
            }
        }


        private void KapakResmiYukle_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                kapakResmiPath = openFileDialog.FileName;

                // Resmi bellekten yükleyip dosya referansını serbest bırak
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(kapakResmiPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Dosya referansını serbest bırak
                bitmap.EndInit();

                kapakResmiImage.Source = bitmap;

                MessageBox.Show("Kapak resmi başarıyla yüklendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
