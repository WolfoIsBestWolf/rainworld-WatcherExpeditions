using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu;
using UnityEngine;
using System.Reflection;

namespace WatcherExpeditions
{
    public class AddWatcherRegionArt
    {
        public static Dictionary<string, Menu.MenuScene.SceneID> landscapeLookup;
        private static Dictionary<Menu.MenuScene.SceneID, string> sceneToRegion;
        public static void Start()
        {
            LandscapeType.RegisterValues();
            On.Region.GetRegionLandscapeScene += Region_GetRegionLandscapeScene;
            On.Menu.MenuScene.BuildScene += MenuScene_BuildScene;
        }

        public static MenuScene.SceneID Region_GetRegionLandscapeScene(On.Region.orig_GetRegionLandscapeScene orig, string regionAcro)
        {
            MenuScene.SceneID origReturn = orig.Invoke(regionAcro);

            if (origReturn != MenuScene.SceneID.Empty)
                return origReturn;

            if (landscapeLookup == null)
                BuildLandscapeLookup();

            if (landscapeLookup.TryGetValue(regionAcro, out var scene))
                return scene;

            return origReturn;
        }

        private static void BuildLandscapeLookup()
        {
            landscapeLookup = new Dictionary<string, Menu.MenuScene.SceneID>();

            var fields = typeof(LandscapeType).GetFields(
                BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Menu.MenuScene.SceneID) &&
                    field.Name.StartsWith("Landscape_"))
                {
                    string regionCode = field.Name.Substring("Landscape_".Length);
                    var value = (Menu.MenuScene.SceneID)field.GetValue(null);

                    if (value != null)
                        landscapeLookup[regionCode] = value;
                }
            }
        }

        private static void MenuScene_BuildScene(On.Menu.MenuScene.orig_BuildScene orig, Menu.MenuScene self)
        {
            orig.Invoke(self);

            if (self.sceneID == null)
                return;

            if (sceneToRegion == null)
                BuildSceneRegionMap();

            if (!sceneToRegion.TryGetValue(self.sceneID, out string region))
                return;

            string folder = $"Scenes{Path.DirectorySeparatorChar}Landscape - {region}";
            string flatName = $"Landscape - {region} - Flat";
            string shadowName = $"Title_{region}_Shadow";
            string titleName = $"Title_{region}";

            self.sceneFolder = folder;

            self.AddIllustration(
                new MenuIllustration(self.menu, self, folder, flatName, new Vector2(683f, 384f), false, true));

            self.AddIllustration(
                new MenuIllustration(self.menu, self, "", shadowName, new Vector2(0.01f, 0.01f), true, false));

            if (self.menu.ID == ProcessManager.ProcessID.FastTravelScreen || self.menu.ID == ProcessManager.ProcessID.RegionsOverviewScreen)
            {
                self.AddIllustration(
                    new MenuIllustration(self.menu, self, "", shadowName, new Vector2(0.01f, 0.01f), true, false));

                self.AddIllustration(
                    new MenuIllustration(self.menu, self, "", titleName, new Vector2(0.01f, 0.01f), true, false));

                self.flatIllustrations[self.flatIllustrations.Count - 1].sprite.shader = self.menu.manager.rainWorld.Shaders["MenuText"];
            }
        }

        private static void BuildSceneRegionMap()
        {
            sceneToRegion = new Dictionary<Menu.MenuScene.SceneID, string>();

            var fields = typeof(LandscapeType).GetFields(
                BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Menu.MenuScene.SceneID) &&
                    field.Name.StartsWith("Landscape_"))
                {
                    var value = (Menu.MenuScene.SceneID)field.GetValue(null);
                    if (value != null)
                    {
                        string region = field.Name.Substring("Landscape_".Length);
                        sceneToRegion[value] = region;
                    }
                }
            }
        }
    }

    public class LandscapeType
    {
        public static Menu.MenuScene.SceneID Landscape_WRFA;
        public static Menu.MenuScene.SceneID Landscape_WARB;
        public static Menu.MenuScene.SceneID Landscape_WBLA;
        public static Menu.MenuScene.SceneID Landscape_WSKC;
        public static Menu.MenuScene.SceneID Landscape_WTDA;
        public static Menu.MenuScene.SceneID Landscape_WARF;
        public static Menu.MenuScene.SceneID Landscape_WARA;
        public static Menu.MenuScene.SceneID Landscape_WARC;
        public static Menu.MenuScene.SceneID Landscape_WARD;
        public static Menu.MenuScene.SceneID Landscape_WARE;
        public static Menu.MenuScene.SceneID Landscape_WARG;
        public static Menu.MenuScene.SceneID Landscape_WAUA;
        public static Menu.MenuScene.SceneID Landscape_WDSR;
        public static Menu.MenuScene.SceneID Landscape_WGWR;
        public static Menu.MenuScene.SceneID Landscape_WHIR;
        public static Menu.MenuScene.SceneID Landscape_WMPA;
        public static Menu.MenuScene.SceneID Landscape_WORA;
        public static Menu.MenuScene.SceneID Landscape_WPGA;
        public static Menu.MenuScene.SceneID Landscape_WPTA;
        public static Menu.MenuScene.SceneID Landscape_WRFB;
        public static Menu.MenuScene.SceneID Landscape_WRRA;
        public static Menu.MenuScene.SceneID Landscape_WRSA;
        public static Menu.MenuScene.SceneID Landscape_WSKA;
        public static Menu.MenuScene.SceneID Landscape_WSKB;
        public static Menu.MenuScene.SceneID Landscape_WSKD;
        public static Menu.MenuScene.SceneID Landscape_WSSR;
        public static Menu.MenuScene.SceneID Landscape_WSUR;
        public static Menu.MenuScene.SceneID Landscape_WTDB;
        public static Menu.MenuScene.SceneID Landscape_WVWA;
        public static Menu.MenuScene.SceneID Landscape_WVWB;

        public static void RegisterValues()
        {
            var fields = typeof(LandscapeType).GetFields(
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.Public);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Menu.MenuScene.SceneID) &&
                    field.Name.StartsWith("Landscape_"))
                {
                    string name = field.Name;
                    var instance = new Menu.MenuScene.SceneID(name, true);
                    field.SetValue(null, instance);
                }
            }
        }

        public static void UnregisterValues()
        {
            var fields = typeof(LandscapeType).GetFields(
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.Public);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Menu.MenuScene.SceneID) &&
                    field.Name.StartsWith("Landscape_"))
                {
                    var id = field.GetValue(null) as Menu.MenuScene.SceneID;
                    if (id != null)
                    {
                        id.Unregister();
                        field.SetValue(null, null);
                    }
                }
            }
        }
    }
}
