using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KKU_Kutuphane
{
    public partial class teslimAlSayfa : Page
    {
        public ObservableCollection<Kitap> Kitaplar { get; set; } = new ObservableCollection<Kitap>();

        private const string connectionString = "Data Source=database.db;Version=3;";

        public teslimAlSayfa()
        {
            InitializeComponent();
        }

        private void KullaniciSecButton_Click(object sender, RoutedEventArgs e)
        {
            KullaniciSecWindow kullaniciSecWindow = new KullaniciSecWindow();
            if (kullaniciSecWindow.ShowDialog() == true)
            {
                secilenKullaniciText.Text = $"Seçilen Kullanıcı: {kullaniciSecWindow.SecilenKullanici.AdSoyad}";
                secilenKullaniciText.Tag = kullaniciSecWindow.SecilenKullanici.Id;
            }
        }

        private void KitaplarıYukle(int kullaniciId)
        {
            Kitaplar.Clear();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("SELECT kitapId, kitapAdi, yazar, teslimTarihi FROM transactions INNER JOIN books ON transactions.kitapId = books.id WHERE kullaniciId = @kullaniciId AND teslimTarihi", conn);
                command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Kitaplar.Add(new Kitap
                    {
                        Id = reader.GetInt32(0),
                        KitapAdi = reader.GetString(1),
                        Yazar = reader.GetString(2)
                    });
                }
            }
            kitapDataGrid.ItemsSource = Kitaplar;
        }

        private void KitapEkleButton_Click(object sender, RoutedEventArgs e)
        {
            if (secilenKullaniciText.Tag == null)
            {
                MessageBox.Show("Lütfen önce bir kullanıcı seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int kullaniciId = (int)secilenKullaniciText.Tag;
            KitapSecWindow kitapSecWindow = new KitapSecWindow(kullaniciId,false);
            if (kitapSecWindow.ShowDialog() == true)
            {
                foreach (var kitap in kitapSecWindow.SecilenKitaplar)
                {
                    kitapDataGrid.Items.Add(kitap);
                }
            }
        }

        private void OnaylaButton_Click(object sender, RoutedEventArgs e)
        {
            if (kitapDataGrid.Items.Count == 0)
            {
                MessageBox.Show("İade alınacak kitap bulunmamaktadır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (Kitap kitap in kitapDataGrid.Items)
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // transactions tablosundan ilgili kitabı ve kullanıcıyı sil
                    var deleteCommand = new SQLiteCommand("DELETE FROM transactions WHERE kitapId = @kitapId AND kullaniciId = @kullaniciId", conn);
                    deleteCommand.Parameters.AddWithValue("@kitapId", kitap.Id);
                    deleteCommand.Parameters.AddWithValue("@kullaniciId", (int)secilenKullaniciText.Tag);
                    deleteCommand.ExecuteNonQuery();

                    // books tablosunda kitabın stok adedini bir artır
                    var updateCommand = new SQLiteCommand("UPDATE books SET adet = adet + 1 WHERE id = @kitapId", conn);
                    updateCommand.Parameters.AddWithValue("@kitapId", kitap.Id);
                    updateCommand.ExecuteNonQuery();
                }
            }

            // Koleksiyondan tüm kitapları sil ve DataGrid'i temizle
            Kitaplar.Clear();
            kitapDataGrid.Items.Clear();

            MessageBox.Show("Tüm kitaplar başarıyla iade alındı ve stok güncellendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }
}
