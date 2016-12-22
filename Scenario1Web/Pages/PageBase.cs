using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenHelperResearchWeb
{
	public class PageBase:System.Web.UI.Page
	{
		private const string CACHE_ENTRY_SEPARATOR = "$$SC$$";
		private const string TOKEN_CACHE_KEY = "cachekey";
		private const string PAGE_REQUEST_SPHOSTURL = "SPHostUrl";

		protected string LoginName { get; set; }
		protected List<String> SPGroupNames { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			string sessionCacheKey = String.Empty;
			string serverCacheToken = String.Empty;
			string hostWebUrl = String.Empty;

			sessionCacheKey = (string)Session[TOKEN_CACHE_KEY];
			if (!String.IsNullOrEmpty(sessionCacheKey))
			{
				string cacheValue = (string)Cache[sessionCacheKey];
				if (!String.IsNullOrEmpty(cacheValue))
				{
					int x = cacheValue.IndexOf(CACHE_ENTRY_SEPARATOR);
					serverCacheToken = cacheValue.Substring(0, x);
					hostWebUrl = cacheValue.Substring(serverCacheToken.Length + CACHE_ENTRY_SEPARATOR.Length);
				}
			}

			if (String.IsNullOrEmpty(serverCacheToken))
			{
				hostWebUrl = Page.Request[PAGE_REQUEST_SPHOSTURL];

				var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);
				if (contextToken == null)
				{
					// log attempt directly from the interwebz
					Response.Clear();
					Response.StatusCode = 404;
					Response.End();
				}

				try
				{
					SharePointContextToken ctxToken = TokenHelper.ReadAndValidateContextToken(contextToken, Page.Request.Url.Authority);
					sessionCacheKey = ctxToken.CacheKey;
					serverCacheToken = contextToken;

					Cache[sessionCacheKey] = serverCacheToken + CACHE_ENTRY_SEPARATOR + hostWebUrl; // or use the Add method for expiration 
					Session["cacheKey"] = sessionCacheKey;
				}
				catch (Microsoft.IdentityModel.Tokens.AudienceUriValidationFailedException audEx)
				{
					// log failed attempt with a token for a different RemoteWeb
					Response.Clear();
					Response.StatusCode = 404;
					Response.End();
				}
				catch (Exception ex)
				{
					// log and display friendly error (it is not an obvious token error)
				}
			}


			// now, we know the request is authenticated, we can do our AuthZ checks 
			using (var clientContext = TokenHelper.GetClientContextWithContextToken(hostWebUrl, serverCacheToken, Request.Url.Authority))
			{
				clientContext.Load(clientContext.Web, web => web.Title);
				clientContext.Load(clientContext.Web.CurrentUser, user => user.LoginName, user => user.Groups);
				clientContext.ExecuteQuery();

				this.LoginName = clientContext.Web.CurrentUser.LoginName;
				this.SPGroupNames = new List<string>();

				foreach (var g in clientContext.Web.CurrentUser.Groups)
				{
					this.SPGroupNames.Add(g.Title);
				}
			}

		}
	}
}