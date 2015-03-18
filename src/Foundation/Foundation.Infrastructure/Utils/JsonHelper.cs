/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: JsonHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for Json
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Helpers;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The JSON utility class.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serialize one object whose type is T with DataContractJsonSerializer to string.
        /// </summary>
        /// <typeparam name="T">The type of the object which need be Serialized.</typeparam>
        /// <param name="source">The object which need be Serialized.</param>
        /// <returns>The serialization result string.</returns>
        public static string Serialize2String<T>(T source)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, source);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Serialize one object whose type is T with DataContractJsonSerializer to bytes array.
        /// </summary>
        /// <typeparam name="T">The type of the object which need be Serialized.</typeparam>
        /// <param name="source">The object which need be Serialized.</param>
        /// <returns>The serialization result bytes array.</returns>
        public static byte[] Serialize2Bytes<T>(T source)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, source);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserialize specified string to a object whose type is T.
        /// </summary>
        /// <typeparam name="T">The type of deserialize target.</typeparam>
        /// <param name="source">The string which need be deserialized.</param>
        /// <returns>The deserialize target object whose type is T.</returns>
        public static T Deserialize<T>(string source)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Deserialize specified bytes array to a object whose type is T.
        /// </summary>
        /// <typeparam name="T">The type of deserialize target.</typeparam>
        /// <param name="source">The bytes array which need be deserialized.</param>
        /// <returns>The deserialize target object whose type is T.</returns>
        public static T Deserialize<T>(byte[] source)
        {
            DataContractJsonSerializer deserialize = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream(source))
            {
                return (T)deserialize.ReadObject(stream);
            }
        }

        /// <summary>
        /// Deserialize specified JSON string to a dynamic object
        /// </summary>
        /// <param name="source">The JSON string which need be deserialized.</param>
        /// <returns>The deserialize target dynamic object.</returns>
        /// <remarks>This method not decode nesting.</remarks>
        public static dynamic Deserialize(string source)
        {
            return Json.Decode(source);
        }
    }
}