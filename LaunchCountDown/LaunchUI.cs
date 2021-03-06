﻿using UnityEngine;
using LaunchCountDown.Extensions;
using KSP.IO;

namespace LaunchCountDown
{
    public class LaunchUI : PartModule
    {
        private static Rect _windowsPosition = new Rect();
        private GUIStyle _windowStyle, _buttonStyle, _labelStyle, _toggleStyle;
        private bool _hasInitStyles = false;
        private bool _isInEditor = false;
        public static bool _buttonPushed = false;
        public static bool _buttonPushed2 = false;
        public static bool _buttonPushed3 = false;
        public static bool _buttonPushed4 = false;
        public static bool _launchSequenceIsActive = false;
        public static int _audioSet;
        public static string _audioSet_name = "";
        public static bool _debug;

        public override void OnStart(PartModule.StartState state)
        {
            if (state != StartState.Editor)
            {
                _isInEditor = false;
                if (!_hasInitStyles) InitStyles();
                _launchSequenceIsActive = false;
                _buttonPushed = false;
                _buttonPushed2 = false;
                //RenderingManager.AddToPostDrawQueue(0, OnDraw);
            }
            else _isInEditor = true;
        }

        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.SetValue("_audioSet", _audioSet);
            config.SetValue("Window Position", _windowsPosition);
            config.SetValue("_debug", _debug);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.load();
            _windowsPosition = config.GetValue<Rect>("Window Position");
            _audioSet = config.GetValue("_audioSet", 0);
            _debug = config.GetValue("_debug", false);
        }

        private void OnGUI()
        {
            if (_isInEditor == false)
            {
                OnDraw();

                if (_buttonPushed == true) OnDraw2();
                if (_buttonPushed2 == true || _buttonPushed4 == true) OnDraw();
                if (_buttonPushed3 == true) OnDraw3();
            }
        }

        private void OnDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow, "Launch Control", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnDraw2()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow2, "Launch Control", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnDraw3()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow3, "Audio Set", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Go Flight !", _buttonStyle))
            {
                if (_launchSequenceIsActive == false)
                    onButtonPush();
            }

            if (GUILayout.Button("settings", _buttonStyle))
            {
                if (_launchSequenceIsActive == false)
                    onSettingsPush();
            }
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void OnWindow2(int windowId)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Abort Launch !", _buttonStyle))
            {
                if (_launchSequenceIsActive == true)
                    onButtonPush2();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void OnWindow3(int windowId)
        {

            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            _debug = GUILayout.Toggle(_debug, "Debug Mode", _toggleStyle);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("◄", _buttonStyle))
            {
                _audioSet--;
                if (_audioSet < 0) _audioSet = 0;
            }
            
            if (_audioSet == 0) _audioSet_name = "Kerbalish";
            if (_audioSet == 1) _audioSet_name = "Apollo";
            //if (_audioSet == 2) _audioSet_name = "English";

            GUILayout.Label(_audioSet_name, _labelStyle);

            if (GUILayout.Button("►", _buttonStyle))
            {
                _audioSet++;
                if (_audioSet > 1) _audioSet = 1;
            }
            
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Back", _buttonStyle))
            {
                onBackPush();
            }
            
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void InitStyles()
        {
            _windowStyle = new GUIStyle(HighLogic.Skin.window);
            _windowStyle.fixedWidth = 128f;

            _buttonStyle = new GUIStyle(HighLogic.Skin.button);
            _buttonStyle.stretchWidth = true;

            _labelStyle = new GUIStyle(HighLogic.Skin.label);
            _labelStyle.alignment = TextAnchor.MiddleCenter;

            _toggleStyle = new GUIStyle(HighLogic.Skin.toggle);
            
            _hasInitStyles = true;
        }

        private void onButtonPush()
        {
            //RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw));
            _buttonPushed = true;
            //RenderingManager.AddToPostDrawQueue(0, OnDraw2);
        }

        private void onButtonPush2()
        {
            //RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw2));
            _buttonPushed2 = true;
            //RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }
        private void onSettingsPush()
        {
            //RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw));
            _buttonPushed3 = true;
            //RenderingManager.AddToPostDrawQueue(0, OnDraw3);
        }
        private void onBackPush()
        {
            //RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw3));
            _buttonPushed4 = true;
            _buttonPushed3 = false;
            //RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }
    }
}
