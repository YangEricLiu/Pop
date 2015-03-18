/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: EntityBase.cs
 * Author	    : Figo
 * Date Created : 2012-02-28
 * Description  : The base class for data entity.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The base class for data entity.
    /// </summary>
    [Serializable]
    public class EntityBase
    {
        private string _name;
        private string _code;
        private string _comment;
        private string _updateUser;

        /// <summary>
        /// Id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        public string Code
        {
            get
            {
                if (string.IsNullOrEmpty(this._code))
                {
                    return this._code;
                }
                else
                {
                    return this._code.Trim();
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._code = value;
                }
                else
                {
                    this._code = value.Trim();
                }
            }
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this._name))
                {
                    return this._name;
                }
                else
                {
                    return this._name.Trim();
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._name = value;
                }
                else
                {
                    this._name = value.Trim();
                }
            }
        }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment
        {
            get
            {
                if (string.IsNullOrEmpty(this._comment))
                {
                    return this._comment;
                }
                else
                {
                    return this._comment.Trim();
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._comment = value;
                }
                else
                {
                    this._comment = value.Trim();
                }
            }
        }

        /// <summary>
        /// Entity Status.
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Update user.
        /// </summary>
        public string UpdateUser
        {
            get
            {
                if (string.IsNullOrEmpty(this._updateUser))
                {
                    return this._updateUser;
                }
                else
                {
                    return this._updateUser.Trim();
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._updateUser = value;
                }
                else
                {
                    this._updateUser = value.Trim();
                }
            }
        }

        /// <summary>
        /// Update time.
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        public long? Version { get; set; }

        /// <summary>
        /// Returns a string that represents the current object for log.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder("Id: ").Append(this.Id).Append(", Code: ").Append(this.Code).Append(", Name: ").Append(this.Name).ToString();
        }

        public virtual object GetCacheKey()
        {
            return this.Id;
        }
    }
}