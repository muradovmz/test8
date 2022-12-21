using {{cookiecutter.ProjectName}}.Framework.Shared.Util;

namespace {{cookiecutter.ProjectName}}.Framework.Shared.Extension
{
    public static class ObjectExtensions
    {
        public static object EnsureNotNull<TException>(this object value, string message = "",
            System.Exception exception = null) where TException : System.Exception
        {
            EnsureUtility.EnsureThat<TException>(value != null, message, exception);
            return value;
        }
    }
}