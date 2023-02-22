using ImGuiNET;

namespace Tengoku.Debugging
{
    public class LocalizerView
    {
        public static void Gui()
        {
            if (ImGui.Begin("Localizer", ImGuiWindowFlags.HorizontalScrollbar))
            {
                var columnCount = 2;
                var rowCount = 12;
                if (ImGui.BeginTable("table1", columnCount, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg, new System.Numerics.Vector2(0, 0)))
                {
                    ImGui.TableNextColumn();
                    ImGui.TableHeader("Game");
                    ImGui.TableNextColumn();
                    ImGui.TableHeader("Description");

                    for (int row = 0; row < rowCount; row++)
                    {
                        ImGui.TableNextRow();
                        for (int column = 0; column < columnCount; column++)
                        {
                            ImGui.TableSetColumnIndex(column);
                            ImGui.Text($"Row {row} Column {column}");
                        }
                    }
                    ImGui.EndTable();
                }

                ImGui.End();
            }
        }
    }
}
