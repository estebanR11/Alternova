using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite pickedSprite;
    [SerializeField] Sprite correctSprite;

    public UnityEvent OnPicked;

    bool isPicked = false;

    Image mainImage;
    [SerializeField] GameObject particles;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clickSound;

    public int Number
    {
        get { return number; }
    }

    private void Start()
    {
        OnPicked.AddListener(OnClicked);
        mainImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnPicked.Invoke();
        });

    }

    public void StartGame()
    {
        mainImage.sprite = defaultSprite;
    }

    public void Setup(int pNumber)
    {
        number = pNumber;
        numberText.text = number.ToString();
        numberText.gameObject.SetActive(false);
    }

    public void OnClicked()
    {
        if(!isPicked && GameManager.Instance.status == GameStatus.Playing)
        {
            GameManager.Instance.OnBlockSelected(this);
            StartCoroutine(RevealAnimation());
            isPicked = true;
            audioSource.clip = clickSound;
            audioSource.Play();
        }
      
    }

    IEnumerator RevealAnimation()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        mainImage.sprite = pickedSprite;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            if (t >= 0.5f)
            {
                numberText.gameObject.SetActive(true);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }


    public void HideBlock()
    {
        StartCoroutine(HideAnimation());
    }

    IEnumerator HideAnimation()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(-1f, 1f, 1f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            if (t >= 0.5f)
            {
                numberText.gameObject.SetActive(false);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        mainImage.sprite = defaultSprite;
        isPicked = false;
    }

    public void OnCorrectSelection()
    {
        mainImage.sprite = correctSprite;
        particles.SetActive(true);
    }
}
