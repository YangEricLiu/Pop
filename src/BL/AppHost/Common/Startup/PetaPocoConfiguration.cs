using PetaPoco;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.Common.Startup
{
    public class PetaPocoConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            Mappers.Register(typeof(Hierarchy), new Pop.MSSQL.Mapper.PopMapper());
            Mappers.Register(typeof(UserCustomer), new Pop.MSSQL.Mapper.PopMapper());
        }
    }
}