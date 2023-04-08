using UnityEngine;

namespace Helpers
{
	public abstract class Singleton<T> : MonoBehaviour where T : UnityEngine.Component
	{

	#region Fields

		/// <summary>
		/// The instance.
		/// </summary>
		private static T m_Instance;

	#endregion

	#region Properties

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				if(m_Instance == null)
				{
					m_Instance = FindObjectOfType<T>();
					if(m_Instance == null)
					{
						GameObject obj = new GameObject();
						obj.name = typeof(T).Name;
						m_Instance = obj.AddComponent<T>();
					}
				}

				return m_Instance;
			}
		}

	#endregion

	#region Methods

		/// <summary>
		/// Use this for initialization.
		/// </summary>
		protected virtual void Awake()
		{
			m_Instance = this as T;
		}

		protected void OnDestroy()
		{
			m_Instance = null;
		}

	#endregion
	}
}