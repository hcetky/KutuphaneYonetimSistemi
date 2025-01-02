using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KKU_Kutuphane
{
    public partial class teslimEtSayfa : Page
    {
        public ObservableCollection<Kitap> Kitaplar { get; set; } = new ObservableCollection<Kitap>();

        private const string connectionString = "Data Source=database.db;";

        public teslimEtSayfa()
        {
            InitializeComponent();
            kitapDataGrid.ItemsSource = Kitaplar; // ObservableCollection'ı DataGrid'e bağla
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

        private void KitapEkleButton_Click(object sender, RoutedEventArgs e)
        {
            if (secilenKullaniciText.Tag == null)
            {
                MessageBox.Show("Lütfen önce bir kullanıcı seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int kullaniciId = (int)secilenKullaniciText.Tag;
            KitapSecWindow kitapSecWindow = new KitapSecWindow(kullaniciId, true);
            if (kitapSecWindow.ShowDialog() == true)
            {
                foreach (var kitap in kitapSecWindow.SecilenKitaplar)
                {
                    // Teslim tarihini bugünün tarihi olarak ayarla
                    kitap.TeslimTarihi = DateTime.Now.AddDays(30);

                    // ObservableCollection'a ekle
                    Kitaplar.Add(kitap);
                }
            }
        }


        private void OnaylaButton_Click(object sender, RoutedEventArgs e)
        {
            if (secilenKullaniciText.Tag == null || Kitaplar.Count == 0)
            {
                MessageBox.Show("Lütfen bir kullanıcı ve en az bir kitap seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int kullaniciId = (int)secilenKullaniciText.Tag;
            foreach (var kitap in Kitaplar)
            {
                int kitapId = kitap.Id;
                DateTime teslimTarihi = kitap.TeslimTarihi;

                // Teslim tarihi geçmiş bir tarih mi kontrol et
                if (teslimTarihi.Date < DateTime.Now.Date)
                {
                    MessageBox.Show($"Kitap ID: {kitapId} için teslim tarihi geçmiş bir tarih seçilemez.", "Geçersiz Tarih", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    var command = new SQLiteCommand("INSERT INTO transactions (kullaniciId, kitapId, teslimTarihi, verilenTarih) VALUES (@kullaniciId, @kitapId, @teslimTarihi, @verilenTarih)", conn);
                    command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                    command.Parameters.AddWithValue("@kitapId", kitapId);
                    command.Parameters.AddWithValue("@teslimTarihi", teslimTarihi);
                    command.Parameters.AddWithValue("@verilenTarih", DateTime.Now);
                    command.ExecuteNonQuery();

                    // Kitap adedini bir azalt
                    var updateCommand = new SQLiteCommand("UPDATE books SET adet = adet - 1 WHERE id = @kitapId", conn);
                    updateCommand.Parameters.AddWithValue("@kitapId", kitapId);
                    updateCommand.ExecuteNonQuery();
                }
            }

            Kitaplar.Clear(); // ObservableCollection'ı temizle
            MessageBox.Show("Kitap(lar) başarıyla teslim edildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null && datePicker.SelectedDate.HasValue)
            {
                if (datePicker.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    MessageBox.Show("Geçmiş bir tarih seçemezsiniz.", "Geçersiz Tarih", MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePicker.SelectedDate = DateTime.Now.Date; // Geçerli tarihe sıfırla
                }
            }
        }


    }
}
