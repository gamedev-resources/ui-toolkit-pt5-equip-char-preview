using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Temporary utility to capture Scene View screenshots to disk.
/// Delete after testing is complete.
/// </summary>
public static class SceneViewScreenshot
{
    private static int _counter = 1;

    [MenuItem("Tools/Capture Scene View Screenshot")]
    public static void Capture()
    {
        var sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null)
        {
            Debug.LogError("No active Scene View found.");
            return;
        }

        string dir = Path.Combine(Application.dataPath, "..", "TestScreenshots");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // Force the scene view to repaint so the camera renders
        sceneView.Repaint();

        // Use the scene view camera to render
        var camera = sceneView.camera;
        int width = 1920;
        int height = 1080;

        var rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;
        camera.Render();

        RenderTexture.active = rt;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt);

        string filename = $"sceneview_{_counter:D2}.png";
        string path = Path.Combine(dir, filename);
        File.WriteAllBytes(path, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);

        _counter++;
        Debug.Log($"Scene View screenshot saved to: {Path.GetFullPath(path)}");
    }

    [MenuItem("Tools/Reset Screenshot Counter")]
    public static void ResetCounter()
    {
        _counter = 1;
        Debug.Log("Screenshot counter reset to 1.");
    }
}
