using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab06
{
    public partial class frmFood : Form
    {
        public frmFood()
        {
            InitializeComponent();
        }

        public void LoadFood(int categoryID)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=ADMIN;Initial Catalog=RestaurantManagement;Integrated Security=True";

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandText = "SELECT Name FROM Category where ID =" + categoryID;

            sqlConnection.Open();

            string catName = sqlCommand.ExecuteScalar().ToString();
            this.Text = "Danh Sách các món ăn thuộc nhóm: " + catName;

            sqlCommand.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = " + categoryID;

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable("Food");
            da.Fill(dt);

            dgvFood.DataSource = dt;

            sqlConnection.Close();
            sqlConnection.Dispose();
            da.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=ADMIN;Initial Catalog=RestaurantManagement;Integrated Security=True";

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandText = "INSERT INTO Food(Name, Unit, FoodCategoryID, Price, Notes)" + "VALUES (N'" + txtFoodName.Text + "'," + "N'" + txtUnit.Text + "'," + Convert.ToInt32(txtCategoryID.Text) + "," + Convert.ToInt32(nbudPrice.Value) + ",N'" + txtNotes.Text + "')";
            sqlConnection.Open();

            int numOfRowEffected = sqlCommand.ExecuteNonQuery();



            sqlConnection.Close();
            if (numOfRowEffected == 1)
            {
                MessageBox.Show("Thêm món ăn thành công");
                LoadFood(Convert.ToInt32(txtCategoryID.Text));


            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại");
            }



        }

    


        private void dgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDelete.Enabled = true;
            btnSave.Enabled = true;

            txtFoodID.Text = dgvFood[0, e.RowIndex].Value.ToString();
            txtFoodName.Text = dgvFood[1, e.RowIndex].Value.ToString();
            txtUnit.Text = dgvFood[2, e.RowIndex].Value.ToString();
            txtCategoryID.Text = dgvFood[3, e.RowIndex].Value.ToString();
            nbudPrice.Value =(int) dgvFood[4, e.RowIndex].Value;
            txtNotes.Text = dgvFood[5, e.RowIndex].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=ADMIN;Initial Catalog=RestaurantManagement;Integrated Security=True";

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandText = "DELETE FROM Food " + "WHERE ID = " + txtFoodID.Text;
            sqlConnection.Open();

            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            if (numOfRowsEffected == 1)
            {
                foreach (DataGridViewRow item in this.dgvFood.SelectedRows)
                {

                    dgvFood.Rows.RemoveAt(this.dgvFood.SelectedRows[0].Index);


                    txtFoodID.Text = "";
                    txtFoodName.Text = "";
                    txtUnit.Text = "";
                    txtCategoryID.Text = "";
                    nbudPrice.Value = 1000;
                    txtNotes.Text = "";

                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;

                    MessageBox.Show("Xóa nhóm món ăn thành công");
                }
               

            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại");
            }
        }
    }
}
