using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage
{
    public class KManageService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KFRAGMENT_TYPE> _kFTRepository;
        private readonly IRepository<PKS_KFRAGMENT_TYPE_PARAMETER> _kFTPRepository;
        private readonly IRepository<PKS_KTEMPLATE_PARAMETER> _kTPRepository;

        public KManageService(IRepository<PKS_KFRAGMENT_TYPE> kTTRepository,
                               IRepository<PKS_KFRAGMENT_TYPE_PARAMETER> kFTPRepository,
                               IRepository<PKS_KTEMPLATE_PARAMETER> kTPRepository
                               )
        {
            _kFTRepository = kTTRepository;
            _kFTPRepository = kFTPRepository;
            _kTPRepository = kTPRepository;
        }

        public List<PKS_KTEMPLATE_PARAMETER> GetGlobalParas()
        {
            return _kTPRepository.GetAll();
        }

        public void SaveGlobalParas(string data)
        {
            var paras = data.JsonTo<List<PKS_KTEMPLATE_PARAMETER>>();
            paras.ForEach(p =>
            {
                if (p.Id == 0)
                {
                    _kTPRepository.Add(p, false);
                }
                else
                {
                    _kTPRepository.Update(p, false);
                }
            });
            _kTPRepository.Submit();
        }

        public void DeleteGlobalParas(int[] ids)
        {
            _kTPRepository.DeleteList(p => ids.Contains(p.Id));
        }


        /// <summary>
        /// 获取组件信息
        /// </summary>
        /// <returns></returns>
        public List<PKS_KFRAGMENT_TYPE> GetWidgets()
        {
            return _kFTRepository.GetAll();
        }

        public void SaveWidgets(List<PKS_KFRAGMENT_TYPE> paras)
        {
            paras.ForEach(p =>
            {
                if (p.Id == 0)
                {
                    _kFTRepository.Add(p, false);
                }
                else
                {
                    _kFTRepository.Update(p, false);
                }
            });
            _kFTRepository.Submit();
        }
        public void SaveWidgetParas(int typeId, List<PKS_KFRAGMENT_TYPE_PARAMETER> paras)
        {
            if (paras.Count == 0)
            {
                _kFTPRepository.DeleteList(p => p.KFRAGMENTTYPEID == typeId);
                return;
            }
            paras.ForEach(p =>
            {
                if (p.Id == 0)
                {
                    _kFTPRepository.Add(p, false);
                }
                else
                {
                    _kFTPRepository.Update(p, false);
                }
            });
            _kFTPRepository.Submit();
        }


        public void DeleteWidgets(int[] ids)
        {
            ids.ForEach(id => _kFTPRepository.DeleteList(p => p.KFRAGMENTTYPEID == id));

            _kFTRepository.DeleteList(p => ids.Contains(p.Id));
        }

        public void DeleteWidgetParas(int id)
        {
            _kFTPRepository.DeleteList(p => p.Id == id);
        }
    }
}
