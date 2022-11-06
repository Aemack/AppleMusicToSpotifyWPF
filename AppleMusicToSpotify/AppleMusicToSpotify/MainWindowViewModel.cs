using AppleMusicToSpotify.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace AppleMusicToSpotify
{
    public class MainWindowViewModel : BaseViewModel
    {
        AppleToSpotifyService atss;
        private string _appleMusicLink;
        public string AppleMusicLink
        {
            get
            {
                return _appleMusicLink;
            }
            set
            {
                _appleMusicLink = value;
                OnPropertyChanged(nameof(AppleMusicLink));
            }
        }

        private string _spotifyLink;
        public string SpotifyLink
        {
            get
            {
                return _spotifyLink;
            }
            set
            {
                _spotifyLink = value;
                OnPropertyChanged(nameof(SpotifyLink));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public void AppleToSpotifyClicked()
        {
            IsLoading = true;
            try
            {
                SpotifyLink = atss.GetSpotifyLinkFromApple(AppleMusicLink);
            } catch
            {
                SpotifyLink = "Failed";
            }
        }

        public MainWindowViewModel()
        {
            atss = new AppleToSpotifyService();
        }


        internal void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
            
        }

    }
}
