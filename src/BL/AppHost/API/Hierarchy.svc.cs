using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Contract;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using SE.DSP.Pop.Entity;
using System.Collections.Generic;
using AutoMapper;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class HierarchyService : IHierarchyService
    {
        private IHierarchyRepository _HierarchyRepository;
        private IHierarchyRepository HierarchyRepository
        {
            set { this._HierarchyRepository = value; }
            get { return this._HierarchyRepository ?? (this._HierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>()); }
        }


        public HierarchyDto GetHierarchyTree(long rootId)
        {
            var entity = this.HierarchyRepository.GetById(rootId);

            var hierarchy = Mapper.Map<HierarchyDto>(entity);

            var children = this.HierarchyRepository.GetByParentId(rootId);
            if (children != null && children.Length > 0)
            {
                List<HierarchyDto> list = new List<HierarchyDto>();
                foreach (var child in children)
                {
                    list.Add(this.GetHierarchyTree(child.Id));
                }

                hierarchy.Children = list.ToArray();
            }

            return hierarchy;
        }

        public HierarchyDto CreateHierarchy(HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);

            entity = this.HierarchyRepository.Add(entity);

            return AutoMapper.Mapper.Map<HierarchyDto>(hierarchy);
        }

        public void DeleteHierarchy(long hierarchyId, bool isRecursive)
        {
            if (!isRecursive)
            {
                this.HierarchyRepository.Delete(hierarchyId);
                return;
            }

            using (var ts = TransactionHelper.CreateRepeatableRead())
            {
                Func<HierarchyDto, IEnumerable<long>> collector = null;
                collector = (tree) =>
                 {
                     List<long> ids = new List<long>();

                     ids.Add(tree.Id);

                     if (tree.Children != null && tree.Children.Length > 0)
                     {
                         foreach (HierarchyDto hierarchy in tree.Children)
                         {
                             ids.AddRange(collector(hierarchy));
                         }
                     }

                     return ids;
                 };

                var root = this.GetHierarchyTree(hierarchyId);

                foreach (var id in collector(root))
                {
                    this.HierarchyRepository.Delete(id);
                }

                ts.Complete();
            }
        }

        public void UpdateHierarchy(BL.API.DataContract.HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);

            this.HierarchyRepository.Update(entity);
        }
    }
}
