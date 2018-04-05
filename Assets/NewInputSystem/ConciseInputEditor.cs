using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ConciseInputBindings))]
public class ConciseInputEditor : Editor {

	private ConciseInputBindings bindings;
	private ReorderableList reorderableList;
	// Use this for initialization
	void Awake () {
		bindings = (ConciseInputBindings)target;
		if (bindings.bindings == null) {
			bindings.bindings = new List<ConciseInputBindings.ConciseBinding> ();
		}
	}

	public void OnEnable(){
		reorderableList = new ReorderableList(bindings.bindings,typeof(ConciseInputBindings.ConciseBinding), true, true, true, true);

		// This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
		// Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
		// which is a UnityEngine.Object
		// reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

		// Add listeners to draw events
		reorderableList.drawHeaderCallback += DrawHeader;
		reorderableList.drawElementCallback += DrawElement;

		reorderableList.onAddCallback += AddItem;
		reorderableList.onRemoveCallback += RemoveItem;
	}

	private void OnDisable()
	{
		// Make sure we don't get memory leaks etc.
		reorderableList.drawHeaderCallback -= DrawHeader;
		reorderableList.drawElementCallback -= DrawElement;

		reorderableList.onAddCallback -= AddItem;
		reorderableList.onRemoveCallback -= RemoveItem;
	}

