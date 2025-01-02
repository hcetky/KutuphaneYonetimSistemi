using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KKU_Kutuphane
{
    /// <summary>
    /// Interaction logic for KitaplarimSayfa.xaml
    /// </summary>
    public partial class KitaplarimSayfa : Page
    {

        private const string connectionString = "Data Source=database.db;Version=3;";
        private string kullaniciId;


        public KitaplarimSayfa(string kullaniciId)
        {
            InitializeComponent();
            this.kullaniciId = kullaniciId;

            Ara();
        }

        void Ara()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Kullanıcının sadece kendi kitaplarını getiren sorgu
                var query = @"
            SELECT 
                transactions.kullaniciId, 
                transactions.kitapId, 
                transactions.verilenTarih AS AlinanTarih, 
                users.adSoyad AS KullaniciAdi, 
                books.kitapAdi AS KitapAdi, 
                transactions.teslimTarihi AS TeslimTarihi
            FROM 
                transactions 
            INNER JOIN 
                users ON transactions.kullaniciId = users.id 
            INNER JOIN 
                books ON transactions.kitapId = books.id 
            WHERE 
                transactions.kullaniciId = @kullaniciId"; // Burada kullanıcıya özel filtreleme yapıyoruz.

                var command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@kullaniciId", kullaniciId); // Kullanıcı ID'sini parametre olarak ekliyoruz.

                var adapter = new SQLiteDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                raporDataGrid.ItemsSource = dataTable.DefaultView; // DataGrid'e sonuçları bağlıyoruz.
            }
        }

    }
}
