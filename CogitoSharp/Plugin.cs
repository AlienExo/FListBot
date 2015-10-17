using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CogitoSharp.IO;

namespace CogitoSharp
{
	internal enum AccessLevel : byte { Everyone = 0, ChannelOps, GlobalOps, RootOnly }
	internal enum AccessPath  : byte { All = 0, ChannelOnly, PMOnly }

	static class Plugins{
		internal static Dictionary<string, CogitoPlugin> PluginStore = null;

		internal abstract class CogitoPlugin{
			internal string Name;									//Plugin Name, e.g. "Dice roll"
			internal string Description;							//Description to be given when a list is requested
			internal string Trigger;								//trigger, e.g. ".roll" 
			internal abstract void MessageLoopMethod(Message m);	//Method to be executed on EACH message received (just pass if not needed)
			internal abstract void ShutdownMethod(Message m);		//Method to be executed when program shuts down,	e.g. saving data
			internal abstract void SetupMethod(Message m);			//Method to be executed when program starts,		e.g. loading data 
																	//and registered trigger via Config.AITriggers.Register(string, Delegate)
			internal abstract void PluginMethod(Message m);			//The method that's actually executed
			internal AccessLevel AccessLevel;						//The level of access required to execute this command
			internal AccessPath AccessPath;							//The avenues through which this command may be executed
		}

		internal static void loadPlugins()
		{
			string[] dllFileNames = null;
			if (System.IO.Directory.Exists(CogitoSharp.Config.AppSettings.PluginsPath)) { dllFileNames = System.IO.Directory.GetFiles(CogitoSharp.Config.AppSettings.PluginsPath, "*.dll"); }
			if (dllFileNames == null) { return; }
			Type CSPluginType = typeof(CogitoPlugin);
			ICollection<Type> CSPluginTypes = new List<Type>();
			foreach (string filename in dllFileNames)
			{
				Assembly a = Assembly.LoadFile(filename);
				if (a != null)
				{
					Type[] types = a.GetTypes();
					foreach (Type t in types)
					{
						if (t.IsInterface || t.IsAbstract) { continue; }
						else { if (t.GetInterface(CSPluginType.FullName) != null) { CSPluginTypes.Add(t); } }
					}	//foreach Type t
				}	// a != null
			} //foreach filename
			foreach (Type _t in CSPluginTypes)
			{
				CogitoPlugin plugin = (CogitoPlugin)Activator.CreateInstance(_t);
				PluginStore.Add(plugin.Name, plugin); //registers name -> Plugin
			}
		}
	}
}
