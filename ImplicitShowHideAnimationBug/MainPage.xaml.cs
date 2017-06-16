using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImplicitShowHideAnimationBug
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void EnableFluidVisibilityAnimation(UIElement element)
        {
            var elementVisual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = elementVisual.Compositor;
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);

            ScalarKeyFrameAnimation hideOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            hideOpacityAnimation.InsertKeyFrame(1.0f, 0.0f);
            hideOpacityAnimation.Duration = TimeSpan.FromMilliseconds(2000);
            hideOpacityAnimation.Target = "Opacity";

            var hideAnimationGroup = compositor.CreateAnimationGroup();
            hideAnimationGroup.Add(hideOpacityAnimation);


            ElementCompositionPreview.SetImplicitHideAnimation(element, hideAnimationGroup);

            ScalarKeyFrameAnimation showOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            showOpacityAnimation.InsertKeyFrame(1.0f, 1.0f);
            showOpacityAnimation.Duration = TimeSpan.FromMilliseconds(2000);
            showOpacityAnimation.Target = "Opacity";

            var showAnimationGroup = compositor.CreateAnimationGroup();
            showAnimationGroup.Add(showOpacityAnimation);

            ElementCompositionPreview.SetImplicitShowAnimation(element, showAnimationGroup);
        }

        private bool _firstTime = true;
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_firstTime)
            {
                return;
            }
            _firstTime = false;

            await Task.Delay(3000);

            var grid = (Grid)sender;
            var border = grid.Children.OfType<Border>().First();
            EnableFluidVisibilityAnimation(border);
            border.Visibility = Visibility.Collapsed;
        }
    }
}
