namespace InPlaceEditBoxDemo
{
    using InPlaceEditBoxDemo.ViewModels;
    using ServiceLocator;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceInjector.InjectServices();      // Start-up services

            var appVM = new AppViewModel();

            this.DataContext = appVM; // Команда, которая позволяет загружать и сохранять xml

            //Loaded += MainWindow_LoadedAsync; // Для начальной загрузке файлов
        }

        //private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e) // Для начальной загрузке файлов
        //{
        //    Loaded -= MainWindow_LoadedAsync;

        //    var appVM = new AppViewModel();
        //    this.DataContext = appVM;

        //    //appVM.ResetDefaults();
        //    //await appVM.LoadSampleDataAsync();
        //}
    }
}
