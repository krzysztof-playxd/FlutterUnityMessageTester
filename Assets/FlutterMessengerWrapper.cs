namespace Assets
{
    /// <summary>
    /// This will allow switching on / off all the interactions with the UnityMessageManager as it will cause errors if the build is not running in Flutter
    /// All the calls to UnityMessageManager are redirected here and can be simply enabled / disabled.
    /// </summary>
    public static class FlutterMessengerWrapper
    {
        private const bool IsEnabled = true;

        public static void Send(string message)
        {
            if(!IsEnabled) return;
            
            UnityMessageManager.Instance.SendMessageToFlutter(message);
        }
    }
}
