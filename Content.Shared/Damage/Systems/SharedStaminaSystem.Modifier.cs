// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage.Components;

namespace Content.Shared.Damage.Systems;

public partial class SharedStaminaSystem
{
    private void InitializeModifier()
    {
        SubscribeLocalEvent<StaminaModifierComponent, ComponentStartup>(OnModifierStartup);
        SubscribeLocalEvent<StaminaModifierComponent, ComponentShutdown>(OnModifierShutdown);
    }

    private void OnModifierStartup(EntityUid uid, StaminaModifierComponent comp, ComponentStartup args)
    {
        if (!TryComp<StaminaComponent>(uid, out var stamina))
            return;

        stamina.CritThreshold *= comp.Modifier;

        // If this modifier prevents stamcrit on removal, ensure we don't instantly enter
        // stamcrit when the modifier is added by clamping current damage below the new threshold.
        if (comp.PreventStamCritOnRemove && stamina.StaminaDamage >= stamina.CritThreshold)
        {
            stamina.StaminaDamage = MathF.Max(0f, stamina.CritThreshold - 0.01f);
            Dirty(uid, stamina);
        }
    }

    private void OnModifierShutdown(EntityUid uid, StaminaModifierComponent comp, ComponentShutdown args)
    {
        if (!TryComp<StaminaComponent>(uid, out var stamina))
            return;

        stamina.CritThreshold /= comp.Modifier;

        // If this modifier is meant to prevent acute withdrawal, ensure the current
        // stamina damage does not instantaneously exceed the new threshold.
        if (comp.PreventStamCritOnRemove && stamina.StaminaDamage >= stamina.CritThreshold)
        {
            // Clamp just below the threshold so it doesn't immediately enter stamcrit.
            stamina.StaminaDamage = MathF.Max(0f, stamina.CritThreshold - 0.01f);
            Dirty(uid, stamina);
        }
    }

    /// <summary>
    /// Change the stamina modifier for an entity.
    /// If it has <see cref="StaminaComponent"/> it will also be updated.
    /// </summary>
    public void SetModifier(EntityUid uid, float modifier, StaminaComponent? stamina = null, StaminaModifierComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        var old = comp.Modifier;

        if (old.Equals(modifier))
            return;

        comp.Modifier = modifier;
        Dirty(uid, comp);

        if (Resolve(uid, ref stamina, false))
        {
            // scale to the new threshold, act as if it was removed then added
            stamina.CritThreshold *= modifier / old;

            // If this modifier requests to prevent stamcrit on removal, clamp current
            // damage if the newly scaled threshold would be breached.
            if (comp.PreventStamCritOnRemove && stamina.StaminaDamage >= stamina.CritThreshold)
            {
                stamina.StaminaDamage = MathF.Max(0f, stamina.CritThreshold - 0.01f);
                Dirty(uid, stamina);
            }
        }
    }
}
