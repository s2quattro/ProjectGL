using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CityStage
{
	public class AdaptedJoystick : MainInterfaceUI, IDragHandler, IPointerUpHandler, IPointerDownHandler
	{
		[Header("Options")]
		[Range(0f, 2f)] public float handleLimit = 1f;
		public JoystickMode joystickMode = JoystickMode.AllAxis;

		[HideInInspector]
		public Vector2 inputVector = Vector2.zero;

		[Header("Components")]
		public RectTransform background;
		public RectTransform handle;

		public float Horizontal { get { return inputVector.x; } }
		public float Vertical { get { return inputVector.y; } }
		public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }


		Vector2 joystickPosition = Vector2.zero;
		private Camera cam = new Camera();

        private Vector2 ratio;

        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        void Start()
		{
			register();

            ratio = new Vector2(GetComponentInParent<CanvasScaler>().referenceResolution.x / Screen.width,
            GetComponentInParent<CanvasScaler>().referenceResolution.y / Screen.height);

            joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position) * ratio;

            //print(string.Format("Radius : {0}, LocPos : {1}, basePos : {2}", background.sizeDelta.x, joystickPosition, background.position));
		}

		public void OnDrag(PointerEventData eventData)
		{
			float joystickRadius = background.sizeDelta.x / 2f;           
                        
            Vector2 direction = eventData.position* ratio - joystickPosition;

			inputVector = (direction.magnitude > joystickRadius) ? direction.normalized : direction / joystickRadius;
			clampJoystickInputValue();
			handle.anchoredPosition = (inputVector * joystickRadius) * handleLimit;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnDrag(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{   // Zero Parameter
			inputVector = Vector2.zero;
			handle.anchoredPosition = Vector2.zero;
		}

		private void clampJoystickInputValue()
		{
			if (joystickMode == JoystickMode.Horizontal)
				inputVector = new Vector2(inputVector.x, 0f);
			if (joystickMode == JoystickMode.Vertical)
				inputVector = new Vector2(0f, inputVector.y);
		}
	}
}
