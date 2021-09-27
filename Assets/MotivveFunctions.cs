using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotivveFunctions : MonoBehaviour
{
    GlobalCommands gc;
    
    public void OnEnable()
    {
        gc = FindObjectOfType<GlobalCommands>();

        gc.SuspendCallback += SuspendGame;
        gc.ResumeCallback += UnSuspendGame;
        gc.ResetCallback += RestartGame;
        gc.MusicStateCallback += GetMusicState;
        gc.SoundStateCallback += GetSoundState;
    }

    private void OnDisable()
    {
        gc.SuspendCallback -= SuspendGame;
        gc.ResumeCallback -= UnSuspendGame;
        gc.ResetCallback -= RestartGame;
        gc.MusicStateCallback -= GetMusicState;
        gc.SoundStateCallback -= GetSoundState;
    }

    void SuspendGame()
    {
        Time.timeScale = 0.0f;
    }

    void UnSuspendGame()
    {
        Time.timeScale = 1.0f;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GetMusicState()
    {
        FlutterMessengerWrapper.Send("GetMusicStateResponse");
    }

    public void GetSoundState()
    {
        FlutterMessengerWrapper.Send("GetSoundStateResponse");
    }
}

//UnityMessageManager.Instance.SendMessageToFlutter("startTrivia");
//UnityMessageManager.Instance.SendMessageToFlutter("showMenuBars");
//UnityMessageManager.Instance.SendMessageToFlutter("hideMenuBars");
