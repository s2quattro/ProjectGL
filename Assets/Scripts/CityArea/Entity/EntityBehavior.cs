


namespace CityStage
{
	public class EntityBehavior : EntityBase
	{
		protected override void register()
		{
			EntityManager.Instance.exeRegisterEntity(this);
		}

		public override void exeSpeakName()
		{
			print(gameObject.name);
		}
	}
}