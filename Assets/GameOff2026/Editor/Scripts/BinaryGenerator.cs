using System.Collections.Generic;
using System.IO;
using Cysharp.Text;
using GameOff2026.Game.Data.DataStore;
using MessagePack;
using MessagePack.Resolvers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GameOff2026.Editor
{
    public static class BinaryGenerator
    {
        [MenuItem("Tools/MasterMemory/" + nameof(BuildBinary))]
        private static void BuildBinary()
        {
            StaticCompositeResolver.Instance.Register(new[]
            {
                MasterMemoryResolver.Instance,
                StandardResolver.Instance,
            });

            MessagePackSerializer.DefaultOptions =
                MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

            var databaseBuilder = new DatabaseBuilder();
            databaseBuilder
                .Append(DeserializeJson<FieldPatternMaster>("field_pattern"));

            const string path = "Assets/Externals/Binary/MasterMemory.bytes";
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(path, databaseBuilder.Build());
            AssetDatabase.Refresh();

            Debug.Log("[MasterMemory] Build bytes completed.");
        }

        private static List<T> DeserializeJson<T>(string fileName)
        {
            var json = File.ReadAllText(ZString.Format("Master/Jsons/{0}.json", fileName));
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}