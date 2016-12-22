<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TokenHelperResearchWeb.WebForm1" %>

<!DOCTYPE html>

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
				appTitle: "Cloud-Hosted App with good hygiene",
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
			<asp:Label ID="hostSiteTitle" runat="server" />
		</div>
		<div style="margin: 10px">
			<asp:Label ID="groups" runat="server" />
		</div>
		<div style="margin: 10px">
			<asp:HyperLink ID="link" runat="server" NavigateUrl="Default.aspx" Text="Return to home page"></asp:HyperLink></div>
	</form>
</body>
</html>
