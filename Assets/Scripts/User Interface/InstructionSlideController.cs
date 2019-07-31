using UnityEngine;
using TMPro;

public class InstructionSlideController : MonoBehaviour
{
    public static InstructionSlideController sharedInstance;

    [SerializeField] GameObject[] slides = null;

    private int initialSlideIndex = 0;
    private int currentSlideIndex;

    private GameObject currentSlide = null;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        currentSlideIndex = initialSlideIndex;
        currentSlide = slides[currentSlideIndex];
        UpdateSlides();
    }

    private void UpdateSlides() {
        foreach(GameObject slide in slides) {
            if (slide == currentSlide) {
                slide.SetActive(true);
            } else {
                slide.SetActive(false);
            }
        }
    }

    public void NextSlide() {
        currentSlideIndex++;
        if (currentSlideIndex == slides.Length) {
            currentSlideIndex = 0;
        }
        currentSlide = slides[currentSlideIndex];
        UpdateSlides();
    }

    public void PreviousSlide() {
        currentSlideIndex--;
        if (currentSlideIndex < 0) {
            currentSlideIndex = slides.Length - 1;
        }
        currentSlide = slides[currentSlideIndex];
        UpdateSlides();
    }
}