	/// <summary>
	/// Draws the header of the list
	/// </summary>
	/// <param name="rect"></param>
	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, "Axis Names");
	}

	/// <summary>
	/// Draws one element of the list (ListItemExample)
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="index"></param>
	/// <param name="active"></param>
	/// <param name="focused"></param>
	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		ConciseInputBindings.ConciseBinding item = bindings.bindings[index];

		EditorGUI.BeginChangeCheck();
		//EditorGUILayout.BeginHorizontal ();
		EditorGUI.LabelField (new Rect (rect.x, rect.y+2, 40, rect.height-6),"Name:");
		item.name = EditorGUI.TextField (new Rect(rect.x + 40, rect.y+2,70,rect.height-6), item.name);

		EditorGUI.LabelField (new Rect (rect.x+120, rect.y+2, 40, rect.height-6),"Desc:");
		item.descriptiveName = EditorGUI.TextArea (new Rect(rect.x + 160, rect.y+2,100,rect.height-6), item.descriptiveName);

		int selectedMode = item.isAxis ? 0 : 1;

		selectedMode = GUI.Toolbar (new Rect(rect.x+280, rect.y+2, 125, rect.height-6),selectedMode,new string[]{"Axis","Button"},EditorStyles.toolbarButton);

		item.isAxis = selectedMode == 0;
		item.isButton = selectedMode == 1;


		EditorGUI.DrawRect (new Rect (rect.x-18, rect.y + rect.height - 1, rect.width+22, 1), Color.gray);
		/*
		EditorGUI.LabelField (new Rect (rect.x+230, rect.y, 40, rect.height),"Axis");
		item.isAxis = EditorGUI.Toggle (new Rect (rect.x+260, rect.y, 25, rect.height),item.isAxis);
		item.isButton = !item.isAxis;
		EditorGUI.LabelField (new Rect (rect.x+310, rect.y, 40, rect.height),"Button");
		item.isButton = EditorGUI.Toggle (new Rect (rect.x+360, rect.y, 25, rect.height),item.isButton);
		item.isAxis = !item.isButton;
		*/
		//EditorGUILayout.EndHorizontal ();
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
		}

		// If you are using a custom PropertyDrawer, this is probably better
		// EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
		// Although it is probably smart to cach the list as a private variable ;)
	}

	private void AddItem(ReorderableList list)
	{
		bindings.bindings.Add(new ConciseInputBindings.ConciseBinding());

		EditorUtility.SetDirty(target);
	}

	private void RemoveItem(ReorderableList list)
	{
		bindings.bindings.RemoveAt(list.index);

		EditorUtility.SetDirty(target);
	}

	public void OnJoystickPlayerConfig(){
		if (bindings.joystickControllers > 0) {
			GUILayout.Label ("Controller" + (bindings.joystickControllers > 1 ? "s " + (bindings.keyboardControllers+1) + "-" + (bindings.keyboardControllers + bindings.joystickControllers) : " " + (bindings.keyboardControllers+1)) + " (Joystick)",EditorStyles.boldLabel);
			GUIStyle boxStyle = EditorStyles.helpBox;
			GUILayout.BeginVertical (boxStyle);
			foreach (ConciseInputBindings.ConciseBinding binding in bindings.bindings) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (binding.name + ":",GUILayout.MinWidth(100));
				if (binding.isAxis) {

					string[] displayedOptions = new string[26];
					displayedOptions [0] = "X axis";
					displayedOptions [1] = "Y axis";
					displayedOptions [2] = "3rd axis";
					for (int i = 3; i < 26; i++) {
						displayedOptions [i] = i + "th axis";
					}

					int[] selectionValues = new int[26];
					for (int i = 0; i < 26; i++) {
						selectionValues [i] = i;
					}
						
					GUILayout.Label ("MacOS",GUILayout.MaxWidth(50));

					binding.macOSJoystickAxis = EditorGUILayout.IntPopup (binding.macOSJoystickAxis,displayedOptions,selectionValues,GUILayout.MaxWidth(75));

					GUILayout.Label ("Windows",GUILayout.MaxWidth(50));

					binding.windowsJoystickAxis = EditorGUILayout.IntPopup (binding.windowsJoystickAxis,displayedOptions,selectionValues,GUILayout.MaxWidth(75));

					GUILayout.Label ("Invert", GUILayout.MaxWidth (50));
					binding.invert = EditorGUILayout.Toggle (binding.invert);

				} else if (binding.isButton) {
					GUILayout.Label ("MacOS",GUILayout.MaxWidth(50));

					binding.macOSJoystickButton = GUILayout.TextField (binding.macOSJoystickButton,GUILayout.MaxWidth(75));

					GUILayout.Label ("Windows",GUILayout.MaxWidth(50));

					binding.windowsJoystickButton = GUILayout.TextField (binding.windowsJoystickButton,GUILayout.MaxWidth(75));
				}

				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}
	}

	public void OnKeyboardPlayerConfig(){
		if (bindings.keyboardControllers > 0) {
			for (int i = 0; i < bindings.keyboardControllers; i++) {
				GUILayout.Label ("Controller " + (i+1) + " (Keyboard)",EditorStyles.boldLabel);
				GUIStyle boxStyle = EditorStyles.helpBox;
				GUILayout.BeginVertical (boxStyle);
				foreach (ConciseInputBindings.ConciseBinding binding in bindings.bindings) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (binding.name + ":",GUILayout.MinWidth(100));
					if (binding.isAxis) {

						GUILayout.Label ("Negative",GUILayout.MaxWidth(50));
						if (binding.keyboardNegative == null) {
							binding.keyboardNegative = new List<string> ();
						}
						if (binding.keyboardNegative.Count <= i) {
							binding.keyboardNegative.Add ("");
						}
						binding.keyboardNegative [i] = GUILayout.TextField (binding.keyboardNegative [i],GUILayout.MaxWidth(75));

						GUILayout.Label ("Positive",GUILayout.MaxWidth(50));
						if (binding.keyboardPositive == null) {
							binding.keyboardPositive = new List<string> ();
						}
						if (binding.keyboardPositive.Count <= i) {
							binding.keyboardPositive.Add ("");
						}

						binding.keyboardPositive [i] = GUILayout.TextField (binding.keyboardPositive [i],GUILayout.MaxWidth(75));

					} else if (binding.isButton) {
						if (binding.keyboardButton == null) {
							binding.keyboardButton = new List<string> ();
						}
						if (binding.keyboardButton.Count <= i) {
							binding.keyboardButton.Add ("");
						}
						binding.keyboardButton [i] = GUILayout.TextField (binding.keyboardButton [i],GUILayout.MaxWidth(75));
					}

					GUILayout.EndHorizontal ();
				}
				GUILayout.EndVertical ();
			}
		}
	}

	private class CompletionPopup : EditorWindow
	{
		private static Texture2D tex;
		public static void Init(Rect rectangle)
		{
			CompletionPopup window = ScriptableObject.CreateInstance<CompletionPopup>();
			window.position = rectangle;
			tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			tex.SetPixel(0, 0, new Color(0.25f, 1f, 0.25f));
			tex.Apply();
			window.ShowPopup();
		}

		void OnGUI()
		{
			if (tex != null) {
				GUI.DrawTexture (new Rect (0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);
			}
			GUIStyle popupStyle = new GUIStyle(EditorStyles.helpBox);
			EditorGUILayout.BeginVertical (popupStyle);
			EditorGUILayout.LabelField("Input Axes have been generated. Check the Input Manager.", EditorStyles.wordWrappedLabel);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Okay")) this.Close();
			EditorGUILayout.EndVertical ();
		}
	}

	public override void OnInspectorGUI(){
		GUILayout.BeginVertical ();

		if(GUILayout.Button("Generate Input Axes", GUILayout.Height(50))){
			bindings.generateInputAxes ();
			Rect windowRect = EditorWindow.focusedWindow.position;
			Rect popupRect = new Rect(windowRect.position.x+windowRect.width/2-100,windowRect.position.y+100,200,150);
			CompletionPopup.Init (new Rect(popupRect));
		}

		// Actually draw the list in the inspector
		reorderableList.elementHeight = 25;
		reorderableList.DoLayoutList ();

		//BEGIN CONTROLLER OPTIONS
		GUIStyle headerStyle = EditorStyles.largeLabel;
		headerStyle.fontStyle = FontStyle.Bold;
		GUILayout.Label ("Controller Options", headerStyle);
		GUILayout.BeginHorizontal ();

		//BEGIN CONTROLLER COUNTS
		GUILayout.BeginVertical();

		//BEGIN KEYBOARD CONTROLLER COUNT
		GUILayout.BeginHorizontal ();
		GUILayout.Label(EditorGUIUtility.Load("ConciseInputEditor/keyboard.png") as Texture,GUILayout.Height(50),GUILayout.Width(50));
		GUILayout.BeginVertical ();
		GUILayout.Space (15);
		bindings.keyboardControllers = EditorGUILayout.IntSlider (bindings.keyboardControllers, 0, 4,GUILayout.MaxWidth(150));
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		//END KEYBOARD CONTROLLER COUNT

		//BEGIN JOYSTICK CONTROLLER COUNT
		GUILayout.BeginHorizontal ();
		GUILayout.Label(EditorGUIUtility.Load("ConciseInputEditor/gamepad.png") as Texture,GUILayout.Height(50),GUILayout.Width(50));
		GUILayout.BeginVertical ();
		GUILayout.Space (15);
		bindings.joystickControllers = EditorGUILayout.IntSlider (bindings.joystickControllers, 0, 4,GUILayout.MaxWidth(150));
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();
		//END JOYSTICK CONTROLLER COUNT

		GUILayout.EndVertical ();
		//END CONTROLLER COUNTS

		//BEGIN CONTROLLER EXTRA SETTINGS
		GUILayout.BeginVertical ();
		GUILayout.Space (15);
		bindings.buttonGravity = EditorGUILayout.FloatField ("Button Gravity", bindings.buttonGravity);
		bindings.buttonSensitivity = EditorGUILayout.FloatField ("Button Sensitivity", bindings.buttonSensitivity);
		bindings.joystickDeadzone = EditorGUILayout.FloatField ("Joystick Deadzone", bindings.joystickDeadzone);
		bindings.keyboardAxisSnap = EditorGUILayout.Toggle ("Keyboard Axis Snap", bindings.keyboardAxisSnap);
		GUILayout.EndVertical ();
		//END CONTROLLER EXTRA SETTINGS


		GUILayout.EndHorizontal ();
		//END CONTROLLER OPTIONS



		OnKeyboardPlayerConfig ();
		OnJoystickPlayerConfig ();

		GUILayout.EndVertical ();
	}
}
