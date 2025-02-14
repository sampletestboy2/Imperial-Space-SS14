using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Logs;
using Content.Shared.Mobs.Components;
using Content.Shared.Standing;
using Robust.Shared.GameStates;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Mobs.Systems;

[Virtual]
public partial class MobStateSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly MobThresholdSystem _trashholdSystem = default!;
    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        _sawmill = _logManager.GetSawmill("MobState");
        base.Initialize();
        SubscribeEvents();
        SubscribeLocalEvent<MobStateComponent, ComponentGetState>(OnGetComponentState);
        SubscribeLocalEvent<MobStateComponent, ComponentHandleState>(OnHandleComponentState);
    }

    #region Public API

    /// <summary>
    ///  Check if a Mob is Alive
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is alive</returns>
    public bool IsAlive(EntityUid target, MobStateComponent? component = null)
    {
        if (!Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Alive;
    }

    /// <summary>
    ///  Check if a Mob is Critical
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical</returns>
    public bool IsCritical(EntityUid target, MobStateComponent? component = null)
    {
        if (!Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Critical;
    }

    /// <summary>
    ///  Check if a Mob is Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Dead</returns>
    public bool IsDead(EntityUid target, MobStateComponent? component = null)
    {
        if (!Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is Critical or Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical or Dead</returns>
    public bool IsIncapacitated(EntityUid target, MobStateComponent? component = null)
    {
        if (!Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Critical or MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is in an Invalid state
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is in an Invalid State</returns>
    public bool IsInvalidState(EntityUid target, MobStateComponent? component = null)
    {
        if (!Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Invalid;
    }

    #endregion

    #region Private Implementation

    private void OnHandleComponentState(EntityUid uid, MobStateComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not MobStateComponentState state)
            return;

        component.CurrentState = state.CurrentState;

        if (!component.AllowedStates.SetEquals(state.AllowedStates))
        {
            component.AllowedStates.Clear();
            component.AllowedStates.UnionWith(state.AllowedStates);
        }
    }

    private void OnGetComponentState(EntityUid uid, MobStateComponent component, ref ComponentGetState args)
    {
        args.State = new MobStateComponentState(component.CurrentState, component.AllowedStates);
    }

    #endregion
}
