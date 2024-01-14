using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;

namespace CheckBook
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var con = getConnection();
            con.Open();

            populateTable();

            string[] comboItems = new[] { "Debit", "Deposit" };
            typeTB.ItemsSource = comboItems;

            datePickerBox.SelectedDate = DateTime.Now;

            showTotal();
            con.Close();
        }

        private void AddNewEntryBN_Clicked(object sender, RoutedEventArgs e)
        {
            var con = getConnection();
            con.Open();

            SqlCommand cmd = new SqlCommand("InsertIntoCheckBook", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Type", typeTB.Text);
            cmd.Parameters.AddWithValue("@Date", datePickerBox.Text);
            cmd.Parameters.AddWithValue("@CheckNum", checknumTB.Text);
            cmd.Parameters.AddWithValue("@Description", descritptionTB.Text);
            cmd.Parameters.AddWithValue("@Withdrawal", "-" + withdrawalTB.Text);
            cmd.Parameters.AddWithValue("@Deposit", depositTB.Text);

            cmd.ExecuteNonQuery();

            populateTable();
            showTotal();
            con.Close();
            clear();
        }

        private void RemoveEntryBn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridXAML.SelectedItem;
            if (selectedItem != null)
            {
                DataRowView dataRow = (DataRowView)DataGridXAML.SelectedItem; //dataRow holds the selection

                var con = getConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand("DeleteFromCheckBook", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionNum", dataRow.Row.ItemArray[0]);

                cmd.ExecuteNonQuery();
                dataRow.Delete();
                showTotal();
                clear();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridXAML.SelectedItem;
            if (selectedItem != null)
            {
                DataRowView dataRow = (DataRowView)DataGridXAML.SelectedItem;

                var con = getConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand("UpdateCheckBook", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionNum", dataRow.Row.ItemArray[0]);
                cmd.Parameters.AddWithValue("@Type", dataRow.Row.ItemArray[1]);
                cmd.Parameters.AddWithValue("@Date", dataRow.Row.ItemArray[2]);
                cmd.Parameters.AddWithValue("@CheckNum", dataRow.Row.ItemArray[3]);
                cmd.Parameters.AddWithValue("@Description", dataRow.Row.ItemArray[4]);
                cmd.Parameters.AddWithValue("@Withdrawal", dataRow.Row.ItemArray[5]);
                cmd.Parameters.AddWithValue("@Deposit", dataRow.Row.ItemArray[6]);

                cmd.ExecuteNonQuery();
                populateTable();
                showTotal();
                con.Close();
                clear();
            }
        }
        // Methods...................................................................................
        public void clear()
        {
            typeTB.SelectedIndex = -1;
            datePickerBox.SelectedDate = DateTime.Now;
            checknumTB.Clear();
            descritptionTB.Clear();
            withdrawalTB.Clear();
            depositTB.Clear();
        }

        public SqlConnection getConnection()
        {
            var ConString = ConfigurationManager.ConnectionStrings["CheckBookCon"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            return con;
        }

        public void showTotal()
        {
            var con = getConnection();
            con.Open();
            string totalQuery = "(SELECT (SUM(Withdrawal) + SUM(Deposit)) FROM dbo.CheckBook)";
            using (SqlCommand showTotalCommand = new SqlCommand(totalQuery, con))
            {
                showTotalCommand.Parameters.AddWithValue("total", (string)(totalTB.Text));
                SqlDataReader data = showTotalCommand.ExecuteReader();
                while (data.Read())
                {
                    totalTB.Text = string.Format("${0:#,##0.00}", data.GetValue(0));
                }
            }
        }

        public void populateTable()
        {
            var con = getConnection();
            con.Open();
            string query = "SELECT * from [dbo].[CheckBook]";
            SqlCommand populateTable = new SqlCommand(query, con);
            populateTable.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(populateTable);
            DataTable dt = new DataTable("dbo.CheckBook");
            da.Fill(dt);
            DataGridXAML.ItemsSource = dt.DefaultView;
            da.Update(dt);
        }

    }
}
