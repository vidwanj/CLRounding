using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CLRSurvey.ClrResponseReport
{
    public partial class ClrResponseReportUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dtcFromDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));
                dtcToDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));
            }
        }
    }
}
