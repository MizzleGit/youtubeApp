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
            var youtube = new YoutubeClient();

            string videoUrl = txtURL.Text;


            if (checkAudio.IsChecked == true)
            {
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

                var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                var video = await youtube.Videos.GetAsync(videoUrl);

                string title = video.Title;
                string[] errorChars = { "?", "/", "*", "|", ":", "<", ">", "\"" };
                foreach (string errorChar in errorChars)
                {
                    title = title.Replace(errorChar, "");
                }

                var outputDirectory = @"C:\Users\Youssef\Downloads\YT_Tests";
                var outputFilePath = System.IO.Path.Combine(outputDirectory, $"{video.Title}.{streamInfo.Container}");

                var progress = new Progress<double>(p =>
                {
                    pbarDownload.Value = p;
                });

                await youtube.Videos.Streams.DownloadAsync(streamInfo, outputFilePath);

            }
            else
            {
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                var video = await youtube.Videos.GetAsync(videoUrl);

                string title = video.Title;
                string[] errorChars = { "?", "/", "*", "|", ":", "<", ">", "\"" };
                foreach (string errorChar in errorChars)
                {
                    title = title.Replace(errorChar, "");
                }
                var outputDirectory = @"C:\Users\Youssef\Downloads\YT_Tests";
                var outputFilePath = System.IO.Path.Combine(outputDirectory, $"{video.Title}.{streamInfo.Container}");

                await youtube.Videos.Streams.DownloadAsync(streamInfo, outputFilePath);
            }
        }
    }
}