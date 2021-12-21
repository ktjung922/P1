namespace NodapParty
{	
	public class SingletonObject<T> where T : class, new()
    {
		private static T m_Instance;


		protected SingletonObject()
        {
		}

		public static T Instance
        {
			get
            {
				if (object.ReferenceEquals(m_Instance, null))
                {
					m_Instance = new T();
				}
				return m_Instance;
			}
		}
	}
}