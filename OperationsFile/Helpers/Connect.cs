using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerceGurseller.Helpers
{
    public static class Connect
    {
        static string strConnString;
        public static void ConnectionString(string conName)
        {
            strConnString = conName;
        }

        public static string Insert(string SpName, Dictionary<string, object> dictionary, string selectProcedur, DataGridView dgv, string outputs = "")
        {
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpName;

            for (int i = 0; i < dictionary.Count; i++)
            {
                cmd.Parameters.Add(dictionary.ElementAt(i).Key, SqlDbTypeDict.GetType(dictionary.ElementAt(i).Value.GetType().Name)).Value = dictionary.ElementAt(i).Value;
            }

            if (outputs != "")
            {
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = outputs;
                outPutParameter.SqlDbType = SqlDbType.Int;
                outPutParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);
            }

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                GetTable(dgv, selectProcedur);
                return "Başarılı";
            }
            catch (Exception ex)
            {
                return "Hata";
                throw ex;
            }

            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static string Update(string SpName, int Id, Dictionary<string, object> dictionary, string selectProcedur, DataGridView dgv, string outputs = "")
        {
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SpName;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Id";
            param.Value = Id;
            cmd.Parameters.Add(param);

            for (int i = 0; i < dictionary.Count; i++)
            {
                cmd.Parameters.Add(dictionary.ElementAt(i).Key, SqlDbTypeDict.GetType(dictionary.ElementAt(i).Value.GetType().Name)).Value = dictionary.ElementAt(i).Value;
            }

            if (outputs != "")
            {
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = outputs;
                outPutParameter.SqlDbType = SqlDbType.Int;
                outPutParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);
            }

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                GetTable(dgv, selectProcedur);
                return "Başarılı";
            }
            catch (Exception ex)
            {
                return "Hata";
                throw ex;
            }

            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static string Delete(DataGridView dgv, string procedurName, string selectProcedur, string where, string outParam = "")
        {
            SqlConnection baglanti = new SqlConnection(strConnString);
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand cmd = new SqlCommand(procedurName, baglanti);

                if (outParam != "")
                {
                    SqlParameter spReturns = new SqlParameter()
                    {
                        ParameterName = outParam,
                        DbType = DbType.Int32,
                        Direction = ParameterDirection.Output,
                    };
                    cmd.Parameters.Add(spReturns);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(where, dgv.SelectedRows[0].Cells[0].Value);
                int returnsCount = cmd.ExecuteNonQuery();

                GetTable(dgv, selectProcedur);
                return "Success";

                baglanti.Dispose();
                baglanti.Close();

            }
            catch (Exception ex)
            {
                return "IMPORTANT ERROR";
            }
        }

        public static void GetTable(DataGridView dgv, string procedurName)
        {
            #region DataGridViewSettings

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.GridColor = System.Drawing.Color.Gainsboro;
            dgv.RowHeadersVisible = false;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.ReadOnly = true;

            #endregion

            SqlConnection baglanti = new SqlConnection(strConnString);
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.ClearSelection();

            DataTable dataTable = new DataTable();
            baglanti.Open();

            SqlCommand cmd = new SqlCommand(procedurName, baglanti);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dt = cmd.ExecuteReader();

            dataTable.Load(dt);
            dgv.DataSource = dataTable;
            dgv.Columns[0].Visible = false;

            baglanti.Dispose();
            baglanti.Close();
        }

        public static void BindCombobox(ComboBox combobox, string tableName, string valueMember, string displayMember)
        {
            SqlConnection con = new SqlConnection(strConnString);
            try
            {
                combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                DataRow dr;

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from " + tableName, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                combobox.DataSource = dt;

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "Lütfen Seçim Yapınız" };
                dt.Rows.InsertAt(dr, 0);

                combobox.ValueMember = valueMember;
                combobox.DisplayMember = displayMember;

                combobox.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error!");
            }
            finally
            {
                con.Close();
            }

        }
    }
}
