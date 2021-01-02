using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider effectsSlider;
    public Slider musicSlider;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();

        effectsSlider.value = PlayerPrefs.GetFloat("EffectsSound", effectsSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat("MusicSound", musicSlider.value);

        //EffectsSound(effectsSlider.value);
        //MusicVolume(musicSlider.value);
    }

    public void EffectsSound(float volume)
    {
        effectsSlider.value = volume;
        soundManager.effectsSource.volume = effectsSlider.value;

        PlayerPrefs.SetFloat("EffectsSound", effectsSlider.value);
    }

    public void MusicVolume(float volume)
    {
        musicSlider.value = volume;
        soundManager.musicSource.volume = musicSlider.value;

        PlayerPrefs.SetFloat("MusicSound", musicSlider.value);
    }
}
