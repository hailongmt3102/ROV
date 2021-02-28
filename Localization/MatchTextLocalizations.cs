using UnityEngine;
public class MatchTextLocalizations : MonoBehaviour
{
    public TextToTranslation[] textToTranslations;

    void Start()
    {
        foreach (TextToTranslation item in textToTranslations) {
            item.textObject.text = Lean.Localization.LeanLocalization.GetTranslationText(item.Translation);
        }
    }
}
