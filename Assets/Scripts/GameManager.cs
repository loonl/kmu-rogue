using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("StageScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] string parameterName = "";

    public void OnValueChanged()
    {
        audioMixer.SetFloat(parameterName,
        (volumeSlider.value <= volumeSlider.minValue) ? -80f : volumeSlider.value);
    }
}
