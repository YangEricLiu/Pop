/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: User.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : User entity
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The user which store in the context.
    /// </summary>
    public class User
    {
     
        /// <summary>
        /// User Id.
        /// </summary>d
        public long Id { get; set; }

        /// <summary>
        /// User Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User Name.
        /// </summary>
        //public string RealName { get; set; }

        /// <summary>
        /// Customer Id.
        /// </summary>
        //public long CustomerId { get; set; }
        /// <summary>
        /// UserType
        /// </summary>
        //public long UserType { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        //public long? Version { get; set; }

        /// <summary>
        /// Identity Provider of the Security Token Service
        /// </summary>
        public string IPSTS { get; set; }

        /// <summary>
        /// Identity of Service Provider
        /// </summary>
        public long SPId { get; set; }

        /// <summary>
        /// Identity of Service Provider
        /// </summary>
        public int DemoStatus { get; set; }
    }
}