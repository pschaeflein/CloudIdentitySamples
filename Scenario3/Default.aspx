<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WAADGroups.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<br />
			<asp:Label ID="lblUserName" runat="server"></asp:Label>

			<asp:GridView ID="grdClaims" runat="server"></asp:GridView>
		</div>
			<asp:Button ID="Button1" runat="server" Text="isMemberOf 'SPLive!'" OnClick="Button1_Click" />
		<asp:Label ID="groupMembership" runat="server" />
	</form>
</body>
</html>
