using System;
using System.Collections.Generic;
using AutoMapper;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.API.ErrorCode;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public abstract class BaseHierarchyService
    {
        protected readonly IHierarchyRepository HierarchyRepository;
        protected readonly IUnitOfWorkProvider UnitOfWorkProvider;
        protected BaseHierarchyService(IHierarchyRepository hierarchyRepository, IUnitOfWorkProvider unitOfWorkProvider)
        {
            this.HierarchyRepository = hierarchyRepository;
            this.UnitOfWorkProvider = unitOfWorkProvider;
        }

        public HierarchyDto GetHierarchyTree(long rootId)
        {
            var user = ServiceContext.CurrentUser;
            var entity = this.HierarchyRepository.GetById(rootId);

            if (entity == null)
            {
                return null;
            }

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

            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                entity = this.CreateHierarchy(unitOfWork, entity);

                unitOfWork.Commit();

                var dto = AutoMapper.Mapper.Map<HierarchyDto>(entity);

                return dto;
            }
        }

        public void DeleteHierarchy(long hierarchyId, bool isRecursive)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.DeleteHierarchy(unitOfWork, hierarchyId, isRecursive);

                unitOfWork.Commit();
            }
        }

        public void UpdateHierarchy(BL.API.DataContract.HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);
        }

        #region validation
        protected bool IsHierarchyCodeDuplicate(Hierarchy hierarchy)
        {
            if (hierarchy.Id != 0)
            {
                if (this.HierarchyRepository.RetrieveSiblingHierarchyCountByCodeUnderParentCustomer(hierarchy.Id, hierarchy.Code, hierarchy.CustomerId) > 0)
                {
                    return true;
                }
            }
            else
            {
                if (this.HierarchyRepository.RetrieveChildHierarchyCountByCodeUnderParentCustomer(hierarchy.Code, hierarchy.CustomerId) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsHierarchyNameDuplicate(Hierarchy hierarchy)
        {
            if (hierarchy.ParentId.HasValue)
            {
                if (hierarchy.Id != 0)
                {
                    if (this.HierarchyRepository.RetrieveSiblingHierarchyCountByNameUnderParentHierarchy(hierarchy.Id, hierarchy.Name, hierarchy.ParentId.Value) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.HierarchyRepository.RetrieveChildHierarchyCountByNameUnderParentHierarchy(hierarchy.Name, hierarchy.ParentId.Value) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                if (hierarchy.Id != 0)
                {
                    if (this.HierarchyRepository.RetrieveSiblingHierarchyCountByNameUnderParentCustomer(hierarchy.Id, hierarchy.Name, hierarchy.CustomerId) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.HierarchyRepository.RetrieveChildHierarchyCountByNameUnderParentCustomer(hierarchy.Name, hierarchy.CustomerId) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected bool IsOrganizationNestingOverLimitation(Hierarchy organization)
        {
            if (organization.ParentId.HasValue)
            {
                return this.HierarchyRepository.RetrieveAncestorAndSelfOrganizationCount(organization.ParentId.Value) + 1 > 5;
            }
            else
            {
                return false;
            }
        }

        protected bool DoesHierarchyHaveParent(Hierarchy hierarchy)
        {
            if (hierarchy.ParentId.HasValue)
            {
                Hierarchy parentHierarchy = this.HierarchyRepository.GetById(hierarchy.ParentId.Value);

                if (parentHierarchy == null)
                {
                    return false;
                }
                else
                {
                    hierarchy.CustomerId = parentHierarchy.Type == HierarchyType.Customer ? parentHierarchy.Id : parentHierarchy.CustomerId;
                    return true;
                }
            }
            else
            {
                return this.HierarchyRepository.GetById(hierarchy.CustomerId) != null;
            }
        }
        #endregion

        protected Hierarchy CreateHierarchy(IUnitOfWork unitOfWork, Hierarchy entity)
        {
            entity.UpdateTime = DateTime.Now;
            entity.TimezoneId = 1;

            if (entity.Type != HierarchyType.Customer && !this.DoesHierarchyHaveParent(entity))
            {
                throw new ConcurrentException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyHasNoParent);
            }

            if (this.IsHierarchyCodeDuplicate(entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyCodeDuplicate);
            }

            if (this.IsHierarchyNameDuplicate(entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyNameDuplicate);
            }

            if (entity.Type == HierarchyType.Organization)
            {
                if (this.IsOrganizationNestingOverLimitation(entity))
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.OrganizationNestingOverLimitation);
                }
            }

            entity = this.HierarchyRepository.Add(unitOfWork, entity);

            return entity;
        }

        protected void UpdateHierarchy(IUnitOfWork unitOfWork, Hierarchy entity)
        {
            if (entity.Type != HierarchyType.Customer && !this.DoesHierarchyHaveParent(entity))
            {
                throw new ConcurrentException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyHasNoParent);
            }

            if (this.IsHierarchyCodeDuplicate(entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyCodeDuplicate);
            }

            if (this.IsHierarchyNameDuplicate(entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyNameDuplicate);
            }

            if (entity.Type == HierarchyType.Organization)
            {
                if (this.IsOrganizationNestingOverLimitation(entity))
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.OrganizationNestingOverLimitation);
                }
            }

            this.HierarchyRepository.Update(unitOfWork, entity);
        }

        protected void DeleteHierarchy(IUnitOfWork unitOfWork, long hierarchyId, bool isRecursive)
        {
            if (!isRecursive)
            {
                var children = this.HierarchyRepository.GetByParentId(hierarchyId);

                if (children != null && children.Length > 0)
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, HierarchyError.HierarchyHasChildren);
                }

                this.HierarchyRepository.Delete(unitOfWork, hierarchyId);
            }
            else
            {
                Func<HierarchyDto, IEnumerable<long>> collector = null;
                collector = (tree) =>
                {
                    List<long> ids = new List<long>();

                    ids.Add(tree.Id.Value);

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
                    this.HierarchyRepository.Delete(unitOfWork, id);
                }
            }
        }

        protected void SetBaseHierarchyInfo(BaseHierarchyDto dto, Hierarchy entity)
        {
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Code = entity.Code;
            dto.ParentId = entity.ParentId.Value;
            dto.Type = entity.Type;
        }
    }
}