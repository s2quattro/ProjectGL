using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Kaibrary;
using GLCore;



namespace CityStage
{	
	[DisallowMultipleComponent]
	public partial class InteractableObjectManager : MonoBehaviour
	{
		//SingleTon Parts
		public static InteractableObjectManager instance;

		//refs : Managers		
		private BlockManager blockmanager;

		//refs
		public LocalProperties localProperties;		
		public TopDown2DCharMoving mainPlayerCtrl;
		private Transform interactCenterPos;

		[Header("Options")]
		[Space]

		[Range(0f, 10f)]
		public float interactDistance = 1f;  //ability Distance Player and interactable Object
		[Range(0f, 10f)]
		public float nearInteractDistance = 0.8f;  //For nearby interactable Object
		//[Range(0f, 180f)]
		//public float sightAngleValue = 45f;

		private float _interactDistanceFactor = 1f;
		public float interactDistanceFactor
		{
			set
			{
				if(_interactDistanceFactor != value)
				{   //if Changed
					interactDistance *= value;
					nearInteractDistance *= value;
					_interactDistanceFactor = value;
				}
			}

			get
			{
				return _interactDistanceFactor;
			}
		}


		//List Container
		private List<Transform> interactableObject = new List<Transform>();
		private List<Transform> nearObjects = new List<Transform>();		

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Awake()
		{
			instance = this;			
		}

		void Start()
		{
            localProperties = LinkContainer.Instance.localProperties;
            //Set Managers' ref
            //uiCtrl = UIManager.instance;
            blockmanager = BlockManager.Instance;

			//Set Outter ref
			interactCenterPos = localProperties.interactionCenterPoint;
		}

		//register and discard
		public void registerObject(Transform item)
		{
			interactableObject.Add(item);
			print("Current Object count : " + interactableObject.Count);
		}
		public void discardObject(Transform item)
		{
			interactableObject.Remove(item);
			print("interaction of Object has been removed");
		}

		void FixedUpdate()
		{
			//Import objects in  Activated Blocks
			extractObjectList();

			//Init
			nearObjects.Clear();

			//Whether has object in Activated Blocks
			if (interactableObject.Count > 0)
			{
				checkObjectInSight();

				checkObjectNearBy();

				if (nearObjects.Count > 0)
				{   //object is here
					selectNearestObject();
				}
				else
				{  	//Nope
					nearestTarget = null;
				}				
			}

			//Draw sight (Debug)
			drawSightLine();
		}		

		//Load Object List from Chunk Loader
		private void extractObjectList()
		{
			//Init
			interactableObject.Clear();

			//Force extract List
			for (int i = 0; i < blockmanager.nextNearBlockNumList.Count; i++)
			{				
				List<EntityBase> pickedList = localProperties.blockDic[blockmanager.nextNearBlockNumList[i]];

				for (int j = 0; j < pickedList.Count; j++)
				{
					interactableObject.Add(pickedList[j].transform);					
				}
			}

			//print(string.Format("Total Loaded Object Count : {0} ", interactableObject.Count));
		}

		//Checker : In my sight
		private void checkObjectInSight()
		{

			//Checking part
			foreach (Transform target in interactableObject)
			{
				float eachDistance = Vector3.Distance(interactCenterPos.position, target.position);

				//check around
				if (eachDistance <= interactDistance)
				{
					//target acquire
					Vector2 eachTargetVector = target.position - mainPlayerCtrl.transform.position;

					//is it in sight
					if (Vector2.Dot(eachTargetVector.normalized, mainPlayerCtrl.lookingVector) > QuartersRadian.sightRadian)
					{
						nearObjects.Add(target);						
					}
				}
			}
			
			//print(string.Format( "{0} counted object nearBy", nearObjects.Count));
		}
		//Checker : nearby me
		private void checkObjectNearBy()
		{
			//Checking part
			foreach (Transform target in interactableObject)
			{
				float eachDistance = VectorTreatTools.distance(interactCenterPos, target);

				//check around
				if (eachDistance <= nearInteractDistance)
				{
					nearObjects.Add(target);
				}
			}
		}

		//Selector : Nearest Object at a List
		private void selectNearestObject()
		{
			//Init
			float miniDistance = interactDistance;
			//nearestTarget = null;


			foreach (Transform target in nearObjects)
			{
				float eachDistance = VectorTreatTools.distance(interactCenterPos, target);

				//Whether Interactable Object here
				if (renewed(miniDistance, eachDistance, out miniDistance))
				{
					nearestTarget = target;
					//print(target.gameObject.name + " is HERE!!");
					//canInteractEvent.Invoke(true);  //trigger Event : True
				}
			}
			//if (nearestTarget == null)  //No Object Here				
			//	canInteractEvent.Invoke(false);  //trigger Event : False
		}
		//new nearest Object Detector
		private bool renewed(float oldOne, float newbie, out float minimum)
		{
			if (oldOne >= newbie)
			{
				minimum = newbie;
				return true;  //newbie
			}
			minimum = oldOne;
			return false;  //oldOne
		}				
	}

