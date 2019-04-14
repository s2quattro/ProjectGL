


namespace CityStage
{
	public class TileEntityBehavior : EntityBase
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
