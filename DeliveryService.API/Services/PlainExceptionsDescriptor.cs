using System;
using System.Text;

namespace DeliveryService.API.Services
{
    internal class PlainExceptionsDescriptor
    {
        private static StringBuilder _builder;
        private StringBuilder _builderInstance;

        private PlainExceptionsDescriptor()
        {
            _builderInstance = new();
        }

        internal static string Descript(Exception exception)
        {
            _builder ??= new StringBuilder();
            _builder.AppendLine(exception.Message);

            while (exception.InnerException != null)
            {
                _builder.AppendLine(exception.InnerException.Message);
                exception = exception.InnerException;
            }

            string result = _builder.ToString();
            _builder.Clear();
            return result;
        }

        internal void AppendMessage(string message) => _builderInstance.AppendLine(message);

        internal string Build()
        {
            string result = _builderInstance.ToString();
            _builderInstance.Clear();
            return result;
        }

        internal static PlainExceptionsDescriptor PopulateSingleInstance()
        {
            PlainExceptionsDescriptor descriptor = new();
            return descriptor;
        }
    }
}
