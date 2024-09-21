using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace Online_Shopping_Backup
{
    public partial class add_cart : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShoppingConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_place_order_Click(object sender, ImageClickEventArgs e)
        {
            int u_id = Convert.ToInt32(Request.Cookies["u_id"].Value);
            string product_id = "";

            try
            {
                conn.Open();
                string query = "SELECT STUFF((SELECT ',' + [product_id] FROM [dbo].[cart_product] WHERE [user_id] = @user_id FOR XML PATH('')), 1, 1, '')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", u_id);

                var result = cmd.ExecuteScalar();
                if (!Convert.IsDBNull(result))
                {
                    product_id = (string)result;
                }
            }
            catch (SqlException ex)
            {
                // Log the error message
                Response.Write("An error occurred: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Response.Redirect("~/order_summary.aspx?product_id=" + product_id);
        }
    }
}
