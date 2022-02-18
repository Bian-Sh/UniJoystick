using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Rigidbody cube;
    public Transform character;
    public bool IsInView(Transform target)
    {
        Vector3 dir = (target.position - character.position).normalized;
        float dot = Vector3.Dot(character.forward, dir);//判断物体是否在相机前面
        Vector2 viewPos = Camera.main.WorldToViewportPoint(target.position);
        return dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (IsInView(cube.transform))
        {
            cube.isKinematic= true;
            cube.transform.SetParent(character);
            cube.transform.localScale *= 1.2f;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        cube.transform.SetParent(null);
        cube.transform.localScale = Vector3.one;
        cube.isKinematic= false;
    }
}
