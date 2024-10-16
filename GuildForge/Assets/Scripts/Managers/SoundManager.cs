using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundManager : MonoBehaviour
{
    public GameState gameState;

    public AudioSource bgMusic;
    public AudioSource buttonSound;
    public AudioSource moneySound;
    bool playing = false;

    private void Start()
    {
        if (gameState.music && playing == false && gameState.sound)
        {
            bgMusic.Play();
            bgMusic.playOnAwake = true;
            playing = true;
        }
    }
    void Update()
    {
        if (gameState.music && playing == false && gameState.sound)
        {
            Debug.Log("Music Play");
            bgMusic.Play();
            bgMusic.playOnAwake = true;
            playing = true;
        }
        else if (gameState.music == false || gameState.sound == false)
        {
            bgMusic.Stop();
            bgMusic.playOnAwake = false;
            playing = false;
        }
    }
    public void ButtonSound()
    {
        if(gameState.effects && gameState.sound)
        {
            buttonSound.Play();
        }
    }
    public void MoneySound()
    {
        if (gameState.effects && gameState.sound)
        {
            moneySound.Play();
        }
    }
}
