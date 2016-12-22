<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AppWithWAADWeb.Pages.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="../Scripts/sp.ui.controls.debug.js"></script>
	<script src="../Scripts/jquery-1.8.2.min.js"></script>
	<script type="text/javascript">

		function getQueryStringParameter(paramToRetrieve) {
			var params = document.URL.split("?")[1].split("&");
			var strParams = "";
			for (var i = 0; i < params.length; i = i + 1) {
				var singleParam = params[i].split("=");
				if (singleParam[0] == paramToRetrieve)
					return singleParam[1];
			}
		}

		$(function () {
			// determine URL back to host web
			var hostWebUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));

			var options = {
				siteUrl: hostWebUrl,
				siteTitle: "Host Web",
				//appHelpPageUrl: "help.aspx?SPHostUrl=" + hostWebUrl,
				//appIconUrl: "/Contents/AppIcon.png",
				appTitle: "App with Windows Azure Active Directory",
				//  settingsLinks: [
				//{ linkUrl: "start.aspx?SPHostUrl=" + hostWebUrl, displayName: "Home" },
				//{ linkUrl: "about.aspx?SPHostUrl=" + hostWebUrl, displayName: "About" },
				//{ linkUrl: "contact.aspx?SPHostUrl=" + hostWebUrl, displayName: "Contact" }
				//  ]
			};

			var nav = new SP.UI.Controls.Navigation("chrome_ctrl_container", options);
			nav.setVisible(true);
		});

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div id="chrome_ctrl_container"></div>

		<div style="margin: 10px">

			Web.CurrentUser.Title: <span style="font-weight: bold"><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></span>
			<div>
				Web.CurrentUser.Groups<br />
				<asp:BulletedList ID="BulletedList1" runat="server">
				</asp:BulletedList>
			</div>

			WAAD Object Id: <span style="font-weight: bold"><asp:Label ID="Label2" runat="server"></asp:Label></span>
			<div>
				WAAD Groups<br />
				<asp:BulletedList ID="BulletedList2" runat="server">
				</asp:BulletedList>
			</div>
		</div>
	</form>
</body>
</html>
