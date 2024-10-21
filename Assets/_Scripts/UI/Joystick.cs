using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		public Vector2 movementInput = Vector2.zero;
		[SerializeField]
		private float handleRange = 1;
		[SerializeField]
		private float deadZone;
		[SerializeField]
		private RectTransform rectTransform;
		[SerializeField]
		private RectTransform handleRectTransform;
		private Camera cam;
		private Canvas canvas;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
		}

		public void OnDrag(PointerEventData eventData)
		{
			cam = null;

			if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				cam = canvas.worldCamera;
			}

			Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, rectTransform.position);
			Vector2 radius = rectTransform.sizeDelta / 2;
			movementInput = (eventData.position - position) / (radius * canvas.scaleFactor);
			HandleInput(movementInput.magnitude, movementInput.normalized);
			handleRectTransform.anchoredPosition = movementInput * radius * handleRange;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnDrag(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			movementInput = Vector2.zero;
			handleRectTransform.anchoredPosition = Vector2.zero;
		}

		private void HandleInput(float magnitude, Vector2 normalised)
		{
			if (magnitude > deadZone)
			{
				if (magnitude > 1)
				{
					movementInput = normalised;
				}
			}
			else
			{
				movementInput = Vector2.zero;
			}
		}
	}
}
