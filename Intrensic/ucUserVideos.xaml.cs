using CodeITBL;
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
    /// Interaction logic for ucUserVideos.xaml
    /// </summary>
    public partial class ucUserVideos : UserControl
    {
        public WrapPanel itemContainer;
        private bool isUpload = false;
        public bool IsUpload
        {
            get { return isUpload; }
            set { isUpload = value; }
        }

        public int ColumnWidth
        {
            get
            {
                if (isUpload)
                    return 412;
                else
                    return 206;
            }
        }

        public int TextWidth
        {
            get
            {
                if (isUpload)
                    return 412;
                else
                    return 206;
            }
        }

        private int controlWidht = 850;
        public int ControlWidht
        {
            get
            {
                return controlWidht;
            }
            set
            {
                controlWidht = value;
                lvItemsContainer.MaxWidth = value;
                lvItemsContainer.UpdateLayout();
                if (itemContainer != null)
                    itemContainer.MaxWidth = value;
                
            }
        }

        public System.Windows.Visibility UploadVisibility
        {
            get
            {
                if (isUpload)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Hidden;
            }
        }

        public System.Windows.Visibility ArchiveVisibility
        {
            get
            {
                if (!isUpload)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Hidden;
            }
        }		


        public System.Windows.Visibility ContextMenuVisibility
        {
            get
            {
                if (!isUpload)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Hidden;
            }
        }

        public ucUserVideos()
        {
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            lvItemsContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            lvItemsContainer.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

       
        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (lvItemsContainer.Items.Count == 0)
            {
                lvItemsContainer.ContextMenu.IsOpen = false;
                return;
            }
            if (lvItemsContainer.SelectedItems == null)
            {
                lvItemsContainer.ContextMenu.IsOpen = false;
                return;
            }

            if (lvItemsContainer.SelectedItems.Count == 0)
            {
                lvItemsContainer.ContextMenu.IsOpen = false;
                return;
            }


            bool canDownload = false;
            bool hasNote = false;

            if (IsUpload)
            {
                canDownload = false;
                hasNote = false;
            }
            else
            {
                canDownload = true;
                hasNote = (lvItemsContainer.SelectedItems.Count == 1 && ((CodeITBL.FileFromDB)lvItemsContainer.SelectedItems[0]).HasNote);
            }

            if (!canDownload || !hasNote)
                foreach (object menuItem in lvItemsContainer.ContextMenu.Items)
                {
                    if (menuItem.GetType() == typeof(Separator)) continue;

                    if ((((MenuItem)menuItem).Name.Equals("muDownloadAndValidate") || ((MenuItem)menuItem).Name.Equals("muDownload")) && !canDownload)
                        ((MenuItem)menuItem).IsEnabled = false;
                    else if (((MenuItem)menuItem).Name.Equals("muNote") && !hasNote)
                        ((MenuItem)menuItem).IsEnabled = false;
                }


            foreach (object menuItem in lvItemsContainer.ContextMenu.Items)
            {
                if (menuItem.GetType() == typeof(Separator)) continue;

                if (((MenuItem)menuItem).Name.Equals("muNote"))
                    ((MenuItem)menuItem).IsEnabled = hasNote;
            }


            lvItemsContainer.ContextMenu.Visibility = System.Windows.Visibility.Visible;

            lvItemsContainer.ContextMenu.IsOpen = true;
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

        private void muPreview_Click(object sender, RoutedEventArgs e)
        {

            if (lvItemsContainer.Items.Count <= 0)
                return;
            if (lvItemsContainer.SelectedItems.Count <= 0)
                return;

            List<string> videos = new List<string>();
            if (!isUpload)
            {
                foreach (CodeITBL.FileFromDB lvItem in lvItemsContainer.SelectedItems)
                    videos.Add(lvItem.OriginalFileLocation);
            }
            else
            {
                foreach (FileFromDB lvItem in lvItemsContainer.SelectedItems)
                    videos.Add(lvItem.OriginalFileLocation);
            }


            if (ContextMenuClick != null)
                ContextMenuClick(sender, e, ContextAction.Preview, videos);

        }

        private void ucCustomVideoList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MessageBox.Show(this.Width.ToString());
        }

        private void wpContainer_Loaded(object sender, RoutedEventArgs e)
        {
            itemContainer = (WrapPanel)sender;
        }

        private void WrapPanel_LostFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((WrapPanel)sender).Background.ToString());
        }

        private void lvItemsContainer_LostFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((ListView)sender).Background.ToString());
        }

        private void cmListItem_MouseMove(object sender, MouseEventArgs e)
        {
            // MessageBox.Show(((ListViewItem)lvItemsContainer.SelectedItems[0]).Background.ToString());
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
                ContextMenuClick(sender, e, ContextAction.Download, new List<string>());
        }

        private void muNote_Click(object sender, RoutedEventArgs e)
        {
            showNote(lvItemsContainer);
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

        private void performDownload(List<CodeITBL.FileFromDB> files, bool shouldValidate)
        {
            if (files == null) return;
            if (files.Count == 0) return;

            Context.progressForm.Show();
            Context.progressForm.BeginDownloadProcess(files.Select(x => new CodeITBL.FileFromDB()
            {
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                FileObject = x,
                Id = x.Id,
                IsArchived = x.IsArchived,
                MD5Checksum = x.MD5Checksum,
                NewFileLocation = x.NewFileLocation,
                NewFileName = x.NewFileName,
                Note = x.Note,
                OriginalFileDate = x.OriginalFileDate,
                OriginalFileLocation = x.OriginalFileLocation,
                OriginalFileName = x.OriginalFileName,
                UserId = x.UserId,
                Duration = x.Duration,
                Resolution = x.Resolution
            }).ToList(), shouldValidate);
        }
        

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount==2)
            {
                if (lvItemsContainer.Items.Count <= 0)
                    return;
                if (lvItemsContainer.SelectedItems.Count <= 0)
                    return;

                List<string> videos = new List<string>();                
                foreach (var lvItem in lvItemsContainer.SelectedItems)
                    if (lvItem is CodeITBL.FileFromDB)
                        videos.Add(((CodeITBL.FileFromDB)lvItem).OriginalFileLocation);
                    else
                        videos.Add(((CodeITDL.File)lvItem).OriginalFileLocation);

                if (ContextMenuClick != null)
                    ContextMenuClick(sender, e, ContextAction.Preview, videos);
            }
        }

        private void ucCustomVideoList_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isUpload)
            {
                //Margin="20,20,19,85"
                //lvItemsContainer.Margin = new Thickness(20, 20, 19, 85);
            }
        }
    }
}
