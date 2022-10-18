namespace FeatureA
{
	/// <summary>
	/// The object/type that is access by parsing the dll.txt file from the main executable
	/// This cannot be a monobehavior as we have to create an instance of this type by 
	/// calling Activator.CreateInstance() in the LoadFeature() function <see cref="HitTrax.Assets.HitTraxRevamp.MainExecutableUI"></see> 
	/// </summary>
	public class FeatureLoader
	{
		public FeatureLoader()
		{
		}

		/// <summary>
		/// Launch the feature/module 
		/// </summary>
		public void Load()
		{
			FeatureARuntimeManager.Initialize();
		}

		/// <summary>
		/// Clean up modules as we close
		/// </summary>
		public void Unload()
		{
			FeatureARuntimeManager.Close();
		}
	}
}

