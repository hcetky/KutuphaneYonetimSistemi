using LoginApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;
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
using System.Windows.Shapes;

namespace KKU_Kutuphane
{
    /// <summary>
    /// Bu sayfa açılan ana penceredir.
    /// Tıklanan butonlara göre frame e yeni sayfalar yüklenir.
    /// </summary>
    public partial class anaForm : Window
    {
        private const string connectionString = "Data Source=database.db;Version=3;";

        bool yetki;
        private string kullaniciId;

        public anaForm(string user_name,string user_id)
        {
            InitializeComponent();


            txt_adSoyad.Content = user_name;
            kullaniciId= user_id;   
            KullaniciYetkisiKontrol(user_id);


            if (!yetki)
            {
                btnKitapTeslimEt.Visibility = Visibility.Collapsed;
                btnKitapTeslimAl.Visibility = Visibility.Collapsed;
                btnKitapYonetimi.Visibility = Visibility.Collapsed;
                btnKullaniciYonetimi.Visibility = Visibility.Collapsed;
                btnRaporlama.Visibility = Visibility.Collapsed;
            }

                btnKitaplarim_Click(btnRaporlama, null);


        }

        private void KullaniciYetkisiKontrol(string user_id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT YetkiliMi FROM users WHERE id = @UserId";


                var command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@UserId", user_id);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    if (result.ToString() == "1")
                    {
                        yetki = true;
                    }
                    else
                    {
                        yetki = false;
                    }
                }

                conn.Close();
            }
        }

        private void Cikis_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            
        }

        void arkaplan(Button aktifButon)
        {
            SolidColorBrush defaultBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212529"));

            btnRaporlama.Background = defaultBrush;
            btnKitapTeslimEt.Background = defaultBrush;
            btnKitapTeslimAl.Background = defaultBrush;
            btnKitapYonetimi.Background = defaultBrush;
            btnKullaniciYonetimi.Background = defaultBrush;
            btnKitaplarim.Background = defaultBrush;

            // Burada aktif butonun rengini farklı bir renkle belirleyebilirsiniz.
            aktifButon.Background = new SolidColorBrush(Colors.DarkSlateGray); // Örnek bir renk
        }

        private void btnRaporlama_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnRaporlama);
            ContentFrame.Navigate(new RaporlamaSayfa());
        }

        private void btnKitapTeslimEt_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnKitapTeslimEt);
            ContentFrame.Navigate(new teslimEtSayfa());
        }

        private void btnKitapTeslimAl_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnKitapTeslimAl);
            ContentFrame.Navigate(new teslimAlSayfa());
        }

        private void btnKitapYonetimi_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnKitapYonetimi);
            ContentFrame.Navigate(new kitapSayfa());
        }

        private void btnKullaniciYonetimi_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnKullaniciYonetimi);
            ContentFrame.Navigate(new kullaniciSayfa());
        }

        private void btnRaporlama_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btnRaporlama.Foreground= new SolidColorBrush(Colors.DarkSlateGray);
        }

        private void btnKitaplarim_Click(object sender, RoutedEventArgs e)
        {
            arkaplan(btnKitaplarim);
            ContentFrame.Navigate(new KitaplarimSayfa(kullaniciId));
        }
        private void btnKitaplarim_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btnKitaplarim.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
        }
    }
}
