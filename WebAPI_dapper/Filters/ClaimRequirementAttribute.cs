using Microsoft.AspNetCore.Mvc;
using WebAPI_dapper.Helpers;

namespace WebAPI_dapper.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(FunctionCode function, ActionCode action) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { function, action };
        }
    }
}
