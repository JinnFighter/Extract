using System.Collections.Generic;
using Common;
using Logic.Descriptions;
using MVVM;

namespace Ui.MainMenu
{
    public class CoterieMainMenuStateModel : IModel
    {
        public List<WidgetCoterieUnitModel> UnitModels { get; } = new();

        public CoterieMainMenuStateModel(UnitDescriptionLibrary unitDescriptionLibrary, UserDataService userDataService)
        {
            for (var i = 0; i < 3; i++)
            {
                UnitModels.Add(new WidgetCoterieUnitModel(unitDescriptionLibrary, userDataService, i));
            }
        }
    }
}