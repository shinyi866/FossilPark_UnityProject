//Slightly adapted and derived from this utility made by ZimM on GitHub :
//https://gist.github.com/ZimM-LostPolygon/7e2f8a3e5a1be183ac19

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace XFurStudioMobile {
    public class XFurAssetUpdater {

        private static List<string> oldGUIDS = new List<string>(new string[] { "219ac8b70ea04ccaa760b7053b48c8a1", "0f408b932ce08024791910248ba6f444", "02279861e9e6a324288c465ecd9aa96d", "997ffdd7c9047f9469bcd7a62dba6458", "5f8e9ffff5683414ea1c2e7a1897e0b2", "d751a68929214844ebb8ea64e163b2af", "a379ec71e631921499a6e6672d74258a", "795833e9d32ee6647af6d94f691171f6", "b67e5e20763f1394e8bbe0bc67fe1a0a", "3bae97178434d5e43acde354525aa4f3", "4254412e8fade924a9a17eb14cf60f9d", "2415c5a0a2c431945beb3eb2c58ce197", "bba2efbc477609141b18a38a420948a5", "2bef95333e7831641b1c7469c7b718c0", "a7d121bcf19d0c44e84174ff8fc8826b", "033393fc0e0b9fb47a4d0a6e3f0661c6", "32c8fcf7a30464b43a04e3b1bcd57c48", "f55be7c19e426c244b61edeb50350fbc", "3bf6c45c0ab64de47806f33c2cb3b81b", "134f366b7aecb1243965344976939e5c", "45c880401910872498be2d69788a1ba6", "4176f95454317224c9819dbd78b06221", "f1e4bae28454ac04690d5b1e1e4c0797", "1e65777799fb6fe4291e3007678d8f37", "21f87ea684f967049beb4c6c20b66786", "7c6ecc9f78aca0f48843ebf1a7df8207", "714a43d795d55d44bb1bf2154ecdab03", "7375b2d62202b3a4cb82cde88c37571f", "88a19c1d9668a504f9b76e4c14461382", "b4e4708d2dba72745a8fd258354080bc", "358303cf74532c4479f7c0fdfaeba4e1", "6d146c8e18f869c4798772e95cdea4c8", "5f35af982a63e4442a90ec3f7d83fdc3", "1dfe01fd29cb8be479949ca38e80421f", "5c3c803551b9a044e9a28b496b70dca1", "13a181896d009ac4680480c136f8e7fd", "f4803037ef3d50c4289d0ef26e890be7", "99c416ec7e9c4d348aef68bab10ac212", "5d4bb98a3b8694c47829514289c20063", "f70cbe5b0d0e3fb48b380605274b12a6", "a92cbacdc625d2c489f3b86c69bd0554", "a4068ff84ceceb14cb691c0cdcb10f99", "c6a1eb031fa04a84b9e1238dcc524416", "1c9a20650eb34d845a512576a9248380", "0ab95b7c61e579647857cd68db735934", "7a416d98af7312e4fa802b25cfa1a07e", "db57434d4dc8fad4b8e458236ad624b4", "1bba5de29460fdd499097cf266a14ab7", "1237325bb47533e42ba3f33ee54b0ad9", "5957cdb0ca0168945846a9f79f9c875e", "e9a963080ede75744ae5952aeb23ff4e", "ea53d2fda9d695c449ab190051150cf2", "f7037134c0937dd4f9cdbc0d5ff5ed62", "1678dd48b5bedd8499a312651e40c8a4", "6bece20f7f02fa84890fe01f949ef7fc", "77a14b1623ea68b49ac27ea7a3b5441c", "cc18b3b648f9ea34d9a03400d8b57497", "a6ee70b293882e84b92d07ea702b5aa6", "415264813c8136942989576abf0fd53f", "a3641430b5166d24fa1314d272088a87", "ca7f779a21df6c540b59fc5e898c9cdd", "dbb5ff9b5437b004d82cc631a9d11aee", "dfc0913cc58158c4f8a9b8b3c9c5ede3", "ddb53a8bf7e87a84c847cca520d42a46", "135ea32b5403e1f40aebb5680e404406", "26b914cfa3db5af4ba58a0f0c1474382", "63f2a396ec504fd429f60fe3ffc58f96", "dea94bdce2789e54b8a6c74ffcdd8903", "111f1325382cf2e40baecb91ebe58f43", "269ba11952ced8d4ba7f879b7e4b0484", "634672430195fae4fae3c0167f484ed6", "5aa6b7460fa1a0e4c85da0774fb4c2a6", "6e364418cadfbf74cb5a762fbf6a5f4f", "8b9959b507c4b864882cb24025dc7fb5", "8c726520f9c473c4da5a73579a6d84c8", "04468e56325003a4688b321e89f8e72b", "1b0b52030fc2e194da43a3aabbab3f5a", "56e19d029fe18384a81698fb1d2411bc", "dfcc1ece725a1bd42a91c6803e794c50", "c675dc9991a977c4fb63e039c744c088", "97e181778398d20428f0c8e21a23dfa1", "c197e67c399c0044eb8591c8064063dd", "6b49ece26e677bd4ea54302c4626a737", "83e826a6ba2e76e49a8e612e5d8ffa35", "d18ceb616fbea1346af7fefa4e794067", "67c1870e04d09b945a380f35124ad560", "92357111bcbebab43894bdb27b8029b4", "f144206461075584094485e6a9c7ba9c", "c2032ced9a460014394c720844cadb96", "0a3ba34faf1daa14e9445c0faded3af6", "183cf9ed22c78ec43801f3592cb1e949", "a295e2ad160aae64da0cfb3dca0f4831", "be09aee970772bd4a985c34dbdbe8a9e", "7c965b29e0ddcc94eaa31d187e9398ce", "c5c95d467ab97f245946325903685a0d", "69f7c432f936c6a4d95180c3fe9f6a92", "bb489247dba9d49469012d653e0c3b66" });
        private static List<string> newGUIDS = new List<string>(new string[] { "b5a6d31743274da99562f9bc004bfba6", "a40cebe38989477bbc9f7e2aa8a6ba3c", "48805441119f4bfcb59f59c9011d8fa8", "bb780b5f2f7a4b66953a2659f26d61d4", "3ffa17f382f74560967e3e6b4e78f34b", "7580255495794dbd9ce056c4683403bb", "9846bbc8f7634876bbbb21cd99c10fa9", "23f85b8d03fc4ca2b8e5075a4bfa7add", "e43c74ed9a6a49f7bc609ba0a07740af", "a2cdc16ea8b647958989ef7a6a115c5d", "ec6f3bb8730a41179464613e485a75a9", "2d9eff41741b4d92921d62f66abe0ebb", "f1189602a83f439bbd4a7673640cd0f2", "c2db1f1f46404f5faf908083abb88ac4", "c3a4d81f4176414a97703a09c71d4f53", "dce25aaf9a24477abee1ffe9a14d5bda", "4462f8749860401db01a5ed449c3aa26", "3b882b83a4af4240a62cba777c4f32c4", "ba98404fa0f3400d84c2c9bc4ba7139b", "508c34b5c8dd4b78b53ceae8a04e2c0f", "f17a660311e64fc9b503f5b4e99a9888", "4e496b13ca58455b86a989e307c185c3", "db7eabf6e9cf4d5aa5f8b389999dea61", "37a51e9a230c41c99c6b3ac44b35aed2", "69a84f2275fe4d6c971f282e35d91e0a", "5e008fce8e584ab2bfb9c8c4c12983e6", "6c9b108d2b5e48f68c9da18f9b2a65cc", "18524eef6ae84ea991c8d625f28eaa76", "ef1760b2177c4563a2c56a0c513f81ac", "bed7a5dadf43474e8333eb1ee1b764e3", "66eff174d33f4598970831c2014d2b53", "53d029ce908e4862862532b73f78e9b2", "b12712cf2a43475e92002dae5c99059f", "d71faa18cbe94b04823a19a7dc56cb45", "b64c68d199e4464398e3bf25dd1f5b8c", "3a814946f2474efb8e224d327eaa3204", "b6e7357cd3b24acfb23649474e80386f", "82c1cdef16f74a028759ff8b8efa26de", "22fea2fb75014ce6a5978e5acd5ad777", "687219c3e5c8402e972617950740ce49", "ef14248913894a498472a16152447518", "202ed5ee9e594182be300cdfefecec2d", "2fbc4a595d5643449643504d606477a3", "eba57e91ec1b4563974755cd5461a2a2", "22d0217e96e04bdaad948755649f3be3", "bfd2182140894cd6977e3af48fe76b05", "4c35651f30b144e6897062c05db1dcb6", "8892a571d59349579e5a52843e76ea13", "86063995e09549b4b1a15efe942be274", "6b44af8a6c8d4fc7895a0732e17b0392", "737d7ed3dbf446d7b38860ec6060a1f9", "70f39885fac14a3084dedd55577057fd", "41d6ec66feca429d968f476f2ad69587", "5bc711aaef26400495b56aa92ee65192", "5db01528af6942da8ecd8ed3ca11ec57", "4a6ee4af30644e5a9344ee1a905c157b", "0cfe907e8f5e4b768aa706838e1e95bf", "e597f7f616264eba9377a9d7327fa282", "fe7e7086e54b481dabfb3f1605b79873", "e3d53c22b0c74dacbfaef83d1c6abd8c", "1b4f3a1f38f24d748c505f35f5a1319b", "d140bc119a7941d89e22a7a15ccbb70a", "50a33228fbc249a7a80b25fe5dd66cd8", "9b9ed8c4e26349d784d96b832b41e795", "7afbbd314ee84ca6ba398653fc54aca3", "66ba1c05216e4044aaec4abd994b7d34", "e3cf110c04464fa998f22d88183038b9", "fa1d84bcf8c94cd7bf3a5a8d59da3fc4", "04fd2b24541f4c37b5b753070630bc39", "2e58f12069594ad9b62dd3b20e79e6a8", "cd3ff48691ec4734924e78ee4cef1adf", "d53a1d46e2984c7f93462f539f544403", "96272ff312f44fef8e98eb6fb1f099dd", "3dc04bf099a14bbba56044ae0868e36a", "3572e97a4e374d97b00f1b53b1aae308", "a57a23c6c57d4dc0ae396d1170aebae6", "f74795bc479c4120b3ac74245be52ef8", "c0f66b6598aa4399b89611cd354d22e0", "9d1f5c996a0448d59bdb92f9684853fc", "c4d81d2e1cdf4524b4fc0d7877e82e5e", "14e01e23433b4221b429a91c389a544b", "87acdbe2e696421ba399afc918fb162e", "712252694fe84ff0964a5933df0ea57f", "dfe7ff0c0c404a4eb18d7b3c2d17a6e6", "15facaf6540f475a8cc8a2194e75261f", "64cc90540909440b8a214dc69710d7ec", "04e168fdf689400aab94c21177d5c619", "ba9b11d959db4907bdf6f1b501975487", "1fdf7ffea85d4c0eb67e126114af159b", "6c9b4e5bd49941c8a8cf5164a41efc04", "968a487bf5304a47bb48a96c2db37832", "c22268df72a54887b089fc68a3622099", "26aaf6695a26439db11b4df9185e0dd3", "f839996996ad452ea9285a206d299177", "d351f8ab8e1f48efb7eb546774be5b99", "acf26958165b4beea042929b03f92729", "1bf9ddd784df4b38b8ae9efe3f302c11" });


        private static string[] kDefaultFileExtensions = {
                "*.meta",
                "*.mat",
                "*.anim",
                "*.prefab",
                "*.unity",
                "*.asset"
            };

        private static string _assetsPath = Path.GetFullPath(".") + Path.DirectorySeparatorChar + "Assets";

        [MenuItem("Tools/XFur Studio Mobile/Upgrade XFur Mobile Internals")]
        public static void RegenerateGuids() {
            if (EditorUtility.DisplayDialog("GUIDs regeneration",
                "You are going to start the process of intensive patching for XFur Mobile. This may have unexpected results or break some links within your project. Make sure that the Assets Serialization Mode is set to Force Text before beginning the patching process. \n\n MAKE A PROJECT BACKUP BEFORE PROCEEDING!",
                "Upgrade XFur", "Cancel")) {
                AssetDatabase.StartAssetEditing();


                RegenerateAssets();

                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();

            }
        }

        /*
        [MenuItem("Tools/XFur Studio Mobile/Debug GUIDS")]
        public static void WriteGUIDS() {
            List<string> filesPaths = new List<string>();
            foreach (string extension in kDefaultFileExtensions) {
                filesPaths.AddRange(
                    Directory.GetFiles(_assetsPath, extension, SearchOption.AllDirectories)
                    );
            }

            int counter = 0;
            var tempGUIDS = "{ ";
            foreach (string filePath in filesPaths) {
                EditorUtility.DisplayProgressBar("Scanning Assets folder", MakeRelativePath(_assetsPath, filePath), counter / (float)filesPaths.Count);
                if (filePath.Contains("XFur Studio Mobile")) {
                    string contents = File.ReadAllText(filePath);

                    IEnumerable<string> guids = GetGuids(contents);
                    bool isFirstGuid = true;
                    foreach (string oldGuid in guids) {
                        // First GUID in .meta file is always the GUID of the asset itself
                        if (isFirstGuid && Path.GetExtension(filePath) == ".meta") {
                            tempGUIDS += '"'+Guid.NewGuid().ToString("N")+'"'+",";
                            isFirstGuid = false;
                        }
                    }
                }

                counter++;
            }

            tempGUIDS += " }";
            Debug.Log(tempGUIDS);

        }
        */

        private static bool IsGuid(string text) {
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                if (
                    !((c >= '0' && c <= '9') ||
                      (c >= 'a' && c <= 'z'))
                    )
                    return false;
            }

            return true;
        }

        private static IEnumerable<string> GetGuids(string text) {
            const string guidStart = "guid: ";
            const int guidLength = 32;
            int textLength = text.Length;
            int guidStartLength = guidStart.Length;
            List<string> guids = new List<string>();

            int index = 0;
            while (index + guidStartLength + guidLength < textLength) {
                index = text.IndexOf(guidStart, index, StringComparison.Ordinal);
                if (index == -1)
                    break;

                index += guidStartLength;
                string guid = text.Substring(index, guidLength);
                index += guidLength;

                if (IsGuid(guid)) {
                    guids.Add(guid);
                }
            }

            return guids;
        }




        private static void RegenerateAssets(string[] regeneratedExtensions = null) {
            if (regeneratedExtensions == null) {
                regeneratedExtensions = kDefaultFileExtensions;
            }

            // Get list of working files
            List<string> filesPaths = new List<string>();
            foreach (string extension in regeneratedExtensions) {
                filesPaths.AddRange(
                    Directory.GetFiles(_assetsPath, extension, SearchOption.AllDirectories)
                    );
            }

            // Create dictionary to hold old-to-new GUID map
            Dictionary<string, string> guidOldToNewMap = new Dictionary<string, string>();
            Dictionary<string, List<string>> guidsInFileMap = new Dictionary<string, List<string>>();

            for (int i = 0; i < oldGUIDS.Count; i++) {
                guidOldToNewMap.Add(oldGUIDS[i], newGUIDS[i]);
            }

            // We must only replace GUIDs for Resources present in Assets. 
            // Otherwise built-in resources (shader, meshes etc) get overwritten.
            // Traverse all files, remember which GUIDs are in which files and generate new GUIDs
            int counter = 0;
            foreach (string filePath in filesPaths) {
                EditorUtility.DisplayProgressBar("Scanning Assets folder", MakeRelativePath(_assetsPath, filePath), counter / (float)filesPaths.Count);

                string contents = File.ReadAllText(filePath);

                IEnumerable<string> guids = GetGuids(contents);
                foreach (string oldGuid in guids) {
                    if (!guidsInFileMap.ContainsKey(filePath))
                        guidsInFileMap[filePath] = new List<string>();

                    if (!guidsInFileMap[filePath].Contains(oldGuid)) {
                        guidsInFileMap[filePath].Add(oldGuid);
                    }
                }

                counter++;
            }

            // Traverse the files again and replace the old GUIDs
            counter = -1;
            int guidsInFileMapKeysCount = guidsInFileMap.Keys.Count;
            foreach (string filePath in guidsInFileMap.Keys) {
                EditorUtility.DisplayProgressBar("Regenerating GUIDs", MakeRelativePath(_assetsPath, filePath), counter / (float)guidsInFileMapKeysCount);
                counter++;

                string contents = File.ReadAllText(filePath);
                foreach (string oldGuid in guidsInFileMap[filePath]) {
                    if (!oldGUIDS.Contains(oldGuid))
                        continue;

                    string newGuid = guidOldToNewMap[oldGuid];
                    if (string.IsNullOrEmpty(newGuid))
                        throw new NullReferenceException("newGuid == null");

                    contents = contents.Replace("guid: " + oldGuid, "guid: " + newGuid);
                }
                File.WriteAllText(filePath, contents);
            }

            EditorUtility.ClearProgressBar();
        }


        private static string MakeRelativePath(string fromPath, string toPath) {
            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath;
        }

    }





}