<%@ Page Inherits="System.Web.Mvc.ViewPage<PagedList.PagedList<NerdDinner.Models.Dinner>>" Language="C#"  %>
<ul title="<%= Resources.Resources.Results %>">
      <% foreach (var dinner in Model) { %>
					<li><a href="<%:Url.RouteUrl("PrettyDetails", new { Id = dinner.DinnerID } ) %>">
						<%: dinner.EventDate.ToString(Resources.Resources.MMMdd)%> <%: HttpUtility.HtmlEncode(dinner.Description) %>
					</a></li> 
      <% } %>    
      <% if (Model.Count == 0) { %>
       <li><%= Resources.Resources.NoNerdDinnersFound %></li>
      <% } %>
</ul>
