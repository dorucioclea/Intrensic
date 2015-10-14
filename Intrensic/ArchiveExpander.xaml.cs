using CodeITBL;
using IntrensicMediaPlayer;
using Microsoft.Win32;
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

namespace Intrensic
{
    /// <summary>
    /// Interaction logic for ArchiveExpander.xaml
    /// </summary>
    public partial class ArchiveExpander : System.Windows.Controls.UserControl
    {

        public WrapPanel itemContainer;

        private int _height;
        public void SetNewHeight(int height)
        {
            //reduce height for archive title bar

            if (height > 115)
                _height = height - 115;
            else
                _height = height;

            ex3Days.MaxHeight = exToday.MaxHeight = ex7Days.MaxHeight = _height;

            if (exToday.IsExpanded)
                Expander_Expanded(exToday, null);
            else if (ex3Days.IsExpanded)
                Expander_Expanded(ex3Days, null);
            else if (ex7Days.IsExpanded)
                Expander_Expanded(ex7Days, null);
        }


        public void BindLists(List<CodeITBL.FileFromDB> files, bool SearchByUploadDate = false)
        {
			lvItemsContainer.ItemsSource = null;
			lvItemsContainer3Days.ItemsSource = null;
			lvItemsContainer7Days.ItemsSource = null;

            lvItemsContainer.Items.Clear();
            lvItemsContainer3Days.Items.Clear();
            lvItemsContainer7Days.Items.Clear();

			if (!SearchByUploadDate)
			{
				lvItemsContainer.ItemsSource = files.Where(x => x.OriginalFileDate.Date == DateTime.Now.Date).ToList();
				lvItemsContainer3Days.ItemsSource = files.Where(x => x.OriginalFileDate.Date >= DateTime.Now.Date.AddDays(-2) && x.OriginalFileDate.Date <= DateTime.Now.Date.AddHours(23).AddMinutes(59)).ToList();
				lvItemsContainer7Days.ItemsSource = files.Where(x => x.OriginalFileDate.Date >= DateTime.Now.Date.AddDays(-6) && x.OriginalFileDate.Date <= DateTime.Now.Date.AddHours(23).AddMinutes(59)).ToList();
			}
			else
			{				
				lvItemsContainer.ItemsSource = files.Where(x => x.CreatedOn.Date == DateTime.Now.Date).ToList();
				lvItemsContainer3Days.ItemsSource = files.Where(x => x.CreatedOn.Date >= DateTime.Now.Date.AddDays(-2) && x.CreatedOn.Date <= DateTime.Now.Date.AddHours(23).AddMinutes(59)).ToList();
				lvItemsContainer7Days.ItemsSource = files.Where(x => x.CreatedOn.Date >= DateTime.Now.Date.AddDays(-6) && x.CreatedOn.Date <= DateTime.Now.Date.AddHours(23).AddMinutes(59)).ToList();
			}

            txtTodayNoItems.Visibility = lvItemsContainer.Items.Count == 0 ? Visibility.Visible : Visibility.Hidden;
            txt3DaysNoItems.Visibility = lvItemsContainer3Days.Items.Count == 0 ? Visibility.Visible : Visibility.Hidden;
            txt7DaysNoItems.Visibility = lvItemsContainer7Days.Items.Count == 0 ? Visibility.Visible : Visibility.Hidden;
        }
        public ArchiveExpander()
        {
            InitializeComponent();
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {

            ListView lvItems = lvItemsContainer;
            if (((ContextMenu)sender).Name == "cmListItem3days")
            {
                lvItems = lvItemsContainer3Days;
            }
            else if (((ContextMenu)sender).Name == "cmListItem7days")
            {
                lvItems = lvItemsContainer7Days;
            }



            if (lvItems.Items.Count == 0)
            {
                lvItems.ContextMenu.IsOpen = false;
                return;
            }
            if (lvItems.SelectedItems == null)
            {
                lvItems.ContextMenu.IsOpen = false;
                return;
            }

            if (lvItems.SelectedItems.Count == 0)
            {
                lvItems.ContextMenu.IsOpen = false;
                return;
            }


            bool canDownload = false;
            bool hasNote = false;
            bool IsUpload = false;

            if (IsUpload)
            {
                canDownload = false;
                hasNote = false;
            }
            else
            {
                canDownload = true;
                hasNote = (lvItems.SelectedItems.Count == 1 && ((CodeITBL.FileFromDB)lvItems.SelectedItems[0]).HasNote);
            }

            if (!canDownload || !hasNote)
                foreach (object menuItem in lvItems.ContextMenu.Items)
                {
                    if (menuItem.GetType() == typeof(Separator)) continue;

                    if ((((MenuItem)menuItem).Name.Equals("muDownloadAndValidate") || ((MenuItem)menuItem).Name.Equals("muDownload")) && !canDownload)
                        ((MenuItem)menuItem).IsEnabled = false;
                    else if (((MenuItem)menuItem).Name.Equals("muNote") && !hasNote)
                        ((MenuItem)menuItem).IsEnabled = false;
                }

            foreach (object menuItem in lvItems.ContextMenu.Items)
            {
                if (menuItem.GetType() == typeof(Separator)) continue;

                if (((MenuItem)menuItem).Name.Equals("muNote"))
                    ((MenuItem)menuItem).IsEnabled = hasNote;
            }


            lvItems.ContextMenu.Visibility = System.Windows.Visibility.Visible;

            lvItems.ContextMenu.IsOpen = true;
        }

        private void lvItemsContainer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
                if (lvItemsContainer != null)
                    if (lvItemsContainer.SelectedItems != null)
                        if (lvItemsContainer.SelectedItems.Count > 0)
                        {
                            bool isOnItem = false;

                            for (int i = 0; i < lvItemsContainer.SelectedItems.Count; i++)
                                if (((ListViewItem)lvItemsContainer.ItemContainerGenerator.ContainerFromItem(lvItemsContainer.SelectedItems[i])).IsMouseOver)
                                {
                                    isOnItem = true;
                                    break;
                                }

                            if (!isOnItem)
                            {
                                lvItemsContainer.SelectedIndex = -1;
                                lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Hidden;
                                lvItemsContainer.ContextMenu.IsOpen = false;
                            }
                        }

        }

