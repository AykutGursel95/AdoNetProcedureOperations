using ECommerceGurseller.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerceGurseller
{
    public partial class Form1 : Form
    {
        static string connString = ConfigurationManager.ConnectionStrings["ECommerceDB"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            Connect.ConnectionString(connString);
        }

        private void btnEkleCon_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@contentType", txtContentType.Text);
            dictionary.Add("@attachment", txtAttachment.Text);
            dictionary.Add("@createdAt", DateTime.Now);
            dictionary.Add("@updatedAt", DateTime.Now);


            string res = Connect.Insert(ProcedurNames.SP_AssetInsert.ToString(), dictionary, ProcedurNames.SP_AssetSelect.ToString(), dgvAsset, "@returns");
            MessageBox.Show(res);

            Clear.textClear(txtAttachment, txtContentType);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connect.BindCombobox(comboBox2,TableList.Asset.ToString(), "Id", "ContentType");
            Connect.GetTable(dgvAsset, ProcedurNames.SP_AssetSelect.ToString());
        }

        private void dgvAsset_DoubleClick(object sender, EventArgs e)
        {
            if (dgvAsset.SelectedRows.Count == 1)
            {
                txtContentType.Text = dgvAsset.SelectedRows[0].Cells["contentType"].Value.ToString();
                txtAttachment.Text = dgvAsset.SelectedRows[0].Cells["attachment"].Value.ToString();
            }
        }

        private void btnSilCon_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Connect.Delete(dgvAsset, ProcedurNames.SP_AssetDelete.ToString(), ProcedurNames.SP_AssetSelect.ToString(), "@Id"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(comboBox1.Text.ToString());
            MessageBox.Show(comboBox1.SelectedValue.ToString());
        }

        private void btnGuncelleCon_Click(object sender, EventArgs e)
        {
            if (dgvAsset.SelectedRows.Count <= 0) return;

            var Id = dgvAsset.SelectedRows[0].Cells["Id"].Value.ToString();

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@contentType", txtContentType.Text);
            dictionary.Add("@attachment", txtAttachment.Text);
            dictionary.Add("@createdAt", DateTime.Now);
            dictionary.Add("@updatedAt", DateTime.Now);

            Clear.textClear(txtAttachment, txtContentType);

            string res = Connect.Update(ProcedurNames.SP_AssetUpdate.ToString(), Convert.ToInt32(Id), dictionary, ProcedurNames.SP_AssetSelect.ToString(), dgvAsset, "@Returns");
            MessageBox.Show(res);

        }

    }
}
