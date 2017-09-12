using System;
using System.Collections.Generic;
using LifxDemo.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Navigation;

namespace LifxDemo.ViewModels
{
	public class MainPageViewModel : LifxViewModelBase
	{
		private SubscriptionToken _token = null;

		public MainPageViewModel()
			: base(false)
		{
		}

		public DelegateCommand RefreshCommand { get; set; }
		public DelegateCommand DiscoverCommand { get; set; }

		protected override void OnRegisterCommands()
		{
			this.RefreshCommand = this.RegisterCommand(this.OnRefreshCommand, this.OnCanRefreshCommand);
			this.DiscoverCommand = this.RegisterCommand(this.OnDiscoverCommand, this.OnCanDiscoverCommand);
			base.OnRegisterCommands();
		}

		public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(e, viewModelState);

			_token = this.EventAggregator.GetEvent<MessageEvent>().Subscribe((args) => this.OnMessageEvent(args));
		}

		public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatingFrom(e, viewModelState, suspending);

			if (_token != null)
			{
				this.EventAggregator.GetEvent<MessageEvent>().Unsubscribe(_token);
			}
		}

		protected void OnRefreshCommand()
		{
			this.EventAggregator.GetEvent<RefreshEvent>().Publish(new RefreshEventArgs(false));
		}

		protected bool OnCanRefreshCommand()
		{
			return this.NotIsActive;
		}

		protected void OnDiscoverCommand()
		{
			this.EventAggregator.GetEvent<RefreshEvent>().Publish(new RefreshEventArgs(true));
		}

		protected bool OnCanDiscoverCommand()
		{
			return this.NotIsActive;
		}

		protected void OnMessageEvent(MessageEventArgs e)
		{
			this.CurrentMessageType = e.Type;
			this.CurrentMessage = e.Message;

			this.RaisePropertyChanged(nameof(this.CurrentMessage));
			this.RaisePropertyChanged(nameof(this.ShowInformationIcon));
			this.RaisePropertyChanged(nameof(this.ShowWarningIcon));
			this.RaisePropertyChanged(nameof(this.ShowErrorIcon));
			this.RaisePropertyChanged(nameof(this.ShowMessage));
		}

		public string CurrentMessage { get; set; } = string.Empty;
		public bool ShowMessage => !string.IsNullOrEmpty(this.CurrentMessage);
		public MessageEventType CurrentMessageType { get; set; } = MessageEventType.None;
		public bool ShowInformationIcon => this.CurrentMessageType == MessageEventType.Information;
		public bool ShowWarningIcon => this.CurrentMessageType == MessageEventType.Warning;
		public bool ShowErrorIcon => this.CurrentMessageType == MessageEventType.Error;
	}
}
