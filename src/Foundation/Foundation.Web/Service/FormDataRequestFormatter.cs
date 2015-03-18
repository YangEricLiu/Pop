using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace SE.DSP.Foundation.Web.Service
{
    public class FormDataRequestFormatter : IDispatchMessageFormatter
    {
        OperationDescription operation;

        public FormDataRequestFormatter(OperationDescription operation)
        {
            if (operation.BeginMethod != null || operation.EndMethod != null)
                throw new InvalidOperationException("The async programming model is not supported by this formatter.");

            this.operation = operation;
        }

        public void DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            if (WebOperationContext.Current.IncomingRequest.ContentType != "application/x-www-form-urlencoded")
                throw new InvalidDataException("Unexpected content type");

            Stream s = GetStream(message);
            string formData = new StreamReader(s).ReadToEnd();
            NameValueCollection parsedForm = System.Web.HttpUtility.ParseQueryString(formData);

            ParameterInfo[] paramInfos = operation.SyncMethod.GetParameters();
            var binder = CreateParameterBinder(parsedForm);
            object[] values = (from p in paramInfos select binder(p)).ToArray<Object>();

            values.CopyTo(parameters, 0);
        }

        private Func<ParameterInfo, object> CreateParameterBinder(NameValueCollection parsedForm)
        {
            JsonQueryStringConverter converter = new JsonQueryStringConverter();
            return delegate(ParameterInfo pi)
            {
                string value = parsedForm[pi.Name];
                if (converter.CanConvert(pi.ParameterType) && value != null)
                    return converter.ConvertStringToValue(value, pi.ParameterType);
                else
                    return value;
            };
        }


        private Stream GetStream(Message message)
        {
            XmlDictionaryReader reader = message.GetReaderAtBodyContents();
            return new XmlReaderStream(reader);
        }

        internal class XmlReaderStream : Stream
        {
            XmlDictionaryReader innerReader;

            internal XmlReaderStream(XmlDictionaryReader xmlReader)
            {
                this.innerReader = xmlReader;
                this.innerReader.ReadStartElement("Binary");
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override long Length
            {
                get { throw new NotSupportedException(); }
            }

            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }

            public override void Flush()
            {
                //noop
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return this.innerReader.ReadContentAsBase64(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }



        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            throw new NotImplementedException();
        }
    }
}
