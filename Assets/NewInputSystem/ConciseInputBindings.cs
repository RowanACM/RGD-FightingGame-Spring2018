using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "Bindings", menuName = "Input/Create Concise Bindings", order = 1)]
public class ConciseInputBindings : ScriptableObject {



	public enum AxisType
	{
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	};

	public class InputAxis
	{
		public string name;
		public string descriptiveName;
		public string descriptiveNegativeName;
		public string negativeButton;
		public string positiveButton;
		public string altNegativeButton;
		public string altPositiveButton;

		public float gravity;
		public float dead;
		public float sensitivity;

		public bool snap = false;
		public bool invert = false;

		public AxisType type;

		public int axis;
		public int joyNum;

		public InputAxis Clone(){
			return (InputAxis) this.MemberwiseClone ();
		}
	}
	[System.Serializable]
	public class ConciseBinding {
		public string name;
		public string descriptiveName;
		public string descriptiveNegativeName;

		public bool isAxis;

		public List<string> keyboardPositive;
		public List<string> keyboardNegative;
		public int windowsJoystickAxis;
		public int macOSJoystickAxis;

		public bool isButton;

		public List<string> keyboardButton;
		public string windowsJoystickButton;
		public string macOSJoystickButton;

		public float gravity;
		public float dead;
		public float sensitivity;

		public bool snap = false;
		public bool invert = false;
	}

	public List<ConciseBinding> bindings;

	public int keyboardControllers;
	public int joystickControllers;

	public float buttonGravity = 3f;
	public float joystickDeadzone = 0.01f;
	public float buttonSensitivity = 1f;
	public bool keyboardAxisSnap = true;

	public void generateInputAxes(){
		ClearInputDefinition ();
		List<InputAxis> generatedAxes = new List<InputAxis> ();
		foreach (ConciseBinding binding in bindings) {
			generatedAxes.AddRange(getInputAxesFromConcise (binding));
		}
		foreach (InputAxis axis in generatedAxes) {
			AddAxis (axis);
		}
	}

	private InputAxis getIntrinsicAxis(ConciseBinding binding){
		InputAxis axis = new InputAxis ();
		axis.name = binding.name;
		axis.descriptiveName = binding.descriptiveName;
		return axis;
	}

	public List<InputAxis> getInputAxesFromConcise(ConciseBinding binding){
		List<InputAxis> axes = new List<InputAxis> ();

		int controllerIndex = 0;
		// Need to make an axis for each keyboard controller
		for (controllerIndex = 0; controllerIndex < keyboardControllers; controllerIndex++) {
			InputAxis inputAxis = getIntrinsicAxis (binding);
			inputAxis.name = "C"+(controllerIndex+1)+"-"+binding.name;
			inputAxis.gravity = buttonGravity;
			inputAxis.dead = 0f;
			inputAxis.sensitivity = buttonSensitivity;
			inputAxis.type = AxisType.KeyOrMouseButton;
			InputAxis macAxis = new InputAxis ();
			InputAxis winAxis = new InputAxis ();
			if (binding.isAxis) {
				//Keyboard inputs are the same on both platforms
				inputAxis.positiveButton = binding.keyboardPositive[controllerIndex];
				inputAxis.negativeButton = binding.keyboardNegative[controllerIndex];
				inputAxis.snap = keyboardAxisSnap;
			} else if (binding.isButton) {
				inputAxis.positiveButton = binding.keyboardButton[controllerIndex];
			} else {
				Debug.LogError ("Concise Binding '" + binding.name + "' isnt an axis or a button!");
			}
			macAxis = inputAxis;
			winAxis = inputAxis.Clone ();
			macAxis.name += "-MacOS";
			winAxis.name += "-Win";
			axes.Add (macAxis);
			axes.Add (winAxis);
		}
		//Need to make an axis for each joystick controller for each platform
		for (; controllerIndex < keyboardControllers + joystickControllers; controllerIndex++) {

			int joystickNum = controllerIndex - keyboardControllers + 1;

			InputAxis inputAxis = getIntrinsicAxis (binding);
			inputAxis.name = "C"+(controllerIndex+1)+"-"+binding.name;

			inputAxis.gravity = buttonGravity;
			inputAxis.dead = joystickDeadzone;
			inputAxis.sensitivity = buttonSensitivity;

			InputAxis macAxis = new InputAxis();
			InputAxis winAxis = new InputAxis();

			if (binding.isAxis) {
				inputAxis.type = AxisType.JoystickAxis;
				inputAxis.joyNum = joystickNum;
				inputAxis.invert = binding.invert;

				//set axis value for macOS and windows axes
				macAxis = inputAxis;
				winAxis = inputAxis.Clone ();
				macAxis.axis = binding.macOSJoystickAxis;
				Debug.Log (macAxis.axis);
				winAxis.axis = binding.windowsJoystickAxis;

			} else if (binding.isButton) {
				inputAxis.type = AxisType.KeyOrMouseButton;

				//set positive button value for macOS and windows
				macAxis = inputAxis;
				winAxis = inputAxis.Clone ();
				macAxis.positiveButton = "joystick " + joystickNum + " button " + binding.macOSJoystickButton;
				winAxis.positiveButton = "joystick " + joystickNum + " button " + binding.windowsJoystickButton;

			} else {
				Debug.LogError ("Concise Binding '" + binding.name + "' isnt an axis or a button!");
			}
			macAxis.name += "-MacOS";
			winAxis.name += "-Win";
			axes.Add (macAxis);
			axes.Add (winAxis);
		}


		return axes;
	}

	private static void ClearInputDefinition(){
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		axesProperty.ClearArray();
		serializedObject.ApplyModifiedProperties();
	}


	private static void AddAxis(InputAxis axis)
	{
		if (AxisDefined(axis.name)) return;

		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();

		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

		GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

		serializedObject.ApplyModifiedProperties();
	}


	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
	{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do
		{
			if (child.name == name) return child;
		}
		while (child.Next(false));
		return null;
	}

	private static bool AxisDefined(string axisName)
	{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

		axesProperty.Next(true);
		axesProperty.Next(true);
		while (axesProperty.Next(false))
		{
			SerializedProperty axis = axesProperty.Copy();
			axis.Next(true);
			if (axis.stringValue == axisName) return true;
		}
		return false;
	}




}
