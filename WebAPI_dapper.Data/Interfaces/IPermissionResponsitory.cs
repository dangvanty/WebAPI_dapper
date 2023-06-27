using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_dapper.Data.ViewModel;

namespace WebAPI_dapper.Data.Interfaces
{
    public interface IPermissionResponsitory
    {
        Task<IEnumerable<FunctionActionViewModel>> GetAllWithPermission();
        Task<IEnumerable<PermissionViewModel>> GetAllRolePermissions(Guid? role);
        Task SavePermissions(Guid role, List<PermissionViewModel> permissions);
        Task<IEnumerable<FunctionViewModel>> GetAllFunctionByPermission(Guid userID);

    }
}
