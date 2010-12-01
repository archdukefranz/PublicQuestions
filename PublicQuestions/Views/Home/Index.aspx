<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>
<%@ Import Namespace="PublicQuestions.Model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Top Questions</h2>
    <% Html.Grid((List<Message>)ViewData["Messages"]).Columns(column =>
        {
            column.For(x => x.Attributes.Votes).Named("Votes");
            column.For(x => x.Attributes.Answers).Named("Answers");
            column.For(x => x.Attributes.Views).Named("Views");
            column.For(x => x.Title).Named("Title").Action(p => { %> 
                     <td style="font-weight:bold"> 
                        <%= Html.ActionLink(p.Title, "Show", new { id = p.Name })%> 
                     </td>  
                <% }); 
        }).RowStart((p,row)  => {      
             if (row.IsAlternate) { %> 
                 <tr style="background-color:#CCDDCC"> 
             <%  }  else  { %> 
                 <tr> 
             <% } 
    }).Render(); %> 
</asp:Content>
