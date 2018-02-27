using PKS.Core;
using PKS.Data;
using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.SysFrame
{
    public class RoleMapService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_ROLE_MAP> _roleMapRepository;

        public RoleMapService(IRepository<PKS_ROLE_MAP> roleMapRepository)
        {
            _roleMapRepository = roleMapRepository;
        }

        public void Update(List<string> ouNames)
        {
            var Depts = GetService<IADIdentityService>().GetDepts(ouNames);
            var newDepts = new List<PKS_ROLE_MAP>();
            Depts.ForEach(d => newDepts.Add(new PKS_ROLE_MAP
            {
                OrgName = d.OrgName,
                OriginalId = d.OriginalId,
                OriginalPId = d.OriginalPId,
                CreatedBy = "",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "",
                LastUpdatedDate = DateTime.Now
            }));
            var oldDept = _roleMapRepository.GetAll();
            var diffDelete = oldDept.Except(newDepts, new MapEquality()).ToList();
            diffDelete.ForEach(d => _roleMapRepository.DeleteList(r => r.OriginalId == d.OriginalId));
            var diffAdd = newDepts.Except(oldDept, new MapEquality()).ToList();
            _roleMapRepository.AddRange(diffAdd);
        }


        public class MapEquality : IEqualityComparer<PKS_ROLE_MAP>
        {
            public bool Equals(PKS_ROLE_MAP x, PKS_ROLE_MAP y)
            {
                return x.OriginalId == y.OriginalId && x.OriginalPId == y.OriginalPId;
            }
            public int GetHashCode(PKS_ROLE_MAP obj)
            {
                return obj == null ? 0 : obj.OriginalId.GetHashCode();
            }
        }
    }
}

