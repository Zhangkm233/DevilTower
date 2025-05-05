using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonAnimManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverMoveAmount = 180f; // ����ʱX���ƶ���
    [SerializeField] private float animationSpeed = 10f; // �����ٶ�

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private void Start() {
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    private void Update() {
        // ƽ�����ɵ�Ŀ��λ��
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * animationSpeed
        );
    }

    public void resetPos() {
        targetPosition = originalPosition;
        transform.localPosition = targetPosition;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Hover");
        targetPosition = originalPosition + new Vector3(-hoverMoveAmount,0,0);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Exit");
        targetPosition = originalPosition;
    }
}
