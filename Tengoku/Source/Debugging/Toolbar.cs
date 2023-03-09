using System.Numerics;
using ImGuiNET;

namespace Tengoku.Debugging
{
    public class Toolbar
    {
        private static Trinkit.Graphics.Texture RunGameBTN = GetIcon("playfromstart");
        private static Trinkit.Graphics.Texture RunGameBTN_Paused = GetIcon("playfromstart_stop");
        private static Trinkit.Graphics.Texture PauseBTN = GetIcon("pause");
        private static Trinkit.Graphics.Texture PauseBTN_Disabled = GetIcon("pausedisabled");
        private static Trinkit.Graphics.Texture NextFrameBTN = GetIcon("nextframe");
        private static Trinkit.Graphics.Texture RunGameBTN_Disabled = GetIcon("playfromstartdisabled");
        private static Trinkit.Graphics.Texture RunSceneBTN = GetIcon("playscene");
        private static Trinkit.Graphics.Texture RunSceneBTN_Disabled = GetIcon("playscenedisabled");
        private static Trinkit.Graphics.Texture BuildBTN = GetIcon("build");
        private static Trinkit.Graphics.Texture BuildBTN_Disabled = GetIcon("builddisabled");
        private static Trinkit.Graphics.Texture ExportBTN = GetIcon("export");
        private static Trinkit.Graphics.Texture ExportBTN_Disabled = GetIcon("exportdisabled");
        private static Trinkit.Graphics.Texture InfoBTN = GetIcon("info");
        private static Trinkit.Graphics.Texture InfoBTN_Disabled = GetIcon("infodisabled");

        private static Trinkit.Graphics.Texture CreateSpriteBTN = GetIcon("createsprite");
        private static Trinkit.Graphics.Texture CreateSoundBTN = GetIcon("createsound");
        private static Trinkit.Graphics.Texture CreateScriptBTN = GetIcon("createscript");
        private static Trinkit.Graphics.Texture CreateMeshBTN = GetIcon("createmesh");
        private static Trinkit.Graphics.Texture CreateSceneBTN = GetIcon("createscene");

        private static Trinkit.Graphics.Texture GetIcon(string path)
        {
            var tex = new Trinkit.Graphics.Texture($"Resources_Debug/icons/{path}.png");
            tex.DontDestroyOnLoad = true;
            return tex;
        }

        public Toolbar()
        {
        }

        public static void Gui()
        {
            var toolbarHeight = 48;

            ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(new Vector2(viewport.Pos.X, viewport.Pos.Y + 22));
            ImGui.SetNextWindowSize(new Vector2(viewport.Size.X + 2, toolbarHeight));
            ImGui.SetNextWindowViewport(viewport.ID);

            ImGuiWindowFlags flags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoTitleBar |
                         ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove |
                         ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoScrollWithMouse;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.PushStyleColor(ImGuiCol.WindowBg, (uint)new Trinkit.Color("151515"));
            ImGui.Begin("Toolbar", flags);
            ImGui.PopStyleVar();

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(4, 0));

            if (Button(RunGameBTN, RunGameBTN_Paused, "Play", !Game.IsPlaying))
            {
                if (Game.IsPlaying)
                    Game.Instance.StopGame();
                else
                    Game.Instance.PlayGame();
            }
            var activePauseBtn = (Game.IsPlaying) ? RunGameBTN : PauseBTN;
            if (Button(PauseBTN, (Game.IsPlaying) ? RunGameBTN : PauseBTN, (Game.IsPaused) ? "Unpause" : "Pause", Game.IsPlaying && !Game.IsPaused))
            {
                if (Game.IsPaused)
                    Game.Instance.PlayGame();
                else
                    Game.Instance.PauseGame();
            }

            VerticalSeparator();
            Button(BuildBTN, BuildBTN_Disabled, "Build", !Game.IsPlaying);
            VerticalSeparator();
            Button(InfoBTN, InfoBTN_Disabled, "Game Info", true);

            ImGui.PopStyleVar(2);
            ImGui.PopStyleColor();

            ImGui.End();
        }

        private static bool Button(Trinkit.Graphics.Texture button, Trinkit.Graphics.Texture disabledBtn, string label, bool active)
        {
            // ImGui.PushStyleColor(ImGuiCol.Button, (uint)Trinkit.Color.transparent);

            var b = (active) ? button : disabledBtn;

            bool clicked = ImGui.ImageButton(b.ID.ToString(), new IntPtr(b.ID), new Vector2(32, 32));
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
                ImGui.SetTooltip(label);

            // ImGui.PopStyleColor();
            ImGui.SameLine();

            return clicked;
        }

        private static void VerticalSeparator()
        {
            var x = ImGui.GetCursorPosX() + 2;
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(9, 0));

            ImGui.PushStyleColor(ImGuiCol.Separator, (uint)Trinkit.Color.transparent);
            ImGui.Separator();
            ImGui.SameLine();
            ImGui.PopStyleColor();

            var drawList = ImGui.GetWindowDrawList();
            drawList.AddLine(new Vector2(x, ImGui.GetCursorScreenPos().Y), new Vector2(x, ImGui.GetCursorScreenPos().Y + 38), (uint)new Trinkit.Color(0.135f, 0.135f, 0.135f, 1), 1);

            ImGui.PopStyleVar();
        }
    }
}
