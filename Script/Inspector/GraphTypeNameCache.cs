using System;
using System.Collections.Generic;
using UnityEditor;
using XNodeEditor;

namespace CustomGraph
{
    [InitializeOnLoad]
    public static class GraphTypeNameCache
    {
        private static Dictionary<Type, string> _formattedNames = new();

        // Static constructor: called on domain reload
        static GraphTypeNameCache()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload += ClearCache;
            // This ensures cleanup also happens when Unity domain is about to reload
            EditorApplication.quitting += OnEditorQuitting;
        }

        private static void OnEditorQuitting()
        {
            Unsubscribe();
        }

        private static void Unsubscribe()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload -= ClearCache;
            EditorApplication.quitting -= OnEditorQuitting;
        }

        /// <summary>
        /// Gets a nicely formatted name for a type, using caching.
        /// </summary>
        public static string GetFormattedName(Type type)
        {
            if (_formattedNames.TryGetValue(type, out string cached))
                return cached;

            _formattedNames.Add(type, FormatNiceName(type));
            return _formattedNames[type];
        }

        /// <summary>
        /// Format your display name here. Adjust to fit your project style.
        /// </summary>
        public static string FormatNiceName(Type type)
        {
            return GetTextAfterLastDot(NodeEditorUtilities.PrettyName(type));
        }

        /// <summary>
        /// Clears the cache manually or via Unity events.
        /// </summary>
        public static void ClearCache()
        {
            _formattedNames.Clear();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state is PlayModeStateChange.EnteredEditMode or PlayModeStateChange.EnteredPlayMode)
                ClearCache();
        }

        public static string GetTextAfterLastDot(string input)
        {
            int lastDotIndex = input.LastIndexOf('.');
            if (lastDotIndex >= 0 && lastDotIndex < input.Length - 1)
            {
                return input.Substring(lastDotIndex + 1);
            }
            return input;
        }
    }
}