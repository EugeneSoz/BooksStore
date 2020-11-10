using System;

namespace BooksStore.Common
{
    public static class ControllerHelper
    {
        public static string GetName(string controllerClassName)
        {
            var index = controllerClassName.IndexOf("Controller", StringComparison.Ordinal);

            return controllerClassName.Substring(0, index);
        }
    }
}
