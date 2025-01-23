using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using MinecraftLaunch.Classes.Enums;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Skin;
using MinecraftLaunch.Skin.Class.Fetchers;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Enums;
using WonderLab.Services.Download;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class AccountSkinLoadBehavior : Behavior<Border> {
    public static readonly StyledProperty<SkinArea> SkinAreaProperty =
        AvaloniaProperty.Register<AccountSkinLoadBehavior, SkinArea>(nameof(SkinArea), SkinArea.Head);

    public static readonly StyledProperty<Account> AccountProperty =
        AvaloniaProperty.Register<AccountSkinLoadBehavior, Account>(nameof(Account));

    public SkinArea SkinArea {
        get => GetValue(SkinAreaProperty);
        set => SetValue(SkinAreaProperty, value);
    }

    public Account Account {
        get => GetValue(AccountProperty);
        set => SetValue(AccountProperty, value);
    }

    private async Task<byte[]> GetSkinAsync(Account account) {
        if (!NetworkInterface.GetIsNetworkAvailable()) {
            return "resm:WonderLab.Assets.Image.steve.png".ToBytes();
        }

        var skin = await Task.Run(async () => {
            return account.Type switch {
                AccountType.Microsoft => await new MicrosoftSkinFetcher(account.Uuid.ToString()).GetSkinAsync(),
                AccountType.Yggdrasil => await new YggdrasilSkinFetcher((account as YggdrasilAccount).YggdrasilServerUrl, account.Uuid.ToString()).GetSkinAsync(),
                AccountType.Offline => "resm:WonderLab.Assets.Image.steve.png".ToBytes(),
                _ => "resm:WonderLab.Assets.Image.steve.png".ToBytes()
            };
        });

        return skin;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {
        var border = AssociatedObject ?? throw new Exception();
        var service = App.Get<CacheService>();

        if (Account is null) {
            return;
        }

        if (service.TryGetArea(Account, out var area)) {
            border.Background = area;
            return;
        }

        Dispatcher.UIThread.Post(async () => {
            var skin = new SkinResolver(await GetSkinAsync(Account));
            var brush = new ImageBrush(skin.CropSkinHeadBitmap().ToBitmap())
                .ToImmutable();

            border.Background = brush;
            service.AddSkin(Account, brush);
        }, DispatcherPriority.Background);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) { }

    protected override void OnAttached() {
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;
    }
}