using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using WinRT.InitializeWithWindow;

namespace AssetInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProductSetup Setup { get; set; }

        public Progress<ZipProgress> _progress;


        public MainWindow()
        {
            InitializeComponent();
        }

        public void InUiThread(Action action)
        {
            if (this.Dispatcher.CheckAccess())
                action();
            else
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Setup = new ProductSetup();
            Setup.InUiThread = this.InUiThread;

            DataContext = Setup;
        }

        private void OnDrag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OnInstall(object sender, RoutedEventArgs e)
        {
            if (Setup.InstallDirectory != null)
            {
                string targetFolder = System.IO.Path.GetFileName(Setup.InstallDirectory.TrimEnd(System.IO.Path.DirectorySeparatorChar));
                if (!targetFolder.Equals("BroadcastProjects"))
                {
                    var dlg = new MessageDialog("Project folder destination is incorrect.\nExample -> D:\\Projects\\BroadcastProjects");
                    this.InitializeWinRTChild(dlg);
                    await dlg.ShowAsync();
                }
                else
                    Setup.StartInstall();
            }
            else
            {
                var dlg = new MessageDialog("Project folder must be selected first.\nLook for [...] above.");
                this.InitializeWinRTChild(dlg);
                await dlg.ShowAsync();
            }
        }

        private void OnRepare(object sender, RoutedEventArgs e)
        {
            //Setup.StartRepair();
        }

        private void OnUninstall(object sender, RoutedEventArgs e)
        {
            Setup.StartUninstall();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Setup.StartCancel();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = Setup.IsRunning;
        }

        private void OnShowLog(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Setup.LogFileCreated && Setup.LogFile != null)
                Process.Start(Setup.LogFile);
        }

        private async void OnHelp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = new MessageDialog("Select the Broadcast Explorer Project folder as your installation directory.\nExample -> D:\\Projects\\BroadcastProjects", "Instructions:");
            this.InitializeWinRTChild(dlg);
            await dlg.ShowAsync();
        }

        private async void OnSelectFolder(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker();
            this.InitializeWinRTChild(folderPicker);

            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                Setup.InstallDirectory = folder.Path;
            }
        }
    }
}
