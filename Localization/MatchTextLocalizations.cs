using UnityEngine;
public class MatchTextLocalizations : MonoBehaviour
{
    public TextToTranslation[] textToTranslations;

    void Start()
    {
        foreach (TextToTranslation item in textToTranslations) {
            if (Lean.Localization.LeanLocalization.GetTranslation(item.Translation) == null)
            {
                Debug.LogError("can't find translation for " + item.textObject.transform.parent.name + " object");
            }
            else
            {
                item.textObject.text = Lean.Localization.LeanLocalization.GetTranslationText(item.Translation);
            }
        }
    }
}
