using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DBCompare3.Views
{
    public partial class ConnectionsForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                XmlToGrid(true);
            }
        }

        public void XmlToGrid(bool addCommand) {
            DataSet ds = new DataSet("Dataset");
            ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
            ds.Tables[0].Rows.RemoveAt(0);
            if (addCommand)
            {
                ButtonField col = new ButtonField();

                col.Text = "Edit";
                col.ButtonType = ButtonType.Button;
                col.CommandName = "EditC";
                col.DataTextField = (""); //or whatever bound column you want to show.
                dataGrid.Columns.Add(col);

                ButtonField col2 = new ButtonField();

                col2.Text = "Delete";
                col2.ButtonType = ButtonType.Button;
                col2.CommandName = "DeleteC";
                col2.DataTextField = (""); //or whatever bound column you want to show.
                dataGrid.Columns.Add(col2);
            }
            dataGrid.DataSource = ds.Tables[0];
            dataGrid.DataBind();
        }

        protected void btnAddConnection(object sender, EventArgs e)
        {
            Response.Redirect("ConnectionAdd.aspx");
        }
    
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            // Get index of row passed as command argument
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName.Equals("DeleteC"))
            {
                DataSet ds = new DataSet("Dataset");
        
                ds.ReadXml(MapPath("../App_Data/DataSources.xml"));
                File.WriteAllText(MapPath("../App_Data/DataSources.xml"), String.Empty);
                File.WriteAllText(MapPath("../App_Data/DataSources.xml"), "<?xml version=\"1.0\" standalone=\"yes\"?> \n <NewDataSet> \n </NewDataSet> ");

                ds.Tables[0].Rows.RemoveAt(index+1);
                ds.WriteXml(MapPath(@"../App_Data/DataSources.xml"));
                //   Response.Redirect("OrderPrint.aspx?orderid=" + Table1.Rows[index].Cells[2].Text);
                XmlToGrid(false);
            }
            else
            {
                Response.Redirect("ConnectionAdd.aspx?id=" + index);
            }
            // Your logic here

        }
    }
}