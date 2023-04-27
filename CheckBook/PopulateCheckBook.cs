using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CheckBook
{
    public class PopulateCheckBook
    {
        public void Populate()
        {

            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-PU52KNI\SQLEXPRESS;Initial Catalog = Finances; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            string query = "SELECT * from [dbo].[CheckBook]";
            SqlCommand populatTable = new SqlCommand(query, con);
            populatTable.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(populatTable);
            DataTable dt = new DataTable("dbo.CheckBook");
            da.Fill(dt);
            da.Update(dt);
            con.Close();
        }

    }
}
