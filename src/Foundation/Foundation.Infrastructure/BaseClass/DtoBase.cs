
using SE.DSP.Foundation.Infrastructure.Validators;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    /// <summary>
    /// The base class for dto. 
    /// </summary>
    [Serializable]
    [DataContract]
    public class DtoBase
    {
        private string _name;

        /// <summary>
        /// Id.
        /// </summary>
        [IdValidator(Ruleset = "update", MessageTemplate = FieldCode.ID)]
        [DataMember]
        public long? Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [NameValidator(Ruleset = "update", MessageTemplate = FieldCode.NAME)]
        [NameValidator(Ruleset = "create", MessageTemplate = FieldCode.NAME)]
        [DataMember]
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
        /// Version.
        /// </summary>
        [VersionValidator(Ruleset = "update", MessageTemplate = FieldCode.VERSION)]
        [DataMember]
        public long? Version { get; set; }

        /// <summary>
        /// Returns a string that represents the current object for log.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder("Id: ").Append(this.Id).Append(", Name: ").Append(this.Name).ToString();
        }

        public virtual object GetCacheKey()
        {
            return this.Id;
        }
    }
}