using Content.Shared.Actions.ActionTypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.TickKing
{
    [RegisterComponent]
    public sealed class TickKingComponent : Component
    {
        /// <summary>
        ///     The action for the Raise Army ability
        /// </summary>
        [DataField("actionRaiseArmy", required: true)]
        public InstantAction ActionRaiseArmy = new();

        /// <summary>
        ///     The amount of hunger one use of Raise Army consumes
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("hungerPerArmyUse", required: true)]
        public float HungerPerArmyUse = 10f;

        /// <summary>
        ///     The entity prototype of the mob that Raise Army summons
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("armyMobSpawnId", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string ArmyMobSpawnId = "MobTick";

        /// <summary>
        ///     The action for the Domain ability
        /// </summary>
        [DataField("actionDomain", required: true)]
        public InstantAction ActionDomain = new();

        /// <summary>
        ///     The amount of hunger one use of Domain consumes
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("hungerPerDomainUse", required: true)]
        public float HungerPerDomainUse = 50f;

        /// <summary>
        ///     How many moles of Miasma are released after one us of Domain
        /// </summary>
        [DataField("molesMiasmaPerDomain")]
        public float MolesMiasmaPerDomain = 100f;
    }
};
