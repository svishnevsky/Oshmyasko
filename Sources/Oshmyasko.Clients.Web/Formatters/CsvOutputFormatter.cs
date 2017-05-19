using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Formatters
{
    public class CsvOutputFormatter : OutputFormatter
    {
        public string ContentType { get; private set; }

        public CsvOutputFormatter()
        {
            ContentType = "text/csv";
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
        }

        protected override bool CanWriteType(Type type)
        {

            if (type == null)
                throw new ArgumentNullException("type");

            return isTypeOfIEnumerable(type);
        }

        private bool isTypeOfIEnumerable(Type type)
        {

            foreach (Type interfaceType in type.GetInterfaces())
            {

                if (interfaceType == typeof(IList))
                    return true;
            }

            return false;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "отчет.csv",
                Inline = false 
            };
            response.Headers.Add("Content-Disposition", cd.ToString());

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            StringWriter _stringWriter = new StringWriter();

            _stringWriter.WriteLine(
                string.Join<string>(
                    ",", itemType.GetProperties().Select(x => GetName(x))
                )
            );


            foreach (var obj in (IEnumerable<object>)context.Object)
            {

                var vals = obj.GetType().GetProperties().Select(
                    pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    }
                );

                string _valueLine = string.Empty;

                foreach (var val in vals)
                {

                    if (val.Value != null)
                    {

                        var _val = val.Value.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (_val.Contains(","))
                            _val = string.Concat("\"", _val, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        _valueLine = string.Concat(_valueLine, _val, ",");

                    }
                    else
                    {

                        _valueLine = string.Concat(_valueLine, string.Empty, ",");
                    }
                }

                _stringWriter.WriteLine(_valueLine.TrimEnd(new[] { ',' }));
            }

            var streamWriter = new StreamWriter(response.Body);
            await streamWriter.WriteAsync(_stringWriter.ToString());
            await streamWriter.FlushAsync();
        }

        private string GetName(PropertyInfo p)
        {
            var attr = p.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(DisplayAttribute));
            if (attr == null)
            {
                return p.Name;
            }

            return ((DisplayAttribute)attr).Name;
        }
    }
}
