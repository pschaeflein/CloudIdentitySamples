using System;
using System.Web.UI;

namespace TokenHelperResearchWeb
{
	public partial class Default : PageBase
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);


			// The following code gets the client context and Title property by using TokenHelper.
			// To access other properties, you may need to request permissions on the host web.

			var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);
			var hostWeb = Page.Request["SPHostUrl"];

			using (var clientContext = TokenHelper.GetClientContextWithContextToken(hostWeb, contextToken, Request.Url.Authority))
			{
				clientContext.Load(clientContext.Web, web => web.Title);
				clientContext.ExecuteQuery();
				this.hostSiteTitle.Text = clientContext.Web.Title;
			}

			this.link.NavigateUrl += "?SPHostUrl=" + hostWeb;
		}

	}
}