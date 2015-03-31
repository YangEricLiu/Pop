using System;
using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.API.ErrorCode;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using SE.DSP.Pop.Entity.Enumeration;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class PopClientService : ServiceBase, IPopClientService
    {
        private readonly IGatewayRepository gatewayRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IHierarchyRepository hierarchyRepository;
        
        public PopClientService()
        {
            this.gatewayRepository = IocHelper.Container.Resolve<IGatewayRepository>();
            this.customerRepository = IocHelper.Container.Resolve<ICustomerRepository>();
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
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

            Hierarchy customer = null;

            if (!this.DoesGatewayCustomerExist(gateway, out customer))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayCustomerNotExist);
            }

            Gateway entity = null;

            if (this.DoesGatewayNameExist(gateway, out entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayAlreadyExist);
            }
            
            Guid guid = Guid.NewGuid();
            string uniqueId = Convert.ToBase64String(guid.ToByteArray());

            gateway.UniqueId = uniqueId;
            gateway.CustomerId = customer.CustomerId;
            
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

            Hierarchy customer = null;

            if (!this.DoesGatewayCustomerExist(gateway, out customer))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayCustomerNotExist);
            }

            Gateway entity = null;

            if (!this.DoesGatewayNameExist(gateway, out entity))
            {
                throw new BusinessLogicException(Layer.BL, Module.Box, GatewayError.GatewayNotExist);
            }

            return AutoMapper.Mapper.Map<GatewayDto>(entity);
        }

        private bool DoesGatewayNameExist(GatewayDto gateway, out Gateway entity)
        {
            entity = this.gatewayRepository.GetByName(gateway.Name);
            if (entity != null)
            {
                return true;
            }

            return false;
        }

        private bool DoesGatewayCustomerExist(GatewayDto gateway, out Hierarchy customer)
        {
            var customerCode = gateway.Name.Split('.').FirstOrDefault();

            customer = this.hierarchyRepository.GetByCode(customerCode);

            if (customer != null && customer.Type == HierarchyType.Customer)
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
                return false;
            }

            return false;
        }
    }
}
