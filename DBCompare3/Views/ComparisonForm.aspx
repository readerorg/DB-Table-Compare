<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Main.Master" AutoEventWireup="true" CodeBehind="ComparisonForm.aspx.cs" Inherits="DBCompare3.Views.ComparisonForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row">
    <div class="col-6 col-md-3 horizontalDiv" >

     <p>Connection 1 </p>   </div> <div class="col-6 col-md-3 horizontalDiv" >
    <asp:DropDownList  ID="dropDownConnection1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropDownConnection1_SelectedIndexChanged"></asp:DropDownList>
    </div>


             <div class="col-6 col-md-3 horizontalDiv" >

     <p>Connection 2 </p>   </div> <div class="col-6 col-md-3 horizontalDiv" >
    <asp:DropDownList ID="dropDownConnection2" runat="server" AutoPostBack="true"></asp:DropDownList>
    </div>
                  <div class="col-12 horizontalDiv" style="align-items:center;justify-content:center;" >
      <asp:Button class="btn btn-primary" style="width:20%" runat="server" onclick="getTables" Text="Get Tables"/>
    </div>
                 <div class="col-3 horizontalDiv" >


     <p>Table</p>   </div> <div class="col-3 horizontalDiv" >
    <asp:DropDownList   ID="dropDownTable1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropDownTable1_SelectedIndexChanged"></asp:DropDownList>
    </div>
               
           <div class="col-12"> <asp:GridView   ID="dataGrid" runat="server"   CssClass="mGrid1" AutoGenerateColumns="False"  OnRowCommand="dataGrid_RowCommand">
               <Columns>
<asp:BoundField DataField="Column Name" HeaderText="Column Name" />
<asp:TemplateField HeaderText="Connection 1">
  <ItemTemplate>

    <asp:TextBox ID="myfield" runat="server" Columns="10"  />
  </ItemTemplate>
</asp:TemplateField>
                   <asp:TemplateField HeaderText="Connection 2">
  <ItemTemplate>
    <asp:TextBox ID="myfield2" runat="server" Columns="10"  />
  </ItemTemplate>
</asp:TemplateField>
                   <asp:TemplateField HeaderText="Include">
    <ItemTemplate>
        <asp:CheckBox ID="CheckBoxProcess" runat="server" Checked="true" />
    </ItemTemplate>
</asp:TemplateField>

               </Columns>

                                </asp:GridView></div>
       
                <div class="col-12 horizontalDiv" style="align-items:center;justify-content:center;" >
      <asp:Button class="btn btn-primary" style="width:20%" runat="server" onclick="startComparing" Text="Compare"/>
    </div>
               <div class="col-12"> <asp:GridView   ID="dataGridResult" runat="server" CssClass="mGrid1"></asp:GridView></div>
</div>

</asp:Content>
