using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhatsMyRun.Services.DataProviders.Workouts;
using WhatsMyRun.Services.DataRequestor;

namespace WPFWhatsMyRun
{
    //Using WPF to get some exposure with it.
    //The charting library for Win8 does not work with .net 4.5.1

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeIOC();
        }

        private void InitializeIOC()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IDataRequestor, LocalTestDataRequestor>();
            SimpleIoc.Default.Register<IWorkoutDataProvider, WorkoutDataProvider>();
        }

        private async Task DisplayWorkoutGraph()
        {
            var workoutDataProvider = ServiceLocator.Current.GetInstance<IWorkoutDataProvider>();
            //var workoutsTask = workoutDataProvider.GetWorkoutsForUserAsync(22);
            //workoutsTask.Wait();
            //var workouts = workoutsTask.Result;
            var workouts = await workoutDataProvider.GetWorkoutsForUserAsync(22).ConfigureAwait(true);

            DateTime[] workoutDates = workouts.Select(w => w.StartTime).ToArray();
            TimeSpan[] workoutTimespans = workouts.Select(w => w.AverageTimePerMile).ToArray();

            var datesDataSource = new EnumerableDataSource<DateTime>(workoutDates);
            datesDataSource.SetXMapping(dt => dateAxis.ConvertToDouble(dt));

            var workoutTimesDataSource = new EnumerableDataSource<TimeSpan>(workoutTimespans);
            workoutTimesDataSource.SetYMapping(ts => ts.TotalMinutes);//dateAxis.ConvertToDouble(ts));

            CompositeDataSource compositeDataSource1 = new CompositeDataSource(datesDataSource, workoutTimesDataSource);
            plotter.AddLineGraph(compositeDataSource1, (Color)ColorConverter.ConvertFromString("#00FF00"));
            plotter.Viewport.FitToView();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await DisplayWorkoutGraph();

        }
    }
}
