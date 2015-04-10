using System;
using System.Collections.Generic;
using System.Linq;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.API.ErrorCode;
using SE.DSP.Pop.BL.AppHost.Common.Ioc;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.API
{
    [IocServiceBehavior]
    public class PopClientService : IPopClientService
    {
        public const string SingleLineDiagramOss = "img-sld-{0}";
        private readonly IGatewayRepository gatewayRepository;
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly ISingleLineDiagramRepository singleDiagramRepository;
        private readonly IOssRepository ossRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ICustomerRepository customerRepository;

        public PopClientService(IGatewayRepository gatewayRepository, IHierarchyRepository hierarchyRepository, ISingleLineDiagramRepository singleDiagramRepository, IOssRepository ossRepository, IUnitOfWorkProvider unitOfWorkProvider, ICustomerRepository customerRepository)
        {
            this.gatewayRepository = gatewayRepository;
            this.hierarchyRepository = hierarchyRepository;
            this.singleDiagramRepository = singleDiagramRepository;
            this.ossRepository = ossRepository;
            this.unitOfWorkProvider = unitOfWorkProvider;
            this.customerRepository = customerRepository;
        }

        public GatewayDto[] GetGatewayByCustomerId(long customerId)
        {
            var gateways = this.gatewayRepository.GetByCustomerId(customerId);

            return gateways.Select(g => AutoMapper.Mapper.Map<GatewayDto>(g)).ToArray();
        }

        public GatewayDto RegisterGateway(GatewayDto gateway)
        {
            if (gateway.Mac.Contains("-"))
            {
                gateway.Mac = gateway.Mac.Replace("-", string.Empty);
            }

            if (!this.IsGatewayNameValid(gateway))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.InvalidGatewayName);
            }

            if (!this.IsGatewayMacValid(gateway))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.InvalidGatewayMac);
            }

            Customer customer = null;

            if (!this.DoesGatewayCustomerExist(gateway, out customer))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayCustomerNotExist);
            }

            Gateway entity = null;

            if (this.DoesGatewayNameExist(gateway, out entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayAlreadyExist);
            }
            
            ////Guid guid = Guid.NewGuid();
            ////string uniqueId = Convert.ToBase64String(guid.ToByteArray());

            string key = string.Format("{0}|{1}", gateway.Name, gateway.Mac);

            string hex = CryptographyHelper.MD5(key);

            string[] ids = new string[4];

            for (int i = 0; i < 4; i++)
            {
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    int index = 0x0000003D & hexint;
                    outChars += SE.DSP.Foundation.Infrastructure.Constant.Constant.ALLLETTERSANDNUMBERS[index];
                    hexint = hexint >> 5;
                }

                ids[i] = outChars;
            }

            string uniqueId = null;
            foreach (var id in ids)
            {
                if (this.gatewayRepository.GetByUniqueId(id) == null)
                {
                    uniqueId = id;
                    break;
                }
            }

            if (string.IsNullOrEmpty(uniqueId))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.CantAcquireUniqueId);
            }

            gateway.UniqueId = uniqueId;
            gateway.CustomerId = customer.HierarchyId;
            gateway.RegisterTime = DateTime.Now;
            
            entity = this.gatewayRepository.Add(AutoMapper.Mapper.Map<Gateway>(gateway));

            return AutoMapper.Mapper.Map<GatewayDto>(entity);
        }

        public GatewayDto ReplaceGateway(GatewayDto gateway)
        {
            if (gateway.Mac.Contains("-"))
            {
                gateway.Mac = gateway.Mac.Replace("-", string.Empty);
            }

            if (!this.IsGatewayNameValid(gateway))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.InvalidGatewayName);
            }

            if (!this.IsGatewayMacValid(gateway))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.InvalidGatewayMac);
            }

            Customer customer = null;

            if (!this.DoesGatewayCustomerExist(gateway, out customer))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayCustomerNotExist);
            }

            Gateway entity = null;

            if (!this.DoesGatewayNameExist(gateway, out entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayNotExist);
            }

            entity.Mac = gateway.Mac;
            this.gatewayRepository.Update(entity);

            return AutoMapper.Mapper.Map<GatewayDto>(entity);
        }

        public SingleLineDiagramDto GetSingleLineDiagramById(long id)
        {
            var entity = this.singleDiagramRepository.GetById(id);

            var ossobject = this.ossRepository.GetById(string.Format(SingleLineDiagramOss, id));

            var result = AutoMapper.Mapper.Map<SingleLineDiagramDto>(entity);

            result.Content = ossobject.Content;

            return null;
        }

        public SingleLineDiagramDto UpdateSingleLineDiagram(SingleLineDiagramDto dto)
        {
            throw new NotImplementedException();
        }

        public SingleLineDiagramDto CreateSingleLineDiagram(SingleLineDiagramDto dto)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var entity = AutoMapper.Mapper.Map<SingleLineDiagram>(dto);

                entity = this.singleDiagramRepository.Add(unitOfWork, entity);

                this.ossRepository.Add(unitOfWork, new Foundation.DataAccess.Entity.OssObject(string.Format(SingleLineDiagramOss, entity.Id), dto.Content));

                unitOfWork.Commit();

                dto.Id = entity.Id;

                return dto;
            }
        }

        public SingleLineDiagramDto[] GetSingleLineDiagramByHierarchyId(long hierarchyId)
        {
            var entities = this.singleDiagramRepository.GetByHierarchyId(hierarchyId);

            var result = new List<SingleLineDiagramDto>();

            foreach (var entity in entities)
            {
                var dto = AutoMapper.Mapper.Map<SingleLineDiagramDto>(entity);

                var ossobject = this.ossRepository.GetById(string.Format(SingleLineDiagramOss, entity.Id));

                dto.Content = ossobject.Content;

                result.Add(dto);
            }

            return result.OrderBy(s => s.Order).ToArray(); 
        }

        public void DeleteSingleLineDiagramById(long id)
        {
            this.singleDiagramRepository.Delete(id);
        }

        public void SaveGatewayHierarchy(string boxId, long timestamp, GatewayHierarchyDto[] gatewayHierarchy)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var gateway = this.gatewayRepository.GetByUniqueId(boxId);

                this.SaveGatewayHierarchyTreeRecursive(gateway.Id, gateway.CustomerId, -1, timestamp, gatewayHierarchy);

                unitOfWork.Commit();                
            }
        }

        private void SaveGatewayHierarchyTreeRecursive(long gatewayId, long customerId, long parentId, long timestamp, GatewayHierarchyDto[] gatewayHierarchy)
        {
            foreach (var item in gatewayHierarchy)
            {
                if (item.Type == GatewayHierarchyType.Parameter)
                {
                    continue;
                }

                var hierarchy = this.hierarchyRepository.GetByCode(customerId, item.Id.ToString());

                if (hierarchy == null)
                {
                    ////create
                    hierarchy = new Hierarchy();
                    hierarchy.ParentId = parentId == -1 ? null : (long?)parentId;
                    hierarchy.Code = item.Id.ToString();
                    hierarchy.Name = item.Name;
                    hierarchy.Type = item.Type == GatewayHierarchyType.Device ? HierarchyType.Device : HierarchyType.Cabinet;
                    hierarchy.UpdateTime = DateTime.Now;
                    hierarchy.TimezoneId = 1;
                    hierarchy.CustomerId = customerId;

                    hierarchy = this.hierarchyRepository.Add(hierarchy);
                }
                else
                {
                    ////update
                    hierarchy.Name = item.Name;
                    hierarchy.ParentId = parentId == -1 ? null : (long?)parentId;
                    hierarchy.Code = item.Id.ToString();
                    hierarchy.Type = item.Type == GatewayHierarchyType.Device ? HierarchyType.Device : HierarchyType.Cabinet;
                    hierarchy.UpdateTime = DateTime.Now;

                    this.hierarchyRepository.Update(hierarchy);
                }

                if (item.Children != null && item.Children.Length > 0)
                {
                    this.SaveGatewayHierarchyTreeRecursive(gatewayId, customerId, hierarchy.Id, timestamp, item.Children);
                }
            }
        }

        private bool DoesGatewayNameExist(GatewayDto gateway, out Gateway entity)
        {
            entity = this.gatewayRepository.GetByName(gateway.Name);

            return entity != null;
        }

        private bool DoesGatewayCustomerExist(GatewayDto gateway, out Customer customer)
        {
            var customerCode = gateway.Name.Split('.').FirstOrDefault();

            customer = this.customerRepository.GetByCode(customerCode);

            if (customer != null)
            {
                return true;
            }

            return false;
        }

        private bool IsGatewayNameValid(GatewayDto gateway)
        {
            if (!string.IsNullOrEmpty(gateway.Name) && gateway.Name.Contains("."))
            {
                var segements = gateway.Name.Split('.');

                if (segements.Length == 2)
                {
                    var customerCode = segements.FirstOrDefault();
                    var boxName = segements.LastOrDefault();

                    if (!string.IsNullOrEmpty(customerCode) && !string.IsNullOrEmpty(boxName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsGatewayMacValid(GatewayDto gateway)
        {
            if (!string.IsNullOrEmpty(gateway.Mac) && gateway.Mac.Length == 12)
            {
                return true;
            }

            return false;
        }
    }
}
