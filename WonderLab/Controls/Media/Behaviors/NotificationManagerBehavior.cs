using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using System;
using WonderLab.Infrastructure.Models.Messaging;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class NotificationManagerBehavior : Behavior<WindowNotificationManager> {
    private static readonly TimeSpan Default = TimeSpan.FromSeconds(4d);

    public NotificationManagerBehavior() {
        WeakReferenceMessenger.Default.Register<NotificationMessage>(this, (_, args) => {
            if (AssociatedObject is null) {
                return;
            }

            _ = Dispatcher.UIThread.InvokeAsync(() => {
                AssociatedObject.Show(new Notification() {
                    Message = args.Text,
                    Expiration = Default,
                    OnClick = args.OnClick,
                    OnClose = args.OnClose,
                    Title = args.NotificationType.ToString(),
                });
            }, DispatcherPriority.ApplicationIdle);
        });
    }
}