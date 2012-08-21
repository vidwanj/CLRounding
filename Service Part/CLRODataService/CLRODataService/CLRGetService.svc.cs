using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace CLRODataService
{
    public class CLRGetService : DataService< CLREntities >
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
             config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
             config.SetServiceOperationAccessRule("UserAuthentication", ServiceOperationRights.AllRead);
             config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        public System.Data.Objects.ObjectResult<DeptContact> UserAuthentication(string EmployeeID)
        {
            return CurrentDataSource.UserAuthentication(EmployeeID);
        }
    
    }
}
