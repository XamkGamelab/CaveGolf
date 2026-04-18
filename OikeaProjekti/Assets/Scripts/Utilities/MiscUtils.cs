using UnityEngine;

namespace Utils
{
    public delegate void Callback(string message);
    public delegate void ErrorCallback(System.Exception e);
    public static class DbUtils
    {
        public static string EmailToUsername(string email)
        {
            return email.Substring(0, email.IndexOf("@"));
        }
    }
}