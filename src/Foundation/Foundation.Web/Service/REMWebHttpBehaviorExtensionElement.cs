using SE.DSP.Foundation.Web.Service;
using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel.Web;

namespace SE.DSP.Foundation.Web
{
    public sealed class REMWebHttpBehaviorExtensionElement : BehaviorExtensionElement
    {
        // Fields
        private ConfigurationPropertyCollection properties;

        // Methods
        protected override object CreateBehavior()
        {
            return new FormBehavior { HelpEnabled = this.HelpEnabled, DefaultBodyStyle = this.DefaultBodyStyle, DefaultOutgoingResponseFormat = this.DefaultOutgoingResponseFormat, AutomaticFormatSelectionEnabled = this.AutomaticFormatSelectionEnabled, FaultExceptionEnabled = this.FaultExceptionEnabled };
        }

        // Properties
        public override Type BehaviorType
        {
            get
            {
                return typeof(FormBehavior);
            }
        }

        [ConfigurationProperty("automaticFormatSelectionEnabled")]
        public bool AutomaticFormatSelectionEnabled
        {
            get
            {
                return (bool)base["automaticFormatSelectionEnabled"];
            }
            set
            {
                base["automaticFormatSelectionEnabled"] = value;
            }
        }

        [ConfigurationProperty("defaultBodyStyle")]
        public WebMessageBodyStyle DefaultBodyStyle
        {
            get
            {
                return (WebMessageBodyStyle)base["defaultBodyStyle"];
            }
            set
            {
                base["defaultBodyStyle"] = value;
            }
        }

        [ConfigurationProperty("defaultOutgoingResponseFormat")]
        public WebMessageFormat DefaultOutgoingResponseFormat
        {
            get
            {
                return (WebMessageFormat)base["defaultOutgoingResponseFormat"];
            }
            set
            {
                base["defaultOutgoingResponseFormat"] = value;
            }
        }

        [ConfigurationProperty("faultExceptionEnabled")]
        public bool FaultExceptionEnabled
        {
            get
            {
                return (bool)base["faultExceptionEnabled"];
            }
            set
            {
                base["faultExceptionEnabled"] = value;
            }
        }

        [ConfigurationProperty("helpEnabled")]
        public bool HelpEnabled
        {
            get
            {
                return (bool)base["helpEnabled"];
            }
            set
            {
                base["helpEnabled"] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                {
                    ConfigurationPropertyCollection propertys = new ConfigurationPropertyCollection();
                    propertys.Add(new ConfigurationProperty("helpEnabled", typeof(bool), false, null, null, ConfigurationPropertyOptions.None));
                    propertys.Add(new ConfigurationProperty("defaultBodyStyle", typeof(WebMessageBodyStyle), WebMessageBodyStyle.Bare, ConfigurationPropertyOptions.None));
                    propertys.Add(new ConfigurationProperty("defaultOutgoingResponseFormat", typeof(WebMessageFormat), WebMessageFormat.Xml, ConfigurationPropertyOptions.None));
                    propertys.Add(new ConfigurationProperty("automaticFormatSelectionEnabled", typeof(bool), false, null, null, ConfigurationPropertyOptions.None));
                    propertys.Add(new ConfigurationProperty("faultExceptionEnabled", typeof(bool), false, null, null, ConfigurationPropertyOptions.None));
                    this.properties = propertys;
                }

                return this.properties;
            }
        }
    }
}