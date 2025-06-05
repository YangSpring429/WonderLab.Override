using System;

namespace WonderLab.Extensions.Hosting.UI;

public record DialogDescriptor(Type DialogType, Type ViewModelType = default);