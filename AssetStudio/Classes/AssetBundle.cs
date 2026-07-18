using System.Collections.Generic;
using System.Diagnostics;

namespace AssetStudio
{
    public class AssetInfo
    {
        public int preloadIndex;
        public int preloadSize;
        public PPtr<Object> asset;
        public string address;
        public AssetInfo(ObjectReader reader)
        {
            preloadIndex = reader.ReadInt32();
            preloadSize = reader.ReadInt32();
            asset = new PPtr<Object>(reader);
            if (reader.Game.Type.isThreeKingdoms())
            {
                address = reader.ReadAlignedString();
            }
        }
    }

    public sealed class AssetBundle : NamedObject
    {
        public List<PPtr<Object>> m_PreloadTable;
        public List<KeyValuePair<string, AssetInfo>> m_Container;
        public AssetInfo m_MainAsset;
        public uint m_RuntimeCompatibility;
        public string m_AssetBundleName;
        public List<string> m_Dependencies;
        public bool m_IsStreamedSceneAssetBundle;
        public int m_ExplicitDataLayout;
        public int m_PathFlags;
        public List<KeyValuePair<string, string>> m_SceneHashes;
        
        public AssetBundle(ObjectReader reader) : base(reader)
        {
            var m_PreloadTableSize = reader.ReadInt32();
            m_PreloadTable = new List<PPtr<Object>>();
            for (int i = 0; i < m_PreloadTableSize; i++)
            {
                m_PreloadTable.Add(new PPtr<Object>(reader));
            }

            var m_ContainerSize = reader.ReadInt32();
            m_Container = new List<KeyValuePair<string, AssetInfo>>();
            for (int i = 0; i < m_ContainerSize; i++)
            {
                if (reader.Game.Type.isThreeKingdoms())
                {
                    var first = reader.ReadUInt64().ToString();
                    var second = new AssetInfo(reader);
                    first = second.address != "" ? second.address : first;
                    m_Container.Add(new KeyValuePair<string, AssetInfo>(first, second));

                }
                else
                {
                    m_Container.Add(new KeyValuePair<string, AssetInfo>(reader.ReadAlignedString(), new AssetInfo(reader)));
                }
            }
            
            if (reader.Game.Type.isGirlsFrontline()){
                m_MainAsset = new AssetInfo(reader);
                m_RuntimeCompatibility = reader.ReadUInt32();
                m_AssetBundleName = reader.ReadAlignedString();
                var m_DependenciesSize = reader.ReadInt32();
                m_Dependencies = new List<string>();
                for (int i = 0; i < m_DependenciesSize; i++)
                {
                    var data = reader.ReadAlignedString();
                    m_Dependencies.Add(data);
                }
                reader.AlignStream();
                m_IsStreamedSceneAssetBundle = reader.ReadBoolean();
                reader.AlignStream();
                m_ExplicitDataLayout = reader.ReadInt32();
                m_PathFlags = reader.ReadInt32();
                var m_SceneHashesSize = reader.ReadInt32();
                m_SceneHashes = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < m_SceneHashesSize; i++)
                {
                    var first = reader.ReadAlignedString();
                    var second = reader.ReadAlignedString();
                    m_SceneHashes.Add(new KeyValuePair<string, string>(first, second));
                }
            }

        }
    }
}
