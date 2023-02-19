using ImGuiNET;

namespace Tengoku.Debugging
{
    public class AnimationEditor
    {
        private static int selected = 0;

        public static void Gui()
        {
            if (ImGui.Begin("Animator"))
            {

                if (ImGui.BeginChild("left pane", new System.Numerics.Vector2(150, 0), true))
                {
                    ImGui.Text("Spaceball");
                    ImGui.Separator();
                    ImGui.Selectable("PlayerIdle");
                    ImGui.Selectable("PlayerSwing");
                    ImGui.Selectable("DispenserPrepare");
                    ImGui.Selectable("DispenserShoot");
                    ImGui.Selectable("UmpireIdle");
                    ImGui.Selectable("UmpireShow");

                    ImGui.EndChild();
                }
                ImGui.SameLine();
                ImGui.BeginGroup();
                {
                    if (ImGui.BeginChild("item view", new System.Numerics.Vector2(0, -ImGui.GetFrameHeightWithSpacing()), true))
                    {
                        ImGui.Text("hello");
                        ImGui.EndChild();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Revert")) { }
                    ImGui.SameLine();
                    if (ImGui.Button("Save")) { }
                }
                ImGui.EndGroup();

                ImGui.End();
            }
        }
    }
}
