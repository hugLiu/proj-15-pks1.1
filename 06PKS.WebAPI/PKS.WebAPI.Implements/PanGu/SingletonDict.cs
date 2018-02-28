using PanGu;
using System.Collections.Generic;

namespace PKS.WebAPI.PanGu
{
    public class SingletonDict
    {
        private static SingletonDict _instance;
        public Dictionary<string, List<string>> TransDict { get; set; }
        public Dictionary<string, List<string>> AliasDict { get; set; }
        public bool NeedInitDict = true;

        private SingletonDict()
        {
            Segment.Init();
        }

        public static SingletonDict Instance => _instance ?? (_instance = new SingletonDict());

        public static void Init()
        {
            _instance = new SingletonDict();
        }

        public void ReloadDict()
        {
            NeedInitDict = true;
            Segment.LoadDictionary();
        }
    }
}
