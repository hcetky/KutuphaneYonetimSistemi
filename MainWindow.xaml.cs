using System.Data.SQLite;  // SQLite kütüphanesini kullanıyoruz
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using KKU_Kutuphane;

namespace LoginApp
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Kullanıcı Girişi
        /// </summary>
        /// 

        private System.Windows.Point _secondWindowStartPosition;
        private string connectionString = "Data Source=database.db;Version=3;";
        string user_name = "";
        string user_id = "";


        public MainWindow()
        {
            InitializeComponent();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message);
                this.Close();
            }


            //anaForm secondWindow = new anaForm("Hüseyin", "1");
            //secondWindow.Show();
            //Close();

            this.Topmost = true;
            text_kullaniciadi.Focus();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = text_kullaniciadi.Text.Trim();
                string password = text_sifre.Password.Trim();

                if (username == "" || password == "")
                {
                    MessageBox.Show("Kullanıcı adı veya şifre boş olamaz!", "Boş Alan Mevcut", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                        {
                            string query = "SELECT adSoyad, id FROM users WHERE tcKimlikNo=@Username AND sifre=@Password";

                            SQLiteCommand cmd = new SQLiteCommand(query, connection);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);

                            connection.Open();

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    user_name = reader["adSoyad"].ToString();
                                    user_id = reader["id"].ToString();



                                    if (WindowState == WindowState.Maximized)
                                    {
                                        anaForm secondWindow = new anaForm(user_name, user_id);
                                        secondWindow.WindowState = WindowState.Maximized;
                                        secondWindow.Show();
                                    }
                                    else
                                    {
                                        
                                        _secondWindowStartPosition = new System.Windows.Point(Left, Top);
                                        anaForm secondWindow = new anaForm(user_name, user_id);
                                        secondWindow.Left = _secondWindowStartPosition.X;
                                        secondWindow.Top = _secondWindowStartPosition.Y;
                                        secondWindow.Show();
                                    }

                                    Close();
                                }
                                else
                                {
                                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı!", "Giriş", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata Giriş: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void text_sifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void text_kullaniciadi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void ForgotPassword_MouseDown(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Şifrenizi unuttuysanız lütfen sistem yöneticinize başvurunuz.", "Şifremi Unuttum", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
