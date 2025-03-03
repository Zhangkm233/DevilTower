using System.Collections;
using UnityEngine;

public class DialogAnimation : MonoBehaviour
{
    //我没写好，先不要使用这个，删掉也行
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject DialogPanel;

    IEnumerator ShowPanel(GameObject panel) {
        float timer = 0f;
        panel.SetActive(true);
        while (timer < 1f) {
            panel.transform.position.Set(0,-540 * showCurve.Evaluate(timer),0);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    IEnumerator HidePanel(GameObject panel) {
        float timer = 0f;
        while (timer < 1f) {
            panel.transform.position.Set(0,-540 * hideCurve.Evaluate(timer),0);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    public void ShowDialogPanel() {
        StartCoroutine(ShowPanel(DialogPanel));
    }
    public void HideDialogPanel() {
        StartCoroutine(HidePanel(DialogPanel));
    }
}
