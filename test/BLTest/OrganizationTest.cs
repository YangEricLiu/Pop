using Rhino.Mocks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.AppHost.API;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SE.DSP.Pop.Test.BLTest
{
    public class OrganizationTest : TestsBase
    {
        [Fact]
        public void AddOrganizationTest()
        {
            var dto = new OrganizationDto
            {
                Name = "Test",
                Administrators = new HierarchyAdministratorDto[]
                 {
                     new HierarchyAdministratorDto
                     {
                          Email = "test1@test.com",
                          Name = "test1",
                          Telephone = "13888888888",
                          Title ="boss"
                     },

                      new HierarchyAdministratorDto
                     {
                          Email = "test2@test.com",
                          Name = "test2",
                          Telephone = "13888888888",
                          Title ="boss"
                     }
                 },
                Gateways = new GatewayDto[] {
                     new GatewayDto{
                          CustomerId = 2,
                          Id = 1,
                          Mac = "10.20.20.10",
                          Name ="gateway1",
                          RegisterTime = new DateTime(2015, 3,1),
                          UniqueId = "123456789"
                     },
                      
                    new GatewayDto{
                        CustomerId = 2,
                        Id = 2,
                        Mac = "10.20.20.20",
                        Name ="gateway2",
                        RegisterTime = new DateTime(2015, 3,1),
                        UniqueId = "123456788"
                    }
                 }
            };

            var hierarchy = new Hierarchy
            {
                Id = 10,
                Name = dto.Name
            };

            var hierarchyAdministrators = new HierarchyAdministrator[]
             {
                 new HierarchyAdministrator
                 {
                      Email = dto.Administrators[0].Email,
                      Name = dto.Administrators[0].Name,
                      Telephone = dto.Administrators[0].Telephone,
                      Title= dto.Administrators[0].Title,
                      HierarchyId = hierarchy.Id,
                      Id = 1
                 },
                 new HierarchyAdministrator
                 {
                      Email = dto.Administrators[1].Email,
                      Name = dto.Administrators[1].Name,
                      Telephone = dto.Administrators[1].Telephone,
                      Title= dto.Administrators[1].Title,
                      HierarchyId = hierarchy.Id,
                      Id = 2
                 }
             };

              var gateways = new Gateway[]
             {
                 new Gateway
                 {
                      CustomerId = dto.Gateways[0].CustomerId,
                        Id = dto.Gateways[0].Id,
                        Mac =dto.Gateways[0].Mac,
                        Name =dto.Gateways[0].Name,
                        RegisterTime = dto.Gateways[0].RegisterTime,
                        UniqueId = dto.Gateways[0].UniqueId,
                        HierarchyId = hierarchy.Id
                 },
                 new Gateway
                 {
                      
                       CustomerId = dto.Gateways[1].CustomerId,
                        Id = dto.Gateways[1].Id,
                        Mac =dto.Gateways[1].Mac,
                        Name =dto.Gateways[1].Name,
                        RegisterTime = dto.Gateways[1].RegisterTime,
                        UniqueId = dto.Gateways[1].UniqueId,
                        HierarchyId = hierarchy.Id
                 }
             };

            Expect.Call(unitOfWorkProvider.GetUnitOfWork()).Return(unitOfWork);
            Expect.Call(hierarchyRepository.Add(unitOfWork, null)).IgnoreArguments().Return(hierarchy);
            Expect.Call(hierarchyAdministratorRepository.AddMany(unitOfWork, null)).IgnoreArguments().Return(hierarchyAdministrators);
            Expect.Call(() => gatewayRepository.UpdateMany(unitOfWork, null)).IgnoreArguments();


            Expect.Call(unitOfWork.Commit);
            Expect.Call(unitOfWork.Dispose);

            Mocks.ReplayAll();


            var hierarchyService = this.GetHierarchyService();

            var result = hierarchyService.CreateOrganization(dto);

            Assert.Equal(result.HierarchyId, hierarchy.Id);
            Assert.Equal(result.Administrators[0].HierarchyId, hierarchy.Id);
            Assert.Equal(result.Administrators[1].HierarchyId, hierarchy.Id);
            Assert.Equal(result.Gateways[0].HierarchyId, hierarchy.Id);
            Assert.Equal(result.Gateways[1].HierarchyId, hierarchy.Id);
        }
    }
}
 

