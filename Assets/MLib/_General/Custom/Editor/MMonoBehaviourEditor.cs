
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MLib
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MMonoBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Get the target object (the component being inspected)
            MonoBehaviour mono = (MonoBehaviour)target;

            // Get all methods of the target object
            MethodInfo[] methods = mono.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            // Iterate through each method
            foreach (MethodInfo method in methods)
            {
                // Check if the method has the InspectorButtonAttribute
                var attributes = (MButtonAttribute[])method.GetCustomAttributes(typeof(MButtonAttribute), true);
                if (attributes.Length > 0)
                {
                    // Get the button label from the attribute, or default to the method name
                    string buttonLabel = attributes[0].ButtonLabel ?? method.Name;

                    // Display a button in the inspector
                    if (GUILayout.Button(buttonLabel))
                    {
                        // Invoke the method when the button is clicked
                        method.Invoke(mono, null);
                    }
                }
            }
        }
    }
}