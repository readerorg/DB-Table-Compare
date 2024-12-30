<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Main.Master" AutoEventWireup="true" CodeBehind="ConnectionAdd.aspx.cs" Inherits="DBCompare3.Views.ConnectionAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="row">
    <div class="col-12 horizontalDiv" >

     <p>Data Source </p>   </div> <div class="col-12 horizontalDiv" >
    <asp:DropDownList ID="dropDownDataSources" runat="server" onchange="dataSourceChanged(this);" AutoPostBack="false"></asp:DropDownList>
    </div>
         <div class="col-12 horizontalDiv">
 <p  id="lblConnectionName">    Connection Name
</p>  </div> <div class="col-12 horizontalDiv"  >
        <asp:TextBox ID="textBoxConnectionName" runat="server"></asp:TextBox>
    </div>
    <div class="col-12 horizontalDiv">
 <p>     Server/File Path
</p>  </div> <div class="col-12 horizontalDiv" >
        <asp:TextBox ID="textBoxPath" runat="server"></asp:TextBox>
             <asp:FileUpload ID="fileUpload" runat="server" CssClass="invisible" accept=".db"/>
    </div>
             
          <div class="col-12 horizontalDiv">
 <p  id="lblUserName" runat="server" >    User Name
</p>  </div> <div class="col-12 horizontalDiv"  >
        <asp:TextBox ID="textBoxUserName" runat="server"></asp:TextBox>
    </div>

          <div class="col-12 horizontalDiv">
 <p  id="lblPassword" runat="server">     Password
</p>  </div> <div class="col-12 horizontalDiv" >
        <asp:TextBox ID="textBoxPassword" runat="server" CssClass="" ></asp:TextBox>
    </div>
          <div class="col-12 horizontalDiv" style="align-items:center;justify-content:center;" >
      <button type="button" class="btn btn-primary" style="width:20%" runat="server" onserverclick="getDatabases"  id="btnGetDatabases">Test Connection & Get Databases</button>
    </div>
          <div class="col-12 horizontalDiv" >

     <p id="lblDatabaseName" runat="server">Database Name </p>   </div> <div class="col-12 horizontalDiv" >
    <asp:DropDownList ID="dropDownDatabases" runat="server"  AutoPostBack="false"  CssClass=""></asp:DropDownList>
    </div>
      <div class="col-12 horizontalDiv" style="align-items:center;justify-content:center;" >
      <button type="button" class="btn btn-primary" style="width:20%" runat="server" onserverclick="submitClick">Save</button>
    </div>
 </div>

    <script>
        function dataSourceChanged(ddl) {

            if (ddl.value == 'SQLite') {
                document.getElementById('<%=textBoxUserName.ClientID%>').style.display = 'none'
                document.getElementById('<%=textBoxPassword.ClientID%>').style.display = 'none'
                document.getElementById('<%=textBoxPath.ClientID%>').style.display = 'none'
                document.getElementById('<%=lblUserName.ClientID%>').style.display = 'none'
                document.getElementById('<%=lblPassword.ClientID%>').style.display = 'none'
                document.getElementById('<%=fileUpload.ClientID%>').className = ''
                document.getElementById('<%=dropDownDatabases.ClientID%>').className = 'invisible'
                document.getElementById('<%=btnGetDatabases.ClientID%>').className = 'invisible'
                document.getElementById('<%=lblDatabaseName.ClientID%>').style.display = 'none'


            }
            else {
                document.getElementById('<%=textBoxUserName.ClientID%>').style.display = ''
                document.getElementById('<%=textBoxPassword.ClientID%>').style.display = ''
                document.getElementById('<%=textBoxPath.ClientID%>').style.display = ''
                document.getElementById('<%=lblUserName.ClientID%>').style.display = ''
                document.getElementById('<%=lblPassword.ClientID%>').style.display = ''
                document.getElementById('<%=fileUpload.ClientID%>').className = 'invisible'
                document.getElementById('<%=dropDownDatabases.ClientID%>').className = ''
                document.getElementById('<%=btnGetDatabases.ClientID%>').className = 'btn btn-primary'
                document.getElementById('<%=lblDatabaseName.ClientID%>').style.display = ''

            }
        }


      
    </script>


</asp:Content>
