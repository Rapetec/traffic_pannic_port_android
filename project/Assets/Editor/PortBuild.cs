using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Ajustes del port y compilación del APK.
//  * En el editor: menú «Traffic Panic» > «Construir APK».
//  * Sin abrir el editor:
//    Unity -batchmode -quit -projectPath <proyecto> -executeMethod PortBuild.BuildAndroid
// El APK sale en Builds/TrafficPanic3D.apk (ARM64 y ARMv7, Android 5.0 o posterior).
public static class PortBuild
{
	private const string ApkPath = "Builds/TrafficPanic3D.apk";

	[MenuItem("Traffic Panic/Aplicar ajustes del port")]
	public static void ApplySettings()
	{
		PlayerSettings.companyName = "Neon Play";
		PlayerSettings.productName = "Traffic Panic 3D";
		PlayerSettings.bundleVersion = "1.5";
		PlayerSettings.Android.bundleVersionCode = 6;
		// Otro identificador que el original: así no choca con él si sigue instalado
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.trafficpanic3d.port");

		// 64 bits (y 32 para móviles antiguos)
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
		PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

		// Sin red ni permisos: el juego ya no habla con ningún servidor
		PlayerSettings.Android.forceInternetPermission = false;
		PlayerSettings.Android.forceSDCardPermission = false;
		PlayerSettings.enableCrashReportAPI = false;

		// Horizontal, como el original
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
		PlayerSettings.allowedAutorotateToLandscapeLeft = true;
		PlayerSettings.allowedAutorotateToLandscapeRight = true;
		PlayerSettings.allowedAutorotateToPortrait = false;
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
		PlayerSettings.Android.startInFullscreen = true;
		PlayerSettings.Android.renderOutsideSafeArea = true;

		// OpenGL ES, como el original (sin Vulkan)
		PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
		PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] {
			UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3, UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 });
		PlayerSettings.colorSpace = ColorSpace.Gamma;
		PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, true);

		// Firma con una clave propia del proyecto (formato JKS, que entiende el Java de Unity 2019).
		// Así no depende de ~/.android/debug.keystore, que los SDK recientes crean en un formato
		// que este Unity no sabe leer. Con la misma clave, cada APK nuevo se instala encima.
		string ks = Path.GetFullPath("Keystore/trafficpanic.keystore");
		if (File.Exists(ks))
		{
			PlayerSettings.Android.useCustomKeystore = true;
			PlayerSettings.Android.keystoreName = ks;
			PlayerSettings.Android.keystorePass = "trafficpanic";
			PlayerSettings.Android.keyaliasName = "trafficpanic";
			PlayerSettings.Android.keyaliasPass = "trafficpanic";
		}

		// Icono del APK original
		Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Port/AppIcon.png");
		if (icon != null)
		{
			PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] { icon });
		}
		AssetDatabase.SaveAssets();
		Debug.Log("Traffic Panic: ajustes del port aplicados");
	}

	[MenuItem("Traffic Panic/Construir APK")]
	public static void BuildAndroid()
	{
		ApplySettings();
		if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
		}
		Directory.CreateDirectory(Path.GetDirectoryName(ApkPath));
		BuildPlayerOptions opts = new BuildPlayerOptions
		{
			scenes = new[] { "Assets/Scenes/Landing.unity", "Assets/Scenes/Menu.unity", "Assets/Scenes/Game.unity" },
			locationPathName = ApkPath,
			target = BuildTarget.Android,
			options = BuildOptions.None,
		};
		BuildReport report = BuildPipeline.BuildPlayer(opts);
		if (report.summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Traffic Panic: APK listo en " + Path.GetFullPath(ApkPath) + " (" + report.summary.totalSize / 1048576 + " MB)");
			if (!Application.isBatchMode)
			{
				EditorUtility.RevealInFinder(ApkPath);
			}
		}
		else
		{
			Debug.LogError("Traffic Panic: la compilación falló (" + report.summary.totalErrors + " errores). Mira la consola.");
			if (Application.isBatchMode)
			{
				EditorApplication.Exit(1);
			}
		}
	}
}
