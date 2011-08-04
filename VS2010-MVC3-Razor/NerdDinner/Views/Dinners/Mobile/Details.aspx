<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile/Site.Master" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.Dinner>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Title %></asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <a href="<%=Url.Content("~/")%>" runat="server">
            <img alt="nerddinner" src="Content/Img/Mobile/logo_medium.jpg" /></a></h1>
    <h2>
        <%: Model.Title %></h2>
    <ul id="Details">
        <li><strong><%= Resources.Resources.When %></strong>
            <%: Model.EventDate.ToShortDateString() %>
            @
            <%: Model.EventDate.ToShortTimeString() %></li>
        <li><strong><%= Resources.Resources.Where %></strong> <a target="_self" href="<%=String.Format("http://m.live.com/search/LocationPage.aspx?l={0}&amp;q=map&amp;a=changelocation",Url.Encode(Model.Address + " " + Model.Country)) %>">
            <%: Model.Address %>,
            <%: Model.Country %></a></li>
        <li><strong><%= Resources.Resources.Description %></strong>
            <%: Model.Description %></li>
        <li><strong><%= Resources.Resources.Organizer %></strong>
            <%: Model.HostedBy %>
            (<%: Model.ContactPhone %>)</li>
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
</asp:Content>
