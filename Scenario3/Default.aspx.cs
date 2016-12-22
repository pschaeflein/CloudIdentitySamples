using Microsoft.WindowsAzure.ActiveDirectory;
using Microsoft.WindowsAzure.ActiveDirectory.GraphHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services.Client;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAADGroups
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.lblUserName.Text = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
			this.grdClaims.DataSource = ClaimsPrincipal.Current.Claims;
			this.grdClaims.DataBind();
		}

		public void CheckGroupMembership()
		{
			//get the tenantName
			string tenantName = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;



			// retrieve the clientId and password values from the Web.config file
			string clientId = ConfigurationManager.AppSettings["ClientId"];
			string password = ConfigurationManager.AppSettings["Password"];
			string groupName = ConfigurationManager.AppSettings["WAADGroup"];



			// get a token using the helper
			AADJWTToken token = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);

			// initialize a graphService instance using the token acquired from previous step
			DirectoryDataService graphService = new DirectoryDataService(tenantName, token);

			//  get Group
			//
			Group group = graphService.groups.Where(g => g.displayName == groupName).SingleOrDefault();
			

			//  For subsequent Graph Calls, the existing token should be used.
			//  The following checks to see if the existing token is expired or about to expire in 2 mins
			//  if true, then get a new token and refresh the graphService
			//
			int tokenMins = 2;
			if (token.IsExpired || token.WillExpireIn(tokenMins))
			{
				AADJWTToken newToken = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);
				token = newToken;
				graphService = new DirectoryDataService(tenantName, token);
			}


			// get the user
			//
			string currentUserId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
			User currentUser = graphService.users.Where(it => (it.objectId == currentUserId)).SingleOrDefault();

			//currentUser = graphService.users.Where(u => (u.userPrincipalName == "alans@sc365demo4.onmicrosoft.com")).SingleOrDefault();
			//User currentUser = graphService.users.Where(it => (it.objectId == currentUserId)).SingleOrDefault();

			// calling the memberOf method on a user will get direct groups (intransitive)
			// for authz, should call checkMemberGroups
			//
			graphService.LoadProperty(currentUser, "memberOf");
			var currentRoles = currentUser.memberOf.OfType<Role>();
			var currentGroups = currentUser.memberOf.OfType<Group>();

			string queryString = String.Format("users/{0}/checkMemberGroups", currentUser.objectId);

			// should really JSONify a collection...
			string[] groupList = new string[] { group.objectId };
			OperationParameter groupListParam = new BodyOperationParameter("groupIds", groupList);

			List<string> x = graphService.Execute<string>(new Uri(queryString, UriKind.Relative), "POST", false, groupListParam).ToList();

			if (x.Contains(group.objectId))
			{
				groupMembership.Text = "Yes";
			}
			else
			{
				groupMembership.Text = "No";
			}
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			this.CheckGroupMembership();
		}
	}
}