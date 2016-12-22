using Microsoft.WindowsAzure.ActiveDirectory;
using Microsoft.WindowsAzure.ActiveDirectory.GraphHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWithWAADWeb.Pages
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// The following code gets the client context and Title property by using TokenHelper.
			// To access other properties, you may need to request permissions on the host web.

			var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);
			var hostWeb = Page.Request["SPHostUrl"];

			string userName = String.Empty;

			using (var clientContext = TokenHelper.GetClientContextWithContextToken(hostWeb, contextToken, Request.Url.Authority))
			{
				clientContext.Load(clientContext.Web, web => web.Title);
				clientContext.ExecuteQuery();

				clientContext.Load(clientContext.Web.CurrentUser, user => user.Title, user => user.Groups, user => user.LoginName);
				clientContext.ExecuteQuery();

				this.Label1.Text = clientContext.Web.CurrentUser.Title;
			  this.BulletedList1.DataSource = clientContext.Web.CurrentUser.Groups;
				this.BulletedList1.DataTextField = "Title";

				userName = clientContext.Web.CurrentUser.LoginName;
			}


			// retrieve the clientId and password values from the Web.config file
			string clientId = ConfigurationManager.AppSettings["WAADClientId"];
			string password = ConfigurationManager.AppSettings["WAADPassword"];
			string tenantName = ConfigurationManager.AppSettings["WAADTenant"];

			// get a token using the helper
			AADJWTToken token = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);

			// initialize a graphService instance using the token acquired from previous step
			DirectoryDataService graphService = new DirectoryDataService(tenantName, token);

			// get the user
			//
			//   would be nice if SPClaimProviderManager.Decode() was available....
			string currentUserId = userName.Substring(userName.LastIndexOf('|') + 1);
			User currentUser = graphService.users.Where(it => (it.objectId == currentUserId)).SingleOrDefault();

			// calling the memberOf method on a user will get direct groups (intransitive)
			// for authz, should call checkMemberGroups
			//
			graphService.LoadProperty(currentUser, "memberOf");
			var currentRoles = currentUser.memberOf.OfType<Role>();
			var currentGroups = currentUser.memberOf.OfType<Group>();

			this.Label2.Text = currentUser.objectId;
			this.BulletedList2.DataSource = currentGroups;
			this.BulletedList2.DataTextField = "displayName";


			this.DataBind();
		}
	}
}