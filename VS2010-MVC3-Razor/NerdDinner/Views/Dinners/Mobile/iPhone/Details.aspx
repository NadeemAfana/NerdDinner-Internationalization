<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.Dinner>" %>

<ul id="Details">
    <li><%= Resources.Resources.Title %>
        <%: Model.Title %></li>
    <li><%= Resources.Resources.When %>
        <%: Model.EventDate.ToShortDateString() %>
        @
        <%: Model.EventDate.ToShortTimeString() %></li>
    <li><a target="_self" href="<%=String.Format("http://maps.google.com/maps?q={0},{1}",Url.Encode(Model.Address),Url.Encode(Model.Country)) %>">
        <%= Resources.Resources.Where %>
        <%: Model.Address %>,
        <%: Model.Country %></a></li>
    <li><%= Resources.Resources.Description %>
        <%: Model.Description %></li>
    <li><a target="_self" href="tel://<%: Model.ContactPhone %>">Organizer:
        <%: Model.HostedBy %>
        (<%: Model.ContactPhone %>)</a></li>
    <li><a href="#whoscoming"><%= Resources.Resources.WhosComingQuestion %></a></li>
</ul>
<div id="whoscoming" title="<%: Model.Title %>" class="panel">
    <h2>
        <%= Resources.Resources.WhosComingQuestion %></h2>
    <%if (Model.RSVPs.Count == 0)
      {%>
    <ul>
        <li><%= Resources.Resources.NoOneHasRegistered %></li></ul>
    <% } %>
    <%if (Model.RSVPs.Count > 0)
      {%>
    <ul id="attendees">
        <%foreach (var RSVP in Model.RSVPs)
          {%>
        <li>
            <%: RSVP.AttendeeName.Replace("@"," at ") %></li>
        <% } %>
    </ul>
    <%} %>
</div>
