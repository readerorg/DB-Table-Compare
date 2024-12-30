using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DBCompare3.Views
{

    public partial class ConnectionAdd : System.Web.UI.Page
    {
        DataSet ds = new DataSet("Dataset");
        protected void Page_Load(object sender, EventArgs e)
        {
            
            FillDataSourceDropDown();
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            if (!IsPostBack)
            {
                try
                {
                    string connectionIndex = Request.QueryString["id"];
                    ViewState["connectionIndex"] = connectionIndex;
                    ds.Tables[0].Rows.RemoveAt(0);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == int.Parse(connectionIndex))
                        {
                            DataRow row = ds.Tables[0].Rows[i];
                            dropDownDataSources.Text = row[1].ToString();
                            DataSourceChange();
                            textBoxConnectionName.Text = row[0].ToString();
                            textBoxPath.Text = row[2].ToString();
                            textBoxUserName.Text = row[3].ToString();
                            textBoxPassword.Text = row[4].ToString();
                            if (row[1].ToString() != "SQLite")
                            {
                                getDatabases();
                                dropDownDatabases.Text = row[5].ToString();
                            }


                        }


                    }
                }
                catch (Exception ex) { }
            }
        }

        public void DataSourceChange() {
            if (dropDownDataSources.Text == "SQLite")
            {
                textBoxUserName.Style.Add("display","none");
                textBoxPassword.Style.Add("display", "none");
                textBoxPath.Style.Add("display", "none");
                lblUserName.Style.Add("display", "none");
                lblPassword.Style.Add("display", "none");
                lblDatabaseName.Style.Add("display", "none");
                fileUpload.CssClass = "";
                dropDownDatabases.CssClass = "invisible";
                btnGetDatabases.Style.Add("display","none");



            }
            else
            {
                textBoxUserName.Style.Add("display", "normal");
                textBoxPassword.Style.Add("display", "normal");
                textBoxPath.Style.Add("display", "normal");
                lblUserName.Style.Add("display", "normal");
                lblPassword.Style.Add("display", "normal");
                lblDatabaseName.Style.Add("display", "normal");
                fileUpload.CssClass = "invisible";
                dropDownDatabases.CssClass = "";
                btnGetDatabases.Style.Add("display", "normal");

            }
        }

        public void FillDataSourceDropDown() {
            dropDownDataSources.Items.Add("SQL Server");
            dropDownDataSources.Items.Add("MySQL");
            dropDownDataSources.Items.Add("SQLite");

        }
        protected void getDatabases(object sender, EventArgs e)
        {
            getDatabases();
        }

        public void getDatabases() {
            if (dropDownDataSources.SelectedIndex == 0)
            {
                SqlConnection sqlServerConnection = new SqlConnection();
                sqlServerConnection.ConnectionString = "Data Source=" + textBoxPath.Text + ";Initial Catalog=master;Integrated Security=false;MultipleActiveResultSets=True;User ID=" + textBoxUserName.Text + ";Password=" + textBoxPassword.Text;
                try
                {
                    sqlServerConnection.Open();
                    SqlCommand sqlServerCommand = new SqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "SELECT name FROM sys.databases WHERE name != 'master'";
                    SqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            dropDownDatabases.Items.Add(sqlServerReader["name"].ToString());
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
            else
            {
                MySqlConnection sqlServerConnection = new MySqlConnection();
                sqlServerConnection.ConnectionString = "Server=" + textBoxPath.Text + ";Uid=" + textBoxUserName.Text + ";Password=" + textBoxPassword.Text + ";CHARSET=utf8";
                try
                {
                    sqlServerConnection.Open();
                    MySqlCommand sqlServerCommand = new MySqlCommand();
                    sqlServerCommand.Connection = sqlServerConnection;
                    sqlServerCommand.CommandText = "Show Databases";
                    MySqlDataReader sqlServerReader = sqlServerCommand.ExecuteReader();
                    if (sqlServerReader.HasRows)
                    {
                        while (sqlServerReader.Read())
                        {
                            dropDownDatabases.Items.Add(sqlServerReader["Database"].ToString());
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



        protected void submitClick(object sender, EventArgs e)
        {

            if (ViewState["connectionIndex"] != null)
            {
                int index = int.Parse(ViewState["connectionIndex"].ToString());
                updateConnection(index);
            }
            else {
                saveConnection();
            }


        }

    
        public void updateConnection(int index) {

         //   ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(index);
            File.WriteAllText(MapPath("../App_Data/DataSources.xml"), "<?xml version=\"1.0\" standalone=\"yes\"?> \n <NewDataSet > \n </NewDataSet>");
            saveConnection();

        }

        public void saveConnection() {
           // DataSet ds = new DataSet("Dataset");

            try
            {
                DataColumn dc = new DataColumn("Connection Name", typeof(String));
                ds.Tables[0].Columns.Add(dc);

                dc = new DataColumn("Data Source", typeof(String));
                ds.Tables[0].Columns.Add(dc);


                dc = new DataColumn("Path", typeof(String));
                ds.Tables[0].Columns.Add(dc);

                dc = new DataColumn("User Name", typeof(String));
                ds.Tables[0].Columns.Add(dc);

                dc = new DataColumn("Password", typeof(String));
                ds.Tables[0].Columns.Add(dc);

                dc = new DataColumn("Database Name", typeof(String));
                ds.Tables[0].Columns.Add(dc);
            }
            catch (Exception ex) { }

            DataRow row = ds.Tables[0].NewRow();

            row["Connection Name"] = textBoxConnectionName.Text;
            row["Data Source"] = dropDownDataSources.SelectedValue;
            row["Path"] = dropDownDataSources.SelectedIndex == 2 ? fileUpload.FileName : textBoxPath.Text;
            row["User Name"] = textBoxUserName.Text;
            row["Password"] = textBoxPassword.Text;
            row["Database Name"] = dropDownDatabases.SelectedValue;

            ds.Tables[0].Rows.Add(row);
            // Save to disk
            ds.WriteXml(MapPath(@"../App_Data/DataSources.xml"));

            if (dropDownDataSources.SelectedIndex == 2)
            {
                fileUpload.SaveAs(MapPath("../App_Data/" + fileUpload.FileName));
            }
        }
    }
}