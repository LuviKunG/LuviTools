# LuviTools
Some useful unity plugins, created by Thanut Panichyotai (@[LuviKunG]((https://github.com/LuviKunG)))

## Version History

### May 27th 2019
- Add ```MonoBehaviourUI``` class. This is helping to get ```RectTransform``` from components in easier way.
- Move ```Yield Instruction``` to deprecate state.

### April 17th 2019
- Add ```StringPath``` attribute.
	- change ```string``` property into path selection window.

### March 8th 2019
- Add ```LayerCoroutine``` class.
	- Why? Because **Unity are sucks**. They changed ```YieldInstruction``` to used by native inside of ```MonoBehaviour``` and they completely broke ```CustomYieldInstruction```. Here is [why](Layer%20Coroutine/readme.md)

### February 28th 2019
- Add visual studio community theme. Require [Color Theme Editor for Visual Studio 2017](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.VisualStudio2017ColorThemeEditor) and [Color Themes for Visual Studio](https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.ColorThemesforVisualStudio)
	- Monokai (Dark)

### February 21st 2019
- Add Camera Sorting component.
 - Using for set a custom transparant sorting by axis per camera.
- Update Camera Aspect component.
- Update full README.md on previous update.

### February 9th 2019
- Add and Update many new components.
- Deprecated components.
	- Apply Selected Prefabs
		- No longer support because it's obsolete in Unity version 2019 and greater.
	- CameraPPU
		- No longer support because it's have new component instead.
	- Game Configuration
		- No longer support because it's have new component instead.
	- and other... No longer support because it's sucks.
- Add Yield Instruction component.
	- ```WaitForCondition```: Using in coroutine by ```yield return new WaitForCondition(isReady);```
	- ```WaitForTimeSpan```: Using in coroutine by ```yield return new WaitForTimeSpan(timespanDuration)```
- Add Android Management component.
	- Using for quick adjust primary android settings such as Target Framerate, Multitouch or Screen Sleep Timeout.
- Add Attribute.
	- ```EnumFlags``` for display list of flags in Unity Inspector by decalre attribute ```[EnumFlags]``` on an enum parameter.
	- ```IntPopup``` for display selectable int value in Unity Inspector by declare attribute ```[IntPopup(ArrayOfName,ArrayOfInt)]```
	- ```NotNull``` for display or labeled as red color when parameter is null in Unity Inspector.
	- ```ReadOnly``` for display the property Unity Inspector as read-only. (cannot edit)
	- ```StringPopup``` same as IntPopup but strings.
- Add Benchmark component.
	- Display a score that affect on device's framerate. (UnityUI)
- Add FPS Meter component.
	- Display framerate in realtime. (UnityUI)
- Add Input Conductor component.
	- Knowing this component are require to holding any input value are pressing (from keyboard or controller). Because some plugins like Unity Oculus Rift Controller are buggy when controller are disconnected while any input are pressing and it's not update the correct value.
- Add Combine Assets Window. (Unity Editor Window)
	- Using for combine any asset in Project.
- Add Extensions.
	- StringBuilderExtension
		- ```Clear();``` for clear any char of string value in StringBuilder.
		```csharp
		var sb = new StringBuilder();
		...
		var sb.Clear();
		```
	- EnumExtension
		- ```HasFlag(Enum flags);``` for checking flags in enum flag type.
		```csharp
		[System.Flags]
		public enum SomeEnumFlags { Hi = 1, Hello = 2, Bye = 4, SeeYou = 8 }
		private SomeEnumFlags emoteFlags = SomeEnumFlags.Hi;
		...
		if (emoteFlags.HasFlag(SomeEnumFlags.Hi | SomeEnumFlags.Hello))
		{
			// Yes it has!
		}
		```
	- RichTextHelper for easy to add rich text format by code.
		- ```RichTextHelper.Color(yourString, yourColor);``` for set color.
		- ```RichTextHelper.Bold(yourString);``` for set bold.
		- ```RichTextHelper.Italic(yourString);``` for set italic.
		- ```RichTextHelper.Size(yourString, size);``` for set size. (int)
- Add LuviKunG's LocaleCore Plugins.
	- This is my original component. Using for switch language translation text / asset in realtime.
	- *I'll write instruction later. (because it's too large)*
- Add Mip Map Bias Window. (Unity Editor Window)
	- Using for set Mip Map Bias in editor.
- Add Positioning struct.
	- It's calculated position by using a custom size.
	- ```PivotPosition``` for set the list of position.
	- ```CirclePosition``` for set the circle position with radius.
	- ```GridPosition``` for set the position as grid.
- Add Thai Font Adjuster Pack.
	- This is require this [unity asset](https://unitylist.com/p/ru/Unity3D.Thai-Font-Adjuster) for making thai font are correctly display by [GPOS and GSUB rules](https://docs.microsoft.com/en-us/typography/opentype/spec/gpos).
	- Using [Google Fonts](https://fonts.google.com/) + [FontForge](https://fontforge.github.io/en-US/) to batch and applied fonts to become GPOS and GSUB format.
	- *I'll write instruction later. (because it's too large)*
	
### March 14th 2018
- Improve CacheBehaviour for Unity v.2017.3.

### May 8th 2017
- Add LocalizationTools/LocalizationExportCSV.

### March 26th 2017
- Add Name (Minimum String Compare Class).

### March 19th 2017
- Add Text (Localization Class).

### December 21st 2016
- Add LuviVungleAds.

### October 4th 2016
- Move obsolete sctipts into 'Obsolete Scripts' folder.
- Add CacheBehaviour.
- Add Loop List.
- Add Limit List.
- Add Sector List.
- Add Gacha.
- Add Modified Canvas Group Inspector.
- Add Pool Object.

### January 29th 2016
- Add LuviUpdate.
- Modified LuviTools to using namespace.
- Modified LuviBehaviour to using namespace.
- Modified CameraPPU able to run in Inspector.
- Remove ILuviUpdate in Lesson.

### Older Version
- Add Inspector Label.
- Add Inspector Divider.
- Add CameraPPU.
- Add Modified Mesh Renderer Inspector.
- Add Do not destroy onload.
- Add Game Configuration.
- Add Game UI Manager.
- Add LuviBehavior.
- Add LuviFacebookAPI.
- Add LuviJSON.
- Add LuviParse.
- Add LuviPushwoosh.
- Add Prefab Scene Manager.
- Add Singleton.
- Add Time Stamp.
- Add Version Control + MiniJSONDecode.
- Add LuviConsole.
- Add Apply Selected Prefabs.
- Add LuviTools.