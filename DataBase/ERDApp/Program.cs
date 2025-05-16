using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ERDApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class FakeDatabase
    {
        private SQLiteConnection conn;
        public FakeDatabase()
        {
            conn = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            conn.Open();
        }
        public void CreateTable()
        {
            var cmd = new SQLiteCommand("CREATE TABLE Client (ClientID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Phone TEXT, Address TEXT, PaymentInfo TEXT)", conn);
            cmd.ExecuteNonQuery();
        }
        public void AddClient(string name, string phone, string address, string paymentInfo)
        {
            var cmd = new SQLiteCommand("INSERT INTO Client (Name, Phone, Address, PaymentInfo) VALUES (@Name, @Phone, @Address, @PaymentInfo)", conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PaymentInfo", paymentInfo);
            cmd.ExecuteNonQuery();
        }
        public void UpdateClient(int clientId, string name, string phone, string address, string paymentInfo)
        {
            var cmd = new SQLiteCommand("UPDATE Client SET Name=@Name, Phone=@Phone, Address=@Address, PaymentInfo=@PaymentInfo WHERE ClientID=@ClientID", conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PaymentInfo", paymentInfo);
            cmd.Parameters.AddWithValue("@ClientID", clientId);
            cmd.ExecuteNonQuery();
        }
        public void DeleteClient(int clientId)
        {
            var cmd = new SQLiteCommand("DELETE FROM Client WHERE ClientID=@ClientID", conn);
            cmd.Parameters.AddWithValue("@ClientID", clientId);
            cmd.ExecuteNonQuery();
        }
        public void PrintClients()
        {
            var cmd = new SQLiteCommand("SELECT * FROM Client", conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ClientID"]}: {reader["Name"]}, {reader["Phone"]}, {reader["Address"]}, {reader["PaymentInfo"]}");
                }
            }
        }
    }
}
