// SPDX-FileCopyrightText: 2025 You <you@example.com>
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Tripping;

[RegisterComponent]
public sealed partial class TripperComponent : Component
{
    // Probability per meter moved to trip
    [DataField("chance")]
    public float Chance = 0.02f;

    // How long to remain knocked down
    [DataField("stunDuration")]
    public float StunDuration = 1.0f;

    // Minimum distance moved to consider a trip test (prevents micro-movements tripping you)
    [DataField("minDistance")]
    public float MinDistance = 0.1f;

    // Optional cooldown in seconds after tripping during which you cannot trip again
    [DataField("immunitySeconds")]
    public float ImmunitySeconds = 0f;

    // Runtime-only: timestamp when next trip is allowed
    [ViewVariables]
    public TimeSpan NextTripAllowed = TimeSpan.Zero;
}
