using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate gameobjects
        else if (instance != null && instance != this)
            Destroy(gameObject);

        // assign initial volumes
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        changeSourceVolume(1, "soundVolume", _change, soundSource);
    }
        public void ChangeMusicVolume(float _change)
    {
        changeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }
        /* get base volume
        float baseVolume = 1;

        // get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat("soundVolume",1);  // load last saved sound from player prefs
        currentVolume += _change;

        // check if we reached the maximum or  minimum value
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        // Assign final value 
        float finalVolume = currentVolume * baseVolume;
        soundSource.volume = finalVolume;

        // save final value to player prefs
        PlayerPrefs.SetFloat("soundVolume", currentVolume);*/
    
    private void changeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        // get initial value of volume and change it 
        float currentVolume = PlayerPrefs.GetFloat("soundVolume", 1);
        currentVolume += change;

        // check if we reached the maximum or  minimum value
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        // Assign final value 
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        // save final value to player prefs
        PlayerPrefs.SetFloat("volumeName", currentVolume);
    }
    /*  public void ChangeMusicVolume(float _change)
      {
          changeSourceVolume(0.3f, "musicVolume", _change, musicSource);*/

    /* get base volume
    float baseVolume = 0.3f;

    // get initial value of volume and change it
    float currentVolume = PlayerPrefs.GetFloat("musicVolume");  // load last saved sound from player prefs
    currentVolume += _change;

    // check if we reached the maximum or  minimum value
    if (currentVolume > 1)
        currentVolume = 0;
    else if (currentVolume < 0)
        currentVolume = 1;
    // Assign final value 
    float finalVolume = currentVolume * baseVolume;
    musicSource.volume = finalVolume;

    // save final value to player prefs
    PlayerPrefs.SetFloat("musicVolume", currentVolume);*/





}

