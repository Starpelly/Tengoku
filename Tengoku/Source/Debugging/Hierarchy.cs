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
                
                for (int i = 0; i < TrinkitApp.Instance.CurrentScene?.SceneComponents.Count; i++)
                {
                    var component = TrinkitApp.Instance.CurrentScene?.SceneComponents[i];
                    ImGui.Text(component?.GetType().Name);
                }

                ImGui.End();
            }
        }
    }
}
