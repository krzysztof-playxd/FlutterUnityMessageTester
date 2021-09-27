using System;
using Newtonsoft.Json.Linq;

public class UnityMessage
{
    public String name;
    public JObject data;
    public Action<object> callBack;
}