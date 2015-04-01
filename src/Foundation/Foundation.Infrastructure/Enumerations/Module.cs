/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Module.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Module enum
 * Copyright    : Schneider Electric (China) Co., Ltd.
 * 
 * Modify by mike 2013-12-29 add Collaborative Service
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Modules, each has 2 numbers.
    /// </summary>
    public enum Module
    {
        /// <summary>
        /// Common module.
        /// </summary>
        Common = 00,

        /// <summary>
        /// Hierarchy module.
        /// </summary>
        Hierarchy = 01,

        /// <summary>
        /// Energy module.
        /// </summary>
        Energy = 02,

        /// <summary>
        /// Administration module.
        /// </summary>
        Administration = 03,


        /// <summary>
        /// System Dimension module
        /// </summary>
        SystemDimension = 04,

        /// <summary>
        /// DashBoard module
        /// </summary>
        Dashboard = 05,

        /// <summary>
        /// Tag module
        /// </summary>
        Tag = 06,
        /// <summary>
        /// AccessControl module
        /// </summary>
        AccessControl = 07,

        /// <summary>
        /// AreaDimension module
        /// </summary>
        AreaDimension = 08,

        /// <summary>
        /// TargetBaseline
        /// </summary>
        TargetBaseline = 09,

        /// <summary>
        /// Cost
        /// </summary>
        Cost = 10,
        /// <summary>
        /// Customer
        /// </summary>
        Customer = 11,
        /// <summary>
        /// Customer
        /// </summary>
        User = 12,

        Alarm = 13,

        ServiceProvider = 14,

        /// <summary>
        /// Collaborative Service added by mike
        /// </summary>
        CollaborativeWidget=15,


        /// <summary>
        /// For blues usage, added by Tan
        /// </summary>
        BuildingCover=16,

        /// <summary>
        /// 
        /// </summary>
        AppKeySecret = 17,
        /// <summary>
        /// 
        /// </summary>
        OpenEnergy = 18,


        /// <summary>
        /// Add by Mike 20140
        /// </summary>
        TagImport=19,


        VEE = 20,

        DataReport = 21,
        
        /// <summary>
        /// Add by Kim 2015
        /// </summary>
        ImpExpHierarchy=22,

        #region Pop modules
        Box = 50,
        #endregion
    }
}
