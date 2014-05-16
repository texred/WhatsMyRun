using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WhatsMyRun.Data;
using WhatsMyRun.Services.Navigation;

namespace WhatsMyRun.Pages.GroupedItems
{
    public class GroupedItemsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<SampleDataGroup> _groups = new ObservableCollection<SampleDataGroup>();

        public ObservableCollection<SampleDataGroup> Groups
        {
            get { return _groups; }
        }

        public GroupedItemsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();

            foreach (var group in sampleDataGroups)
            {
                _groups.Add(group);
            }
        }
    }
}
