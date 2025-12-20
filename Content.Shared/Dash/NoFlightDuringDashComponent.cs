// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-License-Identifier: AGPL-3.0-or-later

using System;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Dash;

[Obsolete("NoFlightDuringDashComponent is deprecated. Dash now removes Flight on component init.")]
public sealed partial class NoFlightDuringDashComponent : Component
{
}
