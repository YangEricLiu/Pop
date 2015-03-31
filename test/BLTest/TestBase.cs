using Rhino.Mocks;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.AppHost.API;
using SE.DSP.Pop.BL.AppHost.Common.Startup;
using SE.DSP.Pop.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Test.BLTest
{
    public class TestsBase : IDisposable
    {
        /// <summary>
        /// mocks repository holder
        /// </summary>
        private readonly static MockRepository mocks = new MockRepository();
        protected IUnitOfWorkProvider unitOfWorkProvider = Mocks.StrictMock<IUnitOfWorkProvider>();
        protected IUnitOfWork unitOfWork = Mocks.StrictMock<IUnitOfWork>();
        protected IHierarchyRepository hierarchyRepository = Mocks.StrictMock<IHierarchyRepository>();
        protected IHierarchyAdministratorRepository hierarchyAdministratorRepository = Mocks.StrictMock<IHierarchyAdministratorRepository>();
        protected IGatewayRepository gatewayRepository = Mocks.StrictMock<IGatewayRepository>();
        protected IBuildingLocationRepository buildingLocationRepositor = Mocks.StrictMock<IBuildingLocationRepository>();
        protected ILogoRepository logoRepository = Mocks.StrictMock<ILogoRepository>();
        protected IOssRepository ossRepository = Mocks.StrictMock<IOssRepository>();


        protected TestsBase()
        {
            var config = new AutoMapperConfiguration();
            config.Configure();
        }

        /// <summary>
        /// Gets the Mock Repository.
        /// </summary>
        protected static MockRepository Mocks
        {
            get
            {
                return mocks;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ////this.mocks.VerifyAll();
            }
        }

        protected IHierarchyService GetHierarchyService()
        {
            return new HierarchyService(this.hierarchyRepository, this.unitOfWorkProvider, this.hierarchyAdministratorRepository, this.gatewayRepository, this.buildingLocationRepositor, this.logoRepository, this.ossRepository);
        }
    }
}
