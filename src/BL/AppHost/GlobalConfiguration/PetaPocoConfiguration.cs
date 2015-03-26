using PetaPoco;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.GlobalConfiguration
{
    public class PetaPocoConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            Mappers.Register(typeof(Hierarchy), new Pop.MSSQL.Mapper.PopMapper());
        }
    }
}