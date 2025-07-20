using System.Collections.Generic;
using System.Linq;
using UiService.Code.Widgets;

namespace Ui.MainMenu
{
    public class WidgetCoterieUnit : BaseUiWidget<WidgetCoterieUnitModel, WidgetCoterieUnitView>
    {
        protected override void InitInner()
        {
            View.Dropdown.ClearOptions();
            var options = new List<string>
            {
                "None"
            };
            options.AddRange(Model.UnitDescriptionLibrary.UnitDescriptions.Select(description => description.NameId).ToList());
            View.Dropdown.AddOptions(options);
            SubscriptionAggregator.ListenEvent(View.Dropdown.onValueChanged, HandleDropdownValueChanged);
        }

        private void HandleDropdownValueChanged(int arg0)
        {
            if (arg0 == 0)
            {
                return;
            }
            
            Model.Select(arg0 - 1);
        }
    }
}