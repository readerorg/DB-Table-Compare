using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DBCompare3.Views
{
    public partial class ComparisonForm : System.Web.UI.Page
    {
        DataTable connections = new DataTable();
        ArrayList exclusions = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["exclusions"] != null) {
                exclusions = ViewState["exclusions"] as ArrayList;
               
            }
            if (!IsPostBack)
            {
                LoadingConnections();
               /* ButtonField col = new ButtonField();

                col.Text = "Exclude";
                col.ButtonType = ButtonType.Button;
                col.CommandName = "Exclude";
                col.DataTextField = (""); //or whatever bound column you want to show.
                dataGrid.Columns.Add(col);*/
            }
          
        }

     

        public void LoadingTablesForConnection1() {
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);
            connections = ds.Tables[0];
            if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "SQL Server")
            {
                SqlConnection sqlServerConnection = new SqlConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Initial Catalog=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Integrated Security=false;MultipleActiveResultSets=True;User ID=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password");
                try
                {
                    sqlServerConnection.Open();
                    SqlCommand sqlServerCommand = new SqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT * FROM  SYSOBJECTS WHERE  xtype = 'U' order by name asc";
                    SqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            dropDownTable1.Items.Add(sqlServerReader["name"].ToString());
                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);

                }
            }
            else if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "MySQL") {
                MySqlConnection sqlServerConnection = new MySqlConnection();
                sqlServerConnection.ConnectionString = "Server=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Database=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Uid=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password") + ";CHARSET=utf8";
                try
                {
                    sqlServerConnection.Open();
                    MySqlCommand sqlServerCommand = new MySqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT table_name FROM information_schema.tables where TABLE_TYPE='BASE TABLE';";
                    MySqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            dropDownTable1.Items.Add(sqlServerReader["table_name"].ToString());
                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                }
            }
            else {
                SQLiteConnection sqlServerConnection = new SQLiteConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + MapPath("../App_Data/" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path")) + ";MultipleActiveResultSets=true;";
                try
                {
                    sqlServerConnection.Open();
                    SQLiteCommand sqlServerCommand = new SQLiteCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' order by name asc";
                    SQLiteDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            dropDownTable1.Items.Add(sqlServerReader["name"].ToString());
                        }
                    }

                    sqlServerReader.Close();
                    sqlServerConnection.Close();

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                }
            }
        }



        public void getPrimaryKeysForTable1()
        {
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);

            connections = ds.Tables[0];
            if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "SQL Server")
            {
                SqlConnection sqlServerConnection = new SqlConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Initial Catalog=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Integrated Security=false;MultipleActiveResultSets=True;User ID=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password");
                try
                {

                    sqlServerConnection.Open();
                    SqlCommand sqlServerCommand = new SqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = @"SELECT i.name AS IndexName, OBJECT_NAME(ic.OBJECT_ID) AS TableName, 
       COL_NAME(ic.OBJECT_ID, ic.column_id) AS ColumnName
FROM sys.indexes AS i
INNER JOIN sys.index_columns AS ic
ON i.OBJECT_ID = ic.OBJECT_ID
AND i.index_id = ic.index_id
WHERE i.is_primary_key = 1 and OBJECT_NAME(ic.OBJECT_ID) = '" + dropDownTable1.SelectedValue + "'";
                    SqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Column Name");
                        dt.Columns.Add("Connection 1");
                        dt.Columns.Add("Connection 2");
                        while (sqlServerReader.Read())
                        {


                         
                            DataRow row = dt.NewRow();
                            if (!exclusions.Contains(sqlServerReader["ColumnName"].ToString()))
                            {
                                row["Column Name"] = sqlServerReader["ColumnName"].ToString();
                                row["Connection 1"] = "";
                                row["Connection 2"] = "";
                                dt.Rows.Add(row);

                            }
                
                     

                        }
                   
                        dataGrid.DataSource = dt;
                        dataGrid.DataBind();
                    }
                    sqlServerReader.Close();

                  
                    sqlServerConnection.Close();

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);

                }
            }
            else if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "MySQL")
            {
                MySqlConnection sqlServerConnection = new MySqlConnection();
                sqlServerConnection.ConnectionString = "Server=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Database=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Uid=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password") + ";CHARSET=utf8";
                try
                {
                    sqlServerConnection.Open();
                    MySqlCommand sqlServerCommand = new MySqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SHOW KEYS FROM " + dropDownTable1.SelectedValue + " WHERE Key_name = 'PRIMARY'";
                    MySqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Column Name");
                        dt.Columns.Add("Connection 1");
                        dt.Columns.Add("Connection 2");
                        while (sqlServerReader.Read())
                        {



                            DataRow row = dt.NewRow();

                            row["Column Name"] = sqlServerReader["Column_Name"].ToString();
                            row["Connection 1"] = "";
                            row["Connection 2"] = "";

                            dt.Rows.Add(row);


                        }
                   
                        dataGrid.DataSource = dt;
                        dataGrid.DataBind();
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                }
            }
            else
            {
                SQLiteConnection sqlServerConnection = new SQLiteConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + MapPath("../App_Data/" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path")) + ";MultipleActiveResultSets=true;";
                try
                {
                    sqlServerConnection.Open();
                    SQLiteCommand sqlServerCommand = new SQLiteCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "PRAGMA table_info('" + dropDownTable1.SelectedValue + "')";
                    SQLiteDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Column Name");
                        dt.Columns.Add("Connection 1");
                        dt.Columns.Add("Connection 2");
                        while (sqlServerReader.Read())
                        {

                            if (sqlServerReader["pk"].ToString() == "1")
                            {

                                DataRow row = dt.NewRow();

                                row["Column Name"] = sqlServerReader["name"].ToString();
                                row["Connection 1"] = "";
                                row["Connection 2"] = "";
                                dt.Rows.Add(row);
                            }



                        }
       
                        dataGrid.DataSource = dt;
                        dataGrid.DataBind();
                      
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                }
            }

        }

        public void LoadingConnections() {
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);
            connections = ds.Tables[0];
            foreach (DataRow row in ds.Tables[0].Rows) {
                dropDownConnection1.Items.Add(row["Connection Name"].ToString());
                dropDownConnection2.Items.Add(row["Connection Name"].ToString());

            }
        }

        protected void dropDownConnection1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void dropDownTable1_SelectedIndexChanged(object sender, EventArgs e)
        {

            getPrimaryKeysForTable1();
        }

        protected void startComparing(object sender, EventArgs e)
        {
           Dictionary<string,string> dict1= getRowToDictionary();
           Dictionary<string, string> dict2 = getRowToDictionary2();
            try
            {
                var dict3 = dict1.Except(dict2).ToDictionary(x => x.Key, x => x.Value);

                var dict4 = dict2.Except(dict1).ToDictionary(x => x.Key, x => x.Value);

                DataTable dt = new DataTable();
                dt.Columns.Add("Field Name", typeof(string));
                dt.Columns.Add("Connection 1", typeof(string));
                dt.Columns.Add("Connection 2", typeof(string));
                foreach (var item in dict3)
                {
                    DataRow dr = dt.NewRow();
                    dr["Field Name"] = item.Key;
                    dr["Connection 1"] = item.Value;
                    try
                    {
                        dr["Connection 2"] = dict4[item.Key];
                    }
                    catch { dr["Connection 2"] = "Column Not Exists"; }
                    dt.Rows.Add(dr);
                }


                dataGridResult.DataSource = dt;
                dataGridResult.DataBind();
            }
            catch {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Primary Keys aren`t not found in both tables')", true);

            }

        }



        protected Dictionary<string, string> getRowToDictionary()
        {
            Dictionary<string, string> dictToReturn = new Dictionary<string, string>();
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);
            connections = ds.Tables[0];
            if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "SQL Server")
            {
                SqlConnection sqlServerConnection = new SqlConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Initial Catalog=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Integrated Security=false;MultipleActiveResultSets=True;User ID=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password");
                try
                {
                    sqlServerConnection.Open();
                    SqlCommand sqlServerCommand = new SqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;




                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";

                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    SqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
            else if (connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Data Source") == "MySQL")
            {
                MySqlConnection sqlServerConnection = new MySqlConnection();
                sqlServerConnection.ConnectionString = "Server=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path") + ";Database=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Database Name") + ";Uid=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Password") + ";CHARSET=utf8";
                try
                {
                    sqlServerConnection.Open();
                    MySqlCommand sqlServerCommand = new MySqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;

                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";
                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    MySqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
            else
            {
                SQLiteConnection sqlServerConnection = new SQLiteConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + MapPath("../App_Data/" + connections.Rows[dropDownConnection1.SelectedIndex].Field<string>("Path")) + ";MultipleActiveResultSets=true;";
                try
                {
                    sqlServerConnection.Open();
                    SQLiteCommand sqlServerCommand = new SQLiteCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";
                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    SQLiteDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                try
                                {
                                    dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                                }
                                catch {
                                    dictToReturn.Add(sqlServerReader.GetName(i), "0");

                                }
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;
                }
                catch (Exception ex)
                {
                    string z = ex.Message.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
        }



             protected Dictionary<string, string> getRowToDictionary2()
        {
            Dictionary<string, string> dictToReturn = new Dictionary<string, string>();
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);
            connections = ds.Tables[0];
            if (connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Data Source") == "SQL Server")
            {
                SqlConnection sqlServerConnection = new SqlConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Path") + ";Initial Catalog=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Database Name") + ";Integrated Security=false;MultipleActiveResultSets=True;User ID=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Password");
                try
                {
                    sqlServerConnection.Open();
                    SqlCommand sqlServerCommand = new SqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;




                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";
                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield2") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    SqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
            else if (connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Data Source") == "MySQL")
            {
                MySqlConnection sqlServerConnection = new MySqlConnection();
                sqlServerConnection.ConnectionString = "Server=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Path") + ";Database=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Database Name") + ";Uid=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("User Name") + ";Password=" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Password") + ";CHARSET=utf8";
                try
                {
                    sqlServerConnection.Open();
                    MySqlCommand sqlServerCommand = new MySqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;

                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";
                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield2") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    MySqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
            else
            {
                SQLiteConnection sqlServerConnection = new SQLiteConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + MapPath("../App_Data/" + connections.Rows[dropDownConnection2.SelectedIndex].Field<string>("Path")) + ";MultipleActiveResultSets=true;";
                try
                {
                    sqlServerConnection.Open();
                    SQLiteCommand sqlServerCommand = new SQLiteCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT * FROM " + dropDownTable1.SelectedValue + " WHERE";
                    foreach (GridViewRow item in dataGrid.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("CheckBoxProcess");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                TextBox txtStoreNumber = item.Cells[1].FindControl("myfield2") as TextBox;
                                sqlServerCommand.CommandText = sqlServerCommand.CommandText + " " + item.Cells[0].Text + "='" + txtStoreNumber.Text + "' and";
                            }
                        }
                    }
                    sqlServerCommand.CommandText = sqlServerCommand.CommandText.Remove(sqlServerCommand.CommandText.Length - 3);
                    string y = sqlServerCommand.CommandText;
                    SQLiteDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            for (int i = 0; i < sqlServerReader.FieldCount; i++)
                            {
                                dictToReturn.Add(sqlServerReader.GetName(i), sqlServerReader.GetValue(i).ToString());
                            }

                        }
                    }
                    sqlServerReader.Close();
                    sqlServerConnection.Close();
                    return dictToReturn;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Connection Error')", true);
                    return null;
                }
            }
        }

        protected void getTables(object sender, EventArgs e)
        {
            LoadingTablesForConnection1();
        }

        protected void dataGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Exclude"))
            {
                exclusions.Add(dataGrid.Rows[index].Cells[0].Text);
                ViewState["exclusions"] = exclusions;
            
            }
        }
    }

  
}