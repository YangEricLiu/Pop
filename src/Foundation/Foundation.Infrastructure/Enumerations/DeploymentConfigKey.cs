using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum DeploymentConfigKey
    {
        OTSEndPoint,
        OTSAccessId,
        OTSAccessKey,

        ProxyHost,
        ProxyPort,
        ProxyUserName,
        ProxyPassword,

        SpDbDatabase,
        SpDbServerIP,
        SpDbUser,
        SpDbPassword,
        SpDbMaxPoolSize,

        AppFabricServerHosts,

        LoginTrialSPs,
        LoginDemoTemplate,
        LoginPlatformDomain,
        LoginValidateDomain,

        CacheVersion,
        SmtpServerIP,

        DIKefDelimiter,
        DICollectorDir,
        DIExtractorDir,
        DIConvertorDir,
        DIImporterDir,
        
        FtpServer,
        FtpDownUser,
        FtpDownPwd,
        FtpDownPath,

        FtpUpDir,
        FtpDownDir, 
        
        CODEREGEX,
        METERCODEREGEX,
        CUSTOMERCODEREGEX,

        WifAudienceUriDomains,
        WifAudienceUriTemplate,
        WifFederationIssuer,
        WifFederationRealm,
    }
}
    