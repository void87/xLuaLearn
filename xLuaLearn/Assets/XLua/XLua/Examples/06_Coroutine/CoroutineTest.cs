using UnityEngine;
using XLua;

// 已看
namespace XLuaTest {
    public class CoroutineTest : MonoBehaviour {
        LuaEnv luaenv = null;
        // Use this for initialization
        void Start() {
            luaenv = new LuaEnv();
            // 加载 各个路径下的 coruntine_test
            luaenv.DoString("require 'coruntine_test'");
        }

        // Update is called once per frame
        void Update() {
            if (luaenv != null) {
                luaenv.Tick();
            }
        }

        void OnDestroy() {
            luaenv.Dispose();
        }
    }
}
