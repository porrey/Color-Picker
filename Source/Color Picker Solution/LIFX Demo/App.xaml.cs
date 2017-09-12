using LifxDemo.Common;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace LifxDemo
{
	public partial class App : PrismUnityApplication
	{
		protected async override Task OnInitializeAsync(IActivatedEventArgs args)
		{
			await this.OnInitializeTitleBar();
			await this.OnRegisterTypes(this.Container);
			await base.OnInitializeAsync(args);

			this.UnhandledException += this.App_UnhandledException;
		}

		private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			
		}

		protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
		{
			this.NavigationService.Navigate(MagicString.Page.Main, null);
			return Task.FromResult(0);
		}

		private Task OnInitializeTitleBar()
		{
			ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

			titleBar.BackgroundColor = (Color)Application.Current.Resources["TitleBarBackgroundColor"];
			titleBar.ForegroundColor = (Color)Application.Current.Resources["TitleBarForegroundColor"];
			titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["TitleBarButtonBackgroundColor"];
			titleBar.ButtonForegroundColor = (Color)Application.Current.Resources["TitleBarForegroundColor"];
			titleBar.InactiveBackgroundColor = (Color)Application.Current.Resources["TitleBarInactiveBackgroundColor"];
			titleBar.InactiveForegroundColor = (Color)Application.Current.Resources["TitleBarInactiveForegroundColor"];
			titleBar.ButtonInactiveBackgroundColor = (Color)Application.Current.Resources["TitleBarButtonInactiveBackgroundColor"];
			titleBar.ButtonInactiveForegroundColor = (Color)Application.Current.Resources["TitleBarInactiveForegroundColor"];
			titleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["TitleBarButtonHoverBackgroundColor"];
			titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["TitleBarForegroundColor"];
			titleBar.ButtonPressedBackgroundColor = (Color)Application.Current.Resources["TitleBarButtonPressedBackgroundColor"];
			titleBar.ButtonPressedForegroundColor = (Color)Application.Current.Resources["TitleBarForegroundColor"];

			return Task.FromResult(0);
		}

		private Task OnRegisterTypes(IUnityContainer container)
		{
			// ***
			// *** System/applications instances.
			// ***
			container.RegisterInstance(this.NavigationService);
			container.RegisterInstance(this.SessionStateService);
			container.RegisterInstance(this.EventAggregator);
			container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

			return Task.FromResult(0);
		}
	}
}
