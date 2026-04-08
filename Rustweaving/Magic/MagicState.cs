using System;
using ProtoBuf;

namespace Rustweaving.Magic;

[ProtoContract]
public sealed class MagicState
{
    public const string ModDataKey = "rustweaving:magicstate";
    public const int DefaultMaxMana = 100;

    [ProtoMember(1)]
    public int CurrentMana { get; set; }

    [ProtoMember(2)]
    public int MaxMana { get; set; }

    public static MagicState CreateDefault()
    {
        return new MagicState
        {
            MaxMana = DefaultMaxMana,
            CurrentMana = DefaultMaxMana
        };
    }

    public MagicState Clone()
    {
        return new MagicState
        {
            CurrentMana = CurrentMana,
            MaxMana = MaxMana
        };
    }

    public void Clamp()
    {
        if (MaxMana < 1)
        {
            MaxMana = DefaultMaxMana;
        }

        CurrentMana = Math.Clamp(CurrentMana, 0, MaxMana);
    }
}
