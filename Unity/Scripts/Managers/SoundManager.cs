using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public bool markHere = false;

    private static SoundManager _instance;

    public AudioSource audioSource, inGameAudioSource;
    public AudioClip[] clips;

    public bool isLoop = true;
    public float mainVolume = 0.2f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance == this) Destroy(gameObject);

        if (inGameAudioSource == null) inGameAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        string sceneNo = scene.name.Substring(scene.name.IndexOf('(') + 1, scene.name.IndexOf(')') - 1);

        if (sceneNo == "2")
        {
            this.audioSource.Stop();
            return;
        }



        foreach (AudioClip clip in clips)
        {
            if (clip.name.Substring(clip.name.LastIndexOf('_') + 1) == sceneNo)
            {
                PlayAudio(clip);
            }
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        this.audioSource.clip = clip;
        this.audioSource.loop = this.isLoop;
        this.audioSource.volume = this.mainVolume * GameManager.masterVolume * GameManager.bgmVolume;

        this.audioSource.Play();
    }
}
