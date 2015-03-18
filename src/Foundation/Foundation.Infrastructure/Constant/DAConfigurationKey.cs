namespace SE.DSP.Foundation.Infrastructure.Constant
{
    /// <summary>
    /// The keys of DA app setting.
    /// </summary>
    public static class DAConfigurationKey
    {
        /// <summary>
        /// OTS access endpoint.
        /// </summary>
        public const string OTSENDPOINT = "OTSEndPoint";

        /// <summary>
        /// OTS access id.
        /// </summary>
        public const string OTSACCESSID = "OTSAccessId";

        /// <summary>
        /// OTS access key.
        /// </summary>
        public const string OTSACCESSKEY = "OTSAccessKey";

        /// <summary>
        /// The proxy host for access OTS endpoint.
        /// </summary>
        public const string PROXYHOST = "ProxyHost";

        /// <summary>
        /// The api versuion of ots.
        /// </summary>
        public const string OTSAPIVERSION = "OTSApiVersion";

        /// <summary>
        /// The proxy port.
        /// </summary>
        public const string PROXYPORT = "ProxyPort";

        /// <summary>
        /// The proxy user name.
        /// </summary>
        public const string PROXYUSERNAME = "ProxyUserName";

        /// <summary>
        /// The proxy password.
        /// </summary>
        public const string PROXYPASSWORD = "ProxyPassword";

        /// <summary>
        /// The max row count per query.
        /// </summary>
        public const string MAXRETRIEVEROWCOUNT = "MaxRetrieveRowCount";

        /// <summary>
        /// Group Count for energy data aggregate
        /// </summary>
        public const string AGGREGATEGROUPCOUNT = "AggregateGroupCount";

        /// <summary>
        /// The max row count per update.
        /// </summary>
        public const string MAXMODIFYROWCOUNT = "MaxModifyRowCount";

        /// <summary>
        /// The table name of raw energy data in OTS.
        /// </summary>
        public const string ENERGYDATATABLENAME = "EnergyDataTableName";

        /// <summary>
        /// The table name of ptag raw data.
        /// </summary>
        public const string TagRawDataTableName = "TagRawDataTableName";

        /// <summary>
        /// The table name of ptag hour data.
        /// </summary>
        public const string TagDataTableName = "TagDataTableName";

        ///// <summary>
        ///// The table name of target/baseline hourly data.
        ///// </summary>
        //public const string TargetBaselineDataTableName = "TargetBaselineDataTableName";

        /// <summary>
        /// The table name of standard coal data.
        /// </summary>
        public const string StandardCoalDataTableName = "StandardCoalDataTableName";

        /// <summary>
        /// The table name of cost data containing hierarchy data
        /// </summary>
        public const string CostHierarchyDataTableName = "CostHierarchyDataTableName";

        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string CostSystemDimensionDataTableName = "CostSystemDimensionDataTableName";


        /// <summary>
        /// The table name of cost data containing area dimension data
        /// </summary>
        public const string CostAreaDimensionDataTableName = "CostAreaDimensionDataTableName";


        /// <summary>
        /// The table name of ptag hour data.
        /// </summary>
        public const string TBDataTableName = "TBDataTableName";

        /// <summary>
        /// The table name of ptag hour data.
        /// </summary>
        public const string TBUnitDataTableName = "TBUnitDataTableName";

        /// <summary>
        /// The table name of standard coal data.
        /// </summary>
        public const string StandardCoalTBDataTableName = "StandardCoalTBDataTableName";

        /// <summary>
        /// The table name of cost data containing hierarchy data
        /// </summary>
        public const string CostHierarchyTBDataTableName = "CostHierarchyTBDataTableName";

        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string CostSystemDimensionTBDataTableName = "CostSystemDimensionTBDataTableName";


        /// <summary>
        /// The table name of cost data containing area dimension data
        /// </summary>
        public const string CostAreaDimensionTBDataTableName = "CostAreaDimensionTBDataTableName";
        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string EnergyConsumptionSystemDimensionDataTableName = "EnergyConsumptionSystemDimensionDataTableName";

        /// <summary>
        /// The table name of cost data containing hierarchy data
        /// </summary>
        public const string EnergyConsumptionHierarchyDataTableName = "EnergyConsumptionHierarchyDataTableName";

        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string EnergyConsumptionBenchMarkDataTableName = "EnergyConsumptionBenchMarkDataTableName";
        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string CostBenchMarkDataTableName = "CostBenchMarkDataTableName";
        /// <summary>
        /// The table name of cost data containing system dimension data 
        /// </summary>
        public const string StandardCoalBenchMarkDataTableName = "StandardCoalBenchMarkDataTableName";

        public const string LabelingDataTableName = "LabelingDataTableName";

        public const string TagDataModifyLogTableName = "TagDataModifyLogTableName";

        public const string TagAnomalyDataTableName = "TagAnomalyDataTableName";

        /// <summary>
        /// Hierarchy cache name
        /// </summary>
        public const string HierarchyCacheName = "hierarchy";

        ////Jacob add for CMep
        /// <summary>
        /// OTS write retry count
        /// </summary>
        public const string RETRYCOUNT = "RetryCount";

        //Jacob add for CMep
        /// <summary>
        /// OTS write retry count
        /// </summary>
        public const string RETRYWAITTIME = "RetryWaitTime";


        public const string SPID = "SpId";

        public const string TAGLIMITINONEQUERY = "TagLimitInOneQuery";
    }
}