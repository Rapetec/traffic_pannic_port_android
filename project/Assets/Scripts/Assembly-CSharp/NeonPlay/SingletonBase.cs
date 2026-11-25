using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace NeonPlay
{
	public abstract class SingletonBase<T> where T : class
	{
		private class SingletonFactory
		{
			internal static readonly T Instance;

			private SingletonFactory()
			{
			}

			[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
			static SingletonFactory()
			{
				Instance = GetInstance();
			}

			[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Type.InvokeMember")]
			private static T GetInstance()
			{
				Type typeFromHandle = typeof(T);
				try
				{
					return (T)typeFromHandle.InvokeMember(typeFromHandle.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null, CultureInfo.InvariantCulture);
				}
				catch (MissingMethodException inner)
				{
					throw new TypeLoadException(string.Format(CultureInfo.CurrentCulture, "The type '{0}' must have a private constructor to be used in the Singleton pattern.", typeFromHandle.FullName), inner);
				}
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static T Instance
		{
			get
			{
				return SingletonFactory.Instance;
			}
		}
	}
}
