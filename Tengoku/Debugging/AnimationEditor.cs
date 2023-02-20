using ImGuiNET;

namespace Tengoku.Debugging
{
    public class AnimationEditor
    {
        private static int selected = 0;
        private static int frame;

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
                        ImGui.Text("AnimSet Spaceball/PlayerSwing");
                        ImGui.Separator();
                        ImGui.Button("<<");
                        ImGui.SameLine();
                        ImGui.Button("Play");
                        ImGui.SameLine();
                        ImGui.Button(">>");
                        ImGui.Separator();
                        
                        ImGui.SliderInt("Frame##FrameSlider", ref frame, 0, 5, $"{frame}/{5}");

                        var spaceball = Game.Instance!.spaceball;
                        var frameStartX = (spaceball.SpaceballPlayerSheet0.width / 5) * (frame);

                        ImGui.Image(new IntPtr(spaceball.SpaceballPlayerSheet0.id), new System.Numerics.Vector2(spaceball.SpaceballPlayerSheet0.width / 5, spaceball.SpaceballPlayerSheet0.height),
                            new System.Numerics.Vector2(frameStartX, 0), 
                            new System.Numerics.Vector2(Trinkit.Mathf.Normalize(frameStartX - ((spaceball.SpaceballPlayerSheet0.width / 5) * (frame + 1)), 0, spaceball.SpaceballPlayerSheet0.width), 1));

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