	//For Interacting (and Include Gizmo Method)
	public partial class InteractableObjectManager : MonoBehaviour
	{
		/*
		//Event : Interactable Object here!
		//public UnityEvent canInteractEvent;
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		[Header("Events")]
		[Space]

		//Event : Interactable Object here!
		public BoolEvent canInteractEvent;
		*/
		
		private Transform _nearestTarget;
		public Transform nearestTarget
		{
			get
			{
				return _nearestTarget;
			}
			set
			{
				//Changing Checker
				if (value != _nearestTarget)
				{   //interactable Object has Changed					
					setInteractionAndHighLighting(value);					
					updateInteractBtn(targetCtrl);
					//print("Changed!!");
				}
					
				_nearestTarget = value;
			}
		}
		private IInteractable targetCtrl;



		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		//Suggest updating a interaction Button
		private void updateInteractBtn(IInteractable target)
		{
			UIManager.Instance.exeChangeInteractBtnState(target);
		}

		private void setInteractionAndHighLighting(Transform target)
		{
			//Is it Interactable Object?
			try
			{
				//Off highlight previous Interaction
				if(targetCtrl != null)
				{
					targetCtrl.isHighLight = false;
				}

				targetCtrl = target.GetComponent<IInteractable>();
			}
			catch(System.NullReferenceException)
			{  //Nope
				targetCtrl = null;
			}
			finally
			{
				//On highlight current Interaction
				if (targetCtrl != null)
				{
					targetCtrl.isHighLight = true;
				}
			}
		}

		//Called by Interact Button On Click Event
		public void exeInteracting()
		{
			if (targetCtrl != null)
				targetCtrl.exeInteract();
		}
		

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
		
		//for Debug
		private void OnDrawGizmos()
		{
			/*
			if(nearestTarget != null)
				Gizmos.DrawLine(mainPlayerLocation.position, nearestTarget.position);*/
			if (interactCenterPos != null)
			{
				Gizmos.DrawWireSphere(interactCenterPos.position, nearInteractDistance);
				Gizmos.DrawWireSphere(interactCenterPos.position, interactDistance);
			}
				
			//Gizmos.draw
		}

		private void drawSightLine()
        {
            Debug.DrawLine(interactCenterPos.position, (Vector2)interactCenterPos.position + mainPlayerCtrl.lookingVector * interactDistance, Color.yellow);
            Debug.DrawLine(interactCenterPos.position, (Vector2)interactCenterPos.position + VectorTreatTools.rotate(mainPlayerCtrl.lookingVector, 65f) * interactDistance, Color.blue);
			Debug.DrawLine(interactCenterPos.position, (Vector2)interactCenterPos.position + VectorTreatTools.rotate(mainPlayerCtrl.lookingVector, -65f) * interactDistance, Color.red);
		}

		/*
		//SingleTon Parts
		public static InteractableObjectManagerParts instance;

		//Main Player Location
		public Transform playerLocation;		
		
		[Header("Option")]
		[Range(0f, 10f)]
		public float interactDistance = 1f;  //ability Distance between Player and interactable Object


		//Event : Interactable Object here!
		//public UnityEvent canInteractEvent;
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		[Header("Events")]
		[Space]

		//Event : Interactable Object here!
		public BoolEvent canInteractEvent;

		//Inner Field
		//■ List Container
		private List<GameObject> interactableObject;
		private List<Transform> nearObjects;
		private List<Transform> nearObjectsTemp;

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

		// Use this for initialization
		void Awake()
		{
			instance = this;
			interactableObject = new List<GameObject>();
			nearObjects = new List<Transform>();
			nearObjectsTemp = new List<Transform>();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			//Whether has object
			if (interactableObject.Count > 0)
				checkNearestObject();
		}

		private void checkNearestObject()
		{
			//Init
			float miniDistance = interactDistance;
			nearObjectsTemp.Clear();

			//checking List
			foreach (GameObject target in interactableObject)
			{
				float eachDistance = VectorTreatTools.distance(playerLocation, target.transform);

				//Whether Interactable Object here
				if (renewed(miniDistance, eachDistance))
				{
					nearObjectsTemp.Add(target.transform);
					//print(target.gameObject.name + " is HERE!!");
					canInteractEvent.Invoke(true);  //trigger Event : True
				}
			}

			//Apply part
			//if(nearObjectsTemp.)
			
			
			//if (nearestTarget == null)  //No Object Here				
			//	canInteractEvent.Invoke(false);  //trigger Event : False
			
		}

		//new nearest Object Detector
		private bool renewed(float oldOne, float newbie)
		{
			if (oldOne >= newbie)
			{
				return true;  //newbie
			}
			return false;  //oldOne
		}

		*/
	}
}
