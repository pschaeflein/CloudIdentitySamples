<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TokenHelperResearchWeb.Default" %>

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

			<div><asp:HyperLink ID="link" runat="server" NavigateUrl="WebForm1.aspx" Text="Link to a different page"></asp:HyperLink></div>

		</div>
	</form>
</body>
</html>
