using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TokenHelperResearchWeb
{
	public partial class WebForm1 : PageBase
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			var hostWeb = Page.Request["SPHostUrl"];

			this.hostSiteTitle.Text = this.LoginName;

			foreach (string groupName in this.SPGroupNames)
			{
				this.groups.Text = groupName + "<br/>";
			}

			this.link.NavigateUrl += "?SPHostUrl=" + hostWeb;
		}
	}
}