using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CogitoSharp.IO;

namespace CogitoSharp
{

	static class Plugins
	{
		internal static Dictionary<string, CogitoPlugin> PluginStore = null;

		internal abstract class CogitoPlugin{
			internal string name;									//Plugin Name, e.g. "Dice roll"
			internal string description;							//Description to be given when a list is requested
			internal string trigger;								//trigger, e.g. ".roll" 
			internal abstract void MessageLoopMethod(Message m);	//Method to be executed on EACH message received (just pass if not needed)
			internal abstract void ShutdownMethod(Message m);		//Method to be executed when program shuts down,	e.g. saving data
			internal abstract void SetupMethod(Message m);			//Method to be executed when program starts,		e.g. loading data.
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
				PluginStore.Add(plugin.name, plugin); //registers name -> Plugin
			}
		}
	}
}
