﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace Momiji
{
    public class AudioEnumCreator
    {
        private const string AUDIO_ENUM_HASH_KEY = "Audio_Enum_Hash";

        [MenuItem ("Assets/Creator/AudioEnumCreator")]
        public static void _AudioEnumCreator ()
        {
            if (EditorApplication.isPlaying || Application.isPlaying)
                return;

            BuildAudioName ();
        }

        [DidReloadScripts (1)]

        static AudioEnumCreator ()
        {
            if (EditorApplication.isPlaying || Application.isPlaying)
                return;

            EditorApplication.delayCall += BuildAudioName;
        }

        static void BuildAudioName ()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder ();

            builder = WriteManagerClass (builder);

            string text = builder.ToString ().Replace (",}", "}");
            string assetPath = Application.dataPath + EditorExtensionConst.SAVE_FILE_POINT + EditorExtensionConst.AutoFileName.AudioEnums.ToString () + ".cs";

            Directory.CreateDirectory (Application.dataPath + EditorExtensionConst.SAVE_FILE_POINT);

            if (AssetDatabase.LoadAssetAtPath (assetPath.Replace ("/Editor/..", ""), typeof (UnityEngine.Object)) != null && EditorPrefs.GetInt (AUDIO_ENUM_HASH_KEY, 0) == text.GetHashCode ())
                return;

            System.IO.File.WriteAllText (assetPath, text);
            EditorPrefs.SetInt (AUDIO_ENUM_HASH_KEY, text.GetHashCode ());
            AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
            EditorApplication.delayCall -= BuildAudioName;
        }

        static System.Text.StringBuilder WriteManagerClass (System.Text.StringBuilder builder)
        {
            WriteAudioScript (builder);
            return builder;
        }

        static void WriteAudioScript (System.Text.StringBuilder builder)
        {
            SoundParameter source = LoadResources.ScriptableObject ("SoundParameter") as SoundParameter;
            WriteAudioEnum (builder, source?.BGMClip ?? new AudioClip[0], SoundType.BGM);
            WriteAudioEnum (builder, source?.SEClip ?? new AudioClip[0], SoundType.SE);
            WriteAudioEnum (builder, source?.VoiceClip ?? new AudioClip[0], SoundType.Voice);
        }

        static void WriteAudioEnum (System.Text.StringBuilder builder, AudioClip[] audioNames, SoundType type)
        {
            builder.AppendLine ("/// <summary>");
            builder.AppendFormat ("/// Access " + type.ToString () + " Enum").AppendLine ();
            builder.AppendLine ("/// </summary>");
            builder.Append ("public enum " + type.ToString ()).AppendLine ();
            builder.AppendLine ("{");
            audioNames.ForEach ((audioName, i) =>
            {
                var comma = (i == audioNames.Count () - 1) ? "" : ",";
                builder.Append ("\t").AppendFormat ("{0} = {1}", audioName?.name.SymbolReplace (), i + comma).AppendLine ();
            });
            builder.AppendLine ("};");
        }
    }
}