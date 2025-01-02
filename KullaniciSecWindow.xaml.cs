using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace KKU_Kutuphane
{
    public partial class KullaniciSecWindow : Window
    {
        public Kullanici SecilenKullanici { get; private set; }

        public KullaniciSecWindow()
        {
            InitializeComponent();
            KullanicilariYukle();
        }

        private void KullanicilariYukle()
        {
            var kullaniciListesi = new ObservableCollection<Kullanici>();
            const string connectionString = "Data Source=database.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("SELECT id, adSoyad FROM users", conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    kullaniciListesi.Add(new Kullanici
                    {
                        Id = reader.GetInt32(0),
                        AdSoyad = reader.GetString(1)
                    });
                }
            }
            kullaniciListView.ItemsSource = kullaniciListesi;
        }

        private void SecButton_Click(object sender, RoutedEventArgs e)
        {
            if (kullaniciListView.SelectedItem != null)
            {
                SecilenKullanici = (Kullanici)kullaniciListView.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bir kullanıcı seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void kullaniciListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // ListView üzerinde bir öğeye çift tıklandığında
            // 'Seç' butonuna basılmış gibi aynı işlemi yap
            SecButton_Click(null, null);
        }
    }

    public class Kullanici
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
    }
}
