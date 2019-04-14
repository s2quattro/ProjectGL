using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;




namespace CityStage
{
	[DisallowMultipleComponent]
	public class EntityManager : MonoSingletonBase<EntityManager>
	{
		//Inner Field
		List<EntityBase> entityList = new List<EntityBase>();

		//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        
		public void SetUp()
		{
            BlockManager.Instance.SetObjectParent(entityList);

            print(string.Format("Registered Count : {0}", entityList.Count));
		}

        public List<EntityBase> GetEntityList()
        {
            return entityList;
        }

		//register and discard
		public void exeRegisterEntity(EntityBase regiObject)
		{
			entityList.Add(regiObject);
		}
		public bool exeCancelEntity(EntityBase targetObject)
		{
			if(entityList.Contains(targetObject))
			{
				entityList.Remove(targetObject);
				return true;
			}
			else
				return false;
		}

	}
}