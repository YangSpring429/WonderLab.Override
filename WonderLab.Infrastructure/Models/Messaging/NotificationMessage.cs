using Avalonia.Controls.Notifications;

namespace WonderLab.Infrastructure.Models.Messaging;

public record NotificationMessage(string Text, NotificationType NotificationType, Action OnClick = default, Action OnClose = default);