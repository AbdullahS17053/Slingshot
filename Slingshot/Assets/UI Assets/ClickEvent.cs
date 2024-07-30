using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickEvent : MonoBehaviour,IPointerDownHandler
{
    public Image _img;
    public Sprite _pressed;
    public AudioSource _audio;
    public AudioClip _clip;
    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _audio.PlayOneShot(_clip);
        SceneManager.LoadScene("Demo_Scene");
    }
}
