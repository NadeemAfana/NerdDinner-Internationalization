<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Mobile/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NerdDinner.Models.Dinner>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Resources.Resources.Results %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Resources.Resources.NerdDinners %></h2>
		<ul class="dinners">
    <% foreach (var dinner in Model) { %>
        <li>     
            <ul>
            <li>
	            <a href="<%: Url.RouteUrl("PrettyDetails", new { Id = dinner.DinnerID }) %>"><%:dinner.Title %></a>
            </li>
            <li>
		          <%:dinner.EventDate.ToString("yyyy-MMM-dd")%> 
							@
							<%: dinner.EventDate.ToString("h:mm tt")%>
            </li>
						</ul>

        </li>
        <% } %>
      <% if (Model.Count() == 0) { %>
       <li><%= Resources.Resources.NoNerdDinnersFound %></li>
      <% } %>

		</ul>
    <p>
        <%: Html.ActionLink(Resources.Resources.Back, "Index", "Home") %>
    </p>

</asp:Content>

