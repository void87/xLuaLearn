/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace XLuaTest {
    [System.Serializable]
    public class Injection {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class LuaBehaviour : MonoBehaviour {
        public TextAsset luaScript;
        public Injection[] injections;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!

        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;

        private LuaTable luaTable;

        void Awake() {

            luaTable = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable metaLuaTable = luaEnv.NewTable();
            // 元表 设置 __index
            metaLuaTable.Set("__index", luaEnv.Global);
            // luaTable 设置 元表
            luaTable.SetMetaTable(metaLuaTable);
            // 元表释放
            metaLuaTable.Dispose();

            // luaTable 设置 self 为 this, lua 里可以调用这个 self
            luaTable.Set("self", this);

            foreach (Injection injection in injections) {
                luaTable.Set(injection.name, injection.value);
            }

            // 关联到 luaTable（将 LuaScript 里的内容 解析到 LuaTable 中?）
            luaEnv.DoString(luaScript.text, "LuaTestScript", luaTable);

            // LuaTable Get<Action>
            // 从 luaTable 从获取 awake 方法
            Action luaAwake = luaTable.Get<Action>("awake");

            // 从 luaTable 从获取 start 方法 赋值给 luaStart
            luaTable.Get("start", out luaStart);
            // 从 luaTable 从获取 update 方法 赋值给 luaUpdate
            luaTable.Get("update", out luaUpdate);
            // 从 luaTable 从获取 ondestroy 方法 赋值给 luaOnDestroy
            luaTable.Get("ondestroy", out luaOnDestroy);


            if (luaAwake != null) {
                // 调用 lua 脚本里的 awake 方法
                luaAwake();
            }
        }

        // Use this for initialization
        void Start() {
            if (luaStart != null) {
                // 调用 lua 脚本里的 start 方法
                luaStart();
            }
        }

        // Update is called once per frame
        void Update() {
            if (luaUpdate != null) {
                // 调用 lua 脚本里的 update 方法
                luaUpdate();
            }

            if (Time.time - LuaBehaviour.lastGCTime > GCInterval) {
                luaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }

        void OnDestroy() {
            if (luaOnDestroy != null) {
                luaOnDestroy();
            }

            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            luaTable.Dispose();
            injections = null;
        }
    }
}
