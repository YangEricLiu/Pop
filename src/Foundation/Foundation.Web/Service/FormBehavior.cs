using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.Web;

namespace SE.DSP.Foundation.Web.Service
{
    public class FormBehavior : WebHttpBehavior
    {
        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            //Messages[0] is the request message
            MessagePartDescriptionCollection parts =
                     operationDescription.Messages[0].Body.Parts;
                
            if (operationDescription.Behaviors.Find<ExportContractAttribute>()
                    != null &&
                parts.Count > 0)
            {
                return new FormDataRequestFormatter(operationDescription);
            }
            else
            {

                return base.GetRequestDispatchFormatter(
                          operationDescription, endpoint);
            }
        }
    }


}
