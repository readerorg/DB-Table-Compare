<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Main.Master" AutoEventWireup="true" CodeBehind="ConnectionsForm.aspx.cs" Inherits="DBCompare3.Views.ConnectionsForm" EnableViewState="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/styles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <div class="container">
             <div class="row" style="padding:20px">
  <button type="button" runat="server" class="btn btn-primary"  id="Button1"  onserverClick="btnAddConnection">Add Connection</button>
</div> 
    <div class="row"> <div class="col-12"> <asp:GridView   ID="dataGrid" runat="server"   OnRowCommand="GridView1_RowCommand" CssClass="mGrid1"></asp:GridView></div>
       

    </div>
       </div>
</asp:Content>
