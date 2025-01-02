using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace KKU_Kutuphane
{
    public partial class KitapSecWindow : Window
    {
        public ObservableCollection<Kitap> SecilenKitaplar { get; private set; } = new ObservableCollection<Kitap>();

        int kullaniciId;

        



        public KitapSecWindow(int kulId, bool ekle)
        {
            InitializeComponent();
            kullaniciId = kulId;
            if(ekle)
            KitaplariYukle();
            else
                VerilenKitaplariYukle();
        }

        private void KitaplariYukle()
        {
            var kitapListesi = new ObservableCollection<Kitap>();
            const string connectionString = "Data Source=database.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Kullanıcının zaten aldığı kitapları getiren sorgu
                var alreadyBorrowedCommand = new SQLiteCommand("SELECT kitapId FROM transactions WHERE kullaniciId = @kullaniciId", conn);
                alreadyBorrowedCommand.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                var reader = alreadyBorrowedCommand.ExecuteReader();
                var borrowedKitapIds = new HashSet<int>();
                while (reader.Read())
                {
                    borrowedKitapIds.Add(reader.GetInt32(0));
                }

                // Tüm kitapları getiren sorgu, ancak kullanıcının zaten aldığı kitapları hariç tutar
                var command = new SQLiteCommand("SELECT id, kitapAdi, yazar FROM books WHERE adet > 0 AND id NOT IN (SELECT kitapId FROM transactions WHERE kullaniciId = @kullaniciId)", conn);
                command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    kitapListesi.Add(new Kitap
                    {
                        Id = reader.GetInt32(0),
                        KitapAdi = reader.GetString(1),
                        Yazar = reader.GetString(2)
                    });
                }
            }
            kitapListView.ItemsSource = kitapListesi;
        }

        private void VerilenKitaplariYukle()
        {
            var kitapListesi = new ObservableCollection<Kitap>();
            const string connectionString = "Data Source=database.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Kullanıcının ödünç aldığı kitapları getiren sorgu. transactions ve books tablolarını JOIN ile birleştirerek kitap bilgilerini çekiyoruz.
                var command = new SQLiteCommand(@"
            SELECT books.id, books.kitapAdi, books.yazar, transactions.teslimTarihi
            FROM transactions
            INNER JOIN books ON transactions.kitapId = books.id
            WHERE transactions.kullaniciId = @kullaniciId AND transactions.teslimTarihi", conn);

                command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    kitapListesi.Add(new Kitap
                    {
                        Id = reader.GetInt32(0), // books.id
                        KitapAdi = reader.GetString(1), // books.kitapAdi
                        Yazar = reader.GetString(2), // books.yazar
                    });
                }
                conn.Close();
            }
            kitapListView.ItemsSource = kitapListesi;
        }


        private void EkleButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Kitap kitap in kitapListView.SelectedItems)
            {
                SecilenKitaplar.Add(kitap);
            }
            this.DialogResult = true;
            this.Close();
        }

        private void kitapListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void kitapListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // ListView içerisindeki herhangi bir öğeye çift tıklandığında
            // Ekle butonunun işlevini tetikle
            EkleButton_Click(null, null);
        }


    }


}
