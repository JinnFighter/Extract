using Common;
using Logic.Descriptions;
using MVVM;

namespace Ui.MainMenu
{
    public class WidgetCoterieUnitModel : IModel
    {
        private readonly int _index;
        private readonly UserDataService _userDataService;
        public WidgetCoterieUnitModel(UnitDescriptionLibrary unitDescriptionLibrary, UserDataService userDataService, int index)
        {
            UnitDescriptionLibrary = unitDescriptionLibrary;
            _userDataService = userDataService;
            _index = index;
        }

        public UnitDescriptionLibrary UnitDescriptionLibrary { get; }
        public void Select(int dropdownValue)
        {
            if (_userDataService.LocalPlayer.Coterie.Count > _index)
            {
                _userDataService.LocalPlayer.SetCoterieId(_index, UnitDescriptionLibrary.UnitDescriptions[dropdownValue].NameId); 
            }
            else
            {
                _userDataService.LocalPlayer.AddCoterie(UnitDescriptionLibrary.UnitDescriptions[dropdownValue].NameId);
            }
        }
    }
}