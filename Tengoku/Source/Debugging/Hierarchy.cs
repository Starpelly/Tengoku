using ImGuiNET;
using Trinkit;

namespace Tengoku.Debugging
{
    public class Hierarchy
    {
        public static void Gui()
        {
            if (ImGui.Begin($"{FontIcon.List}Hierarchy"))
            {
                if (ImGui.BeginTable("hierarchytable", 1, ImGuiTableFlags.ScrollY | ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Hideable))
                {
                    ImGui.TableSetupScrollFreeze(0, 1);
                    ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.WidthFixed);
                    ImGui.TableHeadersRow();

                    DrawComponents(false);

                    ImGui.EndTable();
                }

                ImGui.End();
            }
        }

        private static void DrawComponents(bool destroyOnLoad = false)
        {
            for (int row = 0; row < TrinkitApp.Instance.CurrentScene?.SceneComponents.Count; row++)
            {
                var component = TrinkitApp.Instance.CurrentScene?.SceneComponents[row];
                if (component == null) continue;
                if (destroyOnLoad)
                    if (!component.DontDestroyOnLoad) return;
                    else
                        if (component.DontDestroyOnLoad) return;

                ImGui.TableNextRow();
                for (int column = 0; column < 1; column++)
                {
                    ImGui.TableSetColumnIndex(column);

                   if (column == 0)
                        ImGui.Text(component?.GetType().FullName);
                }
            }
        }
    }
}
