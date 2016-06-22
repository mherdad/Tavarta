using System;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Tavarta.Common.Fabrik.ActionResults
{
    //تمام حقوق مطعلق به سید علیرضا حسینی هستُ

    /// <summary>
    
        /// Action result that serializes the specified object into XML and outputs it to the response stream.
    /// </summary>
    public class XmlResult : ActionResult
    {
        public object Data { get; private set; }
        public string ContentType { get; set; }
        public Encoding Encoding { get; set; }

        public XmlResult(object objectToSerialize)
        {
            if (objectToSerialize == null) throw new ArgumentNullException(nameof(objectToSerialize));


            Data = objectToSerialize;
            ContentType = "text/xml";
            Encoding = Encoding.UTF8;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = ContentType;
            response.HeaderEncoding = Encoding;

            var serializer = new XmlSerializer(Data.GetType());
            serializer.Serialize(response.Output, Data);
        }
    }
}