        private void lvItemsContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvItemsContainer.ContextMenu.IsOpen = false;
        }

        public event ContextClick ContextMenuClick;



        private void ucCustomVideoList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void wpContainer_Loaded(object sender, RoutedEventArgs e)
        {
            itemContainer = (WrapPanel)sender;
        }

        private void WrapPanel_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void lvItemsContainer_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

            if (((Expander)sender).Name == exToday.Name)
            {
                exToday.Height = _height;
                ex3Days.Height = 30;
                ex7Days.Height = 30;
                picToday.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21_expanded.png") as ImageSource;
                ex3Days.IsExpanded = ex7Days.IsExpanded = false;
                return;
            }

            if (((Expander)sender).Name == ex3Days.Name)
            {
                exToday.Height = 30;
                ex3Days.Height = _height;
                ex7Days.Height = 30;
                pic3Days.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21_expanded.png") as ImageSource;
                exToday.IsExpanded = ex7Days.IsExpanded = false;
                return;
            }

            if (((Expander)sender).Name == ex7Days.Name)
            {
                exToday.Height = 30;
                ex3Days.Height = 30;
                ex7Days.Height = _height;

                pic7Days.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21_expanded.png") as ImageSource;
                exToday.IsExpanded = ex3Days.IsExpanded = false;
                return;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer.Items.Count > 0)
            {
                exToday.IsExpanded = true;
                exToday.Height = _height;
                ex3Days.Height = 30;
                ex7Days.Height = 30;
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (((Expander)sender).Name == exToday.Name)
            {
                exToday.Height = 30;
                picToday.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21.png") as ImageSource;
                return;
            }

            if (((Expander)sender).Name == ex3Days.Name)
            {
                ex3Days.Height = 30;
                pic3Days.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21.png") as ImageSource;
                return;
            }

            if (((Expander)sender).Name == ex7Days.Name)
            {
                ex7Days.Height = 30;
                pic7Days.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Intrensic;component/Resources/icons_21.png") as ImageSource;
                return;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void lvItemsContainer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }



        private void performDownload(List<CodeITBL.FileFromDB> files, bool shouldValidate)
        {
            if (files == null) return;
            if (files.Count == 0) return;

            Context.progressForm.Show();
            Context.progressForm.BeginDownloadProcess(files, shouldValidate);



        }

        private void showNote(ListView sender)
        {

            if (sender.Items.Count <= 0)
                return;
            if (sender.SelectedItems.Count <= 0)
                return;

            if (sender.SelectedItems.Count > 1)
            {
				MessageBox.Show("Please select only one item for viewing the note", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CodeITBL.FileFromDB selectedFile = (CodeITBL.FileFromDB)sender.SelectedItems[0];

            if (!selectedFile.HasNote)
            {
                MessageBox.Show("Selected file has no notes", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            var container = sender.ItemContainerGenerator.ContainerFromItem(sender.SelectedItem) as FrameworkElement;

            if (container == null)
                return;

            ToolTip ttp = new ToolTip();
            ttp.Content = selectedFile.Note;
            ttp.PlacementTarget = container;
            ttp.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
            ttp.StaysOpen = false;
            ttp.IsOpen = true;

        }

        void ProgressChanged(object sender, TaskProgressReport report)
        {
        }


        void performPreview(ListView sender)
        {
            List<string> urls = new List<string>();

            List<FileFromDB> files = new List<FileFromDB>();

			foreach (CodeITBL.FileFromDB selectedFile in sender.SelectedItems)
			{
				if (CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Cloud && selectedFile.IsCloudFileSystem == false)
				{
					System.Windows.MessageBox.Show(string.Format("Video {0} is stored local, please use local license", selectedFile.NewFileName), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				if (CodeITLicence.Licence.StorageType == CodeITLicence.StorageType.Local && selectedFile.IsCloudFileSystem == true)
				{
					System.Windows.MessageBox.Show(string.Format("Video {0} is stored on cloud, please use cloud license", selectedFile.NewFileName), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
					files.Add(selectedFile);
			}

			if (files.Count > 0)
			{
				VideoFromStream player = new VideoFromStream();
				player.LoadVideos(files);
				player.Topmost = true;
				player.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
				player.ShowDialog();
			}
        }

        private void muPreview_Click(object sender, RoutedEventArgs e)
        {

            if (lvItemsContainer.Items.Count <= 0)
                return;
            if (lvItemsContainer.SelectedItems.Count <= 0)
                return;

            performPreview(lvItemsContainer);

        }


        private void muDownload_Click(object sender, RoutedEventArgs e)
        {

            if (lvItemsContainer.Items.Count <= 0)
                return;
            if (lvItemsContainer.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, false);

            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muDownloadAndValidate_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer.Items.Count <= 0)
                return;
            if (lvItemsContainer.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, true);


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.DownloadAndVerify, new List<string>());
        }

        private void muNote_Click(object sender, RoutedEventArgs e)
        {
            showNote(lvItemsContainer);
        }

        private void muPreview3days_Click(object sender, RoutedEventArgs e)
        {

            if (lvItemsContainer3Days.Items.Count <= 0)
                return;
            if (lvItemsContainer3Days.SelectedItems.Count <= 0)
                return;

            performPreview(lvItemsContainer3Days);

        }

        private void muDownload3days_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer3Days.Items.Count <= 0)
                return;
            if (lvItemsContainer3Days.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer3Days.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, false);


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muDownloadAndValidate3days_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer3Days.Items.Count <= 0)
                return;
            if (lvItemsContainer3Days.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer3Days.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, true);


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muNote3days_Click(object sender, RoutedEventArgs e)
        {
            showNote(lvItemsContainer3Days);
        }

        private void muPreview7days_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer7Days.Items.Count <= 0)
                return;
            if (lvItemsContainer7Days.SelectedItems.Count <= 0)
                return;

            performPreview(lvItemsContainer7Days);
        }

        private void muDownload7days_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer7Days.Items.Count <= 0)
                return;
            if (lvItemsContainer7Days.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer7Days.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, false);


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muDownloadAndValidate7days_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer7Days.Items.Count <= 0)
                return;
            if (lvItemsContainer7Days.SelectedItems.Count <= 0)
                return;

            List<CodeITBL.FileFromDB> videos = new List<CodeITBL.FileFromDB>();
            foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer7Days.SelectedItems)
                videos.Add(lvItem);
            performDownload(videos, true);


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muNote7days_Click(object sender, RoutedEventArgs e)
        {
            showNote(lvItemsContainer7Days);
        }

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (lvItemsContainer.Items.Count <= 0)
                    return;
                if (lvItemsContainer.SelectedItems.Count <= 0)
                    return;

                performPreview(lvItemsContainer);
            }
        }

        private void WrapPanel_MouseDown3(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (lvItemsContainer3Days.Items.Count <= 0)
                    return;
                if (lvItemsContainer3Days.SelectedItems.Count <= 0)
                    return;

                performPreview(lvItemsContainer3Days);
            }
        }

        private void WrapPanel_MouseDown7(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (lvItemsContainer7Days.Items.Count <= 0)
                    return;
                if (lvItemsContainer7Days.SelectedItems.Count <= 0)
                    return;

                performPreview(lvItemsContainer7Days);
            }
        }


    }
}
