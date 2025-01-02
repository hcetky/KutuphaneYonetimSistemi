using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KKU_Kutuphane
{
    public partial class kullaniciSayfa : Page
    {
        private const string connectionString = "Data Source=database.db;Version=3;";
        private int selectedUserId = -1; // Düzenleme için seçili kullanıcı ID'si

        public kullaniciSayfa()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("SELECT * FROM users", conn);
                var adapter = new SQLiteDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                kullaniciDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Ekle_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var command = new SQLiteCommand("INSERT INTO users (tcKimlikNo, adSoyad, yas, mailAdresi, evAdresi) VALUES (@tc, @ad, @yas, @mail, @adres)", conn);
                command.Parameters.AddWithValue("@tc", tcNoTextBox.Text);
                command.Parameters.AddWithValue("@ad", adSoyadTextBox.Text);
                command.Parameters.AddWithValue("@yas", yasTextBox.Text);
                command.Parameters.AddWithValue("@mail", mailTextBox.Text);
                command.Parameters.AddWithValue("@adres", adresTextBox.Text);
                command.ExecuteNonQuery();
            }

            MessageBox.Show("Kullanıcı başarıyla eklendi.");
            LoadUsers();
            tabmenu.SelectedIndex = 0;

        }

        private void Guncelle_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUserId > -1)
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    var command = new SQLiteCommand("UPDATE users SET tcKimlikNo=@tc, adSoyad=@ad, yas=@yas, mailAdresi=@mail, evAdresi=@adres WHERE id=@id", conn);
                    command.Parameters.AddWithValue("@tc", tcNoTextBox.Text);
                    command.Parameters.AddWithValue("@ad", adSoyadTextBox.Text);
                    command.Parameters.AddWithValue("@yas", yasTextBox.Text);
                    command.Parameters.AddWithValue("@mail", mailTextBox.Text);
                    command.Parameters.AddWithValue("@adres", adresTextBox.Text);
                    command.Parameters.AddWithValue("@id", selectedUserId);
                    command.ExecuteNonQuery();
                }
                LoadUsers();
                selectedUserId = -1;
                tabmenu.SelectedIndex = 0;
                MessageBox.Show("Kullanıcı başarıyla güncellendi.");
            }
        }

        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            if (kullaniciDataGrid.SelectedItem is DataRowView selectedRow)
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    var command = new SQLiteCommand("DELETE FROM users WHERE id=@id", conn);
                    command.Parameters.AddWithValue("@id", selectedRow["id"]);
                    command.ExecuteNonQuery();
                }
                LoadUsers();
            }
        }

        private void Duzenle_Click(object sender, RoutedEventArgs e)
        {
            if (kullaniciDataGrid.SelectedItem is DataRowView selectedRow)
            {
                selectedUserId = Convert.ToInt32(selectedRow["id"]);
                adSoyadTextBox.Text = selectedRow["adSoyad"].ToString();
                tcNoTextBox.Text = selectedRow["tcKimlikNo"].ToString();
                yasTextBox.Text = selectedRow["yas"].ToString();
                mailTextBox.Text = selectedRow["mailAdresi"].ToString();
                adresTextBox.Text = selectedRow["evAdresi"].ToString();

                // İkinci tabı aç
                tabmenu.SelectedIndex = 1;
            }
        }
    }
}
