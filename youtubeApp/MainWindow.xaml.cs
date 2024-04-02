using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace youtubeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        async private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            pbarMain.Value = 0;
            var youtube = new YoutubeClient();

            string videoUrl = txtURL.Text;


            if (checkAudio.IsChecked == true)
            {
                try
                {
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

                    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                    var video = await youtube.Videos.GetAsync(videoUrl);

                    string thumbnail = video.Thumbnails.OrderByDescending(t => t.Resolution.Area).First().Url;

                    string title = video.Title;
                    string[] errorChars = { "?", "/", "*", "|", ":", "<", ">", "\"" };
                    foreach (string errorChar in errorChars)
                    {
                        title = title.Replace(errorChar, "");
                    }

                    var outputDirectory = @"M:\YT";
                    var outputFilePath = System.IO.Path.Combine(outputDirectory, $"{title}.{streamInfo.Container}");

                    var progress = new Progress<double>(p =>
                    {
                        pbarMain.Value = p * 100;
                    });

                    BitmapImage bmImage = new BitmapImage(new Uri(thumbnail));
                    imgThumbnail.Source = bmImage;

                    await youtube.Videos.Streams.DownloadAsync(streamInfo, outputFilePath, progress);

                    if (pbarMain.Value == 100)
                    {
                        pbarMain.Value = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                try
                {
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

                    var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                    var video = await youtube.Videos.GetAsync(videoUrl);

                    string thumbnail = video.Thumbnails.OrderByDescending(t => t.Resolution.Area).First().Url;

                    string title = video.Title;
                    string[] errorChars = { "?", "/", "*", "|", ":", "<", ">", "\"" };
                    foreach (string errorChar in errorChars)
                    {
                        title = title.Replace(errorChar, "");
                    }

                    var outputDirectory = @"M:\YT";
                    var outputFilePath = System.IO.Path.Combine(outputDirectory, $"{title}.{streamInfo.Container}");

                    var progress = new Progress<double>(p =>
                    {
                        pbarMain.Value = p * 100;
                    });

                    BitmapImage bmImage = new BitmapImage(new Uri(thumbnail));
                    imgThumbnail.Source = bmImage;

                    await youtube.Videos.Streams.DownloadAsync(streamInfo, outputFilePath, progress);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}