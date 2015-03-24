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
using SE.DSP.Foundation.DataAccess;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class HierarchyService : IHierarchyService
    {
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;

        public HierarchyService()
        {
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.unitOfWorkProvider = IocHelper.Container.Resolve<IUnitOfWorkProvider>();
        }

        //public HierarchyService(IHierarchyRepository hierarchyRepository, IUnitOfWorkProvider unitOfWorkProvider)
        //{
        //    this.hierarchyRepository = hierarchyRepository;
        //    this.unitOfWorkProvider = unitOfWorkProvider;
        //}


        public HierarchyDto GetHierarchyTree(long rootId)
        {
            var entity = this.hierarchyRepository.GetById(rootId);

            if (entity == null)
            {
                return null;
            }

            var hierarchy = Mapper.Map<HierarchyDto>(entity);

            var children = this.hierarchyRepository.GetByParentId(rootId);
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

            entity.UpdateTime = DateTime.Now;
            entity.TimezoneId = 1;
            entity = this.hierarchyRepository.Add(entity);

            var dto = AutoMapper.Mapper.Map<HierarchyDto>(entity);

            return dto;
        }

        public void DeleteHierarchy(long hierarchyId, bool isRecursive)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                if (!isRecursive)
                {
                    var children = this.hierarchyRepository.GetByParentId(hierarchyId);

                    if (children != null && children.Length > 0)
                    {
                        throw new Exception("Delete not allowed");
                    }

                    this.hierarchyRepository.Delete(unitOfWork, hierarchyId);
                }
                else
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
                    var list = collector(root);

                    foreach (var id in list)
                    {
                        this.hierarchyRepository.Delete(unitOfWork, id);
                    }
                }

                unitOfWork.Commit();
            }
        }

        public void UpdateHierarchy(BL.API.DataContract.HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);

            this.hierarchyRepository.Update(entity);
        }
    }
}