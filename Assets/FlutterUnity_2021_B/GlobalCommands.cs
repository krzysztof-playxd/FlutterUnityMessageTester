using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalCommands : MonoBehaviour
{
    static bool paused = false;
    public UnityAction SuspendCallback = null;
    public UnityAction ResumeCallback = null;
    public UnityAction ResetCallback = null;
    public UnityAction MusicStateCallback = null;
    public UnityAction SoundStateCallback = null;

    public bool Paused
    {
        get { return paused; }
    }

    public void Suspend(string suspend)
    {
        if (bool.Parse(suspend))
        {
            //Time.timeScale = 0.0f;
            SuspendCallback?.Invoke();
            paused = true;
        }
        else
        {
            ResumeCallback?.Invoke();
            //Time.timeScale = 1.0f;
            paused = false;
        }
    }
    
    public void Reset(string milliseconds = "0.0f")
    {
        StartCoroutine(ExecuteAfterTime(float.Parse(milliseconds)));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time/1000.0f);
        //load scene here
        ResetCallback?.Invoke();
    }

    public void MuteGame(string mute)
    {
        if (bool.Parse(mute))
        {
            AudioListener.volume = 0;
            Debug.Log("Muted game");
        }
        else
        {
            AudioListener.volume = 1;
            Debug.Log("Unmuted game");;
        }
    }

    public void GetMusicState()
    {
        MusicStateCallback?.Invoke();
    }

    public void GetSoundState()
    {
        SoundStateCallback?.Invoke();
    }
}
/*examples  
 UnityMessageManager.Instance.SendMessageToFlutter("hideMenuBars");
 UnityMessageManager.Instance.SendMessageToFlutter("showMenuBars");
 UnityMessageManager.Instance.SendMessageToFlutter("startTrivia"); 
 UnityMessageManager.Instance.SendMessageToFlutter("musicMuted");
 UnityMessageManager.Instance.SendMessageToFlutter("musicUnmuted");
 UnityMessageManager.Instance.SendMessageToFlutter("soundMuted");
 UnityMessageManager.Instance.SendMessageToFlutter("soundUnmuted");
 */
